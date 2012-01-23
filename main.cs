using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;

namespace KalenderWelt
{
    class KalenderErzeuger
    {

        public static int Main()
        {

            int eingabejahr;
            int eingabemodus;

            Console.WriteLine("Das Programm berechnet Kalender für die Jahre 1583 bis 4000");
            Console.Write("Jahr eingeben:  ");

            bool eingabeOk = false;
            do
            {
                string eingabe = Console.ReadLine(); //erstmal als string speichern, damit auch eigegebene Buchstaben kein Problem sind
                if (!int.TryParse(eingabe, out eingabejahr)) {
                    Console.WriteLine("Falsche Eingabe, da Buchstaben nicht möglich sind.");
                } else if ((eingabejahr < 1583) || (eingabejahr > 4000)) {
                    Console.WriteLine("Falsche Eingabe. Jahr ist nicht im gueltigen Bereich.");
                } else {
                    eingabeOk = true;
                }
                if (!eingabeOk) {
                    Console.Write("Bitte geben sie das Jahr erneut ein:  ");
                }
            } while (!eingabeOk);

            Console.Write("Soll die Ausgabe \n1) Monatsblock einspaltig oder\n2) Monatsblock zweispaltig oder\n3) Zeilenweise sein?  ");
            eingabeOk = false;
            do
            {
                string eingabe = Console.ReadLine();
                if (!int.TryParse(eingabe, out eingabemodus)) {
                    Console.WriteLine("Falsche Eingabe, da Buchstaben nicht möglich sind.");
                } else if ((eingabemodus < 1) || (eingabemodus > 3)) {
                    Console.WriteLine("Falsche Eingabe. Modus ist nicht im gueltigen Bereich.");
                } else {
                    eingabeOk = true;
                }
                if (!eingabeOk) {
                    Console.Write("Bitte geben sie den Modus erneut ein:  ");
                }
            } while (!eingabeOk);
            Console.WriteLine("");

            Ausgabe ausgabe = null;
            switch (eingabemodus) {
                case 1: ausgabe = new MonatsBlockAusgabe(eingabejahr);
                        break;
                case 2: ausgabe = new MonatsBlockAusgabe2Spaltig(eingabejahr);
                        break;
                case 3: ausgabe = new TageszeilenAusgabe(eingabejahr);
                        break;
            }
            ausgabe.gibAus();

            //Eintrag Experimente
            List<Eintrag> eintraege = new List<Eintrag> ();
            eintraege.Add(new EinmaligerTermin("beruflicher Termin", new DateTime(2012,1,23)));

            //aus xml laden
            XmlDocument doc = new XmlDocument();
            doc.Load("input/geburtstage.xml");
            XmlElement wurzel = doc.DocumentElement;
            XmlNodeList eintragListe = wurzel.SelectNodes("./JaehrlichesEreignisAnFestemTag");
            Console.WriteLine("anzahl der Eintraege in der xml " + eintragListe.Count); 
            foreach (XmlNode eintrag in eintragListe)
            {
                //Console.WriteLine(eintrag.Attributes["titel"].Value);
                //Console.WriteLine(eintrag.SelectSingleNode("./monat").OuterXml);
                //Console.WriteLine(eintrag.SelectSingleNode("./monat").FirstChild.Value);
                string titel = eintrag.Attributes["titel"].Value;
                int monat, tag;
                if (!int.TryParse(eintrag.SelectSingleNode("./monat").FirstChild.Value, out monat)) {
                    Console.WriteLine("Monat ist keine Zahl");
                }
                if (!int.TryParse(eintrag.SelectSingleNode("./tag").FirstChild.Value, out tag)) {
                    Console.WriteLine("Tag ist keine Zahl");
                }
                eintraege.Add(new JaehrlichesEreignisAnFestemTag(titel, monat, tag)); 
            }

            Console.WriteLine("alle Eintraege: ");
            foreach (Eintrag meinEintrag in eintraege) {
                Console.WriteLine("Eintrag: " + meinEintrag.toString());
            }

            //Erwartet Eingabe vor Beendigung des Programms
            Console.ReadLine();
            return 0;
        } //ende main()
    } //ende class KalenderErzeuger
} //ende namespace KalenderWelt
