using NUnit.Framework;
using System;
using System.IO;
using System.Collections.Generic;


//http://kalender-365.de
namespace KalenderWelt
{
    [TestFixture]
    public class KalenderJahrTest 
    {

        [Test]
        public void KalenderJahr () 
        {
            KalenderJahr jahr = new KalenderJahr(2012);
            Assert.AreEqual(jahr.GibMonate().Count, HilfsKonstrukte.MONATE_IM_JAHR);
            Assert.AreEqual(jahr.IstSchaltjahr(), true);
            Assert.AreEqual(jahr.GibTage().Count, HilfsKonstrukte.TAGE_IM_JAHR + 1);
        }

        [Test]
        public void StartWochentage() 
        {
            KalenderJahr jahr = new KalenderJahr(2012);
            Assert.AreEqual(jahr.StartWochenTag(), 6);
            jahr = new KalenderJahr(1583);
            Assert.AreEqual(jahr.StartWochenTag(), 5);
            jahr = new KalenderJahr(1899);
            Assert.AreEqual(jahr.StartWochenTag(), 6);
            jahr = new KalenderJahr(1900);
            Assert.AreEqual(jahr.StartWochenTag(), 0);
            jahr = new KalenderJahr(1981);
            Assert.AreEqual(jahr.StartWochenTag(), 3);
        }

        [Test]
        public void Schaltjahre() 
        {
            KalenderJahr jahr = new KalenderJahr(2012);
            Assert.AreEqual(jahr.IstSchaltjahr(), true);
            jahr = new KalenderJahr(2011);
            Assert.AreEqual(jahr.IstSchaltjahr(), false);
            jahr = new KalenderJahr(1900);
            Assert.AreEqual(jahr.IstSchaltjahr(), false);
            jahr = new KalenderJahr(1984);
            Assert.AreEqual(jahr.IstSchaltjahr(), true);
            jahr = new KalenderJahr(2000);
            Assert.AreEqual(jahr.IstSchaltjahr(), true);
            jahr = new KalenderJahr(2100);
            Assert.AreEqual(jahr.IstSchaltjahr(), false);
        }

        [Test]
        public void Monat() 
        {
            KalenderJahr jahr = new KalenderJahr(2012);
            Assert.AreEqual(jahr.GibMonate()[0].GibTage().Count, 31);
            Assert.AreEqual(jahr.GibMonate()[1].GibTage().Count, 29);
            Assert.AreEqual(jahr.GibMonate()[2].GibTage().Count, 31);
            Assert.AreEqual(jahr.GibMonate()[3].GibTage().Count, 30);
            Assert.AreEqual(jahr.GibMonate()[0].GibIndex(), 0);
            Assert.AreEqual(jahr.GibMonate()[HilfsKonstrukte.MONATE_IM_JAHR - 1].GibIndex(), HilfsKonstrukte.MONATE_IM_JAHR - 1);
            Tag fakeTag = new Tag(32,0,32);
            jahr.GibMonate()[0].FuegeTagAn(ref fakeTag);
            Assert.AreEqual(jahr.GibMonate()[0].GibTage().Count, 32);
        }

        [Test]
        public void Tag() 
        {
            KalenderJahr jahr = new KalenderJahr(2012);
            Assert.AreEqual(jahr.GibMonate()[0].GibTage().Count, 31);
            Assert.AreEqual(jahr.GibMonate()[0].GibTage()[0].GibIndex(), 1);
            Assert.AreEqual(jahr.GibMonate()[0].GibTage()[30].GibIndex(), 31);
        }
    }

