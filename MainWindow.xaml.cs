using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Autok
{
    public partial class MainWindow : Window
    {
        private List<Jelentes> jelentesek = new List<Jelentes>();

        public MainWindow()
        {
            InitializeComponent();
            BeolvasAdatok();
        }

        private void BeolvasAdatok()
        {
            string filePath = "jeladas.txt"; 
            if (!File.Exists(filePath))
            {
                MessageBox.Show("A jeladas.txt nem található!");
                return;
            }

            var sorok = File.ReadAllLines(filePath);
            foreach (var sor in sorok)
            {
                var darabok = sor.Split('\t'); 
                if (darabok.Length >= 4)
                {
                    string rendszam = darabok[0];
                    int ora = int.Parse(darabok[1]);
                    int perc = int.Parse(darabok[2]);
                    int sebesseg = int.Parse(darabok[3]);

                    var jel = new Jelentes(rendszam, ora, perc, sebesseg);
                    jelentesek.Add(jel);
                }
            }

            MessageBox.Show($"Sikeres beolvasás, {jelentesek.Count} sor érkezett.");
        }

        private void BtnLegkesobbi_Click(object sender, RoutedEventArgs e)
        {
            if (jelentesek.Count == 0)
            {
                eredmenyText.Text = "Nincsenek adatok a listában!";
                return;
            }

            Jelentes legkesobbi = jelentesek[0];
            foreach (var jel in jelentesek)
            {
                if ((jel.Ora > legkesobbi.Ora) ||
                    (jel.Ora == legkesobbi.Ora && jel.Perc > legkesobbi.Perc))
                {
                    legkesobbi = jel;
                }
            }

            eredmenyText.Text = $"Legkésőbbi jelzés: {legkesobbi.Rendszam} " +
                                $"- {legkesobbi.Ora}:{legkesobbi.Perc}";
        }

        // ÚJ: Első jármű adatai
        private void BtnElsoJarmu_Click(object sender, RoutedEventArgs e)
        {
            if (jelentesek.Count == 0)
            {
                eredmenyText.Text = "Nincsenek adatok a listában!";
                return;
            }

            string elsoRendszam = jelentesek[0].Rendszam;

            List<string> idopontok = new List<string>();
            foreach (var jel in jelentesek)
            {
                if (jel.Rendszam == elsoRendszam)
                {
                    idopontok.Add($"{jel.Ora}:{jel.Perc}");
                }
            }

            eredmenyText.Text = $"Az első jármű rendszáma: {elsoRendszam}\n" +
                                $"Jelzési időpontjai: {string.Join(", ", idopontok)}";
        }
    }
}
