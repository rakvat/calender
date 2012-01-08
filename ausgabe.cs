using System;
using System.IO;

namespace KalenderWelt
{

    class DateiUndKonsolenAusgabe
    {

        public static void gibAus(int dasJahr)
        {
            int monat = 1, tag = 1;
            int wochentag = 0, jahre = 0, schaltjahr = 0, versatz = 0;
            int[] monate = new int[12] { 0, 3, 3, 6, 1, 4, 6, 2, 5, 0, 3, 5 };
            int[] tage = new int[12] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            string jahr = Convert.ToString(dasJahr);
            string Dateiname;
            Dateiname = jahr.Insert(4, ".txt");
            jahr = Dateiname;
            Dateiname = jahr.Insert(0, "Kalender");
            FileInfo f = new FileInfo("kalenderausgabe/"+Dateiname); //Text Datei anlegen 
            StreamWriter w = f.CreateText();

            w.Write("Kalender fuer das Jahr "); //Jahr in die Datei schreiben
            w.WriteLine(dasJahr);
            w.WriteLine();
            Console.WriteLine();

            for (monat = 1; monat < 13; monat++)
            {

                switch (monat)//Gibt den Namen des Monats aus
                {
                    case 1: Console.WriteLine("      Januar"); //auf Bildschirm ausgeben
                        w.WriteLine("      Januar"); //in Datei schreiben
                        break;
                    case 2: Console.WriteLine("      Februar"); //auf Bildschirm ausgeben
                        w.WriteLine("      Februar"); //in Datei schreiben
                        break;
                    case 3: Console.WriteLine("      Maerz"); //auf Bildschirm ausgeben
                        w.WriteLine("      Maerz"); //in Datei schreiben
                        break;
                    case 4: Console.WriteLine("      April"); //auf Bildschirm ausgeben
                        w.WriteLine("      April"); //in Datei schreiben
                        break;
                    case 5: Console.WriteLine("      Mai"); //auf Bildschirm ausgeben
                        w.WriteLine("      Mai"); //in Datei schreiben
                        break;
                    case 6: Console.WriteLine("      Juni"); //auf Bildschirm ausgeben
                        w.WriteLine("      Juni"); //in Datei schreiben
                        break;
                    case 7: Console.WriteLine("      Juli"); //auf Bildschirm ausgeben
                        w.WriteLine("      Juli"); //in Datei schreiben
                        break;
                    case 8: Console.WriteLine("      August"); //auf Bildschirm ausgeben
                        w.WriteLine("      August"); //in Datei schreiben
                        break;
                    case 9: Console.WriteLine("      September"); //auf Bildschirm ausgeben
                        w.WriteLine("      September"); //in Datei schreiben
                        break;
                    case 10: Console.WriteLine("      Oktober"); //auf Bildschirm ausgeben
                        w.WriteLine("      Oktober"); //in Datei schreiben
                        break;
                    case 11: Console.WriteLine("      November"); //auf Bildschirm ausgeben
                        w.WriteLine("      November"); //in Datei schreiben
                        break;
                    case 12: Console.WriteLine("      Dezember"); //auf Bildschirm ausgeben
                        w.WriteLine("      Dezember"); //in Datei schreiben
                        break;
                    default: Console.WriteLine("Falsche Eingabe. Nur 1 bis 12 erlaubt."); //auf Bildschirm ausgeben
                        w.WriteLine("Falsche Eingabe. Nur 1 bis 12 erlaubt."); //in Datei schreiben     
                        break;
                }//Ende switch Monatsname

                if (dasJahr >= 1900) jahre = dasJahr - 1900;
                else jahre = 1900 - dasJahr;
                schaltjahr = jahre / 4;
                versatz = jahre + schaltjahr;
                versatz = versatz + monate[monat - 1];

                if (((dasJahr % 4 == 0) && (!(dasJahr % 100 == 0))) || ((dasJahr % 100 == 0) && (dasJahr % 400 == 0)))
                {
                    versatz--;
                    tage[1] = 29; //Februar Feld im Arrey bei Schaltjahren auf 29 setzen
                }

                wochentag = versatz % 7;

                switch (wochentag)
                {
                    case 0: Console.WriteLine("MO DI MI DO FR SA SO"); //auf Bildschirm ausgeben
                        w.WriteLine("MO DI MI DO FR SA SO"); //in Datei schreiben
                        break;
                    case 1: Console.WriteLine("DI MI DO FR SA SO MO"); //auf Bildschirm ausgeben
                        w.WriteLine("DI MI DO FR SA SO MO"); //in Datei schreiben
                        break;
                    case 2: Console.WriteLine("MI DO FR SA SO MO DI"); //auf Bildschirm ausgeben 
                        w.WriteLine("MI DO FR SA SO MO DI"); //in Datei schreiben
                        break;
                    case 3: Console.WriteLine("DO FR SA SO MO DI MI"); //auf Bildschirm ausgeben
                        w.WriteLine("DO FR SA SO MO DI MI"); //in Datei schreiben
                        break;
                    case 4: Console.WriteLine("FR SA SO MO DI MI DO"); //auf Bildschirm ausgeben
                        w.WriteLine("FR SA SO MO DI MI DO"); //in Datei schreiben
                        break;
                    case 5: Console.WriteLine("SA SO MO DI MI DO FR"); //auf Bildschirm ausgeben
                        w.WriteLine("SA SO MO DI MI DO FR"); //in Datei schreiben
                        break;
                    case 6: Console.WriteLine("SO MO DI MI DO FR SA"); //auf Bildschirm ausgeben
                        w.WriteLine("SO MO DI MI DO FR SA"); //in Datei schreiben
                        break;
                } //Ende switch Wochentag auswahl

                for (tag = 1; tag <= tage[monat - 1]; tag++) //solange Zähler kleiner gleich Wert aus Arrey Tage 
                {
                    Console.Write(tag); //auf Bildschirm ausgeben
                    if (tag < 10) Console.Write("  ");
                    else Console.Write(" ");
                    if (tag == 7) Console.WriteLine();
                    if (tag == 14) Console.WriteLine();
                    if (tag == 21) Console.WriteLine();
                    if (tag == 28) Console.WriteLine();


                    w.Write(tag); //in Datei schreiben
                    if (tag < 10) w.Write("  ");
                    else w.Write(" ");
                    if (tag == 7) w.WriteLine();
                    if (tag == 14) w.WriteLine();
                    if (tag == 21) w.WriteLine();
                    if (tag == 28) w.WriteLine();
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

