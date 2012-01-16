using System;
using System.IO;
using System.Collections.Generic;

namespace KalenderWelt
{
    public abstract class Ausgabe 
    {
        private StreamWriter _streamWriter;
        protected int _jahr;

        public Ausgabe(int dasJahr) {
            _jahr = dasJahr;
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

        protected void oeffneStream()
        {
            string jahr = Convert.ToString(_jahr);
            string Dateiname;
            Dateiname = jahr.Insert(4, ".txt");
            jahr = Dateiname;
            Dateiname = jahr.Insert(0, "Kalender");
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
        public MonatsBlockAusgabe(int dasJahr) : base(dasJahr) {
        }

        public override void gibAus()
        {
            oeffneStream();
            int monat, tag;
            int wochentagDesErstenImMonat = 0; //Mo = 0, ...
            

            gibZeileAus("Kalender fuer das Jahr " + _jahr);
            gibZeileAus();
            string wochentage = String.Join(" ", HilfsKonstrukte.wochenTagNamenKurz);

            if (HilfsKonstrukte.istSchaltJahr(_jahr))
            {
                HilfsKonstrukte.tageImMonat[1] = 29; //Februar Feld im Array bei Schaltjahren auf 29 setzen
            }
            wochentagDesErstenImMonat = HilfsKonstrukte.startWochenTag(_jahr);
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
        private Dictionary<int, List<string> > preparedStrings = new Dictionary<int, List<string> > ();
        private const int SPALTEN_BREITE = 3 * 7 + 10; //3*7 ist ein Monat breit

        public MonatsBlockAusgabe2Spaltig(int dasJahr) : base(dasJahr) {
        }

        public override void gibAus()
        {
            oeffneStream();

            int monat, tag;
            int wochentagDesErstenImMonat = 0; //Mo = 0, ...

            gibZeileAus("Kalender fuer das Jahr " + _jahr);
            gibZeileAus();
            string wochentage = String.Join(" ", HilfsKonstrukte.wochenTagNamenKurz);

            if (HilfsKonstrukte.istSchaltJahr(_jahr))
            {
                HilfsKonstrukte.tageImMonat[1] = 29; //Februar Feld im Array bei Schaltjahren auf 29 setzen
            }
            wochentagDesErstenImMonat = HilfsKonstrukte.startWochenTag(_jahr);
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
                while(preparedStrings[monat].Count < 9) {
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
        public TageszeilenAusgabe(int dasJahr) : base(dasJahr) {
        }

        public override void gibAus()
        {
            oeffneStream();
            int monat, tag;
            int wochentag = 0;
            gibZeileAus("Kalender fuer das Jahr " + _jahr);
            gibZeileAus();

            if (HilfsKonstrukte.istSchaltJahr(_jahr))
            {
                HilfsKonstrukte.tageImMonat[1] = 29; //Februar Feld im Array bei Schaltjahren auf 29 setzen
            }
            wochentag = HilfsKonstrukte.startWochenTag(_jahr);
            for (monat = 0; monat < 12; monat++) //Schleife für die Erstellung der Monate: 0=Jan bis 11=Dez
            {
                gibZeileAus("      " + HilfsKonstrukte.monatsNamen[monat]);
                if (monat > 0)
                {
                }

                for (tag = 1; tag <= HilfsKonstrukte.tageImMonat[monat]; tag++) //solange Zähler kleiner gleich Wert aus Array Tage 
                {
                    gibZeileAus(HilfsKonstrukte.wochenTagNamenKurz[wochentag] + 
                                " " + tag.ToString().PadLeft(3));
                    if (wochentag == 6) {
                        gibZeileAus("---------------------");
                    }
                    wochentag = (wochentag + 1) % 7;
                }
                //freie Zeilen zwischen den Monaten auf dem Bildschirm 
                gibZeileAus();

            } //ende for Schleife Monat   
            schliesseStream();
        }
    } //ende class TageszeilenAusgabe
} //ende namespace KalenderWelt
