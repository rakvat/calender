#define DEBUG

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.XPath;


namespace KalenderWelt
{

    class KalenderErzeuger
    {

        public static int Main(string[] args)
        {
            bool debug = false;
            int eingabejahr = 0;
            int eingabemodus = 0;

            if (args.Length == 0) //Wenn keine Argumente übergeben werden Anzeige Normaler Start
            {
                Console.WriteLine("Normaler Programm Start");
                debug = false;
            }
            else
            {
                if (args[0] == "debug") //Wenn Argument übergeben worden, prüfen ob debug erwünscht ist
                {
                    Console.WriteLine("Programm Start mit debug Mode. Anzahl der übergebenen Parameter: " + args.Length);
                    debug = true;
                }
            }

            Console.WriteLine("Das Programm berechnet Kalender für die Jahre 1583 bis 4000");
            Console.Write("Jahr eingeben:  ");

            bool eingabeOk = false;
            do
            {
                string eingabe = Console.ReadLine(); //erstmal als string speichern, damit auch eigegebene Buchstaben kein Problem sind
                if (!int.TryParse(eingabe, out eingabejahr))
                {
                    Console.WriteLine("Falsche Eingabe, da Buchstaben nicht möglich sind.");
                }
                else if ((eingabejahr < 1583) || (eingabejahr > 4000))
                {
                    Console.WriteLine("Falsche Eingabe. Jahr ist nicht im gueltigen Bereich.");
                }
                else
                {
                    eingabeOk = true;
                }
                if (!eingabeOk)
                {
                    Console.Write("Bitte geben sie das Jahr erneut ein:  ");
                }
            } while (!eingabeOk);

            Console.Write("Soll die Ausgabe \n1) Monatsblock einspaltig oder\n2) Monatsblock zweispaltig oder\n3) Zeilenweise oder\n4) Zeilenweise mit Eintraegen sein?  ");
            eingabeOk = false;
            do
            {
                string eingabe = Console.ReadLine();
                if (!int.TryParse(eingabe, out eingabemodus))
                {
                    Console.WriteLine("Falsche Eingabe, da Buchstaben nicht möglich sind.");
                }
                else if ((eingabemodus < 1) || (eingabemodus > 4))
                {
                    Console.WriteLine("Falsche Eingabe. Modus ist nicht im gueltigen Bereich.");
                }
                else
                {
                    eingabeOk = true;
                }
                if (!eingabeOk)
                {
                    Console.Write("Bitte geben sie den Modus erneut ein:  ");
                }
            } while (!eingabeOk);
            Console.WriteLine("");


            //Eintrag Experimente
            List<Eintrag> eintraege = leseEintraege(debug); //Aufruf der leseEintraege Funktion mit Übergabe der debug Variablen

            KalenderJahr kalenderJahr = new KalenderJahr(eingabejahr);
            kalenderJahr.TrageEin(ref eintraege);

            Ausgabe ausgabe = null;
            switch (eingabemodus)
            {
                case 1: ausgabe = new MonatsBlockAusgabe(ref kalenderJahr, eingabemodus);
                    break;
                case 2: ausgabe = new MonatsBlockAusgabe2Spaltig(ref kalenderJahr, eingabemodus);
                    break;
                case 3: ausgabe = new TageszeilenAusgabe(ref kalenderJahr, eingabemodus, false);
                    break;
                case 4: ausgabe = new TageszeilenAusgabe(ref kalenderJahr, eingabemodus, true);
                    break;
            }
            ausgabe.gibAus();

#if DEBUG
            ausgabeTest(eingabejahr, eingabemodus, debug); //Aufruf der Testfunktion mit Übergabe der debug Variable
#endif

            //Erwartet Eingabe vor Beendigung des Programms
            Console.ReadLine();
            return 0;
        } //ende main()