    [TestFixture]
    public class EintragTest 
    {
        [Test]
        public void EinmaligerTermin () 
        {
            List<Eintrag> meineEintraege = Eintrag.LeseEintraegeAusDatei("testfixtures/testeintraege.xml");

            KalenderJahr jahr2012 = new KalenderJahr(2012);
            jahr2012.TrageEin(ref meineEintraege);
            Assert.AreEqual(jahr2012.GibMonate()[1].GibTage()[22].EintraegeAlsString(), "Termin");
            
            //teste, ob nicht in anderem Jahr eingetragen
            KalenderJahr jahr2013 = new KalenderJahr(2013);
            jahr2013.TrageEin(ref meineEintraege);
            Assert.AreEqual(jahr2013.GibMonate()[1].GibTage()[22].EintraegeAlsString(), "");

            Assert.IsTrue(jahr2012.GibMonate()[1].GibTage()[28].EintraegeAlsString().Contains("Schalttagtermin"), "Schalttagtermin wurde nicht korrekt eingetragen");
            //Eimalige Termine sollen nicht auf den 28ten übertragen werden sondern als nicht valide ignoriert werden
            Assert.IsFalse(jahr2013.GibMonate()[1].GibTage()[27].EintraegeAlsString().Contains("Schalttagtermin"), "Einmalige Termine wurde auf den 28ten übetragen, soll aber als nicht valide gelten");
        }

        [Test]
        public void JaehrlichesEreignisAnFestemTermin () 
        {
            List<Eintrag> meineEintraege = Eintrag.LeseEintraegeAusDatei("testfixtures/testeintraege.xml");
            KalenderJahr jahr2012 = new KalenderJahr(2012);
            jahr2012.TrageEin(ref meineEintraege);
            Assert.AreEqual(jahr2012.GibMonate()[0].GibTage()[29].EintraegeAlsString(), "Januar Geburtstag");
            
            //teste, ob auch in anderem Jahr eingetragen
            KalenderJahr jahr2013 = new KalenderJahr(2013);
            jahr2013.TrageEin(ref meineEintraege);
            Assert.AreEqual(jahr2013.GibMonate()[0].GibTage()[29].EintraegeAlsString(), "Januar Geburtstag");

            Assert.IsTrue(jahr2012.GibMonate()[1].GibTage()[28].EintraegeAlsString().Contains("Schaltjahr Geburtstag"), "Schaltjahr Geburtstag wurde nicht korrekt eingetragen");
            //jaehrliche Ereignisse werden auf den 28ten übertragen 
            Assert.IsTrue(jahr2013.GibMonate()[1].GibTage()[27].EintraegeAlsString().Contains("Schaltjahr Geburtstag"), "jaehrlicher Termine am Schalttag wurde nicht korrekt auf den 28ten übetragen");
        }

        [Test]
        public void Geburtstag ()
        {
            List<Eintrag> meineEintraege = Eintrag.LeseEintraegeAusDatei("testfixtures/testeintraege.xml");
            KalenderJahr jahr2012 = new KalenderJahr(2012);
            jahr2012.TrageEin(ref meineEintraege);
            //Angabe des Jahrs ist nicht nötig
            Assert.AreEqual(jahr2012.GibMonate()[11].GibTage()[30].EintraegeAlsString(), "Geburtstag Aisha");
            //korrekte Anzeige des Alters
            Assert.AreEqual(jahr2012.GibMonate()[3].GibTage()[29].EintraegeAlsString(), "Geburtstag Barbara (" + (2012 - 1918) + ")");
        }
    }

