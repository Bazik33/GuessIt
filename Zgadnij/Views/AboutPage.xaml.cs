using System;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Zgadnij.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent(); UpdateBestScore();   
        }

        public void UpdateBestScore()
        {
            wynikTutaj.Text = Preferences.Get("najlepszyWynik", 0).ToString();
        }
    }
}