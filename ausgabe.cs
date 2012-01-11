using System;
using System.IO;

namespace KalenderWelt
{

    class DateiUndKonsolenAusgabe
    {

        private StreamWriter _streamWriter;

        public void gibZeileAus() //erzeugt Leere Zeilen, ohne übergabewert
        {
            gibZeileAus("");
        }
        public void gibZeileAus(string dieZeile) //gibt zusammengebaute Kalenderzeilen aus, mit übergabewert
        {
            _streamWriter.WriteLine(dieZeile);
            Console.WriteLine(dieZeile);
        }

        public void gibAus(int dasJahr)
        {
            int monat, tag; 
            int wochentagDesErstenImMonat = 0; //Mo = 0, ...
            string jahr = Convert.ToString(dasJahr);
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

            gibZeileAus("Kalender fuer das Jahr " + dasJahr);
            gibZeileAus();
            string wochentage = String.Join(" ", HilfsKonstrukte.wochenTagNamenKurz);

            if (HilfsKonstrukte.istSchaltJahr(dasJahr))
            {
                HilfsKonstrukte.tageImMonat[1] = 29; //Februar Feld im Array bei Schaltjahren auf 29 setzen
            }
            wochentagDesErstenImMonat = HilfsKonstrukte.startWochenTag(dasJahr);
            for (monat = 0; monat < 12; monat++) //Schleife für die Erstellung der Monate: 0=Jan bis 11=Dez
            {
                gibZeileAus("      " + HilfsKonstrukte.monatsNamen[monat]);
                gibZeileAus(wochentage);

                if (monat > 0 ) 
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

            _streamWriter.Close(); //Datei schliesen
        }
    } //ende class DateiUndKonsolenAusgabe
} //ende namespace KalenderWelt