    [TestFixture]
    public class AusgabeTest 
    {
        [Test]
        public void TageszeilenAusgabe () 
        {
            KalenderJahr jahr = new KalenderJahr(2012);
            int eingabemodus = 3;   //XXX: das sollte nicht nötig sein
            Ausgabe ausgabe = new TageszeilenAusgabe(ref jahr, eingabemodus, false);
            ausgabe.setzeTestModus(true);
            ausgabe.gibAus();
            StreamReader streamReader = new StreamReader(ausgabe.DateiName);
            string text = streamReader.ReadToEnd();
            streamReader.Close();

            Assert.IsTrue(text.Length > 0, "Ausgabe ist leer");
            Assert.IsTrue(text.IndexOf("2012") > -1, "Jahreszahl fehlt");
            int positionFebruar = text.IndexOf(HilfsKonstrukte.monatsNamen[1]);
            int positionMaerz = text.IndexOf(HilfsKonstrukte.monatsNamen[2]);
            Assert.IsTrue(positionFebruar > -1, "Februar-Markierung fehlt");
            Assert.IsTrue(positionMaerz > -1, "März-Markierung fehlt");
            Assert.IsTrue(positionFebruar < positionMaerz);

            //pruefe ob Februar 29 Tage hat
            Assert.IsTrue(text.IndexOf("29",positionFebruar) < positionMaerz, "29 fehlt im Februar");

            //pruefe ob Linie nach Sonntag
            int positionSO = text.IndexOf(HilfsKonstrukte.wochenTagNamenKurz[6], positionMaerz);
            positionSO = text.IndexOf('\n', positionSO);  //bis zur nächstenZeile
            Assert.AreEqual(text.IndexOf("-------------", positionSO), positionSO + 1);

            //pruefeWochentag 1.12.
            int wochenTag1201 = jahr.GibMonate()[11].GibTage()[0].GibWochentag();
            string wochenTagAlsString = HilfsKonstrukte.wochenTagNamenKurz[wochenTag1201];
            int positionDezember = text.IndexOf(HilfsKonstrukte.monatsNamen[11]);
            positionDezember = text.IndexOf('\n', positionDezember);
            Assert.AreEqual(text.IndexOf(wochenTagAlsString, positionDezember), positionDezember + 1);
        }

        [Test]
        public void EintraegeInTageszeilenAusgabe () 
        {
            KalenderJahr jahr = new KalenderJahr(2012);
            List<Eintrag> meineEintraege = Eintrag.LeseEintraegeAusDatei("testfixtures/testeintraege.xml");
            jahr.TrageEin(ref meineEintraege);
            int eingabemodus = 4;   //XXX: das sollte nicht nötig sein
            Ausgabe ausgabe = new TageszeilenAusgabe(ref jahr, eingabemodus, true);
            ausgabe.setzeTestModus(true);
            ausgabe.gibAus();
            StreamReader streamReader = new StreamReader(ausgabe.DateiName);
            string text = streamReader.ReadToEnd();
            streamReader.Close();

            Assert.IsTrue(text.Length > 0, "Ausgabe ist leer");
            Assert.IsTrue(text.IndexOf("2012") > -1, "Jahreszahl kommt nicht vor");
            int positionJanGeburtstag = text.IndexOf("Januar Geburtstag");
            int positionDezGeburtstag = text.IndexOf("Dezember Geburtstag");
            int positionTermin = text.IndexOf("Termin");
            Assert.IsTrue(positionJanGeburtstag > -1, "JanuarGeburtstag ist nicht eingetragen");
            Assert.IsTrue(positionDezGeburtstag > -1, "DezemberGeburtstag ist nicht eingetragen");
            Assert.IsTrue(positionTermin > -1, "Termin ist nicht eingetragen");
            
            Assert.IsTrue(text[positionJanGeburtstag-4] == '3' &&
                          text[positionJanGeburtstag-3] == '0', "JanuarGeburtstag steht an der falschen Stelle");
            Assert.IsTrue(text[positionDezGeburtstag-4] == '3' &&
                          text[positionDezGeburtstag-3] == '0', "DezemberGeburtstag steht an der falschen Stelle");
            Assert.IsTrue(text[positionTermin-4] == '2' &&
                          text[positionTermin-3] == '3', "Termin steht an der falschen Stelle");
            int positionFebruar = text.IndexOf(HilfsKonstrukte.monatsNamen[1]);
            int positionMaerz = text.IndexOf(HilfsKonstrukte.monatsNamen[2]);
            Assert.IsTrue(positionTermin > positionFebruar &&
                          positionTermin < positionMaerz, "Termin steht an der falschen Stelle");
        }
    }
}

