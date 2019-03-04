namespace _0xOrderSigning.Models
{
    public class Wallet
    {
        public Wallet(string address, string key)
        {
            Address = address;
            Key = key;
        }

        public Wallet()
        {
        }

        public string Address { get; set; }

        public string Key { get; set; }

    }
}