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

            Console.Write("Soll die Ausgabe \n1) Monatsblock einspaltig oder\n2) Monatsblock zweispaltig oder\n3) Zeilenweise sein?  ");
            eingabeOk = false;
            do
            {
                string eingabe = Console.ReadLine();
                if (!int.TryParse(eingabe, out eingabemodus))
                {
                    Console.WriteLine("Falsche Eingabe, da Buchstaben nicht möglich sind.");
                }
                else if ((eingabemodus < 1) || (eingabemodus > 3))
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

            Ausgabe ausgabe = null;
            switch (eingabemodus)
            {
                case 1: ausgabe = new MonatsBlockAusgabe(eingabejahr);
                    break;
                case 2: ausgabe = new MonatsBlockAusgabe2Spaltig(eingabejahr);
                    break;
                case 3: ausgabe = new TageszeilenAusgabe(eingabejahr);
                    break;
            }
            ausgabe.gibAus();

            //Eintrag Experimente
            List<Eintrag> eintraege = new List<Eintrag>();
            //eintraege.Add(new EinmaligerTermin("beruflicher Termin", new DateTime(2012, 1, 23)));

            XmlDocument doc = new XmlDocument();  //aus xml laden
            if (!System.IO.Directory.Exists("input/"))
            {
                Directory.CreateDirectory("input"); //anlegen des Ordners wenn nicht vorhanden
                Console.WriteLine("xml Dateien nicht gefunden.");
            }
            else //wenn Ordner vorhanden Datei öffen 
            {
                doc.Load("input/geburtstage.xml");
                XmlElement wurzel = doc.DocumentElement;
                XmlNodeList eintragListe = wurzel.SelectNodes("./JaehrlichesEreignisAnFestemTag");
                Console.WriteLine("\nAnzahl der Eintraege in der xml Datei Geburtstage: " + eintragListe.Count);
                foreach (XmlNode eintrag in eintragListe)
                {
                    string titel = eintrag.Attributes["titel"].Value;
                    int monat, tag;
                    if (!int.TryParse(eintrag.SelectSingleNode("./monat").FirstChild.Value, out monat))
                    {
                        Console.WriteLine("Monat ist keine Zahl");
                    }
                    if (!int.TryParse(eintrag.SelectSingleNode("./tag").FirstChild.Value, out tag))
                    {
                        Console.WriteLine("Tag ist keine Zahl");
                    }
                    eintraege.Add(new JaehrlichesEreignisAnFestemTag(titel, monat, tag));
                }

                foreach (Eintrag meinEintrag in eintraege)
                {
                    Console.WriteLine("Eintrag: " + meinEintrag.toString());
                }


                doc.Load("input/aktuelleTermine.xml"); //zweites xml Dokument laden
                XmlElement wurzel2 = doc.DocumentElement;
                XmlNodeList eintragListe2 = wurzel2.SelectNodes("./EinmaligerTermin");
                eintraege.Clear(); //löscht die Einträge aus der anderen xml Datei
                Console.WriteLine("\nAnzahl der Eintraege in der xml Datei aktuelle Termine: " + eintragListe2.Count);
                foreach (XmlNode eintrag in eintragListe2)
                {
                    string titel = eintrag.Attributes["titel"].Value;
                    int monat, tag;
                    if (!int.TryParse(eintrag.SelectSingleNode("./monat").FirstChild.Value, out monat))
                    {
                        Console.WriteLine("Monat ist keine Zahl");
                    }
                    if (!int.TryParse(eintrag.SelectSingleNode("./tag").FirstChild.Value, out tag))
                    {
                        Console.WriteLine("Tag ist keine Zahl");
                    }
                    eintraege.Add(new JaehrlichesEreignisAnFestemTag(titel, monat, tag));
                }

                foreach (Eintrag meinEintrag in eintraege)
                {
                    Console.WriteLine("Eintrag: " + meinEintrag.toString());
                }
            } //Ende Ordner vorhanden
            //Erwartet Eingabe vor Beendigung des Programms
            Console.ReadLine();
            return 0;
        } //ende main()
    } //ende class KalenderErzeuger
} //ende namespace KalenderWelt