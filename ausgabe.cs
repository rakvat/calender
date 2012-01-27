using System;
using System.IO;
using System.Collections.Generic;

namespace KalenderWelt
{
    public abstract class Ausgabe
    {
        private StreamWriter _streamWriter;
        protected KalenderJahr _jahr;
        protected int _modus;

        public Ausgabe(ref KalenderJahr dasJahr, int eingabemodus) //Konstruktor der Klasse Ausgabe, übergibt zwei int werte von main.cs Datei an ausgabe.cs
        {
            _jahr = dasJahr;
            _modus = eingabemodus;
        }

        protected void gibZeileAus() //erzeugt Leere Zeilen, ohne übergabewert
        {
            gibZeileAus("");
        }

        protected void gibZeileAus(string dieZeile) //gibt zusammengebaute Kalenderzeilen aus, mit übergabewert
        {
            _streamWriter.WriteLine(dieZeile);
            Console.WriteLine(dieZeile);
        }

        protected void oeffneStream(int eingabemodus)
        {
            string jahr = Convert.ToString(_jahr.Jahreszahl());
            string Dateiname;
            Dateiname = jahr.Insert(4, ".txt");
            jahr = Dateiname;
            Dateiname = jahr.Insert(0, "Kalender");

            if (_modus == 1) Dateiname = Dateiname.Insert(0, "1");
            if (_modus == 2) Dateiname = Dateiname.Insert(0, "2");
            if (_modus == 3) Dateiname = Dateiname.Insert(0, "3");

            if (!System.IO.Directory.Exists("kalenderausgabe/"))
            {
                Directory.CreateDirectory("kalenderausgabe"); //anlegen des Ordners wenn nicht vorhanden
            }
            FileInfo f = new FileInfo("kalenderausgabe/" + Dateiname); //Text Datei anlegen 
            _streamWriter = f.CreateText();
        }

        protected void schliesseStream()
        {
            _streamWriter.Close(); //Datei schliesen
        }

        public abstract void gibAus();
    }

    public class MonatsBlockAusgabe : Ausgabe
    {
        public MonatsBlockAusgabe(ref KalenderJahr dasJahr, int eingabemodus)
            : base(ref dasJahr, eingabemodus)
        {
        }

        public override void gibAus()
        {
            oeffneStream(_modus);
            int monat, tag;
            int wochentagDesErstenImMonat = 0; //Mo = 0, ...
            gibZeileAus("Kalender fuer das Jahr " + _jahr.Jahreszahl());
            gibZeileAus();
            string wochentage = String.Join(" ", HilfsKonstrukte.wochenTagNamenKurz);

            if (_jahr.IstSchaltjahr())
            {
                HilfsKonstrukte.tageImMonat[1] = 29; //Februar Feld im Array bei Schaltjahren auf 29 setzen
            }
            wochentagDesErstenImMonat = _jahr.StartWochenTag();
            for (monat = 0; monat < 12; monat++) //Schleife für die Erstellung der Monate: 0=Jan bis 11=Dez
            {
                gibZeileAus("      " + HilfsKonstrukte.monatsNamen[monat]);
                gibZeileAus(wochentage);

                if (monat > 0)
                {
                    wochentagDesErstenImMonat += HilfsKonstrukte.tageImMonat[monat - 1] % 7;
                    wochentagDesErstenImMonat %= 7;
                }

                //Die Woche beginnt immer Montags, nur der 1. eines Monats 
                //kann auf verschiedene Wochentage fallen. 
                int ausgabePosition = wochentagDesErstenImMonat;

                string zeile = "".PadLeft(ausgabePosition * 3);
                for (tag = 1; tag <= HilfsKonstrukte.tageImMonat[monat]; tag++) //solange Zähler kleiner gleich Wert aus Array Tage 
                {
                    zeile += tag.ToString().PadRight(3);
                    ausgabePosition++;

                    //wenn hinterste Position erreicht Zeihlenumbruch einfügen und Bildschirm-Positionszähler zurücksetzen
                    if (ausgabePosition > 6)
                    {
                        gibZeileAus(zeile);
                        ausgabePosition = 0;
                        zeile = "";
                    }
                }
                if (zeile.Length > 0) //wenn mehr als 0 Zeichen vorhanden sind ganze Zeile ausgeben
                {
                    gibZeileAus(zeile);
                }
                //freie Zeilen zwischen den Monaten auf dem Bildschirm 
                gibZeileAus();

            } //ende for Schleife Monat   
            schliesseStream();
        }
    } //ende class MonatsBlockAusgabe

