using System;
using System.IO;

namespace KalenderWelt
{

    public class DateiUndKonsolenAusgabe
    {
        int woche = 0;  //zählt die Anzahl der augegeben Zeilen, dient zur Positionierung bei der zweispaltigen Ausgabe
        string auswahl; //Speichert auswahl der Benutzer: einspaltig oder zweispaltige Ausgabe
        private StreamWriter _streamWriter;

        public void gibZeileAus() //erzeugt Leere Zeilen, ohne übergabewert
        {
            gibZeileAus("");
        }
        public void gibZeileAus(string dieZeile) //gibt zusammengebaute Kalenderzeilen aus, mit übergabewert
        {
            if (auswahl == "2") //Wenn Ausgabe in zwei spalten gewünscht
            {
                int x = 40, y = 5; //Variablen zum bestimmen des Platzes der Consolen Ausgabe
                woche++;

                if (woche == 52) Console.SetCursorPosition(x, y);     /////JULI////
                if (woche == 53) Console.SetCursorPosition(x, y = 6); //für Monate Juli bis Dezember rechts eine zweite Spalte anlegen
                if (woche == 54) Console.SetCursorPosition(x, y = 7);
                if (woche == 55) Console.SetCursorPosition(x, y = 8);
                if (woche == 56) Console.SetCursorPosition(x, y = 9);
                if (woche == 57) Console.SetCursorPosition(x, y = 10);
                if (woche == 58) Console.SetCursorPosition(x, y = 11);
                if (woche == 59) Console.SetCursorPosition(x, y = 12);

                if (woche == 60) Console.SetCursorPosition(x, y = 13); /////AUGUST////
                if (woche == 61) Console.SetCursorPosition(x, y = 14); //für Monate Juli bis Dezember rechts eine zweite Spalte anlegen
                if (woche == 62) Console.SetCursorPosition(x, y = 15);
                if (woche == 63) Console.SetCursorPosition(x, y = 16);
                if (woche == 64) Console.SetCursorPosition(x, y = 17);
                if (woche == 65) Console.SetCursorPosition(x, y = 18);
                if (woche == 66) Console.SetCursorPosition(x, y = 19);
                if (woche == 67) Console.SetCursorPosition(x, y = 20);

                if (woche == 68) Console.SetCursorPosition(x, y = 21); ////SEPTEMBER////
                if (woche == 69) Console.SetCursorPosition(x, y = 22); //für Monate Juli bis Dezember rechts eine zweite Spalte anlegen
                if (woche == 70) Console.SetCursorPosition(x, y = 23);
                if (woche == 71) Console.SetCursorPosition(x, y = 24);
                if (woche == 72) Console.SetCursorPosition(x, y = 25);
                if (woche == 73) Console.SetCursorPosition(x, y = 26);
                if (woche == 74) Console.SetCursorPosition(x, y = 27);
                if (woche == 75) Console.SetCursorPosition(x, y = 28);

                if (woche == 76) Console.SetCursorPosition(x, y = 29); ////OKTOBER////
                if (woche == 77) Console.SetCursorPosition(x, y = 30); //für Monate Juli bis Dezember rechts eine zweite Spalte anlegen
                if (woche == 78) Console.SetCursorPosition(x, y = 31);
                if (woche == 79) Console.SetCursorPosition(x, y = 32);
                if (woche == 80) Console.SetCursorPosition(x, y = 33);
                if (woche == 81) Console.SetCursorPosition(x, y = 34);
                if (woche == 82) Console.SetCursorPosition(x, y = 35);
                if (woche == 83) Console.SetCursorPosition(x, y = 36);

                if (woche == 84) Console.SetCursorPosition(x, y = 37); ////NOVEMBER////
                if (woche == 85) Console.SetCursorPosition(x, y = 38); //für Monate Juli bis Dezember rechts eine zweite Spalte anlegen
                if (woche == 86) Console.SetCursorPosition(x, y = 39);
                if (woche == 87) Console.SetCursorPosition(x, y = 40);
                if (woche == 88) Console.SetCursorPosition(x, y = 41);
                if (woche == 89) Console.SetCursorPosition(x, y = 42);
                if (woche == 90) Console.SetCursorPosition(x, y = 43);
                if (woche == 91) Console.SetCursorPosition(x, y = 44);

                if (woche == 92) Console.SetCursorPosition(x, y = 45); ////Dezember////
                if (woche == 93) Console.SetCursorPosition(x, y = 46); //für Monate Juli bis Dezember rechts eine zweite Spalte anlegen
                if (woche == 94) Console.SetCursorPosition(x, y = 47);
                if (woche == 95) Console.SetCursorPosition(x, y = 48);
                if (woche == 96) Console.SetCursorPosition(x, y = 49);
                if (woche == 97) Console.SetCursorPosition(x, y = 50);
                if (woche == 98) Console.SetCursorPosition(x, y = 51);
                if (woche == 99) Console.SetCursorPosition(x, y = 52);
            } //Ende der Auswahl 2) Ausgabe in zwei Spalten   
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

            Console.Write("Soll die Ausgabe 1)einspaltig  oder  2)zweispaltig sein ?  ");
            auswahl = Console.ReadLine();
            Console.WriteLine("");

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
            Console.SetWindowSize(80, 60); //gibt Größe des Fensters an: Weite=80 Zeichen und Höhe=60 Zeichen, so das viele Monate auf einmal sichtbar sind.
            Console.WindowTop = 0; //Coursor steht am oberen Rand der Console, spart das hochscrollen am Programmende
            _streamWriter.Close(); //Datei schliesen
        }
    } //ende class DateiUndKonsolenAusgabe
} //ende namespace KalenderWelt
