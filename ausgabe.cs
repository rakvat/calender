using System;
using System.IO;
using System.Collections.Generic;

namespace KalenderWelt
{
    public abstract class Ausgabe
    {
        //const members are static in c#
        public const string AUSGABE_DIR = "ausgabe/";
        public const string TEST_AUSGABE_DIR = "testausgabe/";

        private string _dateiName;
        private StreamWriter _streamWriter;
        private bool _testModus;
        protected KalenderJahr _jahr;
        protected int _modus;

        public Ausgabe(ref KalenderJahr dasJahr, int eingabemodus) //Konstruktor der Klasse Ausgabe, übergibt zwei int werte von main.cs Datei an ausgabe.cs
        {
            _jahr = dasJahr;
            _modus = eingabemodus;
        }

        public void setzeTestModus (bool derModus) 
        {
            _testModus = derModus;
        }

        public string DateiName
        {
           get 
           {
              return _dateiName; 
           }
        }

        protected void gibZeileAus() //erzeugt Leere Zeilen, ohne übergabewert
        {
            gibZeileAus("");
            //_streamWriter.Write("<br>"); //erzeugt Zeileumbrauch für die HTML Ausgabe
        }

        protected void gibZeileAus(string dieZeile) //gibt zusammengebaute Kalenderzeilen aus, mit übergabewert
        {
            _streamWriter.WriteLine(dieZeile);
            //_streamWriter.Write("<br>");
            if (!_testModus) {
                Console.WriteLine(dieZeile);
            }
        }

        protected void gibZeilenAus(string[] dieZeilen)
        {
            for (int i = 0; i < dieZeilen.Length; ++i)
            {
                gibZeileAus(dieZeilen[i]);
            }
        }

       // protected void html(int eingabemodus)
       // {
       //     string jahr = Convert.ToString(_jahr.Jahreszahl());
       //     string Dateiname;
       //     Dateiname = jahr.Insert(4, ".html");
       //     jahr = Dateiname;
       //     Dateiname = jahr.Insert(0, "Kalender");

       //     if (_modus == 1) Dateiname = Dateiname.Insert(0, "1"); //Monatsblock einspaltig
       //     if (_modus == 2) Dateiname = Dateiname.Insert(0, "2"); //Monatsblock zweispaltig
       //     if (_modus == 3) Dateiname = Dateiname.Insert(0, "3"); //Tageszeilen ohne Einträge
       //     if (_modus == 4) Dateiname = Dateiname.Insert(0, "4"); //Tageszeilen mit Einträge

       //     FileInfo f;
       //     if (_testModus)
       //     {
       //         f = erzeugeFileInfo(Ausgabe.TEST_AUSGABE_DIR, Dateiname);
       //     }
       //     else
       //     {
       //         f = erzeugeFileInfo(Ausgabe.AUSGABE_DIR, Dateiname);
       //     }
       //     _streamWriter = f.CreateText();
       //     _streamWriter.WriteLine("<html>"); //Anfang der HTML Datei erstellen
       //     _streamWriter.WriteLine("<head>");
       //     _streamWriter.WriteLine("<title>Kalender fuer das Jahr " + Convert.ToString(_jahr.Jahreszahl()) + " im HTML Format mit C#</title>");
       //     _streamWriter.WriteLine("</head>");
       //     _streamWriter.WriteLine("<body>");
       ////   _streamWriter.WriteLine("Inhalt");
       // }

        protected void oeffneStream()
        {
            oeffneStream(".txt");
        }

        protected void oeffneStream(string dieFileEndung)
        {
            string jahr = Convert.ToString(_jahr.Jahreszahl());
            string Dateiname = jahr.Insert(4, dieFileEndung);
            Dateiname = Dateiname.Insert(0, "Kalender");

            //XXX: nicht schön, dass _modus nur für diese Zwecke 
            //durchgeschleift werden muss
            Dateiname = Dateiname.Insert(0, Convert.ToString(_modus)); 

            FileInfo f;
            if (_testModus) 
            {
                f = erzeugeFileInfo(Ausgabe.TEST_AUSGABE_DIR, Dateiname);
            } else {
                f = erzeugeFileInfo(Ausgabe.AUSGABE_DIR, Dateiname);
            }
            _streamWriter = f.CreateText();
        }

