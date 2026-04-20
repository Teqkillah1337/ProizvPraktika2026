using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace RequestsApp
{
    public class SupabaseService
    {
        private static string _supabaseUrl = "https://jdnuiijcotsierswgtkb.supabase.co";
        private static string _supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImpkbnVpaWpjb3RzaWVyc3dndGtiIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NzM5MTIwMzQsImV4cCI6MjA4OTQ4ODAzNH0.zl0H4N5KQd5Cyce2ktgvvJnUvU7O72HiyL3xSRuWGx8";
        private static HttpClient _httpClient = new HttpClient();

        static SupabaseService()
        {
            _httpClient.DefaultRequestHeaders.Add("apikey", _supabaseKey);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_supabaseKey}");
        }

        // Получение заявок пользователя из mobile_requests
        public static async Task<List<RequestItem>> GetMyRequests(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_supabaseUrl}/rest/v1/mobile_requests?user_id=eq.{userId}&order=created_date.desc");
                var result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var requests = JsonSerializer.Deserialize<List<RequestItem>>(result, options);
                    return requests ?? new List<RequestItem>();
                }
                return new List<RequestItem>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
                return new List<RequestItem>();
            }
        }

        // Создание заявки в mobile_requests
        public static async Task<bool> CreateRequest(RequestItem request, string userId)
        {
            try
            {
                var data = new
                {
                    request_number = request.request_number,
                    citizen_name = request.citizen_name,
                    citizen_phone = request.citizen_phone,
                    category_name = request.category_name,
                    description = request.description,
                    status_name = "Новая",
                    created_date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    user_id = userId
                };

                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_supabaseUrl}/rest/v1/mobile_requests", content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания: {ex.Message}");
                return false;
            }
        }
    }
    public class RequestItem
    {
        public int id { get; set; }
        public string request_number { get; set; } = "";
        public string citizen_name { get; set; } = "";
        public string citizen_phone { get; set; } = "";
        public string category_name { get; set; } = "";
        public string description { get; set; } = "";
        public string status_name { get; set; } = "";
        public DateTime created_date { get; set; }
        public string user_id { get; set; } = "";
    }
}