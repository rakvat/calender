using System.Collections.Generic;

namespace KalenderWelt
{
    public class KalenderJahr
    {
        private int _jahr;
        private List<Monat> _monate;
        private List<Tag> _tage;

        public KalenderJahr(int dasJahr) 
        {
            _jahr = dasJahr;
            _monate = new List<Monat>();
            _tage = new List<Tag>();
            int tageImJahr = HilfsKonstrukte.TAGE_IM_JAHR;
            if (IstSchaltjahr()) {
                ++tageImJahr;
                //XXX: dies sollte nur lokal und nicht in den Hilfskonstrukten
                //gesetzt werden
                HilfsKonstrukte.tageImMonat[1] = 29;
            }
            int wochenTag = StartWochenTag();
            int monat = 0;
            int tagImMonat = 0;
            for (int i = 0; i < tageImJahr; ++i)
            {
                if (tagImMonat == 0) 
                {
                    _monate.Add(new Monat(monat));
                }
                Tag meinTag = new Tag(tagImMonat+1, wochenTag, i+1);
                _tage.Add(meinTag);
                _monate[monat].FuegeTagAn(ref meinTag);
                wochenTag = (++wochenTag) % HilfsKonstrukte.TAGE_PRO_WOCHE;
                tagImMonat++;
                if (tagImMonat == HilfsKonstrukte.tageImMonat[monat]) {
                    monat++;
                    tagImMonat = 0;
                }
            }
        }

        public List<Monat> GibMonate() {
            return _monate;
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
            for (int i = 1583; i < _jahr; ++i) 
            {
                wochenTag += HilfsKonstrukte.TAGE_IM_JAHR + (HilfsKonstrukte.IstSchaltjahr(i) ? 1 : 0);
                wochenTag %= 7;
            }
            return wochenTag;
        }
    }

    public class Monat 
    {
        private int _index; //startet mit 0
        private List<Tag> _tage;
        
        public Monat(int derIndex)
        {
            _index = derIndex;
            _tage = new List<Tag>();
        }

        public void FuegeTagAn(ref Tag derTag) 
        {
            _tage.Add(derTag);
        }

        public List<Tag> GibTage() 
        {
            return _tage;
        }

        public int GibIndex()
        {
            return _index;
        }
    }

    //TODO
    //public class Woche
    //{
    //    private int _woche;
    //    private List<Tag> _tage;

    //    public Woche(int dieWoche) 
    //    {
    //        _woche = dieWoche;
    //    }
    //}

    public class Tag
    {
        private int _index;  //startet mit 1
        private int _wochentag;
        private int _tagImJahr;
        private List<Eintrag> _eintraege;

        public Tag(int derIndex, int derWochentag, int derTagImJahr)
        {
            _index = derIndex;
            _wochentag = derWochentag;
            _tagImJahr = derTagImJahr;
            _eintraege = new List<Eintrag>();
        }

        public int GibWochentag()
        {
            return _wochentag;
        }

        public int GibIndex() 
        {
            return _index;
        }

        public void TrageEin(ref Eintrag derEintrag)
        {
            _eintraege.Add(derEintrag);
        }

        public string EintraegeAlsString()
        {
            string meineEintraege = "";
            for (int i = 0, l = _eintraege.Count; i < l; ++i)
            {
                meineEintraege += _eintraege[i].toString();
                if (i != _eintraege.Count - 1) 
                {
                    meineEintraege += ", ";
                }
            }
            return meineEintraege;
        }
    }
}
