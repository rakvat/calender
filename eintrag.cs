using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace KalenderWelt
{
    public class EintragParseException : System.ApplicationException
    {
        public EintragParseException() {}
        public EintragParseException(string message) 
        {
            Console.WriteLine("EintragParseException: " + message);
        }
        public EintragParseException(string message, System.Exception inner) {}
        protected EintragParseException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) {}
    }

    public abstract class Eintrag 
    {
        public const string TEST_INPUT_DIR = "testinput/";
        public const string INPUT_DIR = "input/";
        public static string[] EINTRAG_TYPEN = {"JaehrlichesEreignisAnFestemTag", 
                                                "Geburtstag",
                                                "RelativesEreignisNterWochentagImMonat",
                                                "RelativesEreignisLetzteWocheImMonat",
                                                "EinmaligerTermin"};
        protected string _titel;

        public Eintrag(string derTitel) 
        {
            _titel = derTitel;
        }

        public Eintrag(XmlNode derKnoten) 
        {
            if (derKnoten != null && 
                derKnoten.SelectSingleNode("./titel") != null &&
                derKnoten.SelectSingleNode("./titel").FirstChild != null) {
                _titel = derKnoten.SelectSingleNode("./titel").FirstChild.Value;
            }
        }

        public string GibTitel() 
        {
            return _titel;
        }

        public virtual string GibEintragText(KalenderJahr dasJahr) 
        {
            return GibTitel();
        }

        public abstract string toString();
        public abstract void TrageEinIn(KalenderJahr dasJahr);

        public static List<Eintrag> LeseEintraegeAusDatei(string dieDatei)
        {
            List<Eintrag> meineEintraege = new List<Eintrag>();
            if (!File.Exists(dieDatei))
            {
                Console.WriteLine("[Eintrag.LeseEintraegeAusDatei] Datei " + dieDatei + " nicht gefunden");
                return meineEintraege;
            }
            XmlDocument doc = new XmlDocument();  //aus xml laden
            doc.Load(dieDatei);
            XmlElement wurzel = doc.DocumentElement;

            for (int i = 0; i < Eintrag.EINTRAG_TYPEN.Length; ++i)
            {
                string xmlNodeName = Eintrag.EINTRAG_TYPEN[i];
                XmlNodeList eintragListe = wurzel.SelectNodes("./" + xmlNodeName);
                //noch eine Art der Debug Ausgabe, aber unklar wie in mono aktivierbar...
                Debug.WriteLine("\nAnzahl der " + xmlNodeName + " in der xml Datei " + dieDatei + ": " + eintragListe.Count);
                foreach (XmlNode eintrag in eintragListe)
                {
                    Eintrag neuerEintrag;
                    try 
                    {
                        switch (xmlNodeName)
                        {
                            case "JaehrlichesEreignisAnFestemTag":
                                neuerEintrag = new JaehrlichesEreignisAnFestemTag(eintrag);
                                meineEintraege.Add(neuerEintrag);
                                break;
                            case "Geburtstag":
                                neuerEintrag = new Geburtstag(eintrag);
                                meineEintraege.Add(neuerEintrag);
                                break;
                            case "EinmaligerTermin":
                                neuerEintrag = new EinmaligerTermin(eintrag);
                                meineEintraege.Add(neuerEintrag);
                                break;
                            case "RelativesEreignisNterWochentagImMonat":
                                neuerEintrag = new RelativesEreignisNterWochentagImMonat(eintrag);
                                meineEintraege.Add(neuerEintrag);
                                break;
                            case "RelativesEreignisLetzteWocheImMonat":
                                neuerEintrag = new RelativesEreignisLetzteWocheImMonat(eintrag);
                                meineEintraege.Add(neuerEintrag);
                                break;
                        } //ende switch
                    }
                    catch (EintragParseException )
                    {
                        Console.WriteLine("Eintrag kann nicht hinzugefügt werden");
                    }
                } //ende foreach XmlNode
            } //ende for Schleife
            return meineEintraege;
        }

        public static List<Eintrag> LeseEintraegeAusVerzeichnis(string dasVerzeichnis)
        {
            List<Eintrag> meineEintraege = new List<Eintrag>();
            DirectoryInfo d = new DirectoryInfo(dasVerzeichnis);
            foreach (FileInfo f in d.GetFiles("*.xml"))
            {
                Debug.WriteLine("Lese Eintraege aus " + f.Name + ";  " + f.Length + "; " + f.CreationTime);
                List<Eintrag> neueEintraege = Eintrag.LeseEintraegeAusDatei(dasVerzeichnis + f.Name);
                meineEintraege.AddRange(neueEintraege);
            } //ende foreach fileinfo
            return meineEintraege;
        }
    }

    public class EinmaligerTermin : Eintrag 
    {
        private DateTime _datum;

        public EinmaligerTermin(string derTitel, DateTime dasDatum) 
            : base(derTitel) 
        {
            _datum = dasDatum;
        }
        
        public EinmaligerTermin(XmlNode derKnoten)
            : base(derKnoten)
        {
            int jahr, monat, tag;
            try 
            {
                jahr = HilfsKonstrukte.KonvertiereZuInt(
                        derKnoten.SelectSingleNode("./jahr").FirstChild.Value, 
                        "Jahr");
                monat = HilfsKonstrukte.KonvertiereZuInt(
                        derKnoten.SelectSingleNode("./monat").FirstChild.Value, 
                        "Monat");
                tag = HilfsKonstrukte.KonvertiereZuInt(
                        derKnoten.SelectSingleNode("./tag").FirstChild.Value, 
                        "Tag");
            } 
            catch (KeineZahlException )
            {
                throw new EintragParseException("Xml could not be parsed correctly");
            }
            //Termine an 29.2. in nicht Schaltenjahren sollten nicht auf den 28. korrigiert werden, sondern als nicht valide gelten
            //tag = HilfsKonstrukte.Korrigiere29zu28inNichtSchaltjahrFebruar(jahr, monat, tag);
            try
            {
                HilfsKonstrukte.PruefeObValidesDatum(jahr, monat, tag); 
            }
            catch (FalscherBereichException )
            {
                throw new EintragParseException("Xml could not be parsed correctly");
            }
            _datum = new DateTime(jahr, monat, tag);
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
            dasJahr.GibMonate()[_datum.Month-1].GibTage()[_datum.Day-1].TrageEin(this, GibEintragText(dasJahr));
        }
    }

    public class JaehrlichesEreignisAnFestemTag : Eintrag 
    {
        protected int _monat;
        protected int _tag;

        public JaehrlichesEreignisAnFestemTag(string derTitel, int derMonat, int derTag) 
            : base(derTitel) 
        {
            _monat = derMonat;
            _tag = derTag;
        }

        public JaehrlichesEreignisAnFestemTag(XmlNode derKnoten) 
            : base(derKnoten)
        {
            try
            {
                _monat = HilfsKonstrukte.KonvertiereZuInt(
                            derKnoten.SelectSingleNode("./monat").FirstChild.Value,
                            "Monat");
                _tag = HilfsKonstrukte.KonvertiereZuInt(
                            derKnoten.SelectSingleNode("./tag").FirstChild.Value, 
                            "Tag");
                int dummySchaltjahr = 2012;
                HilfsKonstrukte.PruefeObValidesDatum(dummySchaltjahr, _monat, _tag); 
            }
            catch (KeineZahlException ) 
            {
                throw new EintragParseException("Xml could not be parsed correctly");
            }
            catch (FalscherBereichException )
            {
                throw new EintragParseException("Xml could not be parsed correctly");
            }
        }

        public override string toString() 
        {
            return _tag + "." + _monat + ".: " + _titel;
        }

        public override void TrageEinIn(KalenderJahr dasJahr) 
        {
            int korrigierterTag = HilfsKonstrukte.Korrigiere29zu28inNichtSchaltjahrFebruar(dasJahr.Jahreszahl(), _monat, _tag);
            dasJahr.GibMonate()[_monat-1].GibTage()[korrigierterTag-1].TrageEin(this, GibEintragText(dasJahr));
        }
    }

    public class Geburtstag : JaehrlichesEreignisAnFestemTag
    {
        private int _geburtsjahr = 0;

        public Geburtstag(string derTitel, int derMonat, int derTag, int dasGeburtsjahr) 
            : base(derTitel, derMonat, derTag) 
        {
            _geburtsjahr = dasGeburtsjahr;
        }

        public Geburtstag(XmlNode derKnoten) 
            : base(derKnoten)
        {
            try
            {
                if (derKnoten.SelectSingleNode("./jahr") != null) {
                    _geburtsjahr = HilfsKonstrukte.KonvertiereZuInt(
                                derKnoten.SelectSingleNode("./jahr").FirstChild.Value,
                                "Jahr");
                }
            }
            catch (KeineZahlException ) 
            {
                throw new EintragParseException("Xml could not be parsed correctly");
            }
        }

        public override string toString() 
        {
            return _tag + "." + _monat + ".: " + _titel + "(" + _geburtsjahr + ")";
        }

        public override string GibEintragText(KalenderJahr dasJahr) 
        {
            string start = "Geburtstag ";
            if (_geburtsjahr != 0) 
            {
                return start + GibTitel() + " (" + (dasJahr.Jahreszahl() - _geburtsjahr) + ")";
            }
            return start + GibTitel();
        }
    }

    public abstract class RelativerEintrag : Eintrag
    {
        protected int _monat;
        protected int _wochentag;

        public RelativerEintrag(string derTitel, int derMonat, int derWochentag)
            :base(derTitel)
        {
            _monat = derMonat;
            _wochentag = derWochentag;
        }

        public RelativerEintrag(XmlNode derKnoten)
            :base(derKnoten)
        {
            try
            {
                _monat = HilfsKonstrukte.KonvertiereZuInt(
                         derKnoten.SelectSingleNode("./monat").FirstChild.Value,
                         "Monat");
                _wochentag = HilfsKonstrukte.KonvertiereZuInt(
                        derKnoten.SelectSingleNode("./wochentag").FirstChild.Value, 
                        "Wochentag");
                HilfsKonstrukte.PruefeBereich(_monat, 1, 12);
                HilfsKonstrukte.PruefeBereich(_wochentag, 0, HilfsKonstrukte.TAGE_PRO_WOCHE-1);
            }
            catch (KeineZahlException ) 
            {
                throw new EintragParseException("Xml could not be parsed correctly");
            }
            catch (FalscherBereichException )
            {
                throw new EintragParseException("Xml could not be parsed correctly");
            }
        }
    }

    public class RelativesEreignisNterWochentagImMonat : RelativerEintrag
    {
        private int _n;

        public RelativesEreignisNterWochentagImMonat(string derTitel, int derMonat, int derWochentag, int n) 
            :base(derTitel, derMonat, derWochentag)
        {
            _n = n;
        }

        public RelativesEreignisNterWochentagImMonat(XmlNode derKnoten)
            :base(derKnoten)
        {
            try
            {
                _n = HilfsKonstrukte.KonvertiereZuInt(
                        derKnoten.SelectSingleNode("./n").FirstChild.Value, 
                        "n");
                HilfsKonstrukte.PruefeBereich(_n, 1, 5);
            }
            catch (KeineZahlException ) 
            {
                throw new EintragParseException("Xml could not be parsed correctly");
            }
            catch (FalscherBereichException )
            {
                throw new EintragParseException("Xml could not be parsed correctly");
            }

        }

        public override string toString() 
        {
            return _n + "ter" + HilfsKonstrukte.wochenTagNamen[_wochentag] + " im Monat " + 
                   HilfsKonstrukte.monatsNamen[_monat-1] + ": " + _titel;
        }

        public override void TrageEinIn(KalenderJahr dasJahr) 
        {
            List<Tag> tageImMonat = dasJahr.GibMonate()[_monat-1].GibTage();
            int index = 0;
            Tag ersterTag = tageImMonat[index];
            while (ersterTag.GibWochentag() != _wochentag) {
                index++;
                ersterTag = tageImMonat[index];
            }
            index = index + (_n-1) * HilfsKonstrukte.TAGE_PRO_WOCHE;
            if (index >= tageImMonat.Count) {
                return;
            }
            Tag nterTag = tageImMonat[index];
            nterTag.TrageEin(this, GibEintragText(dasJahr));
        }
    }

    public class RelativesEreignisLetzteWocheImMonat : RelativerEintrag
    {
        public RelativesEreignisLetzteWocheImMonat(string derTitel, int derMonat, int derWochentag) 
            :base(derTitel, derMonat, derWochentag)
        {
        }

        public RelativesEreignisLetzteWocheImMonat(XmlNode derKnoten)
            :base(derKnoten)
        {

        }
        public override string toString() 
        {
            return "letzter" + HilfsKonstrukte.wochenTagNamen[_wochentag] + " im Monat " + 
                   HilfsKonstrukte.monatsNamen[_monat-1] + ": " + _titel;
        }

        public override void TrageEinIn(KalenderJahr dasJahr) 
        {
            List<Tag> tageImMonat = dasJahr.GibMonate()[_monat-1].GibTage();
            int index = tageImMonat.Count-1;
            Tag letzterTag = tageImMonat[index];
            while (letzterTag.GibWochentag() != _wochentag) {
                index--;
                letzterTag = tageImMonat[index];
            }
            letzterTag.TrageEin(this, GibEintragText(dasJahr));
        }
    }
}
