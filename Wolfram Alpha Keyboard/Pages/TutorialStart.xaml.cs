using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Wolfram_Alpha_Keyboard.Pages
{
    public partial class TutorialStart
    {
        public TutorialStart()
        {
            InitializeComponent();
        }

        private void Button_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml?parameter=1", UriKind.Relative));
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            string parameter;
            if (!NavigationContext.QueryString.TryGetValue("parameter", out parameter))
            {
                Application.Current.Terminate();
            }
        }
    }
}