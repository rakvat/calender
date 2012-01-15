using System;
using System.IO;

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
            Console.ReadLine();
            return 0;
        } //ende main()
    } //ende class KalenderErzeuger
} //ende namespace KalenderWelt
