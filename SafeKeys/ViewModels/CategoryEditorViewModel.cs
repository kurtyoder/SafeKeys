using Caliburn.Micro;
using SafeKeys.EventModels;
using SafeKeys.Library;
using SafeKeys.Library.API;
using SafeKeys.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeKeys.ViewModels
{
    public class CategoryEditorViewModel : Screen
    {

        #region Properties
        private BindingList<CategoryModel> _categories;

        public BindingList<CategoryModel> Categories
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

                EditingCategory = value != null;
                CategoryName = value != null ? _selectedCategory.Name : string.Empty;                
            }
        }

        private string _categoryName;

        public string CategoryName
        {
            get => _categoryName;
            set 
            { 
                _categoryName = value;
                NotifyOfPropertyChange(() => CategoryName);
                NotifyOfPropertyChange(() => CanSave);
            }
        }




        private bool _editingCategory;

        public bool EditingCategory
        {
            get => _editingCategory;
            set 
            { 
                _editingCategory = value;
                NotifyOfPropertyChange(() => EditingCategory);
            }
        }

        public bool CanSave => !string.IsNullOrWhiteSpace(CategoryName);
     


        #endregion

        private readonly IEventAggregator events;
        private readonly IApiHelper apiHelper;


        public CategoryEditorViewModel(IEventAggregator events, IApiHelper apiHelper)
        {
            this.events = events;
            this.apiHelper = apiHelper;

            //Get data
            var catList = apiHelper.GetCategories();
            //Assign

            Categories = new BindingList<CategoryModel>(catList.ToList());

        }

        #region Methods
        /// <summary>
        /// Return to <see cref="KeysViewModel"/>
        /// </summary>
        public async void Return()
        {
            await events.PublishOnUIThreadAsync(new OpenPasswordsViewEvent());
        }

        /// <summary>
        /// Clear selected category to add new category
        /// </summary>
        public void AddCategory()
        {
            SelectedCategory = null;
        }

        /// <summary>
        /// Add new category or update existing one
        /// </summary>
        public void Save()
        {
            if (SelectedCategory == null)
            {
                var c = new CategoryModel() { Name = CategoryName };
                apiHelper.CreateCategory(c);
                Categories.Add(c);      
            }
            else
            {                
                SelectedCategory.Name = CategoryName;
                Categories.ResetItem(Categories.IndexOf(SelectedCategory));

                apiHelper.UpdateCategory(SelectedCategory);
            }
        }

        /// <summary>
        /// Remove selected category
        /// </summary>
        public void Delete ()
        {
            if(SelectedCategory == null)
            {
                //Really shouldn't be here
                EditingCategory = false;
                return;
            }

            int id = SelectedCategory.Id;

            Categories.Remove(SelectedCategory);

            apiHelper.RemoveCategory(id);           
                 
            SelectedCategory = Categories.FirstOrDefault();     
        }

        #endregion



    }
}
