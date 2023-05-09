using Proyecto1.Models;
using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Proyecto1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListLugares : ContentPage
    {
        public ListLugares()
        {
            InitializeComponent();
        }

        // Gets the places from the db and sets the
        // list to this
        private async void Cargar_Sitios()
        {
            // Get the lists from the db
            var sitios = await App.DBase.getListSitio();
            Lista.ItemsSource = sitios;
        }

        // Function that opens a prompt
        private async void ListSitio_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var sitio = (Lugares)e.Item;

            bool answer = await DisplayAlert("AVISO", "¿Quiere ir al mapa?", "Si", "No");
            Debug.WriteLine("Answer: " + answer);

            if (answer == true)
            {
                // Opens the map
                Map map = new Map();
                map.BindingContext = sitio;
                await Navigation.PushAsync(map);
            };


        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            Lista.ItemsSource = await App.DBase.getListSitio();
        }

        // Long press to delete
        private async void Eliminar_Clicked(object sender, EventArgs e)
        { 
            bool answer = await DisplayAlert("Confirmacion", "¿Quiere eliminar el registro?", "Si", "No");
            Debug.WriteLine("Answer: " + answer);
            if (answer == true)
            {
                var idSitio = (Lugares)(sender as MenuItem).CommandParameter;
                var result = await App.DBase.DeleteSitio(idSitio);

                if (result == 1)
                {
                    await DisplayAlert("Aviso", "Registro Eliminado", "OK");
                    Cargar_Sitios();
                }
                else
                {
                    await DisplayAlert("Aviso", "Revisa", "OK");
                }
            };
        }

        // Opens the map
        private async void IrMapa_Clicked(object sender, EventArgs e)
        {
            var idSitio = (Lugares)(sender as MenuItem).CommandParameter;
            //await DisplayAlert("Aviso", "sitio " + idSitio, "ok");

            bool answer = await DisplayAlert("AVISO", "¿Quiere ir al mapa?", "Si", "No");
            Debug.WriteLine("Answer: " + answer);

            if (answer == true)
            {
                try
                {
                    // Request the GPS location
                    var georequest = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                    var tokendecancelacion = new System.Threading.CancellationTokenSource();
                    var location = await Geolocation.GetLocationAsync(georequest, tokendecancelacion.Token);
                    
                    // If there's a location
                    if (location != null)
                    {
                        // Opens the map
                        Map map = new Map();
                        await Navigation.PushAsync(map);
                    }
                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    await DisplayAlert("Advertencia", "Este dispositivo no soporta GPS" + fnsEx, "Ok");
                }
                catch (FeatureNotEnabledException fneEx)
                {
                    await DisplayAlert("Advertencia", "Error de Dispositivo, validar si su GPS esta activo", "Ok");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();

                }
                catch (PermissionException pEx)
                {
                    await DisplayAlert("Advertencia", "Sin Permisos de Geolocalizacion" + pEx, "Ok");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Advertencia", "Sin Ubicacion " + ex, "Ok");
                }
            };
        }

        private async void Actualizar_Clicked(object sender, EventArgs e)
        {
            var lugar = (Lugares)(sender as MenuItem).CommandParameter;
            await Navigation.PushAsync(new EditarLugar(lugar));
        }
    }
}