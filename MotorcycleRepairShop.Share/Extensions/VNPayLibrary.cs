using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MotorcycleRepairShop.Share.Extensions
{
    public class VNPayLibrary
    {
        private readonly SortedList<string, string> _requestData = [];
        private readonly SortedList<string, string> _responseData = [];

        public void AddRequestData(string key, string value) => _requestData[key] = value;
        public void AddResponseData(string key, string value) => _responseData[key] = value;

        public string CreateRequestUrl(string baseUrl, string secretKey)
        {
            var data = _requestData.Select(kvp => $"{kvp.Key}={HttpUtility.UrlEncode(kvp.Value)}");
            var rawData = string.Join("&", data);

            var hash = HmacSHA512(secretKey, rawData);
            return $"{baseUrl}?{rawData}&vnp_SecureHash={hash}";
        }

        public bool ValidateSignature(string secretKey)
        {
            var vnpSecureHash = _responseData["vnp_SecureHash"];
            _responseData.Remove("vnp_SecureHash");

            var data = _responseData.Select(kvp => $"{kvp.Key}={HttpUtility.UrlEncode(kvp.Value)}");
            var rawData = string.Join("&", data);

            var hash = HmacSHA512(secretKey, rawData);
            return vnpSecureHash == hash;
        }

        private static string HmacSHA512(string key, string input)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            return BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", "").ToLower();
        }
    }
}
