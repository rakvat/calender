using System;
using System.IO;

namespace KalenderWelt
{

    class DateiUndKonsolenAusgabe
    {

        private StreamWriter _streamWriter;

        public void gibZeileAus() {
            gibZeileAus("");
        }
        public void gibZeileAus(string dieZeile) {
            _streamWriter.WriteLine(dieZeile);
            Console.WriteLine(dieZeile);
        }

        public void gibAus(int dasJahr)
        {
            int monat = 1, tag = 1;
            int wochentag = 0, jahre = 0, versatz = 0;
            int[] monate = new int[12] { 0, 3, 3, 6, 1, 4, 6, 2, 5, 0, 3, 5 };

            string jahr = Convert.ToString(dasJahr);
            string Dateiname;
            Dateiname = jahr.Insert(4, ".txt");
            jahr = Dateiname;
            Dateiname = jahr.Insert(0, "Kalender");
            if (!System.IO.Directory.Exists("kalenderausgabe/")) { 
                Directory.CreateDirectory("kalenderausgabe"); //anlegen des Ordners wenn nicht vorhanden
            }
            FileInfo f = new FileInfo("kalenderausgabe/" + Dateiname); //Text Datei anlegen 
            _streamWriter = f.CreateText();

            gibZeileAus("Kalender fuer das Jahr " + dasJahr);
            gibZeileAus();

            for (monat = 1; monat < 13; monat++)
            {
                gibZeileAus("      " + HilfsKonstrukte.monatsNamen[monat - 1]); 
                gibZeileAus("MO DI MI DO FR SA SO"); 

                if (dasJahr >= 1900) jahre = dasJahr - 1900;
                else jahre = 1900 - dasJahr;
                versatz = jahre + jahre / 4;
                versatz = versatz + monate[monat - 1];

                if (HilfsKonstrukte.istSchaltJahr(dasJahr))
                {
                    versatz--;
                    HilfsKonstrukte.tageImMonat[1] = 29; //Februar Feld im Array bei Schaltjahren auf 29 setzen
                }


                wochentag = versatz % 7;

                //die Woche beginnt immer Montags 
                int ausgabePosition = wochentag;

                //In Schaltjahen, ab Monat März "Ausgabe Bug" beheben ??? jedem Monat oder nur bei März ???
                if (HilfsKonstrukte.istSchaltJahr(dasJahr) && monat > 2) 
                {
                    ausgabePosition++;
                }


                string zeile = "".PadLeft(ausgabePosition * 3);
                for (tag = 1; tag <= HilfsKonstrukte.tageImMonat[monat - 1]; tag++) //solange Zähler kleiner gleich Wert aus Array Tage 
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
                if (zeile.Length > 0) {
                    gibZeileAus(zeile);
                }
                //freie Zeilen zwischen den Monaten auf dem Bildschirm 
                gibZeileAus();

            } //ende for Schleife Monat   

            _streamWriter.Close(); //Datei schliesen
        }
    } //ende class DateiUndKonsolenAusgabe
} //ende namespace KalenderWelt