        protected void schliesseStream()
        {
        //    _streamWriter.WriteLine("</body>"); //HTML Dokument abschliesen
        //    _streamWriter.WriteLine("</html>");

            _streamWriter.Close(); //Datei schliesen
        }

        public abstract void gibAus();

        private FileInfo erzeugeFileInfo(string dasVerzeichnis, string derDateiname) 
        {
            if (!System.IO.Directory.Exists(dasVerzeichnis))
            {
                Directory.CreateDirectory(dasVerzeichnis); //anlegen des Ordners wenn nicht vorhanden
            }
            _dateiName = dasVerzeichnis + derDateiname;
            FileInfo f = new FileInfo(_dateiName); //Text Datei anlegen 
            return f;
        }
    }

    public class MonatsBlockAusgabe : Ausgabe
    {
        public MonatsBlockAusgabe(ref KalenderJahr dasJahr, int eingabemodus)
            : base(ref dasJahr, eingabemodus)
        {
        }

        public override void gibAus()
        {
            oeffneStream();     //Textdatei erzeigen
       //     html(_modus);       //HTML Datei anlegen
            int monat, tag;
            int wochentagDesErstenImMonat = 0; //Mo = 0, ...
            gibZeileAus("Kalender fuer das Jahr " + _jahr.Jahreszahl());
            gibZeileAus();
            string wochentage = String.Join(" ", HilfsKonstrukte.wochenTagNamenKurz);

            wochentagDesErstenImMonat = _jahr.StartWochenTag();
            for (monat = 0; monat < 12; monat++) //Schleife für die Erstellung der Monate: 0=Jan bis 11=Dez
            {
                gibZeileAus("      " + HilfsKonstrukte.monatsNamen[monat]);
                gibZeileAus(wochentage);

                if (monat > 0)
                {
                    wochentagDesErstenImMonat += HilfsKonstrukte.TageImMonat(monat, _jahr.Jahreszahl()) % 7;
                    wochentagDesErstenImMonat %= 7;
                }

                //Die Woche beginnt immer Montags, nur der 1. eines Monats 
                //kann auf verschiedene Wochentage fallen. 
                int ausgabePosition = wochentagDesErstenImMonat;

                string zeile = "".PadLeft(ausgabePosition * 3);
                for (tag = 1; tag <= HilfsKonstrukte.TageImMonat(monat+1, _jahr.Jahreszahl()); tag++) //solange Zähler kleiner gleich Wert aus Array Tage 
                {
                    zeile += tag.ToString().PadRight(3);
                    ausgabePosition++;

                    //wenn hinterste Position erreicht Zeihlenumbruch einfügen und Bildschirm-Positionszähler zurücksetzen
                    if (ausgabePosition > 6)
                    {
                        gibZeileAus(zeile);
                        ausgabePosition = 0;
                        zeile = "";
                    }
                }
                if (zeile.Length > 0) //wenn mehr als 0 Zeichen vorhanden sind ganze Zeile ausgeben
                {
                    gibZeileAus(zeile);
                }
                //freie Zeilen zwischen den Monaten auf dem Bildschirm 
                gibZeileAus();

            } //ende for Schleife Monat   
            schliesseStream();
        }
    } //ende class MonatsBlockAusgabe

    public class MonatsBlockAusgabe2Spaltig : Ausgabe
    {
        private Dictionary<int, List<string>> preparedStrings = new Dictionary<int, List<string>>();
        private const int SPALTEN_BREITE = 3 * 7 + 10; //3*7 ist ein Monat breit

        public MonatsBlockAusgabe2Spaltig(ref KalenderJahr dasJahr, int eingabemodus)
            : base(ref dasJahr, eingabemodus)
        {
        }

