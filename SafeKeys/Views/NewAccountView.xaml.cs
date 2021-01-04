using SafeKeys.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SafeKeys.Views
{
    /// <summary>
    /// Interaction logic for NewAccountView.xaml
    /// </summary>
    public partial class NewAccountView : UserControl
    {
        public NewAccountView()
        {
            InitializeComponent();
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((NewAccountViewModel)DataContext).Password = ((PasswordBox)sender).SecurePassword;
            }
        }

        private void ConfirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((NewAccountViewModel)DataContext).ConfirmPassword = ((PasswordBox)sender).SecurePassword;
            }
        }
    }
}
