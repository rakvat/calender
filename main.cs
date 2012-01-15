using System;
using System.IO;
using System.Collections.Generic;

namespace KalenderWelt
{
    class KalenderErzeuger
    {

        public static int Main()
        {
            string eingabe = "";
            int eingabejahr = 0;

            Console.WriteLine("Das Programm berechnet Kalender für die Jahre 1583 bis 4000");
            Console.Write("Jahr eingeben:  ");
            eingabe = Console.ReadLine(); //erstmal als string speichern, damit auch eigegebene Buchstaben kein Problem sind

            while (!int.TryParse(eingabe, out eingabejahr)) //Solange die Eingabe nicht korrekt ist wird weiter abgefragt
            {
                Console.WriteLine("Falsche Eingabe, da Buchstaben nicht möglich sind.");
                Console.Write("Bitte geben sie das Jahr erneut ein:  ");
                eingabe = Console.ReadLine();
            } //ende Eingabe Buchstaben Prüfung

            while ((eingabejahr < 1583) || (eingabejahr > 4000)) //Wiederhole solange bis Eingabe im gültigen Bereich ist  
            {
                Console.WriteLine("Falsche Eingabe. Jahr ist nicht im gueltigen Bereich.");
                Console.Write("Bitte geben sie das Jahr erneut ein:  ");
                eingabe = Console.ReadLine();
                int.TryParse(eingabe, out eingabejahr);
            } //ende Eingabe Bereich Prüfung

            DateiUndKonsolenAusgabe ausgabe = new DateiUndKonsolenAusgabe();
            Console.WriteLine();
            Console.WriteLine();
            ausgabe.gibAus(eingabejahr);

            //Eintrag Experimente
            List<Eintrag> eintraege = new List<Eintrag> ();
            eintraege.Add(new EinmaligerTermin("beruflicher Termin", new DateTime(2012,1,23)));
            eintraege.Add(new JaehrlichesEreignisAnFestemTag("Christophs Geburtstag", 6, 30));
            foreach (Eintrag meinEintrag in eintraege) {
                Console.WriteLine("Eintrag: " + meinEintrag.toString());
            }

            Console.ReadLine();
            return 0;

        } //ende main()
    } //ende class KalenderErzeuger
} //ende namespace KalenderWelt
