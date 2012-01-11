using System;
using System.IO;

namespace KalenderWelt
{

    class DateiUndKonsolenAusgabe
    {

        private StreamWriter _streamWriter;

        public void gibZeileAus() //erzeugt Leere Zeilen, ohne �bergabewert
        {
            gibZeileAus("");
        }
        public void gibZeileAus(string dieZeile) //gibt zusammengebaute Kalenderzeilen aus, mit �bergabewert
        {
            _streamWriter.WriteLine(dieZeile);
            Console.WriteLine(dieZeile);
        }

        public void gibAus(int dasJahr)
        {
            int monat = 0, tag = 0; //alle Veriablen fangen jetzt mit null an, das erspart das [monat-1] im Code
            int wochentag = 0, jahre = 0, versatz = 0;
            // Ja Fe M� Ap Ma Ju Jl Ag Sp Ok Nv Dz, bei M�rz versatz von 3 auf 4 �ndern f�r Schaltjahren wie 2012
            int[] monate = new int[12] { 0, 3, 3, 6, 1, 4, 6, 2, 5, 0, 3, 5 };
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

            for (monat = 0; monat < 12; monat++) //Schleife f�r die Erstellung der Monate: 0=Jan bis 11=Dez
            {
                gibZeileAus("      " + HilfsKonstrukte.monatsNamen[monat]);
                gibZeileAus("MO DI MI DO FR SA SO");

                if (dasJahr >= 1900) jahre = dasJahr - 1900; //vorarbeiten zur Berechnug des passenden Wochentags
                else jahre = 1900 - dasJahr;
                versatz = jahre + jahre / 4;
                versatz = versatz + monate[monat];

                if (HilfsKonstrukte.istSchaltJahr(dasJahr))
                {
                    versatz--;
                    HilfsKonstrukte.tageImMonat[1] = 29; //Februar Feld im Array bei Schaltjahren auf 29 setzen
                    monate[2] = 4; //versatz von M�rz bei Schaltjahren korigieren, mu� eins mehr sein als sonst
                }

                wochentag = versatz % 7; //Bestimmung des Wochentags durch die erechnete Ver�nderung seit 1900

                //Die Woche beginnt immer Montags, nur der 1. eines Monats kann auf verschiedene Wochentage fallen. 
                int ausgabePosition = wochentag;

                //In Schaltjahen, ab Monat M�rz "Ausgabe Bug" beheben 
                if (HilfsKonstrukte.istSchaltJahr(dasJahr) && monat > 2)
                {
                    ausgabePosition = (ausgabePosition + 1) % 7;
                }

                string zeile = "".PadLeft(ausgabePosition * 3);
                for (tag = 1; tag <= HilfsKonstrukte.tageImMonat[monat]; tag++) //solange Z�hler kleiner gleich Wert aus Array Tage 
                {
                    zeile += tag.ToString().PadRight(3);
                    ausgabePosition++;

                    //wenn hinterste Position erreicht Zeihlenumbruch einf�gen und Bildschirm-Positionsz�hler zur�cksetzen
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
