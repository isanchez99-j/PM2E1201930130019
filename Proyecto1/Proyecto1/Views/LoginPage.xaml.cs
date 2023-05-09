using Proyecto1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Proyecto1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }

        private async void LoginBtn_Clicked(object sender, EventArgs e)
        {
            string username = Username.Text;
            string password = Password.Text;

            if (username != null && password != null)
            {
                // Validar los usuarios
                if (await App.DBase.LoginAsync(username, password))
                {
                    await SecureStorage.SetAsync("IsLoggedIn", "true");
                    await SecureStorage.SetAsync("Username", username);
                    await Navigation.PushAsync(new MainPage());
                }
                else
                {
                    await DisplayAlert("Error", "Credenciales Incorrectas", "OK");
                }
            }
            else
            {
               await DisplayAlert("Error", "Ingrese sus credenciales", "OK");
            }
        }

        private async void Registrar_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Register());
        }

        private void SalirBtn_Clicked(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Username_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool isValid = Regex.IsMatch(e.NewTextValue, "^[a-zA-Z0-9]{0,10}$");

            if (!isValid)
            {
                ((Entry)sender).Text = e.OldTextValue;
            }
        }
    }
}