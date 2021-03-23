using BasicMaths;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Test_BasicMaths
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_ToplaMethod()
        {
            TemelIslemler tm = new TemelIslemler();
            double sonuc = tm.Topla(10, 10);
            Assert.AreEqual(sonuc, 20);

        }

        [TestMethod]
        public void Test_CarpMethod()
        {
            TemelIslemler tm = new TemelIslemler();
            double sonuc = tm.Carp(5, 5);
            Assert.AreEqual(sonuc, 25);

        }

        [TestMethod]
        public void Test_Pozitifmi()
        {
            TemelIslemler tm = new TemelIslemler();
            bool sonuc = tm.Pozitifmi(1);
            Assert.IsTrue(sonuc);
        }
    }
}
