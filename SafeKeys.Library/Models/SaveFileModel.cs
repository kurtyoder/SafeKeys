using System;
using System.Collections.Generic;
using System.Text;

namespace SafeKeys.Library.Models
{
    public class SaveFileModel
    {

        public SaveFileModel()
        {

        }

        public SaveFileModel(string auth, byte[] cryptoSalt)
        {
            Auth = auth;
            CryptoSalt = cryptoSalt;

            UsernameGenPref = new StringGenerationModel(1, 1, 0, 0, 0, 0, 1, 0, true, 0);
            PasswordGenPref = new StringGenerationModel(0, 0, 0, 0, 2, 1, 4, 4, true, 0);
        }

        public string Auth { get; set; }
        public byte[] CryptoSalt { get; set; }
        public StringGenerationModel UsernameGenPref { get; set; }
        public StringGenerationModel PasswordGenPref { get; set; }
        public List<KeyModel> Keys { get; set; }
        public List<CategoryModel> Categories { get; set; }
    }
}
