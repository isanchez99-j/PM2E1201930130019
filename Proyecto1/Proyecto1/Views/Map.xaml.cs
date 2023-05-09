using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace Proyecto1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Map : ContentPage
    {
        public Map()
        {
            InitializeComponent();
        }

        Plugin.Media.Abstractions.MediaFile Filefoto = null;

        protected async override void OnAppearing()
        {

            base.OnAppearing();
            try
            {
                var georequest = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                var tokendecancelacion = new System.Threading.CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(georequest, tokendecancelacion.Token);
                if (location != null)
                {

                    var lat = Convert.ToDouble(mtxtLat.Text);
                    var lon = Convert.ToDouble(mtxtLon.Text);
                    var Nomsitio = nomSitio.Text;

                    var placemarks = await Geocoding.GetPlacemarksAsync(lat, lon);

                    var placemark = placemarks?.FirstOrDefault();
                    if (placemark != null)
                    {
                        var geocodeAddress =
                               $"\nPais:\t  {placemark.CountryName}\n" +
                               $"Depto:\t   {placemark.AdminArea}\n" +
                               $"Ciudad:\t {placemark.SubAdminArea}\n" +
                               $"Colonia:\t {placemark.Locality}\n";

                        await DisplayAlert("Ubicacion", geocodeAddress, "ok");

                        var pin = new Pin()
                        {
                            Position = new Position(lat, lon),
                            Label = Nomsitio,

                        };

                        Mapa.Pins.Add(pin);
                        Mapa.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(lat, lon), Distance.FromMeters(100.00)));

                    }
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



        }

        private async void btn_Compartir_Clicked(object sender, EventArgs e)
        {
            var geocodeAddress = "";

            var lat = Convert.ToDouble(mtxtLat.Text);
            var lon = Convert.ToDouble(mtxtLon.Text);
            var Nomsitio = nomSitio.Text;

            var placemarks = await Geocoding.GetPlacemarksAsync(lat, lon);

            var placemark = placemarks?.FirstOrDefault();

            if (placemark != null)
            {
                geocodeAddress =
                               $"\nPais:\t  {placemark.CountryName}\n" +
                               $"Depto:\t   {placemark.AdminArea}\n" +
                               $"Ciudad:\t {placemark.SubAdminArea}\n" +
                               $"Colonia:\t {placemark.Locality}\n";
            }

            try
            {
                await Share.RequestAsync(new ShareTextRequest
                {

                    Title = "Compartiendo ubicacion \n",
                    Text = "Lugar " + Nomsitio + geocodeAddress + " \n",
                    Uri = "https://maps.google.com/?q=" + lon + "," + lat

                });
            }
            catch
            {

            }
        }
    }
}