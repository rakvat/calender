using System;

namespace KalenderWelt
{
    public abstract class Eintrag 
    {
        protected string _titel;

        public Eintrag(string derTitel) 
        {
            _titel = derTitel;
        }

        public string GibTitel() 
        {
            return _titel;
        }

        public abstract string toString();
        public abstract void TrageEinIn(KalenderJahr dasJahr);
    }

    public class EinmaligerTermin : Eintrag 
    {
        private DateTime _datum;

        public EinmaligerTermin(string derTitel, DateTime dasDatum) 
            : base(derTitel) 
        {
            _datum = dasDatum;
        }

        public override string toString() 
        {
            return _datum.ToShortDateString() + ": " + _titel;
        }

        public override void TrageEinIn(KalenderJahr dasJahr) 
        {
            if (dasJahr.Jahreszahl() != _datum.Year) {
                return;
            }
            dasJahr.GibMonate()[_datum.Month-1].GibTage()[_datum.Day-1].TrageEin(this);
        }
    }

    public class JaehrlichesEreignisAnFestemTag : Eintrag 
    {
        private int _monat;
        private int _tag;

        public JaehrlichesEreignisAnFestemTag(string derTitel, int derMonat, int derTag) 
            : base(derTitel) 
        {
            _monat = derMonat;
            _tag = derTag;
        }

        public override string toString() 
        {
            return _tag + "." + _monat + ".: " + _titel;
        }

        public override void TrageEinIn(KalenderJahr dasJahr) 
        {
            dasJahr.GibMonate()[_monat-1].GibTage()[_tag-1].TrageEin(this);
        }
    }
}
