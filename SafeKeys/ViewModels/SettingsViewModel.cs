using Caliburn.Micro;
using Microsoft.Win32;
using SafeKeys.EventModels;
using SafeKeys.Library.API;
using SafeKeys.Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace SafeKeys.ViewModels
{
    public class SettingsViewModel : Screen
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IApiHelper apiHelper;
        private int userAdj;
        private int userName;
        private int userWord;
        private int userBigWord;
        private int userRegChar;
        private int userIrregChar;
        private int userLetter;
        private int userNumber;
        private int userFakeWord;
        private int passwordAdj;
        private int passwordName;
        private int passwordWord;
        private int passwordBigWord;
        private int passwordRegChar;
        private int passwordIrregChar;
        private int passwordLetter;
        private int passwordNumber;
        private int passwordFakeWord;

        public int UserAdj { 
            get => userAdj;
            set
            {
                userAdj = value;
                NotifyOfPropertyChange(() => UserAdj);
            }
        }
        public int UserName { 
            get => userName;
            set 
            { 
                userName = value;
                NotifyOfPropertyChange(() => UserName);
            } 
        }
        public int UserWord { get => userWord; set
            {
                userWord = value;
                NotifyOfPropertyChange(() => UserWord);
            }
        }
        public int UserBigWord { get => userBigWord; set
            {
                userBigWord = value;
                NotifyOfPropertyChange(() => UserBigWord);
            }
        }
        public int UserRegChar { get => userRegChar; set
            {
                userRegChar = value;
                NotifyOfPropertyChange(() => UserRegChar);
            }
        }
        public int UserIrregChar { get => userIrregChar; set
            {
                userIrregChar = value;
                NotifyOfPropertyChange(() => UserIrregChar);
            }
        }
        public int UserLetter { get => userLetter; set
            {
                userLetter = value;
                NotifyOfPropertyChange(() => UserLetter);
            }
        }
        public int UserNumber { get => userNumber; set
            {
                userNumber = value;
                NotifyOfPropertyChange(() => UserNumber);
            }
        }
        public int UserFakeWord { get => userFakeWord; set
            {
                userFakeWord = value;
                NotifyOfPropertyChange(() => UserFakeWord);
            }
        }

        public int PasswordAdj { get => passwordAdj; set
            {
                passwordAdj = value;
                NotifyOfPropertyChange(() => PasswordAdj);
            }
        }
        public int PasswordName { get => passwordName; set
            {
                passwordName = value;
                NotifyOfPropertyChange(() => PasswordName);
            }
        }
        public int PasswordWord { get => passwordWord; set
            {
                passwordWord = value;
                NotifyOfPropertyChange(() => PasswordWord);
            }
        }
        public int PasswordBigWord { get => passwordBigWord; set
            {
                passwordBigWord = value;
                NotifyOfPropertyChange(() => PasswordBigWord);
            }
        }
        public int PasswordRegChar { get => passwordRegChar; set
            {
                passwordRegChar = value;
                NotifyOfPropertyChange(() => PasswordRegChar);
            }
        }
        public int PasswordIrregChar { get => passwordIrregChar; set
            {
                passwordIrregChar = value;
                NotifyOfPropertyChange(() => PasswordIrregChar);
            }
        }
        public int PasswordLetter { get => passwordLetter; set
            {
                passwordLetter = value;
                NotifyOfPropertyChange(() => PasswordLetter);
            }
        }
        public int PasswordNumber { get => passwordNumber; set
            {
                passwordNumber = value;
                NotifyOfPropertyChange(() => PasswordNumber);
            }
        }
        public int PasswordFakeWord { get => passwordFakeWord; set
            {
                passwordFakeWord = value;
                NotifyOfPropertyChange(() => PasswordFakeWord);
            }
        }


        public SettingsViewModel(IEventAggregator eventAggregator, IApiHelper apiHelper)
        {
            this.eventAggregator = eventAggregator;
            this.apiHelper = apiHelper;

            StringGenerationModel username = apiHelper.GetUsernameGenPref();

            UserAdj = username.Adj;
            UserName = username.Name;
            UserWord = username.Word;
            UserBigWord = username.BigWord;
            UserRegChar = username.RegChar;
            UserIrregChar = username.IrregChar;
            UserLetter = username.Letter;
            UserNumber = username.Number;
            UserFakeWord = username.FakeWord;

            StringGenerationModel password = apiHelper.GetPasswordGenPref();

            PasswordAdj = password.Adj;
            PasswordName = password.Name;
            PasswordWord = password.Word;
            PasswordBigWord = password.BigWord;
            PasswordRegChar = password.RegChar;
            PasswordIrregChar = password.IrregChar;
            PasswordLetter = password.Letter;
            PasswordNumber = password.Number;
            PasswordFakeWord = password.FakeWord;

        }

        public async void Return()
        {
            await eventAggregator.PublishOnUIThreadAsync(new OpenPasswordsViewEvent());
        }

        public async void Save()
        {
            var username = new StringGenerationModel(UserAdj, UserName, UserWord, UserBigWord, UserRegChar, UserIrregChar, UserLetter, UserNumber, true, UserFakeWord);

            var password = new StringGenerationModel(PasswordAdj, PasswordName, PasswordWord, PasswordBigWord, PasswordRegChar, PasswordIrregChar, PasswordLetter, PasswordNumber, true, PasswordFakeWord);

            apiHelper.UpdatePasswordGenPref(password);
            apiHelper.UpdateUsernameGenPref(username);


            await eventAggregator.PublishOnUIThreadAsync(new OpenPasswordsViewEvent());
        }

        public void Import()
        {
            using (var p = new System.Windows.Forms.OpenFileDialog())
            {
                p.Filter = "CSV File (*.csv)|*.csv";
                p.DefaultExt = ".csv";
                p.AddExtension = true;

                if(p.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    apiHelper.ImportKeys(p.FileName);
                }
            }
        }

       

    }
}
