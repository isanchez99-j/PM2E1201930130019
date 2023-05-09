using Plugin.Media;
using Proyecto1.Views;
using System;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Proyecto1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            TestInternet();
        }

        public async void TestInternet()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Alerta", "No hay Internet disponible!", "OK");
            }
        }

        public async void LoadCoord()
        {
            try
            {
                // Gets the location
                var georequest = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                var tokendecancelacion = new System.Threading.CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(georequest, tokendecancelacion.Token);

                // If there's a location
                if (location != null)
                {
                    var lon = location.Longitude;
                    var lat = location.Latitude;

                    // Set the text to the label
                    txtLat.Text = lat.ToString();
                    txtLon.Text = lon.ToString();
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

        Plugin.Media.Abstractions.MediaFile Filefoto = null;

        private Byte[] ConvertImageToByteArray()
        {
            if (Filefoto != null)
            {
                using (MemoryStream memory = new MemoryStream()) 
                {
                    Stream stream = Filefoto.GetStream();
                    stream.CopyTo(memory);
                    return memory.ToArray();
                }

            }
            return null;

        }
        private async void btnAdd_Clicked(object sender, EventArgs e)
        {
            if (Filefoto == null)
            {
                await DisplayAlert("Advertencia", "Debe tomar una foto", "OK");
            }
            else if (string.IsNullOrEmpty(txtDescripcion.Text))
            {
                await DisplayAlert("Advertencia", "El campo del Descripcion es obligatorio.", "OK");
            }
            else if (string.IsNullOrEmpty(txtDireccion.Text))
            {
                await DisplayAlert("Advertencia", "El campo del Direccion es obligatorio.", "OK");
            }
            else if (string.IsNullOrEmpty(txtLat.Text) && string.IsNullOrEmpty(txtLon.Text))
            {
                await DisplayAlert("Advertencia", "No se puede agregar Registro. Faltan coordenadas.", "OK");

                LoadCoord();
            }
            else
            {
                var sitio = new Models.Lugares
                {
                    id = 0,
                    latitud = txtLat.Text,
                    longitud = txtLon.Text,
                    descripcion = txtDescripcion.Text,
                    direccion = txtDireccion.Text,
                    foto = ConvertImageToByteArray(),
                };

                // await DisplayAlert("Aviso", "Sitio Adicionado" + sitio.foto, "OK");
                var result = await App.DBase.SitioSave(sitio);

                if (result > 0)//se usa como una super clase
                {
                    await DisplayAlert("Aviso", "Sitio Registrado", "OK");
                    Clear();

                }
                else
                {
                    await DisplayAlert("Aviso", "Error al Registrar", "OK");
                }
            }
        }

        private async void btnList_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListLugares());
        }

        private async void btnFoto_Clicked(object sender, EventArgs e)
        {
            // Tomar foto
            Filefoto = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "MisFotos",
                Name = "test.jpg",
                SaveToAlbum = true,
            });

            // Si hay foto
            if (Filefoto != null)
            {
                fotoSitio.Source = ImageSource.FromStream(() =>
                {
                    return Filefoto.GetStream();
                });
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var username = await SecureStorage.GetAsync("Username");
            Username.Text = "Bienvenido de vuelta " + username;
            LoadCoord();
            try
            {
                var georequest = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                var tokendecancelacion = new System.Threading.CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(georequest, tokendecancelacion.Token);
                if (location != null)
                {

                    var lon = location.Longitude;
                    var lat = location.Latitude;

                    txtLat.Text = lat.ToString();
                    txtLon.Text = lon.ToString();
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Advertencia", "Este dispositivo no soporta GPS" + fnsEx, "Ok");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                await DisplayAlert("Advertencia", "Error de Dispositivo, validar si su GPS esta activo", "Ok");
                System.Diagnostics.Process.GetCurrentProcess().Kill(); //cerramos la aplicacion hasta que el usuario active el GPS

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

        private async void btnSalir_Clicked(object sender, EventArgs e)
        {
            // Closes the app and pushes the user to the login page
            await SecureStorage.SetAsync("IsLoggedIn", "false");
            await SecureStorage.SetAsync("Username", "");
            await Navigation.PushAsync(new LoginPage());
        }

        private void Clear()
        {
            txtDescripcion.Text = "";
            txtDireccion.Text = "";
            fotoSitio.Source = null;
        }
    }
}
