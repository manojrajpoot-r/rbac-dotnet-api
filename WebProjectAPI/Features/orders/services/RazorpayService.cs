using Razorpay.Api;
using System.Security.Cryptography;
using System.Text;

namespace WebProjectAPI.Features.orders.services
{
    public class RazorpayService
    {
        private readonly IConfiguration _config;

        public RazorpayService(
            IConfiguration config)
        {
            _config = config;
        }

        public Order CreateOrder(
            decimal amount)
        {
            var client =
                new RazorpayClient(

                _config["Razorpay:KeyId"],

                _config["Razorpay:KeySecret"]

            );

            Dictionary<string, object>
                options = new()
            {
                { "amount", amount * 100 },

                { "currency", "INR" },

                { "payment_capture", 1 }
            };

            return client.Order.Create(options);
        }

        public string GetKey()
        {
            return _config["Razorpay:KeyId"];
        }

        public bool VerifySignature(
            string orderId,
            string paymentId,
            string signature)
        {
            string secret =
                _config["Razorpay:KeySecret"];

            string payload =
                orderId + "|" + paymentId;

            using var hmac =
                new HMACSHA256(
                    Encoding.UTF8.GetBytes(secret)
                );

            var hash =
                hmac.ComputeHash(
                    Encoding.UTF8.GetBytes(payload)
                );

            var generatedSignature =
                BitConverter.ToString(hash)
                .Replace("-", "")
                .ToLower();

            return generatedSignature ==
                   signature;
        }
    }
}