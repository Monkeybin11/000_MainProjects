using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelLib.Monad;


namespace ModelLibTest
{
    [TestClass]
    public class MaybeTest
    {
        [TestMethod]
        public void MaybeStringTest()
        {
            var testc = new testclass();
            var classmaybe = testc.ToMaybe();

            Console.WriteLine( classmaybe.ToString() );
            Console.ReadLine();

            Assert.Fail();


        }
    }

    public class testclass
    {

    }
}
