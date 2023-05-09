using Proyecto1.Controller;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Proyecto1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        public Register()
        {
            InitializeComponent();
        }

        private async void RegisterBtn_Clicked(object sender, EventArgs e)
        {
            string username = Username.Text;
            string password = Password.Text;
            string password_confirmation = Password_Confirmacion.Text;

            if (username != null && password != null && password_confirmation != null)
            {
                if (password == password_confirmation) 
                {
                    if (await App.DBase.RegisterAsync(username, password))
                    {
                        await DisplayAlert("FELICIDADES!", "Su usuario ha sido creado!", "OK");
                        await Navigation.PushAsync(new LoginPage());
                    }
                    else
                    {
                        await DisplayAlert("Error", "Hubo un error en el registro", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Su contra no coincide con su confimacion", "OK");
                }
            } else
            {
                await DisplayAlert("Error", "Ingrese sus credenciales", "OK");
            }
        }
    }
}