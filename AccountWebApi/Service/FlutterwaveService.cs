using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace AccountWebApi.Service
{
    public class FlutterwaveService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "https://api.flutterwave.com/v3/charges?type=bank_transfer";

        private readonly string _publicKey = "FLWPUBK_TEST-3c0917ffb340529efa7790095ef8188b-X";
        private readonly string _secretKey = "FLWSECK_TEST-3741ed79805a8a6c9843dced140060d4-X";
        public FlutterwaveService()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _secretKey);
        }

        public async Task<string> MakePayment(decimal amount, string email)
        {
         
            var request = new
            {
                amount = amount,
                email = email,
                currency = "NGN",
                tx_ref = "PaymentRef"
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"https://api.flutterwave.com/v3/charges?type=bank_transfer", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
        public async Task<string> MakeTransfer(decimal amount, string SourceAccountNo,string DestinationAccountNo, string email)
        {
            var request = new
            {
                //     account_bank = bankCode,  // Bank code e.g., "044" for Access Bank
                SourceAccountNo = SourceAccountNo,
                DestinationAccountNo = DestinationAccountNo,
                amount = amount,
                email = email,
                tx_ref = "TransferRef",
          //      narration = narration,  // Description of the transaction
                currency = "NGN",
                reference = Guid.NewGuid().ToString(),  // Unique reference for the transaction
                callback_url = "https://yourcallbackurl.com"  // Optional: Callback URL for the response
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"https://api.flutterwave.com/v3/charges?type=bank_transfer", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }

    }
}
