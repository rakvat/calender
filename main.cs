using System;
using System.IO;

namespace test
{

    class myClass
    {

        public static int Main()
        {
            int eingabejahr = 1, eingabemonat = 1, eingabetag = 1;
            int wochentag = 0, jahre = 0, schaltjahr = 0, versatz = 0;
            int[] monat = new int[12] { 0, 3, 3, 6, 1, 4, 6, 2, 5, 0, 3, 5 };
            int[] tage = new int[12] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            Console.WriteLine("Das Programm berechnet Kalender für die Jahre 1583 bis 4000");
            Console.Write("Jahr eingeben ");
            eingabejahr = Convert.ToInt32(Console.ReadLine());

            if ((eingabejahr < 1583) || (eingabejahr > 4000)) Console.WriteLine("Falsche Eingabe. Jahr nicht im gueltigen Bereich.");
            else
            {
                string jahr = Convert.ToString(eingabejahr);
                string Dateiname;
                Dateiname = jahr.Insert(4, ".txt");
                jahr = Dateiname;
                Dateiname = jahr.Insert(0, "Kalender");
                FileInfo f = new FileInfo("kalenderausgabe/"+Dateiname); //Text Datei anlegen 
                StreamWriter w = f.CreateText();

                w.Write("Kalender fuer das Jahr "); //Jahr in die Datei schreiben
                w.WriteLine(eingabejahr);
                w.WriteLine();
                Console.WriteLine();

                for (eingabemonat = 1; eingabemonat < 13; eingabemonat++)
                {

                    switch (eingabemonat)//Gibt den Namen des Monats aus
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

                    if (eingabejahr >= 1900) jahre = eingabejahr - 1900;
                    else jahre = 1900 - eingabejahr;
                    schaltjahr = jahre / 4;
                    versatz = jahre + schaltjahr;
                    versatz = versatz + monat[eingabemonat - 1];

                    if (((eingabejahr % 4 == 0) && (!(eingabejahr % 100 == 0))) || ((eingabejahr % 100 == 0) && (eingabejahr % 400 == 0)))
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

                    for (eingabetag = 1; eingabetag <= tage[eingabemonat - 1]; eingabetag++) //solange Zähler kleiner gleich Wert aus Arrey Tage 
                    {
                        Console.Write(eingabetag); //auf Bildschirm ausgeben
                        if (eingabetag < 10) Console.Write("  ");
                        else Console.Write(" ");
                        if (eingabetag == 7) Console.WriteLine();
                        if (eingabetag == 14) Console.WriteLine();
                        if (eingabetag == 21) Console.WriteLine();
                        if (eingabetag == 28) Console.WriteLine();


                        w.Write(eingabetag); //in Datei schreiben
                        if (eingabetag < 10) w.Write("  ");
                        else w.Write(" ");
                        if (eingabetag == 7) w.WriteLine();
                        if (eingabetag == 14) w.WriteLine();
                        if (eingabetag == 21) w.WriteLine();
                        if (eingabetag == 28) w.WriteLine();
                    }
                    Console.WriteLine(); //freie Zeilen zwischen den Monaten auf dem Bildschirm 
                    Console.WriteLine();

                    w.WriteLine(); //freie Zeilen zwischen den Monaten in der Datei
                    w.WriteLine();

                } //ende for Schleife Monat   

                w.Close(); //Datei schliesen
            } //ende if Eingabe gültig
            Console.ReadLine();
            return 0;
        } //ende main()
    } //ende class myClass
} //ende namespace test

