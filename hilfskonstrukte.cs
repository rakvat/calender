using System;
using System.IO;

namespace KalenderWelt
{

    class HilfsKonstrukte {
        public static string[] monatsNamen = 
            {"Januar", "Februar", "Maerz", "April", 
             "Mai", "Juni", "Juli", "August", 
             "September", "Oktober", "November", "Dezember"};
        //Schaltjahr wird spaeter korrigiert
        public static int[] tageImMonat = new int[12] 
            { 31, 28, 31, 30,   
              31, 30, 31, 31, 
              30, 31, 30, 31 };

        public static bool istSchaltJahr(int dasJahr) {
            return (((dasJahr % 4 == 0) && 
                    (!(dasJahr % 100 == 0))) || 
                    ((dasJahr % 100 == 0) && 
                     (dasJahr % 400 == 0)));
        }
    } //ende class HilfsKonstrukte
} //ende namespace KalenderWelt

