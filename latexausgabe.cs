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

            private const string MONATS_TITEL = "\\textbf{{\\large{{{0}}}}\\\\
            
            private readonly string[] MONAT_START = {
                "\\begin{tabular*}{20cm}{|l|l|p{5cm}|l| }",
                "\\hline",
                "\\textbf{} & \\textbf{} & \\textbf{} & \\textbf{...}\\\\",
                "\\hline",
                "\\hline"
                };

            private readonly string[] MONAT_ENDE = {
                "\\end{tabular*}",
                "\\newpage"
                };

            private const string ZEILE = "{0} & \\textbf{{{1}}} & \\tiny{{{3}}} & }}\\\\ \\hline";
            private const string WOCHENENDE = "\\hline";

        public LatexTageszeilenAusgabe(ref KalenderJahr dasJahr)
            :base(ref dasJahr, 5) //XXX: modus sollte nicht nötig sein
        {
        }

        public override void gibAus()
        {
            oeffneStream(".tex");
            gibZeilenAus(START);

            gibZeileAus(String.Format(JAHR, _jahr.Jahreszahl()));

            foreach (Monat monat in _jahr.GibMonate()) 
            {
                gibZeileAus(String.Format(MONATS_TITEL, ersetzeFuerLatex(
                        HilfsKonstrukte.monatsNamen[monat.GibIndex()])));
                gibZeilenAus(MONAT_START);
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
                        gibZeileAus(WOCHENENDE);
                    }
                }
                gibZeilenAus(MONAT_ENDE);
            }

            gibZeileAus(ENDE);
            schliesseStream();
        }
    }
}
