using SafeKeys.Library.Helpers;
using SafeKeys.Library.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Resources;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SafeKeys.Library.API
{
    public class ApiHelper : IApiHelper
    {
        private ActiveAccountModel account;
        private readonly ICrypto crypto;
        private readonly IDataAccess dataAccess;
        private readonly IGenerateStrings generateStrings;
        private SaveFileModel saveFile;

        public ApiHelper( ICrypto crypto, IDataAccess dataAccess, IGenerateStrings generateStrings)
        {            
            this.crypto = crypto;
            this.dataAccess = dataAccess;
            this.generateStrings = generateStrings;
        }

        public bool Login(string password, string filePath)
        {
            saveFile = dataAccess.Get(filePath);

            if (saveFile == null)
            {
                return false;
            }

            if (crypto.VerifyAuthHash(password, saveFile.Auth))
            {
                account = new ActiveAccountModel
                {
                    Key = crypto.GenerateCrptoKey(password, saveFile.CryptoSalt),
                    Timestamp = DateTime.Now,
                    FilePath = filePath
                };

                //Decrypt 
                DecryptKeys();

                return true;
            }

            saveFile = new SaveFileModel();
            return false;
        }

        public void CreateAccount(string filePath, string password)
        {
            var newFile = new SaveFileModel(crypto.GenerateAuthHash(password), crypto.GenerateSalt());

            dataAccess.Save(newFile, filePath);
        }

        public void Logout()
        {     
            if(account?.FilePath != null)
            {
                EncryptKeys();

                dataAccess.Save(saveFile, account.FilePath);

                account.Logout();

                saveFile = new SaveFileModel();
            }            
        }

       

        private void DecryptKeys()
        {
            foreach(KeyModel c in saveFile.Keys)
            {
                c.Username = crypto.Decrypt(c.Username, account.Key, c.IV);
                c.Title = crypto.Decrypt(c.Title, account.Key, c.IV);
                c.Password = crypto.Decrypt(c.Password, account.Key, c.IV);
            }
        }        

        private void EncryptKeys()
        {
            foreach (KeyModel c in saveFile.Keys)
            {
                string[] value = crypto.Encrypt(c.Username, account.Key);
                
                c.Username = value[0];
                c.IV = value[1];

                c.Title = crypto.Encrypt(c.Title, account.Key, c.IV)[0];
                c.Password = crypto.Encrypt(c.Password, account.Key, c.IV)[0];
            }
        }

        public void ImportKeys(string filePath)
        {
            foreach (string line in File.ReadAllLines(filePath))
            {
                string[] data = line.Split("\",\""); 

                if (data.Length < 7)
                    continue;

                //remove first quotation and last

                data[0] = data[0].Substring(1);
                data[6] = data[6][0..^2];

                var key = new KeyModel()
                {
                    Id = Convert.ToInt32(data[0]),
                    Title = data[1],
                    Username = data[2],
                    Password = data[3],
                    DateCreated = Convert.ToDateTime(data[4]),
                    DateModified = Convert.ToDateTime(data[5]),
                    DateAccessed = Convert.ToDateTime(data[6]),
                    CategoryId = -1
                };

                key.DatePasswordChanged = key.DateModified;

                saveFile.Keys.Add(key);
            }
        }

      
        #region Boring CRUD operations

        public int CreateKey(KeyModel key)
        {
            key.DatePasswordChanged = DateTime.Now;
            key.DateCreated = DateTime.Now;
            key.DateAccessed = DateTime.Now;
            key.DateModified = DateTime.Now;

            key.Id = saveFile.Keys.Sum(x => x.Id) + 1;

            saveFile.Keys.Add(key);


            return key.Id;
        }

        public void UpdateKey(KeyModel updatedKey)
        {
            KeyModel key = saveFile.Keys.Single(x => x.Id == updatedKey.Id);

            if(key.Password != updatedKey.Password)
            {
                key.DatePasswordChanged = DateTime.Now;
            }

            key.DateModified = DateTime.Now;
            key.Password = updatedKey.Password;
            key.Title = updatedKey.Title;
            key.Username = updatedKey.Username;
            key.CategoryId = updatedKey.CategoryId;
        }

        public void SetKeyAccessDate(int id)
        {
            KeyModel key = saveFile.Keys.Single(x => x.Id == id);
            key.DateAccessed = DateTime.Now;
        }

        public void RemoveKey(int id)
        {
            _ = saveFile.Keys.Remove(saveFile.Keys.Single(x => x.Id == id));
        }

        public void CreateCategory(CategoryModel category)
        {
            category.Id = saveFile.Categories.Sum(x => x.Id) + 1;

            saveFile.Categories.Add(category);           
        }

        public void UpdateCategory(CategoryModel updatedCategory)
        {
            CategoryModel category = saveFile.Categories.Single(x => x.Id == updatedCategory.Id);

            category.Name = updatedCategory.Name;

        }

        public void RemoveCategory(int id)
        {
            _ = saveFile.Categories.Remove(saveFile.Categories.FirstOrDefault(x => x.Id == id));
        }

        public List<KeyModel> GetKeys()
        {
            return saveFile.Keys;
        }

        public List<CategoryModel> GetCategories()
        {
            return saveFile.Categories;
        }

        public string GenUsername() 
        { 
            return generateStrings.Generate(saveFile.UsernameGenPref); 
        }

        public string GenPassword() { return generateStrings.Generate(saveFile.PasswordGenPref);  }

        public void UpdatePasswordGenPref(StringGenerationModel pref)
        {
            saveFile.PasswordGenPref = pref;
        }

        public void UpdateUsernameGenPref(StringGenerationModel pref)
        {
            saveFile.UsernameGenPref = pref;
        }

        public StringGenerationModel GetPasswordGenPref()
        {
            return saveFile.PasswordGenPref;
        }

        public StringGenerationModel GetUsernameGenPref()
        {
            return saveFile.UsernameGenPref;
        }

        #endregion
    }
}
