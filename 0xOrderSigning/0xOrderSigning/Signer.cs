using _0xOrderSigning.Models;
using Nethereum.ABI.Encoders;
using Nethereum.ABI.Model;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
namespace _0xOrderSigning
{
    public class Signer
    {
        private Wallet _wallet;

        public Signer(Wallet wallet)
        {
            _wallet = wallet;
        }

        public string Sign(UnsignedOrder order)
        {
            var EIP191_HEADER = "1901".HexToByteArray();
            var messageSigner = new EthereumMessageSigner();
            var stringSchemaHash = "EIP712Domain(string name,string version,address verifyingContract)";
            var addressTypeEncoder = new AddressTypeEncoder();


            byte[] EIP712_DOMAIN_SEPARATOR_SCHEMA_HASH = messageSigner.Hash(Encoding.UTF8.GetBytes(stringSchemaHash));
            var newParams = new List<byte>();
            newParams.AddRange(EIP712_DOMAIN_SEPARATOR_SCHEMA_HASH);
            newParams.AddRange(messageSigner.Hash(Encoding.UTF8.GetBytes("0x Protocol")));
            newParams.AddRange(messageSigner.Hash(Encoding.UTF8.GetBytes("2")));
            newParams.AddRange(addressTypeEncoder.Encode(_wallet.Address));

            var newParamsHex = newParams.ToArray().ToHex(true);
            byte[] EIP712_DOMAIN_HASH = messageSigner.Hash(newParams.ToArray());
            var hashToHex = EIP712_DOMAIN_HASH.ToHex(true);
            var orderSchemaHash =
                "Order(address makerAddress,address takerAddress,address feeRecipientAddress,address senderAddress,uint256 makerAssetAmount,uint256 takerAssetAmount,uint256 makerFee,uint256 takerFee,uint256 expirationTimeSeconds,uint256 salt,bytes makerAssetData,bytes takerAssetData)";

            var EIP712_ORDER_SCHEMA_HASH = messageSigner.Hash(Encoding.UTF8.GetBytes(orderSchemaHash));
            var dataForMakerAssetDataHash = order.MakerAssetData.HexToByteArray();

            var dataForTakerAssetDataHash = order.TakerAssetData.HexToByteArray();

            var hashMakerAssetData =
                messageSigner.Hash(dataForMakerAssetDataHash);
            var hashTakerAssetData =
                messageSigner.Hash(dataForTakerAssetDataHash);

            var plainData = new object[]
            {
                EIP712_ORDER_SCHEMA_HASH,
                order.MakerAddress,
                order.TakerAddress,
                order.FeeRecipientAddress,
                order.SenderAddress,
                (BigInteger)order.MakerAssetAmount,
                (BigInteger)order.TakerAssetAmount,
                (BigInteger)order.MakerFee,
                (BigInteger)order.TakerFee,
                (BigInteger)order.ExpirationTimeSeconds,
                (BigInteger)order.Salt,
                hashMakerAssetData,
                hashTakerAssetData

            };
            
            var parameters = new[]
            {
                new Parameter("bytes", 1),
                new Parameter("address", 1),
                new Parameter("address", 1),
                new Parameter("address", 1),
                new Parameter("address", 1),
                new Parameter("uint256", 1),
                new Parameter("uint256", 1),
                new Parameter("uint256", 1),
                new Parameter("uint256", 1),
                new Parameter("uint256", 1),
                new Parameter("uint256", 1),
                new Parameter("bytes", 1),
                new Parameter("bytes", 1)

            };
            var packedData = Pack(plainData, parameters);
            var hexPackedData = packedData.ToHex(true);

            var hashOrderParams = messageSigner.Hash(packedData);
            var hexhashOrdParams = hashOrderParams.ToHex(true);

            var finalByteArray = new List<byte>();
            finalByteArray.AddRange(EIP191_HEADER);
            finalByteArray.AddRange(EIP712_DOMAIN_HASH);
            finalByteArray.AddRange(hashOrderParams);

            //add message for eth sig and hash
            var finalOrderHash = messageSigner.Hash(finalByteArray.ToArray());
            var hashHex = finalOrderHash.ToHex(true);
            var newHash = messageSigner.HashPrefixedMessage(finalOrderHash);


            var signatureRaw = messageSigner.SignAndCalculateV(newHash, _wallet.Key);
            var v = signatureRaw.V;
            var r = signatureRaw.R;
            var s = signatureRaw.S;
            var signature = EthECDSASignature.CreateStringSignature(signatureRaw);
            var byteListSig = new List<byte>();
            byteListSig.AddRange(v);
            byteListSig.AddRange(r);
            byteListSig.AddRange(s);
            var sigHex = byteListSig.ToArray().ToHex(true);
            var concatenatedSigWithSigType = string.Concat(sigHex, "03");
            var bytesSig = signature.HexToByteArray();
            return concatenatedSigWithSigType;
        }
        private byte[] Pack(object[] paramsArray, Parameter[] paramsTypes)
        {
            var intTypeEncoder = new IntTypeEncoder();
            var addressTypeEncoder = new AddressTypeEncoder();

            var cursor = new List<byte>();
            for (var i = 0; i < paramsArray.Length; i++)
            {

                if (paramsTypes[i].Type == "address")
                {
                    cursor.AddRange(addressTypeEncoder.Encode(paramsArray[i]));
                }
                else if (paramsTypes[i].Type == "uint256")
                {
                    cursor.AddRange(intTypeEncoder.Encode(paramsArray[i]));
                }
                else if (paramsTypes[i].Type == "bytes")
                {
                    cursor.AddRange(EncodePackedBytes(paramsArray[i]));
                }
                else if (paramsTypes[i].Type == "string")
                {
                    cursor.AddRange(EncodePackedString(paramsArray[i]));
                }

            }

            return cursor.ToArray();
        }

        

        private byte[] EncodePackedBytes(object value)
        {
            if (!(value is byte[]))
                throw new Exception("byte[] value expected for type 'bytes'");
            return (byte[])value;
        }

        public byte[] EncodePackedString(object value)
        {
            if (!(value is string))
                throw new Exception("String value expected for type 'string'");

            return Encoding.UTF8.GetBytes((string)value);
        }
    }
}

