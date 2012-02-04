using NUnit.Framework;
using System;
using System.IO;

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

            Assert.IsTrue(text.Length > 0);
            Assert.IsTrue(text.IndexOf("2012") > -1);
            int positionFebruar = text.IndexOf(HilfsKonstrukte.monatsNamen[1]);
            int positionMaerz = text.IndexOf(HilfsKonstrukte.monatsNamen[2]);
            Assert.IsTrue(positionFebruar > -1);
            Assert.IsTrue(positionMaerz > -1);
            Assert.IsTrue(positionFebruar < positionMaerz);

            //pruefe ob Februar 29 Tage hat
            Assert.IsTrue(text.IndexOf("29",positionFebruar) < positionMaerz);

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
    }
}