        public override void gibAus()
        {
            oeffneStream();

            int monat, tag;
            int wochentagDesErstenImMonat = 0; //Mo = 0, ...

            gibZeileAus("Kalender fuer das Jahr " + _jahr.Jahreszahl());
            gibZeileAus();
            string wochentage = String.Join(" ", HilfsKonstrukte.wochenTagNamenKurz);

            wochentagDesErstenImMonat = _jahr.StartWochenTag();
            for (monat = 0; monat < 12; monat++) //Schleife für die Erstellung der Monate: 0=Jan bis 11=Dez
            {
                preparedStrings[monat] = new List<string>();
                preparedStrings[monat].Add("      " + HilfsKonstrukte.monatsNamen[monat]);
                preparedStrings[monat].Add(wochentage);

                if (monat > 0)
                {
                    wochentagDesErstenImMonat += HilfsKonstrukte.TageImMonat(monat, _jahr.Jahreszahl()) % 7;
                    wochentagDesErstenImMonat %= 7;
                }

                //Die Woche beginnt immer Montags, nur der 1. eines Monats 
                //kann auf verschiedene Wochentage fallen. 
                int ausgabePosition = wochentagDesErstenImMonat;

                string zeile = "".PadLeft(ausgabePosition * 3);
                for (tag = 1; tag <= HilfsKonstrukte.TageImMonat(monat+1, _jahr.Jahreszahl()); tag++) //solange Zähler kleiner gleich Wert aus Array Tage 
                {
                    zeile += tag.ToString().PadRight(3);
                    ausgabePosition++;

                    //wenn hinterste Position erreicht Zeihlenumbruch einfügen und Bildschirm-Positionszähler zurücksetzen
                    if (ausgabePosition > 6)
                    {
                        preparedStrings[monat].Add(zeile);
                        ausgabePosition = 0;
                        zeile = "";
                    }
                }
                if (zeile.Length > 0) //wenn mehr als 0 Zeichen vorhanden sind ganze Zeile ausgeben
                {
                    preparedStrings[monat].Add(zeile);
                }
                //freie Zeilen zwischen den Monaten auf dem Bildschirm 
                while (preparedStrings[monat].Count < 9)
                {
                    preparedStrings[monat].Add("");
                }

            } //ende for Schleife Monat   

            for (monat = 0; monat < 6; monat++)
            {
                for (int i = 0, l = preparedStrings[monat].Count; i < l; ++i)
                {
                    gibZeileAus(preparedStrings[monat][i].PadRight(SPALTEN_BREITE)
                                + preparedStrings[monat + 6][i]);
                }
            }
            schliesseStream();
        }
    } //ende class MonatsBlockAusgabe2Spaltig

    public class TageszeilenAusgabe : Ausgabe
    {
        bool _mitEintraegen = false;
        public TageszeilenAusgabe(ref KalenderJahr dasJahr, int eingabemodus, bool mitEintraegen)
            : base(ref dasJahr, eingabemodus)
        {
            _mitEintraegen = mitEintraegen;
        }

        public override void gibAus()
        {
            oeffneStream();
            gibZeileAus("Kalender fuer das Jahr " + _jahr.Jahreszahl());
            gibZeileAus();

            foreach (Monat monat in _jahr.GibMonate()) 
            {
                gibZeileAus("      " + HilfsKonstrukte.monatsNamen[monat.GibIndex()]);
                foreach (Tag tag in monat.GibTage())
                {
                    int wochentag = tag.GibWochentag();
                    string tagstring = HilfsKonstrukte.wochenTagNamenKurz[wochentag] + 
                                        " " + tag.GibIndex().ToString().PadLeft(3);
                    if (_mitEintraegen) {
                        tagstring += "  " + tag.EintraegeAlsString();
                    }
                    gibZeileAus(tagstring);
                    if (wochentag == 6)
                    {
                        gibZeileAus("---------------------");
                    }
                }
                gibZeileAus();
            }
            schliesseStream();
        }
    } //ende class TageszeilenAusgabe
} //ende namespace KalenderWelt