        static List<Eintrag> leseEintraege(bool debug)
        {
            List<Eintrag> meineEintraege = new List<Eintrag>();
            if (!System.IO.Directory.Exists(Eintrag.INPUT_DIR))
            {
                Console.WriteLine("xml Dateien nicht gefunden.");
                return meineEintraege;
            }

            meineEintraege = Eintrag.LeseEintraegeAusVerzeichnis(Eintrag.INPUT_DIR);

            //fuer debug
            if (debug == true)
            {
                foreach (Eintrag meinEintrag in meineEintraege)
                {
                    Console.WriteLine("debug info Eintrag: " + meinEintrag.toString());
                }
            }
            return meineEintraege;
        } //ende leseEintraege

#if DEBUG
        static void ausgabeTest(int eingabejahr, int eingabemodus, bool debug)
        {
            //----------Anfang TEST Code zum vergleichen des erzeugten Kalenders mit dem im Verzeichnis /testfixtures abgeletem Muster ------------
            string[] eingabetext = new string[600];
            string[] mustertext = new string[600];
            int korekt = 100, zeilen1 = 0, zeilen2 = 0;

            string fileName = Ausgabe.AUSGABE_DIR + eingabemodus + "Kalender" + eingabejahr + ".txt";
            if (!File.Exists(fileName)) //prüfen ob .txt Datei vorhanden ist
            {
                Console.WriteLine("Eingabe Datei '" + fileName + "' nicht gefunden.");
                return;
            }
            FileStream fs = new FileStream(fileName, FileMode.Open);  //angegebene Text Datei öffnen
            StreamReader sr = new StreamReader(fs);      //streamrader anlegen
            while (sr.Peek() >= 0)                     //Text Datei auslesen solange etwas vorhanden ist
            {
                eingabetext[zeilen1] = sr.ReadLine();                //Text Datei Inhalt in Veriable übertragen 
                zeilen1++;                                           //ermitteln der Datei länge, wichtig für Vergleich mit for Schleife
                //   MessageBox.Show(eingabe[i]);                    //Inhalt anzeigen    
            }
            sr.Close(); //Eingabe Datei schliesen  


            fileName = "testfixtures/" + eingabemodus + "Kalender" + eingabejahr + ".txt";
            if (!File.Exists(fileName)) //prüfen ob .txt Datei vorhanden ist
            {
                Console.WriteLine("Test Datei '" + fileName + "' nicht gefunden.");
                return;
            }
            FileStream fs2 = new FileStream(fileName, FileMode.Open);  //angegebene Text Datei öffnen
            StreamReader sr2 = new StreamReader(fs2);   //streamrader anlegen
            while (sr2.Peek() >= 0)                     //Text Datei auslesen solange etwas vorhanden ist
            {
                mustertext[zeilen2] = sr2.ReadLine();                 //Text Datei Inhalt in Veriable übertragen
                zeilen2++;                                            //ermitteln der Datei länge, wichtig für Vergleich mit for Schleife
                // MessageBox.Show(muster[i]);                        //Inhalt anzeigen 
            }
            sr2.Close(); //Test Datei schliesen  

            if (debug == true)
            {
                Console.WriteLine("debug info: Anzahl der Zeilen in Eingabe Datei " + zeilen1);
                Console.WriteLine("debug info: Anzahl der Zeilen in Test Datei " + zeilen2);
            }

            for (zeilen1 = 0; zeilen1 < zeilen2; zeilen1++) //Vergleich läuft so lange wie Daten vorhanden sind
            {
                if (eingabetext[zeilen1].Equals(mustertext[zeilen1])) korekt = korekt - 0; //bei jedem Fehler der beim Vergleich auftritt 1 abziehen
                else
                {
                    korekt = korekt - 1;
                }
            }

            Console.WriteLine("\nTest wird durchgeführt für das Kalenderjahr: " + eingabejahr + " mit dem Eingabebemodus: " + eingabemodus);
            if (korekt == 100) Console.WriteLine("Ergebnis: Kalender sind gleich " + korekt + "% Übereinstimmung");  //wenn die Variable am Ende noch bei 100 ist sind beide Text Dateien gleich.
            else Console.WriteLine("Ergebnis: Kalender sind verschieden, " + korekt + "% Übereinstimmung"); //die Variabel korekt entspricht in etwar der Übeinstimmung in %
            //------------ Ende TEST Code --------------------------------------------------------------------------
        } //ende ausgbabeTest
#endif
    } //ende class KalenderErzeuger
} //ende namespace KalenderWelt
