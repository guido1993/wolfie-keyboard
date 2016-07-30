using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Wolfram_Alpha_Keyboard.Resources;
using Microsoft.Phone.Tasks;
using System.IO.IsolatedStorage;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Wolfram_Alpha_Keyboard.Model;

namespace Wolfram_Alpha_Keyboard
{
    public partial class MainPage : PhoneApplicationPage
    {
        ObservableCollection<Storico> rigaStorico = new ObservableCollection<Storico>();
        ObservableCollection<Preferiti> rigaPreferiti = new ObservableCollection<Preferiti>();
        ObservableCollection<Curiosità> rigaCuriosità = new ObservableCollection<Curiosità>();

        string _lastUrlSearched = string.Empty;

        public MainPage()
        {
            InitializeComponent();
            BuildApplicationBar();
            //popolo  la lista dello storico
            int i = 0;
            Storico.ItemsSource = rigaStorico;
            IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile("history.txt", FileMode.OpenOrCreate, FileAccess.Read);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string tmp1;
                string tmp2 = string.Empty;
                while (!reader.EndOfStream)
                {
                    tmp1 = reader.ReadLine();
                    if (tmp1 != tmp2)
                    {
                        rigaStorico.Insert(0, new Storico(tmp1));
                        tmp2 = tmp1;
                        i++;
                    }
                }
                if (i == 0)
                {
                    EmptyHistory.Visibility = Visibility.Visible;
                    CancellaStorico.IsEnabled = false;
                }
            }

            //popolo la lista dei preferiti
            i = 0;
            Preferiti.ItemsSource = rigaPreferiti;
            fileStream = myIsolatedStorage.OpenFile("favourites.txt", FileMode.OpenOrCreate, FileAccess.Read);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                rigaPreferiti.Clear(); //svuoto la lista
                //riempio di nuovo la lista con tutti gli elementi
                string tmp1;
                string tmp2 = string.Empty;
                while (!reader.EndOfStream)
                {
                    tmp1 = reader.ReadLine();
                    if (tmp1 != tmp2)
                    {
                        rigaPreferiti.Insert(0, new Preferiti(tmp1));
                        tmp2 = tmp1;
                        i++;
                    }
                }
                if (i == 0)
                {
                    EmptyFavourites.Visibility = Visibility.Visible;
                    CancellaPreferiti.IsEnabled = false;
                }
            }

