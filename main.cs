using System;
using System.IO;

namespace KalenderWelt
{

    class KalenderErzeuger
    {

        public static int Main()
        {
            string eingabe = "";
            int eingabejahr = 0;

            Console.WriteLine("Das Programm berechnet Kalender f�r die Jahre 1583 bis 4000");
            Console.Write("Jahr eingeben:  ");
            eingabe = Console.ReadLine(); //erstmal als string speichern, damit auch eigegebene Buchstaben kein Problem sind

            while (!int.TryParse(eingabe, out eingabejahr)) //Solange die Eingabe nicht korrekt ist wird weiter abgefragt
            {
                Console.WriteLine("Falsche Eingabe, da Buchstaben nicht m�glich sind.");
                Console.Write("Bitte geben sie das Jahr erneut ein:  ");
                eingabe = Console.ReadLine();
            } //ende Eingabe Buchstaben Pr�fung

            while ((eingabejahr < 1583) || (eingabejahr > 4000)) //Wiederhole solange bis Eingabe im g�ltigen Bereich ist  
            {
                Console.WriteLine("Falsche Eingabe. Jahr ist nicht im gueltigen Bereich.");
                Console.Write("Bitte geben sie das Jahr erneut ein:  ");
                eingabe = Console.ReadLine();
                int.TryParse(eingabe, out eingabejahr);
            } //ende Eingabe Bereich Pr�fung

            DateiUndKonsolenAusgabe ausgabe = new DateiUndKonsolenAusgabe();
            ausgabe.gibAus(eingabejahr);
            Console.ReadLine();
            return 0;

        } //ende main()
    } //ende class KalenderErzeuger
} //ende namespace KalenderWelt