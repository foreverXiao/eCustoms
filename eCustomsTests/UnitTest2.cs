using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eCustomsTests
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual("abc", "cba");
        }
    }
}
