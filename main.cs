using System;
using System.IO;

namespace KalenderWelt
{

    class KalenderErzeuger
    {

        public static int Main()
        {
            Console.WriteLine("Das Programm berechnet Kalender für die Jahre 1583 bis 4000");
            Console.Write("Jahr eingeben ");
            int eingabejahr = Convert.ToInt32(Console.ReadLine());

            if ((eingabejahr < 1583) || (eingabejahr > 4000)) Console.WriteLine("Falsche Eingabe. Jahr nicht im gueltigen Bereich.");
            else
            {
                DateiUndKonsolenAusgabe ausgabe = new DateiUndKonsolenAusgabe();
                ausgabe.gibAus(eingabejahr);
            } //ende if Eingabe gültig
            Console.ReadLine();
            return 0;
        } //ende main()
    } //ende class KalenderErzeuger
} //ende namespace KalenderWelt

