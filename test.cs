using NUnit.Framework;
using System;

//http://kalender-365.de
namespace KalenderWelt
{
    [TestFixture]
    public class KalenderJahrTest {

        [Test]
        public void KalenderJahr () {
            KalenderJahr jahr = new KalenderJahr(2012);
            Assert.AreEqual(jahr.GibMonate().Count, HilfsKonstrukte.MONATE_IM_JAHR);
            Assert.AreEqual(jahr.IstSchaltjahr(), true);
            Assert.AreEqual(jahr.GibTage().Count, HilfsKonstrukte.TAGE_IM_JAHR + 1);
        }

        [Test]
        public void StartWochentage() {
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
        public void Schaltjahre() {
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
        public void Monat() {
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
        public void Tag() {
            KalenderJahr jahr = new KalenderJahr(2012);
            Assert.AreEqual(jahr.GibMonate()[0].GibTage().Count, 31);
            Assert.AreEqual(jahr.GibMonate()[0].GibTage()[0].GibIndex(), 1);
            Assert.AreEqual(jahr.GibMonate()[0].GibTage()[30].GibIndex(), 31);
        }
    }
}

