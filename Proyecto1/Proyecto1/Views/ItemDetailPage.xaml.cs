using Proyecto1.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Proyecto1.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}