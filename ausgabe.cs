using System;
using System.IO;

namespace KalenderWelt
{

    class DateiUndKonsolenAusgabe
    {

        public static void gibAus(int dasJahr)
        {
            int monat = 1, tag = 1;
            int wochentag = 0, jahre = 0, versatz = 0, ausgabePosition = 0, dateiPosition = 0;
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
            StreamWriter w = f.CreateText();

            w.Write("Kalender fuer das Jahr "); //Jahr in die Datei schreiben
            w.WriteLine(dasJahr);
            w.WriteLine();
            Console.WriteLine();

            for (monat = 1; monat < 13; monat++)
            {

                Console.WriteLine("      " + HilfsKonstrukte.monatsNamen[monat - 1]); //auf Bildschirm ausgeben
                w.WriteLine("      " + HilfsKonstrukte.monatsNamen[monat - 1]); //in Datei schreiben

                Console.WriteLine("MO DI MI DO FR SA SO"); //auf Bildschirm ausgeben
                w.WriteLine("MO DI MI DO FR SA SO"); //in Datei schreiben

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

                switch (wochentag)
                {
                    case 0:                       //Woche beginnt Montags, keine Verschiebung das ersten Tages     
                        break;
                    case 1: Console.Write("   "); //Woche beginnt Dienstags, ein leer Block einfügen
                        w.Write("   ");
                        ausgabePosition = 1;  //ausgabePosition + 1;
                        dateiPosition = 1; //dateiPosition + 1;
                        break;
                    case 2: Console.Write("      "); //Woche beginnt Mittwochs, zwei leere Blöcke einfügen 
                        w.Write("      ");
                        ausgabePosition = 2; //ausgabePosition + 2;
                        dateiPosition = 2;  //dateiPosition + 2;
                        break;
                    case 3: Console.Write("         "); //Woche beginnt Donnerstags, drei leer Blöcke einfügen
                        w.Write("         ");
                        ausgabePosition = 3; //ausgabePosition + 3;
                        dateiPosition = 3;  //dateiPosition + 3;
                        break;
                    case 4: Console.Write("            "); //Woche beginnt Freitags, vier leer Blöcke einfügen 
                        w.Write("            ");
                        ausgabePosition = 4; //ausgabePosition + 4;
                        dateiPosition = 4; //dateiPosition + 4;
                        break;
                    case 5: Console.Write("               "); //Woche beginnt Samstags, fünf leer Blöcke einfügen
                        w.Write("               ");
                        ausgabePosition = 5; //ausgabePosition + 5;
                        dateiPosition = 5;  //dateiPosition + 5;
                        break;
                    case 6: Console.Write("                  "); // Woche geginnt Sonntags, sechs leere Blöcke einfügen
                        w.Write("                  ");
                        ausgabePosition = 6;  //ausgabePosition + 6;
                        dateiPosition = 6;  //dateiPosition + 6;
                        break;
                } //Ende switch Wochentag auswahl


                if (HilfsKonstrukte.istSchaltJahr(dasJahr))
                {
                    if (monat > 2) //In Schaltjahen, ab Monat März "Ausgabe Bug" beheben ??? jedem Monat oder nur bei März ???
                    {
                        Console.Write("   ");
                        w.Write("   ");
                        ausgabePosition++;
                        dateiPosition++;
                    }
                }


                for (tag = 1; tag <= HilfsKonstrukte.tageImMonat[monat - 1]; tag++) //solange Zähler kleiner gleich Wert aus Array Tage 
                {
                    Console.Write(tag); //auf Bildschirm ausgeben
                    ausgabePosition++; // ausgabePositon eins erhöhen bei jedem Tag
                    if (tag < 10) Console.Write("  ");
                    else Console.Write(" ");
                    if (ausgabePosition > 6)
                    {
                        Console.WriteLine(); //wenn hinterste Position erreicht Zeihlenumbruch einfügen und Bildschirm-Positionszähler zurücksetzen
                        ausgabePosition = 0;
                    }

                    w.Write(tag); //in Datei schreiben
                    dateiPosition++; // dateiPositon eins erhöhen bei jedem Tag
                    if (tag < 10) w.Write("  ");
                    else w.Write(" ");
                    if (dateiPosition > 6)
                    {
                        w.WriteLine(); //wenn hinterste Position erreicht Zeihlenumbruch einfügen und Datei-Positionszähler zurücksetzen
                        dateiPosition = 0;
                    }
                }
                Console.WriteLine(); //freie Zeilen zwischen den Monaten auf dem Bildschirm 
                Console.WriteLine();

                w.WriteLine(); //freie Zeilen zwischen den Monaten in der Datei
                w.WriteLine();

            } //ende for Schleife Monat   

            w.Close(); //Datei schliesen
        }
    } //ende class DateiUndKonsolenAusgabe
} //ende namespace KalenderWelt
