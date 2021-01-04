using Caliburn.Micro;
using SafeKeys.EventModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SafeKeys.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        
        readonly DispatcherTimer t;
        int counter;

        public ShellView()
        {
            InitializeComponent();

            t = new DispatcherTimer();
            t.Tick += T_Tick;
            t.Interval = new TimeSpan(0, 0, 1);

            t.Start();
          
        }

        private async void T_Tick(object sender, EventArgs e)
        {
            if (++counter >= 1800)
            {
                IEventAggregator events = IoC.Get<IEventAggregator>();

                await events.PublishOnUIThreadAsync(new SaveCurrentEvent());
                await events.PublishOnUIThreadAsync(new LogoutEvent());
                counter = 0;
            }       
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            counter = 0;
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            IEventAggregator events = IoC.Get<IEventAggregator>();

           
            await events.PublishOnUIThreadAsync(new LogoutEvent());

           


        }
    }
}