    public class MonatsBlockAusgabe2Spaltig : Ausgabe
    {
        private Dictionary<int, List<string>> preparedStrings = new Dictionary<int, List<string>>();
        private const int SPALTEN_BREITE = 3 * 7 + 10; //3*7 ist ein Monat breit

        public MonatsBlockAusgabe2Spaltig(ref KalenderJahr dasJahr, int eingabemodus)
            : base(ref dasJahr, eingabemodus)
        {
        }

        public override void gibAus()
        {
            oeffneStream(_modus);

            int monat, tag;
            int wochentagDesErstenImMonat = 0; //Mo = 0, ...

            gibZeileAus("Kalender fuer das Jahr " + _jahr.Jahreszahl());
            gibZeileAus();
            string wochentage = String.Join(" ", HilfsKonstrukte.wochenTagNamenKurz);

            if (_jahr.IstSchaltjahr())
            {
                HilfsKonstrukte.tageImMonat[1] = 29; //Februar Feld im Array bei Schaltjahren auf 29 setzen
            }
            wochentagDesErstenImMonat = _jahr.StartWochenTag();
            for (monat = 0; monat < 12; monat++) //Schleife für die Erstellung der Monate: 0=Jan bis 11=Dez
            {
                preparedStrings[monat] = new List<string>();
                preparedStrings[monat].Add("      " + HilfsKonstrukte.monatsNamen[monat]);
                preparedStrings[monat].Add(wochentage);

                if (monat > 0)
                {
                    wochentagDesErstenImMonat += HilfsKonstrukte.tageImMonat[monat - 1] % 7;
                    wochentagDesErstenImMonat %= 7;
                }

                //Die Woche beginnt immer Montags, nur der 1. eines Monats 
                //kann auf verschiedene Wochentage fallen. 
                int ausgabePosition = wochentagDesErstenImMonat;

                string zeile = "".PadLeft(ausgabePosition * 3);
                for (tag = 1; tag <= HilfsKonstrukte.tageImMonat[monat]; tag++) //solange Zähler kleiner gleich Wert aus Array Tage 
                {
                    zeile += tag.ToString().PadRight(3);
                    ausgabePosition++;

                    //wenn hinterste Position erreicht Zeihlenumbruch einfügen und Bildschirm-Positionszähler zurücksetzen
                    if (ausgabePosition > 6)
                    {
                        preparedStrings[monat].Add(zeile);
                        ausgabePosition = 0;
                        zeile = "";
                    }
                }
                if (zeile.Length > 0) //wenn mehr als 0 Zeichen vorhanden sind ganze Zeile ausgeben
                {
                    preparedStrings[monat].Add(zeile);
                }
                //freie Zeilen zwischen den Monaten auf dem Bildschirm 
                while (preparedStrings[monat].Count < 9)
                {
                    preparedStrings[monat].Add("");
                }

            } //ende for Schleife Monat   

            for (monat = 0; monat < 6; monat++)
            {
                for (int i = 0, l = preparedStrings[monat].Count; i < l; ++i)
                {
                    gibZeileAus(preparedStrings[monat][i].PadRight(SPALTEN_BREITE)
                                + preparedStrings[monat + 6][i]);
                }
            }
            schliesseStream();
        }
    } //ende class MonatsBlockAusgabe2Spaltig

    public class TageszeilenAusgabe : Ausgabe
    {
        public TageszeilenAusgabe(ref KalenderJahr dasJahr, int eingabemodus)
            : base(ref dasJahr, eingabemodus)
        {
        }

        public override void gibAus()
        {
            oeffneStream(_modus);
            gibZeileAus("Kalender fuer das Jahr " + _jahr.Jahreszahl());
            gibZeileAus();

            foreach (Monat monat in _jahr.GibMonate()) 
            {
                gibZeileAus("      " + HilfsKonstrukte.monatsNamen[monat.GibIndex()]);
                foreach (Tag tag in monat.GibTage())
                {
                    int wochentag = tag.GibWochentag();
                    gibZeileAus(HilfsKonstrukte.wochenTagNamenKurz[wochentag] + 
                                " " + tag.GibIndex().ToString().PadLeft(3));
                    if (wochentag == 6)
                    {
                        gibZeileAus("---------------------");
                    }
                }
                gibZeileAus();
            }
            schliesseStream();
        }
    } //ende class TageszeilenAusgabe
} //ende namespace KalenderWelt
