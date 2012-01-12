using NUnit.Framework;
using System;

namespace KalenderWelt
{
    [TestFixture]
    public class DateiUndKonsolenAusgabeTest {

        [Test]
        public void AusgabeTest () {
            //TODO
            Assert.AreEqual(42,42);
        }

        [Test]
        public void Test1() {
            Assert.AreEqual("a","a");
        }

        [Test]
        public void Test2() {
            Assert.AreEqual("a","a");
            Assert.AreEqual(42,23);
            Assert.AreEqual("a","ab");
        }

        [Test]
        public void Test3() {
            Assert.AreEqual("a","a");
            Assert.AreEqual("a","ab");
            Assert.AreEqual(42,23);
        }
    }
}

