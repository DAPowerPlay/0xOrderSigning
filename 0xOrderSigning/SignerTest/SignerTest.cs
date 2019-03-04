using _0xOrderSigning;
using _0xOrderSigning.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SignerTest
{
    [TestClass]
    public class SignerTest
    {
        [TestMethod]
        public void Signer_GeneratesSignature()
        {
            var userWalletAddress = "0xYOURADDRESSHERE";
            var userPrivateKey = "0xPRIVATEKEY";
            //This is just for example purpose, store key somewhere safe and load it encrypted to your code and decrypt for usage here.
            var wallet = new Wallet(userWalletAddress, userPrivateKey);
            var sigClient = new Signer(wallet);
           
            var order = new UnsignedOrder("0x0000000000000000000000000000000000000000", userWalletAddress, "0x0000000000000000000000000000000000000000",
                0.0M, 0.0M, 8000000000000000M, 20000000000000000000M, "0xf47261b0000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc2",
                "0xf47261b000000000000000000000000001b3ec4aae1b8729529beb4965f27d008788b0eb", 1551283709282, "0x4f833a24e1f95d70f028921e27040ca56e09ab0b", 
                "0xa258b39954cef5cb142fd567a46cddb31a670124", 1551844000,string.Empty);
            //there's address validation already in signer, so if singning fails 
            var signature = sigClient.Sign(order);

            
            //there's address validation already in signer, so if singer fails to recover correct address, it will return null
            Assert.IsTrue(signature != null);
        }
    }
}
