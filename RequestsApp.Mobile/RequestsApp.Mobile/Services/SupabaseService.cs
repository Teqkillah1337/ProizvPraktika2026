using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RequestsApp.Mobile.Models;

namespace RequestsApp.Mobile.Services
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

        public static async Task<(bool success, string message)> SignUp(string email, string password)
        {
            try
            {
                var data = new { email, password };
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_supabaseUrl}/auth/v1/signup", content);
                if (response.IsSuccessStatusCode)
                    return (true, "Регистрация успешна!");
                return (false, "Ошибка регистрации");
            }
            catch { return (false, "Ошибка подключения"); }
        }

        public static async Task<(bool success, string message, string userId)> SignIn(string email, string password)
        {
            try
            {
                var data = new { email, password };
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_supabaseUrl}/auth/v1/token?grant_type=password", content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(result);
                    var userId = doc.RootElement.GetProperty("user").GetProperty("id").GetString();
                    return (true, "Вход выполнен!", userId ?? "");
                }
                return (false, "Неверный email или пароль", "");
            }
            catch { return (false, "Ошибка подключения", ""); }
        }

        public static async Task<List<Request>> GetMyRequests(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_supabaseUrl}/rest/v1/mobile_requests?user_id=eq.{userId}&order=created_date.desc");
                var result = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var requests = JsonSerializer.Deserialize<List<Request>>(result, options);
                    return requests ?? new List<Request>();
                }
                return new List<Request>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetMyRequests error: {ex.Message}");
                return new List<Request>();
            }
        }

        public static async Task<bool> CreateRequest(Request request, string userId)
        {
            try
            {
                var data = new
                {
                    request_number = request.RequestNumber,
                    citizen_name = request.CitizenName,
                    citizen_phone = request.CitizenPhone ?? "",
                    category_name = request.CategoryName ?? "",
                    description = request.Description,
                    status_name = "Новая",
                    created_date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    user_id = userId
                };
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_supabaseUrl}/rest/v1/mobile_requests", content);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public static async Task<bool> DeleteRequest(int requestId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, $"{_supabaseUrl}/rest/v1/mobile_requests?id=eq.{requestId}");
                request.Headers.Add("apikey", _supabaseKey);
                request.Headers.Add("Authorization", $"Bearer {_supabaseKey}");
                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public static async Task SignOut() => await Task.CompletedTask;
    }
}