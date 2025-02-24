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
                var darabok = sor.Split('\t'); // tabulátoros feldolgozás
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
    }
}
