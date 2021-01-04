using AutoMapper;
using Caliburn.Micro;
using Microsoft.VisualBasic;
using SafeKeys.EventModels;
using SafeKeys.Library;
using SafeKeys.Library.API;
using SafeKeys.Library.Models;
using SafeKeys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace SafeKeys.ViewModels
{
    public class KeysViewModel : Screen
    {
        private readonly IEventAggregator _events;
        private readonly IMapper _mapper;
        private readonly IApiHelper _apiHelper;

        #region Properties

        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    NotifyOfPropertyChange(() => SearchText);
                    SearchKeys();
                }
            }
        }

        private List<KeyDisplayModel> _keys;

        public List<KeyDisplayModel> Keys
        {
            get => _keys;
            set
            {
                _keys = value;
                NotifyOfPropertyChange(() => Keys);
            }
        }

        private List<CategoryDisplayModel> _categories;

        public List<CategoryDisplayModel> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                NotifyOfPropertyChange(() => Categories);
            }
        }

        private CategoryDisplayModel _category;

        public CategoryDisplayModel SelectedCategory
        {
            get => _category;
            set
            {
                if (_category != value)
                {
                    _category = value;

                    if (_category != null)
                    {
                        SelectedSystemCategory = null;
                    }

                    NotifyOfPropertyChange(() => SelectedCategory);
                    FilterDisplayedKeys();
                }
            }
        }

        private List<CategoryDisplayModel> _systemCategories;

        public List<CategoryDisplayModel> SystemCategories
        {
            get => _systemCategories;
            set
            {
                _systemCategories = value;
                NotifyOfPropertyChange(() => SystemCategories);
            }
        }

        private CategoryDisplayModel _systemCategory;

        public CategoryDisplayModel SelectedSystemCategory
        {
            get => _systemCategory;
            set
            {
                if (_systemCategory != value)
                {
                    _systemCategory = value;

                    if (_systemCategory != null)
                    {
                        SelectedCategory = null;
                    }

                    NotifyOfPropertyChange(() => SelectedSystemCategory);
                    FilterDisplayedKeys();
                }
            }
        }

        private List<KeyDisplayModel> _displayKeys;

        public List<KeyDisplayModel> DisplayKeys
        {
            get => _displayKeys;
            set
            {
                _displayKeys = value;
                NotifyOfPropertyChange(() => DisplayKeys);
            }
        }

        public List<string> Sorts => new List<string>()
                {
                    "Alphabetical",
                    "Date Accessed",
                    "Date Modified"
                };

        private string _selectedSort;

        public string SelectedSort
        {
            get => _selectedSort;
            set
            {
                _selectedSort = value;
                NotifyOfPropertyChange(() => SelectedSort);
                SortDisplayedKeys();
            }
        }

     

        #endregion Properties

        public KeysViewModel(IEventAggregator events, IMapper mapper, IApiHelper apiHelper)
        {
            //Setup
            _events = events;
            _mapper = mapper;
            _apiHelper = apiHelper;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadData();
        }

        #region Filter Data

        private void SearchKeys()
        {
            if (SelectedSystemCategory?.Id != 0 && string.IsNullOrEmpty(SearchText) == false)
            {
                SelectedSystemCategory = SystemCategories.First();
            }

            DisplayKeys = string.IsNullOrEmpty(SearchText) ? Keys.ToList() : Keys.Where(x => x.Username.ToLower().Contains(SearchText) || x.Title.ToLower().Contains(SearchText)).ToList();
        }

        private void FilterDisplayedKeys()
        {
            if (SelectedSystemCategory?.Id != 0)
            {
                SearchText = null;
            }

            if (SelectedCategory != null)
            {
                DisplayKeys = Keys.Where(x => x.CategoryId == SelectedCategory?.Id).ToList();
            }
            else
            {
                if (SelectedSystemCategory == null)
                    SelectedSystemCategory = SystemCategories.First();

                if (SelectedSystemCategory.Id == 0)
                {
                    DisplayKeys = Keys.ToList();
                }
                else if (SelectedSystemCategory.Id == 1)
                {
                    DisplayKeys = Keys.Where(x => DateTime.Now.Subtract(x.DateAccessed).Days <= 15).ToList();
                }
                else if (SelectedSystemCategory.Id == 2)
                {
                    DisplayKeys = Keys.Where(x => x.PasswordGood == false).ToList();
                }
            }
        }

        private void SortDisplayedKeys()
        {
            if (SelectedSort == "Alphabetical")
            {
                Keys = Keys.OrderBy(x => x.Title).ToList();
                DisplayKeys = DisplayKeys?.OrderBy(x => x.Title).ToList();
            }
            else if (SelectedSort == "Date Accessed")
            {
                Keys = Keys.OrderByDescending(x => x.DateAccessed).ToList();
                DisplayKeys = DisplayKeys?.OrderByDescending(x => x.DateAccessed).ToList();
            }
            else if (SelectedSort == "Date Modified")
            {
                Keys = Keys.OrderByDescending(x => x.DateModified).ToList();
                DisplayKeys = DisplayKeys?.OrderByDescending(x => x.DateModified).ToList();
            }
        }

        #endregion Filter Data

        #region Load Data

        private async Task LoadData()
        {
            //Get data
            var keyList = await Task.Run(() => _apiHelper.GetKeys());
            var catList = await Task.Run(() => _apiHelper.GetCategories());

            //Map
            List<KeyDisplayModel> keys = _mapper.Map<List<KeyDisplayModel>>(keyList);
            List<CategoryDisplayModel> cats = _mapper.Map<List<CategoryDisplayModel>>(catList);

            //Assign
            Keys = new List<KeyDisplayModel>(keys);
            Categories = new List<CategoryDisplayModel>(cats);

            SystemCategories = new List<CategoryDisplayModel>()
            {
               new CategoryDisplayModel()
               {
                   Name = "All",
                   Id = 0
               },
               new CategoryDisplayModel()
               {
                   Name = "Recent",
                   Id = 1
               },
               new CategoryDisplayModel()
               {
                   Name = "Should Update",
                   Id = 2
               }
            };

            //Assign counts
            AssignCategoryCounts();

            //Select
            SelectedSort = Sorts.First();
            SelectedSystemCategory = SystemCategories.First();
        }

        private void AssignCategoryCounts()
        {
            foreach (CategoryDisplayModel cat in Categories)
            {
                cat.CategoryCount = ToCatLabel(Keys.Where(x => x?.CategoryId == cat?.Id)?.Count() ?? 0);
            }

            SystemCategories[0].CategoryCount = ToCatLabel(Keys.Count());
            SystemCategories[1].CategoryCount = ToCatLabel(Keys.Where(x => DateTime.Now.Subtract(x.DateAccessed).Days <= 15).Count());
            SystemCategories[2].CategoryCount = ToCatLabel(Keys.Where(x => x.PasswordGood == false).Count());
        }

        private string ToCatLabel(int count)
        {
            return count > 999 ? "99+" : count.ToString();
        }

        #endregion Load Data

        #region Events

        public async void Open(KeyDisplayModel key)
        {
            await _events.PublishOnUIThreadAsync(new OpenPasswordEditorEvent(key));
        }

        public void Copy(KeyDisplayModel key, RoutedEventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(key.Password);

            key.DateAccessed = DateTime.Now;
            _apiHelper.SetKeyAccessDate(key.Id);

            e.Handled = true;
        }

        public async void AddKey()
        {
            await _events.PublishOnUIThreadAsync(new OpenPasswordEditorEvent(null));
        }

        public async void AddCategory()
        {
            await _events.PublishOnUIThreadAsync(new OpenCategoryEditorEvent());
        }

        public async void Logout()
        {
            await _events.PublishOnUIThreadAsync(new LogoutEvent());
        }

        public async void OpenSettings()
        {
            await _events.PublishOnUIThreadAsync(new OpenSettingsEvent());
        }

        #endregion Events
    }
}