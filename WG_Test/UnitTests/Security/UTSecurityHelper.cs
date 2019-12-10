using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using WishGrid.Security;

namespace WishGrid.Tests
{
    [TestClass]
    public class UTSecurityHelper
    {
        [TestCategory("SecurityHelper")]
        [TestMethod]
        public void TestVerifyEncryption()
        {
            string value = "password";
            byte[] arrayHash, arraySalt;
            SecurityHelper.Encrypt(value, out arrayHash, out arraySalt);
            string outHash = Encoding.UTF8.GetString(arrayHash);
            string outSalt = Encoding.UTF8.GetString(arraySalt);
            Assert.IsTrue(SecurityHelper.VerifyEncryption(value, arrayHash, arraySalt), "The Hash Value is not equals");
        }
    }
}
