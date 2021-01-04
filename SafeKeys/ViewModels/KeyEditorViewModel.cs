using Caliburn.Micro;
using SafeKeys.EventModels;
using SafeKeys.Library;
using SafeKeys.Library.API;
using SafeKeys.Library.Models;
using SafeKeys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Threading;

namespace SafeKeys.ViewModels
{
    public class KeyEditorViewModel : Screen, IHandle<SaveCurrentEvent>
    {
        #region Properties
        
        private string _title;

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                NotifyOfPropertyChange(() => Title);
                NotifyOfPropertyChange(() => CanSave);
            }
        }

        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanSave);
            }
        }

        private string _username;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                NotifyOfPropertyChange(() => Username);
                NotifyOfPropertyChange(() => CanSave);
            }
        }

        private bool _passwordGood;

        public bool PasswordGood
        {
            get => _passwordGood;
            set
            {
                _passwordGood = value;
                NotifyOfPropertyChange(() => PasswordGood);
            }
        }

        private string _dateCreated;

        public string DateCreated
        {
            get => "Key Created " + _dateCreated;
            set 
            { 
                _dateCreated = value;
                NotifyOfPropertyChange(() => DateCreated);
            
            }
        }

        private string _datePasswordChanged;

        public string DatePasswordChanged
        {
            get => "Password Last Changed " + _datePasswordChanged;
            set 
            { 
                _datePasswordChanged = value;
                NotifyOfPropertyChange(() => DatePasswordChanged);
            }
        }




        private List<CategoryModel> _categories;

        public List<CategoryModel> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                NotifyOfPropertyChange(() => Categories);
            }
        }

        private CategoryModel _selectedCategory;

        public CategoryModel SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                NotifyOfPropertyChange(() => SelectedCategory);
            }
        }


        public bool IsEditing => _key != null;

        private bool deleteClick = false;

        public string DeleteButtonText => deleteClick ? "Click again to confirm" : "Delete";


        public bool HasCategories => (Categories?.Count() ?? 0) > 0;

        #endregion Properties

        private readonly IEventAggregator events;
        private readonly IApiHelper apiHelper;
        private readonly DispatcherTimer timer;

        private bool isActive = false;


        public KeyEditorViewModel(IEventAggregator events, IApiHelper apiHelper)
        {
            this.events = events;
            this.apiHelper = apiHelper;

            events.SubscribeOnPublishedThread(this);

            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 5)
            };

            timer.Tick += Timer_Tick;

            isActive = true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            deleteClick = false;
            NotifyOfPropertyChange(() => DeleteButtonText);
            timer.Stop();
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadData();
        }

        private async Task LoadData()
        {
            //Get data

            var catList = await Task.Run(() => apiHelper.GetCategories());

            //Assign
            Categories = new List<CategoryModel>(catList);

            SelectedCategory = Categories.SingleOrDefault(x => x?.Id == _key?.CategoryId);


            NotifyOfPropertyChange(() => HasCategories);
        }

        private KeyDisplayModel _key;

        public void LoadKey(KeyDisplayModel key)
        {
            Username = key?.Username;
            Title = key?.Title;
            Password = key?.Password;
            PasswordGood = key?.PasswordGood ?? false;
            DatePasswordChanged = key?.DatePasswordChanged.ToShortDateString();
            DateCreated = key?.DateCreated.ToShortDateString();

            _key = key;

            if(_key != null)
            {
                apiHelper.SetKeyAccessDate(_key.Id);
            }

            NotifyOfPropertyChange(() => IsEditing);
        }

        public void GeneratePassword()
        {
            Password = apiHelper.GenPassword();
        }

        public void GenerateUsername()
        {
            Username = apiHelper.GenUsername();
        }

        public void CopyPassword()
        {
            Clipboard.Clear();
            Clipboard.SetText(Password);
        }

        public void CopyUsername()
        {
            Clipboard.Clear();
            Clipboard.SetText(Username);
        }

        public async void Return()
        {
            await events.PublishOnUIThreadAsync(new OpenPasswordsViewEvent());
        }

        public bool CanSave => Password?.Length > 0 && Title?.Length > 0 && Username?.Length > 0;
        

        public async void Save(bool noNav)
        {
            var k = new KeyModel()
            {
                Password = Password,
                Username = Username,
                Title = Title,
                CategoryId = SelectedCategory?.Id ?? -1,               

            };

            if (_key == null)
            {
                k.Id = apiHelper.CreateKey(k); 
            }
            else
            {
                k.Id = _key.Id;
                apiHelper.UpdateKey(k);
            }

            if(!noNav)
            {
                await events.PublishOnUIThreadAsync(new OpenPasswordsViewEvent());
            }

            isActive = false;
           
        }

        public async void Delete()
        {
            if(deleteClick)
            {
                apiHelper.RemoveKey(_key.Id);
                await events.PublishOnUIThreadAsync(new OpenPasswordsViewEvent());

                isActive = false;
            }
            else
            {
                System.Windows.Forms.Cursor.Position = new System.Drawing.Point(0, 0);

                deleteClick = true;
                NotifyOfPropertyChange(() => DeleteButtonText);
                timer.Start();
            }

            

           
        }

        public async Task HandleAsync(SaveCurrentEvent message, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(Password) && isActive)
            {
                if (string.IsNullOrEmpty(Username)) 
                    Username = "None";

                if (string.IsNullOrEmpty(Title))
                    Title = "None";

                await Task.Run(() => Save(true));              
            }
            else
            {
                await Task.CompletedTask;
            }
                    

        }
    }
}