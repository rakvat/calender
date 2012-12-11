using System;
using System.IO;
using System.Collections.Generic;

namespace KalenderWelt
{
    public class KalenderJahr
    {
        private int _jahr;
        private List<Monat> _monate;
        private List<Woche> _wochen;
        private List<Tag> _tage;

        public KalenderJahr(int dasJahr) 
        {
            _jahr = dasJahr;
            _monate = new List<Monat>();
            _wochen = new List<Woche>();
            _tage = new List<Tag>();
            int tageImJahr = HilfsKonstrukte.TAGE_IM_JAHR;
            if (IstSchaltjahr()) {
                ++tageImJahr;
            }
            int wochenTag = StartWochenTag();
            int wochenNummer = StartWochenNummer();
            int monat = 0;
            int tagImMonat = 0;
            Woche woche = new Woche(wochenNummer);
            for (int i = 0; i < tageImJahr; ++i)
            {
                if (tagImMonat == 0) 
                {
                    _monate.Add(new Monat(monat));
                }
                if (wochenTag == 0 && i !=  0) 
                {
                    wochenNummer++;
                    woche = new Woche(wochenNummer);
                    _wochen.Add(woche);
                }
                Tag meinTag = new Tag(tagImMonat+1, wochenTag, i+1);
                meinTag.SetzeWoche(ref woche);
                _tage.Add(meinTag);
                _monate[monat].FuegeTagAn(ref meinTag);
                wochenTag = (++wochenTag) % HilfsKonstrukte.TAGE_PRO_WOCHE;
                tagImMonat++;
                if (tagImMonat == HilfsKonstrukte.TageImMonat(monat+1, _jahr)) {
                    monat++;
                    tagImMonat = 0;
                }
            }
        }

        public List<Monat> GibMonate() {
            return _monate;
        }

        public List<Tag> GibTage() {
            return _tage;
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
            for (int i = HilfsKonstrukte.MIN_JAHR; i < _jahr; ++i) 
            {
                wochenTag += HilfsKonstrukte.TAGE_IM_JAHR + (HilfsKonstrukte.IstSchaltjahr(i) ? 1 : 0);
                wochenTag %= 7;
            }
            return wochenTag;
        }

        //die erste Kalenderwoche des Jahres ist die, die den 4.1. enthält
        //Kalenderwochen beginnen mit Montag und enden mit Sonntag
        public int StartWochenNummer()
        {
            int startWochenTag = StartWochenTag();
            if (startWochenTag < 4) {
                return 1;
            }
            return 0; //eigentlich 52 oder 53 (letzte Woche des Vorjahrs)
        }

        public void TrageEin(ref List<Eintrag> dieEintraege) 
        {
            foreach (Eintrag eintrag in dieEintraege) 
            {
                eintrag.TrageEinIn(this);
            }
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

    public class Woche
    {
        private int _woche;

        public Woche(int dieWoche) 
        {
            _woche = dieWoche;
        }

        public int GibNummer()
        {
            return _woche;
        }
    }

    public class Tag
    {
        private int _index;  //startet mit 1
        private int _wochentag;
        private int _tagImJahr;
        private List<Eintrag> _eintraege;
        private List<string> _eintraegeStrings;
        private Woche _woche;

        public Tag(int derIndex, int derWochentag, int derTagImJahr)
        {
            _index = derIndex;
            _wochentag = derWochentag;
            _tagImJahr = derTagImJahr;
            _eintraege = new List<Eintrag>();
            _eintraegeStrings = new List<string>();
        }

        public int GibWochentag()
        {
            return _wochentag;
        }

        public int GibIndex() 
        {
            return _index;
        }

        public Woche GibWoche()
        {
            return _woche;
        }

        public void SetzeWoche(ref Woche dieWoche)
        {
            _woche = dieWoche;
        }

        public void TrageEin(Eintrag derEintrag, string derString)
        {
            _eintraege.Add(derEintrag);
            _eintraegeStrings.Add(derString);
        }

        public string EintraegeAlsString()
        {
            string meineEintraege = "";
            for (int i = 0, l = _eintraegeStrings.Count; i < l; ++i)
            {
                meineEintraege += _eintraegeStrings[i];
                if (i != _eintraege.Count - 1) 
                {
                    meineEintraege += ", ";
                }
            }
            return meineEintraege;
        }
    }
}
