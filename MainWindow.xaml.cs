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

        private void BtnKeresIdopont_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtOra.Text, out int ora))
            {
                eredmenyText.Text = "Hibás óra érték, nem szám!";
                return;
            }
            if (!int.TryParse(txtPerc.Text, out int perc))
            {
                eredmenyText.Text = "Hibás perc érték, nem szám!";
                return;
            }

            int db = 0;
            foreach (var jel in jelentesek)
            {
                if (jel.Ora == ora && jel.Perc == perc)
                {
                    db++;
                }
            }

            eredmenyText.Text = $"A(z) {ora}:{perc} időpontban {db} jeladás történt.";
        }

        private void BtnIdoKiir_Click(object sender, RoutedEventArgs e)
        {
            if (jelentesek.Count == 0)
            {
                eredmenyText.Text = "Nincsenek adatok.";
                return;
            }

            var dict = new Dictionary<string, (int minOra, int minPerc, int maxOra, int maxPerc)>();

            foreach (var j in jelentesek)
            {
                if (!dict.ContainsKey(j.Rendszam))
                {
                    dict[j.Rendszam] = (j.Ora, j.Perc, j.Ora, j.Perc);
                }
                else
                {
                    var adat = dict[j.Rendszam];
                    if ((j.Ora < adat.minOra) ||
                        (j.Ora == adat.minOra && j.Perc < adat.minPerc))
                    {
                        adat.minOra = j.Ora;
                        adat.minPerc = j.Perc;
                    }
                    if ((j.Ora > adat.maxOra) ||
                        (j.Ora == adat.maxOra && j.Perc > adat.maxPerc))
                    {
                        adat.maxOra = j.Ora;
                        adat.maxPerc = j.Perc;
                    }
                    dict[j.Rendszam] = adat;
                }
            }

            using (var sw = new StreamWriter("ido.txt"))
            {
                foreach (var kvp in dict)
                {
                    string rendszam = kvp.Key;
                    var (minOra, minPerc, maxOra, maxPerc) = kvp.Value;
                    sw.WriteLine($"{rendszam} {minOra}:{minPerc} {maxOra}:{maxPerc}");
                }
            }

            eredmenyText.Text = "ido.txt elkészült (minden jármű első és utolsó jelzése).";
        }
    }
}
