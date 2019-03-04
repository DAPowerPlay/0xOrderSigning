using Newtonsoft.Json;
namespace _0xOrderSigning.Models
{
    public class UnsignedOrder
    {
        public UnsignedOrder(string senderAddress, string makerAddress, string takerAddress, decimal makerFee, decimal takerFee,
            decimal makerAssetAmount, decimal takerAssetAmount, string makerAssetData, string takerAssetData, long salt,
            string exchangeAddress, string feeRecipientAddress, long expirationTimeSeconds, string signature)
        {
            SenderAddress = senderAddress;
            MakerAddress = makerAddress;
            TakerAddress = takerAddress;
            MakerFee = makerFee;
            TakerFee = takerFee;
            MakerAssetAmount = makerAssetAmount;
            TakerAssetAmount = takerAssetAmount;
            MakerAssetData = makerAssetData;
            TakerAssetData = takerAssetData;
            Salt = salt;
            ExchangeAddress = exchangeAddress;
            FeeRecipientAddress = feeRecipientAddress;
            ExpirationTimeSeconds = expirationTimeSeconds;
            Signature = signature;
        }

        public UnsignedOrder()
        {
        }


        [JsonProperty(PropertyName = "senderAddress")]
        public string SenderAddress { get; set; }

        [JsonProperty(PropertyName = "makerAddress")]
        public string MakerAddress { get; set; }

        [JsonProperty(PropertyName = "takerAddress")]
        public string TakerAddress { get; set; }

        [JsonProperty(PropertyName = "makerFee")]
        public decimal MakerFee { get; set; }

        [JsonProperty(PropertyName = "takerFee")]
        public decimal TakerFee { get; set; }

        [JsonProperty(PropertyName = "makerAssetAmount")]
        public decimal MakerAssetAmount { get; set; }

        [JsonProperty(PropertyName = "makerAssetAmount")]
        public decimal TakerAssetAmount { get; set; }

        [JsonProperty(PropertyName = "makerAssetData")]
        public string MakerAssetData { get; set; }

        [JsonProperty(PropertyName = "takerAssetData")]
        public string TakerAssetData { get; set; }

        [JsonProperty(PropertyName = "salt")]
        public long Salt { get; set; }

        [JsonProperty(PropertyName = "exchangeAddress")]
        public string ExchangeAddress { get; set; }

        [JsonProperty(PropertyName = "feeRecipientAddress")]
        public string FeeRecipientAddress { get; set; }

        [JsonProperty(PropertyName = "expirationTimeSeconds")]
        public long ExpirationTimeSeconds { get; set; }

        [JsonProperty(PropertyName = "signature")]
        public string Signature { get; set; }
    }
}
