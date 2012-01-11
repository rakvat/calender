using System;
using System.IO;

namespace KalenderWelt
{

    class HilfsKonstrukte {
        public const int TAGE_IM_JAHR = 365;
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

        public static bool istSchaltJahr(int dasJahr) {
            return ((dasJahr % 4 == 0) && 
                    ((dasJahr % 100 != 0) || 
                     (dasJahr % 400 == 0)));
        }

        public static int startWochenTag(int dasJahr) {
            //15.10.1582 (Korrekturtag) war Fr
            //-> 1.1.1583 Sa=5
            int wochenTag = 5;
            for (int i = 1583; i < dasJahr; ++i) {
                wochenTag += TAGE_IM_JAHR + (istSchaltJahr(i) ? 1 : 0);
                wochenTag %= 7;
            }
            return wochenTag;
        }
    } //ende class HilfsKonstrukte
} //ende namespace KalenderWelt

