using System.Collections.Generic;

namespace KalenderWelt
{
    public class KalenderJahr
    {
        private int _jahr;
        private List<Monat> _monate;

        public KalenderJahr(int dasJahr) {
            _jahr = dasJahr;
        }

        public int Jahreszahl() 
        {
            return _jahr;
        }

        public bool IstSchaltjahr() 
        {
            return HilfsKonstrukte.IstSchaltjahr(_jahr);
        }

        public int StartWochenTag() 
        {
            //15.10.1582 (Korrekturtag) war Fr
            //-> 1.1.1583 Sa=5
            int wochenTag = 5;
            for (int i = 1583; i < _jahr; ++i) {
                wochenTag += HilfsKonstrukte.TAGE_IM_JAHR + (HilfsKonstrukte.IstSchaltjahr(i) ? 1 : 0);
                wochenTag %= 7;
            }
            return wochenTag;
        }
    }

    public class Monat 
    {
        private List<Tag> _tage;
    }

    public class Tag
    {
        private List<Eintrag> _eintraege;
    }
}