            //carico il file con le curiosità
            Curiosità.ItemsSource = rigaCuriosità;
            Curiosità.Visibility = Visibility.Visible;
            var resrouceStream = Application.GetResourceStream(new Uri("Resources/funnies.txt", UriKind.Relative));
            if (resrouceStream != null)
            {
                Stream myFileStream = resrouceStream.Stream;
                if (myFileStream.CanRead)
                {
                    StreamReader reader = new StreamReader(myFileStream);
                    while (!reader.EndOfStream)
                    {
                        rigaCuriosità.Add(new Curiosità(reader.ReadLine()));
                    }                    
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
            try
            {                
                myIsolatedStorage.OpenFile("tutorial.txt", FileMode.Open);
            }
            catch (IsolatedStorageException)
            {
                string parameter = string.Empty;
                if (!NavigationContext.QueryString.TryGetValue("parameter", out parameter))
                {
                    try
                    {
                        IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile("tutorial.txt", FileMode.OpenOrCreate, FileAccess.Read);
                        NavigationService.Navigate(new Uri("/Pages/TutorialStart.xaml", UriKind.Relative));
                    }
                    catch (IsolatedStorageException) { }
                }
            }
        }

        private void SetProgressIndicator(bool value)
        {
            SystemTray.ProgressIndicator.IsIndeterminate = value;
            SystemTray.ProgressIndicator.IsVisible = value;
        }

        private void BasicButton_Click(object sender, RoutedEventArgs e)
        {
            int pos = Display.SelectionStart;

            if (Piu.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "+");
            }
            else if (Meno.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "-");
            }
            else if (Per.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "*");
            }
            else if (Diviso.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "/");
            }
            else if (Elevamento.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "^");
            }
            else if (TondaAperta.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "()");
            }
            else if (TondaChiusa.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, ")");
            }
            else if (QuadraAperta.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "[]");
            }
            else if (QuadraChiusa.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "]");
            }
            else if (PiGreco.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "π");
            }
            else if (RadiceQuadrata.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "√()");
                pos++;
            }
            else if (Virgola.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, ",");
            }
            else if (Punto.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, ".");
            }
            else if (Uguale.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "=");
            }

            Display.SelectionStart = pos + 1;
            Display.Focus();
        }

        private void AdvancedButton_Click(object sender, RoutedEventArgs e)
        {
            int pos = Display.SelectionStart;

            if (Percento.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "%");
            }
            else if (Logaritmo.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "log()");
                pos = pos + 3;
            }
            else if (Limite.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "lim(x->)");
                pos = pos + 6;
            }
            else if (Assoluto.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "abs()");
                pos = pos + 3;
            }
            else if (Integrale.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "∫()");
                pos++;
            }
            else if (Infinito.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "∞");
            }
            else if (Sommatoria.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "∑(,)");
                pos++;
            }
            else if (Barra.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "|");
            }
            else if (PuntoEsclamativo.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "!");
            }
            else if (E.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "e");
            }
            else if (Derivata.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "derivative()");
                pos = pos + 10;
            }
            else if (I.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "i");
            }
            else if (ECommerciale.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "&");
            }

            Display.SelectionStart = pos + 1;
            Display.Focus();
        }

        private void StatsTrigonometricButton_Click(object sender, RoutedEventArgs e)
        {
            int pos = Display.SelectionStart;

            if (Seno.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "sin()");
                pos = pos + 3;
            }
            else if (Coseno.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "cos()");
                pos = pos + 3;
            }
            else if (Tangente.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "tan()");
                pos = pos + 3;
            }
            else if (Theta.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "ϑ");
                pos = pos + 1;
            }
            else if (SenoIperbolico.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "sinh()");
                pos = pos + 4;
            }
            else if (CosenoIperbolico.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "cosh()");
                pos = pos + 4;
            }
            else if (nPr.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "nPr(,)");
                pos = pos + 3;
            }
            else if (nCr.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "nCr(,)");
                pos = pos + 3;
            }
            else if (DeviazioneStandard.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "standard deviation(,)");
                pos = pos + 18;
            }
            else if (Media.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "mean(,)");
                pos = pos + 4;
            }
            else if (ChiQuadro.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "χ²()");
                pos = pos + 2;
            }
            else if (Gamma.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "λ");
            }
            else if (Ro.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "ρ");
            }

            Display.SelectionStart = pos + 1;
            Display.Focus();
        }

        private void ChemistryBiology_Click(object sender, RoutedEventArgs e)
        {
            int pos = Display.SelectionStart;

            if (FrecciaSinistra.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "←");
                pos = pos + 3;
            }
            else if (FrecciaDestra.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "→");
                pos = pos + 3;
            }
            else if (DoppiaFreccia.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "⇌");
                pos = pos + 3;
            }
            else if (Armstrong.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "Å");
                pos = pos + 1;
            }
            else if (Alpha.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "α");
                pos = pos + 4;
            }
            else if (Beta.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "β");
                pos = pos + 4;
            }
            else if (GammaMin.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "γ");
                pos = pos + 3;
            }
            else if (Phi.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "Φ");
                pos = pos + 3;
            }
            else if (DeltaMin.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "δ");
                pos = pos + 18;
            }
            else if (DeltaMaius.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "Δ");
                pos = pos + 4;
            }
            else if (Nabla.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "∇");
                pos = pos + 2;
            }
            else if (Ohm.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "Ω");
            }
            else if (Mu.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "µ");
            }
            else if (Tau.IsFocused)
            {
                Display.Text = Display.Text.Insert(Display.SelectionStart, "τ");
            }

            Display.SelectionStart = pos + 1;
            Display.Focus();
        }

        private void Display_GotFocus(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(""); //svuoto la clipboard per rimuovere la fastidiosa barra che occlude i pulsanti

            Piu.Visibility = Visibility.Visible;
            Meno.Visibility = Visibility.Visible;
            Per.Visibility = Visibility.Visible;
            Diviso.Visibility = Visibility.Visible;
            Elevamento.Visibility = Visibility.Visible;
            TondaAperta.Visibility = Visibility.Visible;
            TondaChiusa.Visibility = Visibility.Visible;
            QuadraAperta.Visibility = Visibility.Visible;
            QuadraChiusa.Visibility = Visibility.Visible;
            PiGreco.Visibility = Visibility.Visible;
            RadiceQuadrata.Visibility = Visibility.Visible;
            Virgola.Visibility = Visibility.Visible;
            Punto.Visibility = Visibility.Visible;
            Uguale.Visibility = Visibility.Visible;

            Percento.Visibility = Visibility.Visible;
            ECommerciale.Visibility = Visibility.Visible;
            Barra.Visibility = Visibility.Visible;
            PuntoEsclamativo.Visibility = Visibility.Visible;
            E.Visibility = Visibility.Visible;
            Derivata.Visibility = Visibility.Visible;
            I.Visibility = Visibility.Visible;
            Logaritmo.Visibility = Visibility.Visible;
            Assoluto.Visibility = Visibility.Visible;
            Integrale.Visibility = Visibility.Visible;
            Infinito.Visibility = Visibility.Visible;
            Sommatoria.Visibility = Visibility.Visible;
            Limite.Visibility = Visibility.Visible;

            Seno.Visibility = Visibility.Visible;
            Coseno.Visibility = Visibility.Visible;
            Tangente.Visibility = Visibility.Visible;
            Theta.Visibility = Visibility.Visible;
            SenoIperbolico.Visibility = Visibility.Visible;
            CosenoIperbolico.Visibility = Visibility.Visible;
            nPr.Visibility = Visibility.Visible;
            nCr.Visibility = Visibility.Visible;
            DeviazioneStandard.Visibility = Visibility.Visible;
            Media.Visibility = Visibility.Visible;
            ChiQuadro.Visibility = Visibility.Visible;
            Gamma.Visibility = Visibility.Visible;
            Ro.Visibility = Visibility.Visible;

            FrecciaSinistra.Visibility = Visibility.Visible;
            FrecciaDestra.Visibility = Visibility.Visible;
            DoppiaFreccia.Visibility = Visibility.Visible;
            Armstrong.Visibility = Visibility.Visible;
            Alpha.Visibility = Visibility.Visible;
            Beta.Visibility = Visibility.Visible;
            GammaMin.Visibility = Visibility.Visible;
            Phi.Visibility = Visibility.Visible;
            DeltaMin.Visibility = Visibility.Visible;
            DeltaMaius.Visibility = Visibility.Visible;
            Nabla.Visibility = Visibility.Visible;
            Ohm.Visibility = Visibility.Visible;
            Mu.Visibility = Visibility.Visible;
            Tau.Visibility = Visibility.Visible;

            ArSx.Visibility = Visibility.Visible;
            ArDx.Visibility = Visibility.Visible;

            BaseComands.Header = AppResources.Basic;
            AdvancedComands.Header = AppResources.Advanced;
            StatsTrigonometry.Header = AppResources.StatsTrigonometry;
            ChemistryBiology.Header = AppResources.ChemistryBiology;

            TitoloStorico.Visibility = Visibility.Collapsed;
            Storico.Visibility = Visibility.Collapsed;
            EmptyHistory.Visibility = Visibility.Collapsed;
            TitoloPreferiti.Visibility = Visibility.Collapsed;
            Preferiti.Visibility = Visibility.Collapsed;
            EmptyFavourites.Visibility = Visibility.Collapsed;
            TitoloFunzioni.Visibility = Visibility.Collapsed;
            Tutorial.Visibility = Visibility.Collapsed;
            ResetApp.Visibility = Visibility.Collapsed;
            PinTransparentTile.Visibility = Visibility.Collapsed;
            FunnySearches.Visibility = Visibility.Collapsed;
            Curiosità.Visibility = Visibility.Collapsed;
        }

        private void Display_LostFocus(object sender, RoutedEventArgs e)
        {
            Piu.Visibility = Visibility.Collapsed;
            Meno.Visibility = Visibility.Collapsed;
            Per.Visibility = Visibility.Collapsed;
            Diviso.Visibility = Visibility.Collapsed;
            Elevamento.Visibility = Visibility.Collapsed;
            TondaAperta.Visibility = Visibility.Collapsed;
            TondaChiusa.Visibility = Visibility.Collapsed;
            QuadraAperta.Visibility = Visibility.Collapsed;
            QuadraChiusa.Visibility = Visibility.Collapsed;
            PiGreco.Visibility = Visibility.Collapsed;
            RadiceQuadrata.Visibility = Visibility.Collapsed;
            Virgola.Visibility = Visibility.Collapsed;
            Punto.Visibility = Visibility.Collapsed;
            Uguale.Visibility = Visibility.Collapsed;

            Percento.Visibility = Visibility.Collapsed;
            ECommerciale.Visibility = Visibility.Collapsed;
            Barra.Visibility = Visibility.Collapsed;
            PuntoEsclamativo.Visibility = Visibility.Collapsed;
            E.Visibility = Visibility.Collapsed;
            Derivata.Visibility = Visibility.Collapsed;
            I.Visibility = Visibility.Collapsed;
            Logaritmo.Visibility = Visibility.Collapsed;
            Assoluto.Visibility = Visibility.Collapsed;
            Integrale.Visibility = Visibility.Collapsed;
            Infinito.Visibility = Visibility.Collapsed;
            Sommatoria.Visibility = Visibility.Collapsed;
            Limite.Visibility = Visibility.Collapsed;

            Seno.Visibility = Visibility.Collapsed;
            Coseno.Visibility = Visibility.Collapsed;
            Tangente.Visibility = Visibility.Collapsed;
            Theta.Visibility = Visibility.Collapsed;
            SenoIperbolico.Visibility = Visibility.Collapsed;
            CosenoIperbolico.Visibility = Visibility.Collapsed;
            nPr.Visibility = Visibility.Collapsed;
            nCr.Visibility = Visibility.Collapsed;
            DeviazioneStandard.Visibility = Visibility.Collapsed;
            Media.Visibility = Visibility.Collapsed;
            ChiQuadro.Visibility = Visibility.Collapsed;
            Gamma.Visibility = Visibility.Collapsed;
            Ro.Visibility = Visibility.Collapsed;

            FrecciaSinistra.Visibility = Visibility.Collapsed;
            FrecciaDestra.Visibility = Visibility.Collapsed;
            DoppiaFreccia.Visibility = Visibility.Collapsed;
            Armstrong.Visibility = Visibility.Collapsed;
            Alpha.Visibility = Visibility.Collapsed;
            Beta.Visibility = Visibility.Collapsed;
            GammaMin.Visibility = Visibility.Collapsed;
            Phi.Visibility = Visibility.Collapsed;
            DeltaMin.Visibility = Visibility.Collapsed;
            DeltaMaius.Visibility = Visibility.Collapsed;
            Nabla.Visibility = Visibility.Collapsed;
            Ohm.Visibility = Visibility.Collapsed;
            Mu.Visibility = Visibility.Collapsed;
            Tau.Visibility = Visibility.Collapsed;

            if (Display.Text == "")
            {
                ArSx.Visibility = Visibility.Collapsed;
                ArDx.Visibility = Visibility.Collapsed;
            }

            BaseComands.Header = "";
            AdvancedComands.Header = "";
            StatsTrigonometry.Header = "";
            ChemistryBiology.Header = "";

            TitoloStorico.Visibility = Visibility.Visible;
            Storico.Visibility = Visibility.Visible;
            if (!rigaStorico.Any())
            {
                EmptyHistory.Visibility = Visibility.Visible;
            }
            TitoloPreferiti.Visibility = Visibility.Visible;
            Preferiti.Visibility = Visibility.Visible;
            if (!rigaPreferiti.Any())
            {
                EmptyFavourites.Visibility = Visibility.Visible;
            }
            TitoloFunzioni.Visibility = Visibility.Visible;
            Tutorial.Visibility = Visibility.Visible;
            ResetApp.Visibility = Visibility.Visible;
            PinTransparentTile.Visibility = Visibility.Visible;
            FunnySearches.Visibility = Visibility.Visible;
            Curiosità.Visibility = Visibility.Visible;
        }

        private void Risolvi_Click(object sender, RoutedEventArgs e)
        {
            if (Display.Text != "")
            {
                //memorizzo una nuova riga nel file di testo dello storico
                IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
                using (StreamWriter writeFile = new StreamWriter(new IsolatedStorageFileStream("history.txt", FileMode.Append, FileAccess.Write, myIsolatedStorage)))
                {
                    writeFile.WriteLine(Display.Text);
                    writeFile.Close();
                }
                //abilito il tasto per cancellare lo storico
                CancellaStorico.Visibility = Visibility.Collapsed;
                CancellaStorico.IsEnabled = true;
                //faccio scomparire lo storico
                Storico.Visibility = Visibility.Collapsed;
                TitoloStorico.Visibility = Visibility.Collapsed;
                EmptyHistory.Visibility = Visibility.Collapsed;
                EmptyFavourites.Visibility = Visibility.Collapsed;
                TitoloFunzioni.Visibility = Visibility.Collapsed;
                Tutorial.Visibility = Visibility.Collapsed;
                ResetApp.Visibility = Visibility.Collapsed;
                FunnySearches.Visibility = Visibility.Collapsed;
                Curiosità.Visibility = Visibility.Collapsed;
                //disabilito il display
                Display.IsEnabled = false;
                Risolvi.IsEnabled = false;
                //faccio scomparire le frecce direzionali
                ArSx.Visibility = Visibility.Collapsed;
                ArDx.Visibility = Visibility.Collapsed;
                //introduco il tasto indietro
                Indietro.Visibility = Visibility.Visible;
                //abilito il browser
                WolframBrowser.Visibility = Visibility.Visible;
                //faccio puntare il browser all'indirizzo che mi serve, creandolo
                _lastUrlSearched = ("http://m.wolframalpha.com/input/?i=" + HttpUtility.UrlEncode(Display.Text));
                WolframBrowser.Navigate(new Uri(_lastUrlSearched, UriKind.Absolute));
                //creo la barra di caricamento nella System Tray e la imposto
                SystemTray.ProgressIndicator = new ProgressIndicator {Text = AppResources.LoadingText};
                SetProgressIndicator(true);
            }
        }

        private void WolframBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            SetProgressIndicator(false);
        }

        private void Indietro_Click(object sender, RoutedEventArgs e)
        {
            Storico.ItemsSource = rigaStorico;
            IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile("history.txt", FileMode.OpenOrCreate, FileAccess.Read);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                rigaStorico.Clear(); //svuoto la lista
                //riempio di nuovo la lista con tutti gli elementi
                int i=0;
                string tmp1;
                string tmp2 = string.Empty;
                while (!reader.EndOfStream)
                {
                    tmp1 = reader.ReadLine();
                    if (tmp1 != tmp2)
                    {
                        rigaStorico.Insert(0, new Storico(tmp1));
                        tmp2 = tmp1;
                        i++;
                    }
                }
            }

            WolframBrowser.Visibility = Visibility.Collapsed;
            Indietro.Visibility = Visibility.Collapsed;
            ArSx.Visibility = Visibility.Visible;
            ArDx.Visibility = Visibility.Visible;
            Storico.Visibility = Visibility.Visible;
            TitoloStorico.Visibility = Visibility.Visible;
            CancellaStorico.Visibility = Visibility.Visible;
            Display.IsEnabled = true;
            Risolvi.IsEnabled = true;
            if (!rigaStorico.Any())
            {
                EmptyHistory.Visibility = Visibility.Visible;
            }
            if (!rigaPreferiti.Any())
            {
                EmptyFavourites.Visibility = Visibility.Visible;
            }
            TitoloFunzioni.Visibility = Visibility.Visible;
            Tutorial.Visibility = Visibility.Visible;
            ResetApp.Visibility = Visibility.Visible;
            FunnySearches.Visibility = Visibility.Visible;
            Curiosità.Visibility = Visibility.Visible;
            SetProgressIndicator(false);
        }

        private void ApplicationBarRATE_Click(object sender, EventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }

        private void Arrow_Click(object sender, RoutedEventArgs e)
        {
            if (ArDx.IsFocused)
            {
                if (Display.SelectionStart == Display.Text.Length) {
                    Display.SelectionStart = 0;
                }
                else {
                    Display.SelectionStart = Display.SelectionStart + 1; 
                }
            }
            else if (ArSx.IsFocused)
            {
                if (Display.SelectionStart > 0) {
                    Display.SelectionStart = Display.SelectionStart - 1;
                }
                else if (Display.SelectionStart == 0)
                {
                    Display.SelectionStart = Display.Text.Length;
                }
            }
            Display.Focus();
        }

        private void Storico_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                if (((Storico)Storico.SelectedItem).Name != null)
                {
                    Display.Text = ((Storico)Storico.SelectedItem).Name;
                    Storico.SelectedItem = null;
                }
            }
            catch (NullReferenceException) { }
        }

        private void Preferiti_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                if (((Preferiti)Preferiti.SelectedItem).Name != null)
                {
                    Display.Text = ((Preferiti)Preferiti.SelectedItem).Name;
                    Preferiti.SelectedItem = null;
                }
            }
            catch (NullReferenceException) { }        
        }

        private void CancellaStorico_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(AppResources.EmptyListsMessage, AppResources.ConfirmText, MessageBoxButton.OKCancel) ==  MessageBoxResult.OK)
            {
                CancellaStorico.IsEnabled = false;
                rigaStorico.Clear();
                IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
                storage.DeleteFile("history.txt");
                //faccio ricomparire il box di benvenuto per lo storico
                EmptyHistory.Visibility = Visibility.Visible;
            }
        }

        private void CancellaPreferiti_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(AppResources.EmptyListsMessage, AppResources.ConfirmText, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                CancellaPreferiti.IsEnabled = false;
                rigaPreferiti.Clear();
                IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
                storage.DeleteFile("favourites.txt");
                //faccio ricomparire il box di benvenuto per i preferiti
                EmptyFavourites.Visibility = Visibility.Visible;
            }
        }

        private void Display_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Display.Text != "")
            {
                Risolvi_Click(Risolvi, new RoutedEventArgs());
            }
        }

        private void TextBlock_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            ((UIElement)sender).RenderTransform = new System.Windows.Media.TranslateTransform() { X = 2, Y = 2 };
        }

        private void TextBlock_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            ((UIElement)sender).RenderTransform = null;
        }

        private void ApplicationBarCOPY_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Display.Text);
        }

        private void ApplicationBarCLEAR_Click(object sender, EventArgs e)
        {
            Display.Text = "";
            ArSx.Visibility = Visibility.Collapsed;
            ArDx.Visibility = Visibility.Collapsed;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            //quando premo indietro, vado dal browser alla schermata principale
            if (WolframBrowser.Visibility == Visibility.Visible)
            {
                Indietro_Click(Indietro, new RoutedEventArgs());
                EmptyHistory.Visibility = Visibility.Collapsed;
                e.Cancel = true;
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings.Save();
                Application.Current.Terminate();
            }
        }

        private void ApplicationBarABOUT_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/About.xaml", UriKind.Relative));
        }

        private void CopyURLtoClipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_lastUrlSearched);
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            //rimuovo l'oggetto graficamente dalla lista
            Storico premuto = ((MenuItem)sender).DataContext as Storico;
            rigaStorico.Remove(premuto);

            string[] contenutoStorico = rigaStorico.Select(x => x.Name).ToArray();

            //scrivo il contenuto della lista sul file, in ordine inverso
            IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
            using (StreamWriter writeFile = new StreamWriter(new IsolatedStorageFileStream("history.txt", FileMode.Create, FileAccess.Write, myIsolatedStorage)))
            {
                for (int i = 0; i < contenutoStorico.Length; i++)
                {
                    writeFile.WriteLine(contenutoStorico[contenutoStorico.Length - i - 1]);
                }
                writeFile.Close();
            }

            //se la lista è vuota, lo scrivo e imposto lo stato del pulsante per svuotarla a "disattivato"
            if (!rigaStorico.Any())
            {
                EmptyHistory.Visibility = Visibility.Visible;
                CancellaStorico.IsEnabled = false;
            }
        }

        private void DeleteItemFavourites_Click(object sender, RoutedEventArgs e)
        {
            //rimuovo l'oggetto graficamente dalla lista
            Preferiti premuto = ((MenuItem)sender).DataContext as Preferiti;
            rigaPreferiti.Remove(premuto);

            string[] contenutoPreferiti = rigaPreferiti.Select(x => x.Name).ToArray();

            //scrivo il contenuto della lista sul file, in ordine inverso
            IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
            using (StreamWriter writeFile = new StreamWriter(new IsolatedStorageFileStream("favourites.txt", FileMode.Create, FileAccess.Write, myIsolatedStorage)))
            {
                for (int i = 0; i < contenutoPreferiti.Length; i++)
                {
                    writeFile.WriteLine(contenutoPreferiti[contenutoPreferiti.Length - i - 1]);
                }
                writeFile.Close();
            }

            //se la lista è vuota, lo scrivo e imposto lo stato del pulsante per svuotarla a "disattivato"
            if (!rigaPreferiti.Any())
            {
                EmptyFavourites.Visibility = Visibility.Visible;
                CancellaPreferiti.IsEnabled = false;
            }
        }

        private void AddToFavourites_Click(object sender, RoutedEventArgs e)
        {
            //tolgo il messaggio di preferiti non presenti
            EmptyFavourites.Visibility = Visibility.Collapsed;
            //abilito il pulsante di pulizia preferiti
            CancellaPreferiti.IsEnabled = true;

            //prelevo la riga che l'utente vuole aggiungere
            Storico premuto = ((MenuItem)sender).DataContext as Storico;
            //salvo la riga nella lista di preferiti
            IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
            using (StreamWriter writeFile = new StreamWriter(new IsolatedStorageFileStream("favourites.txt", FileMode.Append, FileAccess.Write, myIsolatedStorage)))
            {
                writeFile.WriteLine(premuto.Name);
                writeFile.Close();
            }
            //popolo la lista dei preferiti, come al solito in ordine "inverso"
            Preferiti.ItemsSource = rigaPreferiti;
            IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile("favourites.txt", FileMode.OpenOrCreate, FileAccess.Read);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                rigaPreferiti.Clear(); //svuoto la lista
                //riempio di nuovo la lista con tutti gli elementi
                int i = 0;
                string tmp1;
                string tmp2 = string.Empty;
                while (!reader.EndOfStream)
                {
                    tmp1 = reader.ReadLine();
                    if (tmp1 != tmp2)
                    {
                        rigaPreferiti.Insert(0, new Preferiti(tmp1));
                        tmp2 = tmp1;
                        i++;
                    }
                }
            }
        }

        private void AddToFavouritesFromCuriosities_Click(object sender, RoutedEventArgs e)
        {
            //tolgo il messaggio di preferiti non presenti
            EmptyFavourites.Visibility = Visibility.Collapsed;
            //abilito il pulsante di pulizia preferiti
            CancellaPreferiti.IsEnabled = true;

            //prelevo la riga che l'utente vuole aggiungere
            Curiosità premuto = ((MenuItem)sender).DataContext as Curiosità;
            //salvo la riga nella lista di preferiti
            IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
            using (StreamWriter writeFile = new StreamWriter(new IsolatedStorageFileStream("favourites.txt", FileMode.Append, FileAccess.Write, myIsolatedStorage)))
            {
                writeFile.WriteLine(premuto.Name);
                writeFile.Close();
            }
            //popolo la lista dei preferiti, come al solito in ordine "inverso"
            Preferiti.ItemsSource = rigaPreferiti;
            IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile("favourites.txt", FileMode.OpenOrCreate, FileAccess.Read);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                rigaPreferiti.Clear(); //svuoto la lista
                //riempio di nuovo la lista con tutti gli elementi
                int i = 0;
                string tmp1;
                string tmp2 = string.Empty;
                while (!reader.EndOfStream)
                {
                    tmp1 = reader.ReadLine();
                    if (tmp1 != tmp2)
                    {
                        rigaPreferiti.Insert(0, new Preferiti(tmp1));
                        tmp2 = tmp1;
                        i++;
                    }
                }
            }
        }

        private void CopyClipboardHistory_Click(object sender, RoutedEventArgs e)
        {
            Storico premuto = ((MenuItem)sender).DataContext as Storico;
            Clipboard.SetText(premuto.Name);
            MessageBox.Show(AppResources.CopiedToClipboard);
        }

        private void CopyClipboardFavourites_Click(object sender, RoutedEventArgs e)
        {
            Preferiti premuto = ((MenuItem)sender).DataContext as Preferiti;
            Clipboard.SetText(premuto.Name);
            MessageBox.Show(AppResources.CopiedToClipboard);
        }

        private void CopyClipboardCuriosities_Click(object sender, RoutedEventArgs e)
        {
            Curiosità premuto = ((MenuItem)sender).DataContext as Curiosità;
            Clipboard.SetText(premuto.Name);
            MessageBox.Show(AppResources.CopiedToClipboard);
        }

        private void Tutorial_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/TutorialStart.xaml?parameter=1", UriKind.Relative));
        }

        private void ResetApp_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(AppResources.ResetAppConfirmation, AppResources.ResetAppConfirmationButton, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                CancellaPreferiti.IsEnabled = false;
                CancellaStorico.IsEnabled = false;
                rigaPreferiti.Clear();
                rigaPreferiti.Clear();
                IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
                storage.Remove();  
                EmptyFavourites.Visibility = Visibility.Visible;
                EmptyHistory.Visibility = Visibility.Visible;

                NavigationService.Navigate(new Uri("/Pages/TutorialStart.xaml", UriKind.Relative));
            }
        }

        private void Curiosità_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                if (((Curiosità)Curiosità.SelectedItem).Name != null)
                {
                    Display.Text = ((Curiosità)Curiosità.SelectedItem).Name;
                    Curiosità.SelectedItem = null;
                }
            }
            catch (NullReferenceException) { }
        }

        private void BuildApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBar.Mode = ApplicationBarMode.Minimized;

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButton1 = new ApplicationBarIconButton(new Uri("/Assets/AppBar/about.png", UriKind.Relative));
            appBarButton1.Text = AppResources.About;
            appBarButton1.Click += new EventHandler(ApplicationBarABOUT_Click);
            ApplicationBar.Buttons.Add(appBarButton1);

            ApplicationBarIconButton appBarButton2 = new ApplicationBarIconButton(new Uri("/Assets/AppBar/rate.png", UriKind.Relative));
            appBarButton2.Text = AppResources.RateThisApp;
            appBarButton2.Click += new EventHandler(ApplicationBarRATE_Click);
            ApplicationBar.Buttons.Add(appBarButton2);

            // Create a new menu item with the localized string from AppResources.
            ApplicationBarMenuItem appBarMenuItem1 = new ApplicationBarMenuItem(AppResources.CopyToClipboard);
            appBarMenuItem1.Click += new EventHandler(ApplicationBarCOPY_Click);
            ApplicationBar.MenuItems.Add(appBarMenuItem1);

            ApplicationBarMenuItem appBarMenuItem2 = new ApplicationBarMenuItem(AppResources.CopyURLtoClipboard);
            appBarMenuItem2.Click += new EventHandler(CopyURLtoClipboard_Click);
            ApplicationBar.MenuItems.Add(appBarMenuItem2);

            ApplicationBarMenuItem appBarMenuItem3 = new ApplicationBarMenuItem(AppResources.ClearDisplay);
            appBarMenuItem3.Click += new EventHandler(ApplicationBarCLEAR_Click);
            ApplicationBar.MenuItems.Add(appBarMenuItem3);
        }

        private void PinTransparent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var wideTile = new FlipTileData
                {
                    BackgroundImage = new Uri("/Images/Tiles/Transparent/Medium_tile_clean_tr.png", UriKind.Relative),
                    WideBackgroundImage = new Uri("/Images/Tiles/Transparent/Large_tile_clean_tr.png", UriKind.Relative),
                    SmallBackgroundImage = new Uri("/Images/Tiles/Transparent/Small_tile_clean_tr.png", UriKind.Relative)
                };
                ShellTile.Create(new Uri("/MainPage.xaml", UriKind.Relative), wideTile, true);
            }
            catch (InvalidOperationException) { MessageBox.Show(AppResources.TileAlreadyPinned); }
        }
    }
}