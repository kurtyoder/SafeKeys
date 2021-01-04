using Caliburn.Micro;
using SafeKeys.EventModels;
using SafeKeys.Library.API;
using System;
using System.Collections.Generic;
using System.Text;
using SafeKeys.Library.Helpers;
using System.Security;
using System.Resources;
using SafeKeys.Library.Models;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using SafeKeys.Commands;

namespace SafeKeys.ViewModels
{
    public class LoginViewModel : Screen
    {
        private readonly IEventAggregator events;
        private readonly IApiHelper apiHelper;
        private readonly ISettingHandler settingHandler;
        private int tryCount = 0;

        public string FullPath 
        { 
            get => fullPath;
            set
            {              
                    fullPath = value;
               

                
                NotifyOfPropertyChange(() => DisplayPath);
            }        
        }

        public string DisplayPath

        {
            get
            {
                if(string.IsNullOrEmpty(FullPath))
                {
                    return "Select a file or create a new one";
                }
                else
                {
                    return FullPath.Length > 25 ? "..." + FullPath.Substring(FullPath.Length - 25) : FullPath;
                }
            }
        }

        public bool Message => FailedLogin != "";

        private string _failedLogin = "";
        private string fullPath;

        public string FailedLogin
        {
            get => _failedLogin;
            set
            {
                _failedLogin = value;
                NotifyOfPropertyChange(() => FailedLogin);
                NotifyOfPropertyChange(() => Message);
            }
        }
        public bool CanLogin => (Password?.Length ?? 0) > 0 && tryCount < 5 && string.IsNullOrEmpty(fullPath) == false;

        private SecureString _password;
        public SecureString Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => CanLogin);
                if (tryCount < 5)
                {
                    FailedLogin = "";
                }
            }
        }

        private bool _loginBusy;

        public bool LoginBusy
        {
            get => _loginBusy;
            set
            {
                _loginBusy = value;
                NotifyOfPropertyChange(() => LoginBusy);
            }
        }

        public ICommand LoginCommand { get; set; }

        public LoginViewModel(IEventAggregator events, IApiHelper apiHelper, ISettingHandler settingHandler )
        {
            this.events = events;
            this.apiHelper = apiHelper;
            this.settingHandler = settingHandler;

            string path = settingHandler.GetPath();

            if(File.Exists(path))
            {
                FullPath = path;
            }

            LoginCommand = new RelayCommand(() => Login());
        }


      
        public async void Login()
        {
            tryCount++;
            NotifyOfPropertyChange(() => CanLogin);

            if (!CanLogin)
            {
                FailedLogin = "You are locked from the account";
                return;
            }

            LoginBusy = true;
            FailedLogin = "";

            if (await Task.Run(() => apiHelper.Login(Password.Unsecure(), FullPath)))
            {
                await events.PublishOnUIThreadAsync(new OpenPasswordsViewEvent());
            }
            else
            {
                FailedLogin = "Incorrect Password";
            }

            LoginBusy = false;


        }

        public void SwitchFile()
        {
            using var o = new System.Windows.Forms.OpenFileDialog
            {
                Filter = "KEY database file (*.key)|*.key",
                DefaultExt = ".key",
                Title = "Browse to open KEY file",
                AddExtension = true,
                Multiselect = false
            };

            if (o.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FullPath = o.FileName;
                settingHandler.SavePath(FullPath);
            }
        }

        public async void CreateFile()
        {
            await events.PublishOnUIThreadAsync(new OpenAccountEditorEvent());
        }
    }
}