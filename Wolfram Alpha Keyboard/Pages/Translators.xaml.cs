using System;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Wolfram_Alpha_Keyboard.Pages
{
    public partial class Translators : PhoneApplicationPage
    {
        public Translators()
        {
            InitializeComponent();
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

        private void LumiaESWordpress_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask {Uri = new Uri("http://www.nokialumia.es/")};
            task.Show();
        }

        private void LumiaESTwitter_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask {Uri = new Uri("https://twitter.com/NokiaLumiaES")};
            task.Show();
        }

        private void LumiaESFacebook_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask {Uri = new Uri("https://www.facebook.com/NokiaLumiaEs")};
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
    }
}