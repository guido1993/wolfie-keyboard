using System;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Wolfram_Alpha_Keyboard.Pages
{
    public partial class About
    {
        public About()
        {
            InitializeComponent();
        }

        private void CryptoCurrencies_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask
            {
                Uri =
                    new Uri(
                        "http://www.windowsphone.com/en-us/store/app/cryptocurrencies/bc00dfd9-beec-45fa-a12d-e447d2141607")
            };
            task.Show();
        }

        private void JamMe_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask
            {
                Uri = new Uri("http://www.windowsphone.com/en-us/store/app/jamme/cdbcda90-2e99-4edd-8df9-fb2bbbdf711d")
            };
            task.Show();
        }

        private void Red_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask
            {
                Uri = new Uri("http://www.windowsphone.com/it-it/store/app/red/5215bd82-b60a-4edb-9baa-2ee6c0508972")
            };
            task.Show();
        }

        private void Wolfie_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask
            {
                Uri = new Uri("http://www.windowsphone.com/it-it/store/app/wolfie/6ae2e102-a1b1-4f78-8469-3128dfb7b8b4")
            };
            task.Show();
        }

        private void CineSnap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask
            {
                Uri =
                    new Uri("http://www.windowsphone.com/en-us/store/app/cinesnap/38cc9856-d6bc-442a-81f0-e99f433e9e18")
            };
            task.Show();
        }

        private void Blurp_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask
            {
                Uri = new Uri("http://www.windowsphone.com/it-it/store/app/blurp/c156cce8-9b9c-4294-91a1-e0c1c75b7b59")
            };
            task.Show();
        }

        private void WordpressGuido_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask {Uri = new Uri("http://guido1993.wordpress.com/")};
            task.Show();
        }

        private void TwitterGuido_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask {Uri = new Uri("https://twitter.com/GuidoMagrin")};
            task.Show();
        }

        private void FacebookGuido_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask {Uri = new Uri("https://www.facebook.com/guido.magrin")};
            task.Show();
        }

        private void WordpressMarc_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask {Uri = new Uri("http://marcellusamadeus.com/")};
            task.Show();
        }

        private void TwitterMarc_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask {Uri = new Uri("https://twitter.com/marcelluzs")};
            task.Show();
        }

        private void FacebookMarc_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask {Uri = new Uri("https://www.facebook.com/marcellusamadeus")};
            task.Show();
        }

        private void MailGuido_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask
            {
                To = "guido1993@gmail.com",
                Subject = "Wolfie Keyboard 2.0.2.0 - "
            };
            emailComposeTask.Show();
        }

        private void MailMarc_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask
            {
                To = "7marcellus@gmail.com",
                Subject = "Wolfie Keyboard 2.0.2.0 - "
            };
            emailComposeTask.Show();
        }

        private void Translators_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Translators.xaml", UriKind.Relative));
        }
    }
}