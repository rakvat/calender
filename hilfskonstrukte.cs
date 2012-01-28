using System;
using System.IO;

namespace KalenderWelt
{

    public class HilfsKonstrukte {
        public const int TAGE_IM_JAHR = 365;
        public const int MONATE_IM_JAHR = 12;
        //nach der Revolution koennen wir hier einfach auf 10-Tages-Woche aendern ;-)
        public const int TAGE_PRO_WOCHE = 7;
        public static string[] monatsNamen = 
            {"Januar", "Februar", "Maerz", "April", 
             "Mai", "Juni", "Juli", "August", 
             "September", "Oktober", "November", "Dezember"};
        public static string[] wochenTagNamen =
            {"Montag", "Dienstag", "Mittwoch", 
             "Donnerstag", "Freitag", "Samstag", "Sonntag"};
        public static string[] wochenTagNamenKurz = 
            {"MO", "DI", "MI", "DO", "FR", "SA", "SO"};
        //Schaltjahr wird spaeter korrigiert
        public static int[] tageImMonat = new int[12] 
            { 31, 28, 31, 30,   
              31, 30, 31, 31, 
              30, 31, 30, 31 };

        public static bool IstSchaltjahr(int dasJahr) 
        {
            return ((dasJahr % 4 == 0) && 
                    ((dasJahr % 100 != 0) || 
                     (dasJahr % 400 == 0)));
        }

        public static int KonvertiereZuInt(string derString, string derDebugName)
        {
            int meinInt;
            if (!int.TryParse(derString, out meinInt))
            {
                Console.WriteLine(derDebugName + " ist keine Zahl");
            }
            return meinInt;
        }

    } //ende class HilfsKonstrukte
} //ende namespace KalenderWelt

