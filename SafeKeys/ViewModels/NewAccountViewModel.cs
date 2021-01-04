using Caliburn.Micro;
using SafeKeys.EventModels;
using SafeKeys.Library.API;
using SafeKeys.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SafeKeys.ViewModels
{
    public class NewAccountViewModel : Screen
    {
        private readonly IApiHelper apiHelper;
        private readonly IEventAggregator eventAggregator;
        private readonly ISettingHandler settingHandler;
        private SecureString confirmPassword;
        private SecureString password;

        private string fullPath;
        public string FullPath
        {
            get => fullPath;
            set
            {
                fullPath = value;
                NotifyOfPropertyChange(() => DisplayPath);
                NotifyOfPropertyChange(() => CanCreateAccount);
            }
        }

        public string DisplayPath

        {
            get
            {
                if (string.IsNullOrEmpty(FullPath))
                {
                    return "Browse to choose file location";
                }
                else
                {
                    return FullPath.Length > 25 ? "..." + FullPath.Substring(FullPath.Length - 25) : FullPath;
                }
            }
        }



        public SecureString Password
        {
            get => password;
            set { password = value; NotifyOfPropertyChange(() => CanCreateAccount); }
        }
        public SecureString ConfirmPassword
        {
            get => confirmPassword;
            set { confirmPassword = value; NotifyOfPropertyChange(() => CanCreateAccount); }
        }

        private bool _createBusy;

        public bool CreateBusy
        {
            get => _createBusy;
            set
            {
                _createBusy = value;
                NotifyOfPropertyChange(() => CreateBusy);
            }
        }


        public bool CanCreateAccount
        {
            get
            {
                NotifyOfPropertyChange(() => ErrorMessage);

                return password.Unsecure() == confirmPassword.Unsecure() &&
                       (password?.Length ?? 0) > 12 &&
                       string.IsNullOrWhiteSpace(FullPath) == false;
            }
        }


        public string ErrorMessage
        {
            get
            {
                string message = string.Empty;

                if (password.Unsecure() != confirmPassword.Unsecure())
                    message += "Passwords do not match\n";

                if ((password?.Length ?? 0) <= 12)
                    message += "Password must be longer than twelve characters\n";

                if (string.IsNullOrWhiteSpace(FullPath))
                    message += "Choose a valid file path for database\n";

                return message.Trim();
            }
        }



        public NewAccountViewModel(IApiHelper apiHelper, IEventAggregator eventAggregator, ISettingHandler settingHandler)
        {
            this.apiHelper = apiHelper;
            this.eventAggregator = eventAggregator;
            this.settingHandler = settingHandler;
        }

        public void Browse()
        {
            using var o = new System.Windows.Forms.SaveFileDialog
            {
                Filter = "KEY database file (*.key)|*.key",
                DefaultExt = ".key",
                Title = "Browse to save KEY file",
                AddExtension = true
            };

            if (o.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FullPath = o.FileName;
            }
        }

        public async void CreateAccount()
        {
            CreateBusy = true;

            await Task.Run(() => apiHelper.CreateAccount(FullPath, Password.Unsecure()));

            await Task.Run(() => settingHandler.SavePath(FullPath));

            await eventAggregator.PublishOnUIThreadAsync(new OpenLoginEvent());
        }

        public async void Return()
        {
            await eventAggregator.PublishOnUIThreadAsync(new OpenLoginEvent());
        }

    }
}
