using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;

namespace KalenderWelt
{
    public abstract class Eintrag 
    {
        public const string INPUT_DIR = "input/";
        public static string[] EINTRAG_TYPEN = {"JaehrlichesEreignisAnFestemTag", 
                                               "EinmaligerTermin"};
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

        public static List<Eintrag> LeseEintraegeAusDatei(string dieDatei)
        {
            List<Eintrag> meineEintraege = new List<Eintrag>();
            XmlDocument doc = new XmlDocument();  //aus xml laden
            doc.Load(Eintrag.INPUT_DIR + dieDatei);
            XmlElement wurzel = doc.DocumentElement;

            for (int i = 0; i < Eintrag.EINTRAG_TYPEN.Length; ++i)
            {
                string xmlNodeName = Eintrag.EINTRAG_TYPEN[i];
                XmlNodeList eintragListe = wurzel.SelectNodes("./" + xmlNodeName);
                Console.WriteLine("\nAnzahl der " + xmlNodeName + " in der xml Datei " + dieDatei + ": " + eintragListe.Count);
                foreach (XmlNode eintrag in eintragListe)
                {
                    string titel = eintrag.Attributes["titel"].Value;
                    switch (xmlNodeName)
                    {
                        case "JaehrlichesEreignisAnFestemTag":
                            meineEintraege.Add(
                                new JaehrlichesEreignisAnFestemTag(titel, eintrag));
                            break;
                        case "EinmaligerTermin":
                            meineEintraege.Add(
                                new EinmaligerTermin(titel, eintrag));
                            break;
                    } //ende switch
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
                Console.WriteLine("Lese Eintraege aus " + f.Name + ";  " + f.Length + "; " + f.CreationTime);
                List<Eintrag> neueEintraege = Eintrag.LeseEintraegeAusDatei(f.Name);
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
        
        public EinmaligerTermin(string derTitel, XmlNode derKnoten)
            : base(derTitel)
        {
            int jahr, monat, tag;
            jahr = HilfsKonstrukte.KonvertiereZuInt(
                    derKnoten.SelectSingleNode("./jahr").FirstChild.Value, 
                    "Jahr");
            monat = HilfsKonstrukte.KonvertiereZuInt(
                    derKnoten.SelectSingleNode("./monat").FirstChild.Value, 
                    "Monat");
            tag = HilfsKonstrukte.KonvertiereZuInt(
                    derKnoten.SelectSingleNode("./tag").FirstChild.Value, 
                    "Tag");
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

        public JaehrlichesEreignisAnFestemTag(string derTitel, XmlNode derKnoten) 
            : base(derTitel)
        {
            _monat = HilfsKonstrukte.KonvertiereZuInt(
                        derKnoten.SelectSingleNode("./monat").FirstChild.Value,
                        "Monat");
            _tag = HilfsKonstrukte.KonvertiereZuInt(
                        derKnoten.SelectSingleNode("./tag").FirstChild.Value, 
                        "Tag");
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
