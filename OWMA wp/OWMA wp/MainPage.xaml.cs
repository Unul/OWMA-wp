using OWMA_wp.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace OWMA_wp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // Delegate implentations
        Del ShowLoggedButton;

        public MainPage()
        {
            this.InitializeComponent();
            ShowLoggedButton = delegate ()
            {
                AppBarLogoutButton.Visibility = Visibility.Visible;

                AppBarConnectButton.Visibility = Visibility.Collapsed;
                AppBarRegisterButton.Visibility = Visibility.Collapsed;
            };
        }


        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarConnectButton.Flyout.Hide();
            ObjectUser oUser = await UserNetwork.Login(ConnectLoginInput.Text, ConnectPasswordInput.Password);
            if (oUser != null && oUser.user != null)
            {
                ShowLoggedButton();
                Utils.Notify("Connexion réussie", "Compte :" + oUser.user.email);
            }
            this.Frame.Navigate(typeof(HubPage));
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (RegisterPasswordInput.Password.Equals(RegisterPasswordConfirmationInput.Password))
            {
                AppBarRegisterButton.Flyout.Hide();
                ObjectUser oUser = await UserNetwork.Register(RegisterLoginInput.Text, RegisterPasswordInput.Password, RegisterPasswordConfirmationInput.Password);
                if (oUser != null && oUser.user != null)
                    Utils.Notify("Connexion réussie", "Compte :" + oUser.user.email);
            }
            else
                Utils.Notify("Erreur", "Le mot de passe et sa confirmation ne sont pas identiques");
        }
    }
}
