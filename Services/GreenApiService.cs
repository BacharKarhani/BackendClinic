using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace backendclinic.Services
{
    public class GreenApiService
    {
        private readonly string _instanceId;
        private readonly string _apiToken;
        private readonly HttpClient _httpClient;

        public GreenApiService(IConfiguration configuration)
        {
            _instanceId = configuration["GreenAPI:InstanceId"];
            _apiToken = configuration["GreenAPI:ApiToken"];
            _httpClient = new HttpClient();
        }

        public async Task<bool> SendWhatsAppMessageAsync(string userPhoneNumber, string message)
        {
            if (userPhoneNumber.StartsWith("0"))
            {
                userPhoneNumber = "961" + userPhoneNumber.TrimStart('0');
            }
            else if (!userPhoneNumber.StartsWith("961"))
            {
                userPhoneNumber = "961" + userPhoneNumber; 
            }

            var chatId = $"{userPhoneNumber}@c.us"; 

            var url = $"https://7105.api.green-api.com/waInstance{_instanceId}/sendMessage/{_apiToken}";

            var requestData = new
            {
                chatId = chatId,
                message = message
            };

            var jsonContent = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"✅ WhatsApp Message Sent to: {chatId}");
            Console.WriteLine($"🔹 WhatsApp Response: {responseContent}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"❌ Error: {responseContent}");
            }

            return response.IsSuccessStatusCode;
        }
    }
}
