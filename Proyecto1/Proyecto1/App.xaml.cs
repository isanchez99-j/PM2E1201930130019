using Proyecto1.Controller;
using System;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Proyecto1
{
    public partial class App : Application
    {
        static DataBase db;

        public static DataBase DBase
        {
            get
            {
                if (db == null)
                {
                    String FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Casas.db3");
                    db = new DataBase(FolderPath);
                }
                return db;
            }
        }

        public App()
        {
            InitializeComponent();
            string isLoggedIn = SecureStorage.GetAsync("IsLoggedIn").Result;

            if (isLoggedIn == "true")
            {
                MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                MainPage = new NavigationPage(new Views.LoginPage());
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
