using Proyecto1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Proyecto1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditarLugar : ContentPage
    {
        private readonly Lugares lugar;

        public EditarLugar(Lugares lugar)
        {
            InitializeComponent();

            this.lugar = lugar;

            Descripcion.Text = lugar.descripcion;
            Latitud.Text = lugar.latitud;
            Longitud.Text = lugar.longitud;
            Direccion.Text = lugar.direccion;
        }

        private async void SaveBtn_Clicked(object sender, EventArgs e)
        {
            lugar.descripcion = Descripcion.Text;
            lugar.latitud = Latitud.Text;
            lugar.longitud= Longitud.Text;
            lugar.direccion= Direccion.Text;

            // Save the changes to the database
            var result = await App.DBase.SitioSave(lugar);

            Debug.WriteLine(result);
         
            await DisplayAlert("Aviso", "Registro Actualizado", "OK");
            await Navigation.PopAsync();
        }
    }
}