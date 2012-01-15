using System;

namespace KalenderWelt
{
    public abstract class Eintrag {
        protected string _titel;

        public Eintrag(string derTitel) {
            _titel = derTitel;
        }

        public abstract string toString();
    }

    public class EinmaligerTermin : Eintrag {
        private DateTime _datum;

        public EinmaligerTermin(string derTitel, DateTime dasDatum) 
            : base(derTitel) {
            _datum = dasDatum;
        }

        public override string toString() {
            return _datum.ToShortDateString() + ": " + _titel;
        }
    }

    public class JaehrlichesEreignisAnFestemTag : Eintrag {
        private int _monat;
        private int _tag;

        public JaehrlichesEreignisAnFestemTag(string derTitel, int derMonat, int derTag) 
            : base(derTitel) {
            _monat = derMonat;
            _tag = derTag;
        }

        public override string toString() {
            return _tag + "." + _monat + ".: " + _titel;
        }
    }
}
