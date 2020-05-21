using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Zgadnij.Views;
using Zgadnij.ViewModels;
using Xamarin.Essentials;

namespace Zgadnij.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        int los = 0; int min = 0; int max = 1000; bool wygrana = false; int ktoraProba = 0;
        int[] proby = new int[1001];
        Random rnd = new Random(); 

        private void Alert()
        {
            var alert_player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            alert_player.Load("DING.mp3");
            alert_player.Play();
        }

        private void Victory()
        {
            var victory_player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            victory_player.Load("TADA.mp3");
            victory_player.Play();
        }

        public ItemsPage()
        {
            InitializeComponent();
            los = rnd.Next(1001); 
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (wygrana)
            {
                Alert(); DisplayAlert("Wygrałeś!", "Musisz użyć przycisku Reset by grać dalej. ", "OK"); 

            }
            else
            {
                int odp = 0;
                try
                {
                    odp = int.Parse(ans.Text);
                    ans.Text = "";
                    if (odp == los)
                    {
                        ktoraProba++;
                        wygrana = true;
                        txt.TextColor = Color.DarkRed;
                        txt.Text = "Wygrałeś!";

                        Preferences.Set("iloscGier", Preferences.Get("iloscGier", 0) + 1);

                        int wynikZBazy = Preferences.Get("najlepszyWynik", 2147483647);
                        if (wynikZBazy > ktoraProba)
                        {
                            Victory(); DisplayAlert("Wygrałeś!", "Pobiłeś swój najlepszy wynik " + (wynikZBazy == 2147483647 ? 0 : wynikZBazy) + " kończąc grę z wynikiem " + ktoraProba + "!", "OK"); Preferences.Set("najlepszyWynik", ktoraProba); 
                        }
                        else
                        {
                            Victory(); DisplayAlert("Wygrałeś!", "Potrzebowałeś " + ktoraProba + " prób żeby wygrać.", "OK");
                        }

                    }
                    else
                    {
                        try
                        {
                            foreach (int i in proby) if (odp == i) throw new Exception();
                            if (odp < min || odp > max) throw new Exception(); 

                            proby[ktoraProba] = odp;
                            ktoraProba++; 

                            if(min+1 == los) { Alert(); DisplayAlert("Info", "Jesteś już tak blisko wygranej :>", "OK"); }
                            else if (odp < los) min++;

                            if(max-1 == los) { Alert(); DisplayAlert("Info", "Jesteś już tak blisko wygranej :>", "OK"); }
                            else if (odp > los) max--;

                            txt.TextColor = Color.Default;
                            txt.Text = "Jest to liczba z przedziału " + min + " - " + max + " i zarazem twoja " + ktoraProba + " próba.";
                        }
                        catch (Exception)
                        {
                            Alert(); DisplayAlert("Błąd!", "Ta liczba została już wprowadzona wcześniej lub jest spoza zakresu. Wprowadź poprawną liczbę naturalną z zakresu " + min + " - " + max + ".", "OK");
                        }
                    }
                }
                catch (Exception)
                {
                    Alert(); DisplayAlert("Błąd!", "Musisz wprowadzić poprawną liczbę naturalną z zakresu " + min + " - " + max + ".", "OK");
                    txt.TextColor = Color.DarkRed;
                    txt.Text = "Wprowadź poprawną liczbę naturalną z zakresu " + min + " - " + max;
                }
            }
        }

        private void ResetBtn_Clicked(object sender, EventArgs e)
        {
            min = 0; max = 1000; wygrana = false; ktoraProba = 0; proby = new int[1001];
            los = rnd.Next(1001);
            txt.TextColor = Color.Default;
            txt.Text = "Jest to liczba z przedziału 0 - 1000.";
        }
    }
}