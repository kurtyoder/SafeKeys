using SafeKeys.Library.Models;
using System.Collections.Generic;

namespace SafeKeys.Library.API
{
    public interface IApiHelper
    {
        string GenPassword();
        string GenUsername();

        void CreateAccount(string filePath, string password);
        void CreateCategory(CategoryModel category);
        int CreateKey(KeyModel key);
        List<CategoryModel> GetCategories();
        List<KeyModel> GetKeys();
        bool Login(string password, string filePath);
        void Logout();
        void RemoveCategory(int id);
        void RemoveKey(int id);
        void UpdateCategory(CategoryModel updatedCategory);
        void UpdateKey(KeyModel updatedKey);
        void UpdatePasswordGenPref(StringGenerationModel pref);
        void UpdateUsernameGenPref(StringGenerationModel pref);
        StringGenerationModel GetUsernameGenPref();
        StringGenerationModel GetPasswordGenPref();
        void SetKeyAccessDate(int id);
        void ImportKeys(string filePath);
    }
}