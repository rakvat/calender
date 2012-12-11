using System;
using System.IO;

namespace KalenderWelt
{

    public class KeineZahlException : System.ApplicationException
    {
        public KeineZahlException() {}
        public KeineZahlException(string message) 
        {
            Console.WriteLine("KeineZahlException: " + message);
        }
        public KeineZahlException(string message, System.Exception inner) {}
        protected KeineZahlException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) {}
    }

    public class FalscherBereichException : System.ApplicationException
    {
        public FalscherBereichException() {}
        public FalscherBereichException(string message) 
        {
            Console.WriteLine("FalscherBereichException: " + message);
        }
        public FalscherBereichException(string message, System.Exception inner) {}
        protected FalscherBereichException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) {}
    }
        
    public class HilfsKonstrukte 
    {
        public const int MIN_JAHR = 1583; // 1582 fand die Kalenderkorrektur statt
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
            {"Mo", "Di", "Mi", "Do", "Fr", "Sa", "So"};
        //Schaltjahr wird spaeter korrigiert
        private static int[] tageImMonat = new int[12] 
            { 31, 28, 31, 30,   
              31, 30, 31, 31, 
              30, 31, 30, 31 };
        public static int TageImMonat(int derMonat, int dasJahr) 
        {
           if (derMonat == 2 && IstSchaltjahr(dasJahr))
           {
               return 29;
           } 
           return tageImMonat[derMonat-1];
        }

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
                Console.WriteLine("'" + derDebugName + "' ist keine Zahl");
                throw new KeineZahlException(derDebugName + " ist keine Zahl");
            }
            return meinInt;
        }

        public static void PruefeBereich(int dieZahl, int dasMinimum, int dasMaximum)
        {
            if (!(dieZahl >= dasMinimum && dieZahl <= dasMaximum)) 
            {
                throw new FalscherBereichException(dieZahl + " ist nicht im Bereich " + dasMinimum + " - " + dasMaximum);
            }
        }

        public static int Korrigiere29zu28inNichtSchaltjahrFebruar(int dasJahr, int derMonat, int derTag)
        {
            if (derMonat == 2 && derTag == 29 && !IstSchaltjahr(dasJahr)) 
            {
                return 28;
            }
            return derTag;
        }

        //throws FalscherBereichException
        public static void PruefeObValidesDatum(int dasJahr, int derMonat, int derTag) 
        {
            PruefeBereich(dasJahr, MIN_JAHR, int.MaxValue);
            PruefeBereich(derMonat, 1, 12);
            PruefeBereich(derTag, 1, TageImMonat(derMonat, dasJahr));
        }
    } //ende class HilfsKonstrukte
} //ende namespace KalenderWelt

