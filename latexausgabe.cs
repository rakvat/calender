using System;

namespace KalenderWelt
{
    public class LatexTageszeilenAusgabe : Ausgabe
    {

        private readonly string[] START = {
            "\\documentclass[8pt, a4paper]{article}",
            "\\usepackage[german,ngerman]{babel}",
            "\\parindent 0mm",
            "\\hoffset -3.3cm",
            "\\textwidth 20cm",
            "\\pagestyle{empty}",
            "",
            "\\begin{document}",
            ""
            };
            private const string ENDE = "\\end{document}";
            private const string JAHR = "\\textbf{{\\huge{{Kalender f\"ur das Jahr {0}}}}}\\\\\\\\";

        public LatexTageszeilenAusgabe(ref KalenderJahr dasJahr)
            :base(ref dasJahr, 5) //XXX: modus sollte nicht nötig sein
        {
        }

        public override void gibAus()
        {
            oeffneStream(".tex");
            for (int i = 0; i < START.Length; ++i)
            {
                gibZeileAus(START[i]);
            }

            gibZeileAus(String.Format(JAHR, _jahr.Jahreszahl()));

            gibZeileAus(ENDE);
            schliesseStream();
        }
    }
}
