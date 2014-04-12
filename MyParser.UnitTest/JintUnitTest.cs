using Jint;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyParser.UnitTest
{
    [TestClass]
    public class JintUnitTest
    {
        [TestMethod]
        public void JintTest()
        {
            const string script = @"
                  function square(x) { 
                    return x * x; 
                  };
  
                  return square(number);
                  ";

            object result = new JintEngine()
                .SetParameter("number", 3)
                .Run(script);

            Assert.AreEqual(9.0, result);
        }
    }
}