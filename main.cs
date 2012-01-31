using System;
using System.IO;
using System.Collections.Generic;
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

            if (args.Length == 0) //Wenn keine Argumente �bergeben werden Anzeige Normaler Start
            {
                Console.WriteLine("Normaler Programm Start");
                debug = false;
            }
            else
            {
                if (args[0] == "debug") //Wenn Argument �bergeben worden, pr�fen ob debug erw�nscht ist
                {
                    Console.WriteLine("Programm Start mit debug Mode. Anzahl der �bergebenen Parameter: " + args.Length);
                    debug = true;
                }
            }

            Console.WriteLine("Das Programm berechnet Kalender f�r die Jahre 1583 bis 4000");
            Console.Write("Jahr eingeben:  ");

            bool eingabeOk = false;
            do
            {
                string eingabe = Console.ReadLine(); //erstmal als string speichern, damit auch eigegebene Buchstaben kein Problem sind
                if (!int.TryParse(eingabe, out eingabejahr))
                {
                    Console.WriteLine("Falsche Eingabe, da Buchstaben nicht m�glich sind.");
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
                    Console.WriteLine("Falsche Eingabe, da Buchstaben nicht m�glich sind.");
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
            List<Eintrag> eintraege = leseEintraege(debug); //Aufruf der leseEintraege Funktion mit �bergabe der debug Variablen

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

            ausgabeTest(eingabejahr, eingabemodus, debug); //Aufruf der Testfunktion mit �bergabe der debug Variable

            //Erwartet Eingabe vor Beendigung des Programms
            Console.ReadLine();
            return 0;
        } //ende main()


        static List<Eintrag> leseEintraege(bool debug)
        {
            List<Eintrag> meineEintraege = new List<Eintrag>();
            if (!System.IO.Directory.Exists("input/"))
            {
                Directory.CreateDirectory("input"); //anlegen des Ordners wenn nicht vorhanden
                Console.WriteLine("xml Dateien nicht gefunden.");
                return meineEintraege;
            }

            string[] eintragTypen = {"JaehrlichesEreignisAnFestemTag", 
                                     "EinmaligerTermin"};
            string eintragVerzeichnis = @"input/";
            DirectoryInfo d = new DirectoryInfo(eintragVerzeichnis);
            foreach (FileInfo f in d.GetFiles("*.xml"))
            {
                Console.WriteLine("Lese Eintraege aus " + f.Name + ";  " + f.Length + "; " + f.CreationTime);
                XmlDocument doc = new XmlDocument();  //aus xml laden
                doc.Load(eintragVerzeichnis + "/" + f.Name);
                XmlElement wurzel = doc.DocumentElement;

                for (int i = 0; i < eintragTypen.Length; ++i)
                {
                    string xmlNodeName = eintragTypen[i];
                    XmlNodeList eintragListe = wurzel.SelectNodes("./" + xmlNodeName);
                    Console.WriteLine("\nAnzahl der " + xmlNodeName + " in der xml Datei " + f.Name + ": " + eintragListe.Count);
                    foreach (XmlNode eintrag in eintragListe)
                    {
                        string titel = eintrag.Attributes["titel"].Value;
                        switch (xmlNodeName)
                        {
                            case "JaehrlichesEreignisAnFestemTag":
                                meineEintraege.Add(
                                    new JaehrlichesEreignisAnFestemTag(titel, eintrag));
                                break;
                            case "EinmaligerTermin":
                                meineEintraege.Add(
                                    new EinmaligerTermin(titel, eintrag));
                                break;
                        } //ende switch
                    } //ende foreach XmlNode
                } //ende for Schleife
            } //ende foreach fileinfo

            //fuer debug
            //if (debug == true)
            //{
            //    foreach (Eintrag meinEintrag in eintraege)
            //    {
            //        Console.WriteLine("debug info Eintrag: " + meinEintrag.toString());
            //    }
            //}
            return meineEintraege;
        } //ende leseEintraege

        static void ausgabeTest(int eingabejahr, int eingabemodus, bool debug)
        {
            //----------Anfang TEST Code zum vergleichen des erzeugten Kalenders mit dem im Verzeichnis /test abgeletem Muster ------------
            string[] eingabetext = new string[600];
            string[] mustertext = new string[600];
            int korekt = 100, zeilen1 = 0, zeilen2 = 0;

            string fileName = "kalenderausgabe/" + eingabemodus + "Kalender" + eingabejahr + ".txt";
            if (!File.Exists(fileName)) //pr�fen ob .txt Datei vorhanden ist
            {
                Console.WriteLine("Eingabe Datei '" + fileName + "' nicht gefunden.");
                return;
            }
            FileStream fs = new FileStream(fileName, FileMode.Open);  //angegebene Text Datei �ffnen
            StreamReader sr = new StreamReader(fs);      //streamrader anlegen
            while (sr.Peek() >= 0)                     //Text Datei auslesen solange etwas vorhanden ist
            {
                eingabetext[zeilen1] = sr.ReadLine();                //Text Datei Inhalt in Veriable �bertragen 
                zeilen1++;                                           //ermitteln der Datei l�nge, wichtig f�r Vergleich mit for Schleife
                //   MessageBox.Show(eingabe[i]);                    //Inhalt anzeigen    
            }
            sr.Close(); //Eingabe Datei schliesen  


            fileName = "test/" + eingabemodus + "Kalender" + eingabejahr + ".txt";
            if (!File.Exists(fileName)) //pr�fen ob .txt Datei vorhanden ist
            {
                Console.WriteLine("Test Datei '" + fileName + "' nicht gefunden.");
                return;
            }
            FileStream fs2 = new FileStream("test/" + eingabemodus + "Kalender" + eingabejahr + ".txt", FileMode.Open);  //angegebene Text Datei �ffnen
            StreamReader sr2 = new StreamReader(fs2);   //streamrader anlegen
            while (sr2.Peek() >= 0)                     //Text Datei auslesen solange etwas vorhanden ist
            {
                mustertext[zeilen2] = sr2.ReadLine();                 //Text Datei Inhalt in Veriable �bertragen
                zeilen2++;                                            //ermitteln der Datei l�nge, wichtig f�r Vergleich mit for Schleife
                // MessageBox.Show(muster[i]);                        //Inhalt anzeigen 
            }
            sr2.Close(); //Test Datei schliesen  

            if (debug == true)
            {
                Console.WriteLine("debug info: Anzahl der Zeilen in Eingabe Datei " + zeilen1);
                Console.WriteLine("debug info: Anzahl der Zeilen in Test Datei " + zeilen2);
            }

            for (zeilen1 = 0; zeilen1 < zeilen2; zeilen1++) //Vergleich l�uft so lange wie Daten vorhanden sind
            {
                if (eingabetext[zeilen1].Equals(mustertext[zeilen1])) korekt = korekt - 0; //bei jedem Fehler der beim Vergleich auftritt 1 abziehen
                else
                {
                    korekt = korekt - 1;
                }
            }

            Console.WriteLine("\nTest wird durchgef�hrt f�r das Kalenderjahr: " + eingabejahr + " mit dem Eingabebemodus: " + eingabemodus);
            if (korekt == 100) Console.WriteLine("Ergebnis: Kalender sind gleich " + korekt + "% �bereinstimmung");  //wenn die Variable am Ende noch bei 100 ist sind beide Text Dateien gleich.
            else Console.WriteLine("Ergebnis: Kalender sind verschieden, " + korekt + "% �bereinstimmung"); //die Variabel korekt entspricht in etwar der �beinstimmung in %
            //------------ Ende TEST Code --------------------------------------------------------------------------
        } //ende ausgbabeTest
    } //ende class KalenderErzeuger
} //ende namespace KalenderWelt