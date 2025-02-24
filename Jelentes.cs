namespace Autok
{
    public class Jelentes
    {
        public string Rendszam { get; set; }
        public int Ora { get; set; }
        public int Perc { get; set; }
        public int Sebesseg { get; set; }

        public Jelentes(string rendszam, int ora, int perc, int sebesseg)
        {
            Rendszam = rendszam;
            Ora = ora;
            Perc = perc;
            Sebesseg = sebesseg;
        }
    }
}
