using System;

namespace KalenderWelt
{
    public class htmlTageszeilenAusgabe : Ausgabe
    {
        public htmlTageszeilenAusgabe(ref KalenderJahr dasJahr)
            :base(ref dasJahr, 6) 
        {
        }

        public override void gibAus()
        {
            oeffneStream(".html");
        
            gibZeileAus("<html>");  //Anfang der HTML Datei erstellen
            gibZeileAus("<head>");
            gibZeileAus("<title>Kalender fuer das Jahr " + Convert.ToString(_jahr.Jahreszahl()) + " im HTML Format mit C#</title>" );
            gibZeileAus("</head>");
            gibZeileAus("<body>");

            gibZeileAus(Convert.ToString(_jahr.Jahreszahl()));
            gibZeileAus("<br>");

            foreach (Monat monat in _jahr.GibMonate())
            {
                gibZeileAus("<br>");
                gibZeileAus(String.Format(
                        HilfsKonstrukte.monatsNamen[monat.GibIndex()]));
                gibZeileAus("<br>");

                foreach (Tag tag in monat.GibTage())
                {
                    int wochentag = tag.GibWochentag();
                    string wochentagString =
                        HilfsKonstrukte.wochenTagNamenKurz[wochentag];
                    string tagString = tag.GibIndex().ToString();

                    gibZeileAus(String.Format( wochentagString, tagString));
                    if (wochentag == 6)
                    {
                        gibZeileAus("<br>");
                    }
                }
                gibZeileAus("<br>");
            }

            gibZeileAus("</body>"); //Ende der HTML Datei schreiben
            gibZeileAus("</html>");
            schliesseStream();
        }

    }
}