using System;
using System.Threading.Tasks;
using Supabase;

namespace SupabaseAuthLib
{
    public class SupabaseAuth
    {
        private static Supabase.Client _supabase;
        private static string _supabaseUrl = "https://jdnuiijcotsierswgtkb.supabase.co";
        private static string _supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImpkbnVpaWpjb3RzaWVyc3dndGtiIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NzM5MTIwMzQsImV4cCI6MjA4OTQ4ODAzNH0.zl0H4N5KQd5Cyce2ktgvvJnUvU7O72HiyL3xSRuWGx8";

        public static async Task<Supabase.Client> GetClient()
        {
            if (_supabase == null)
            {
                _supabase = new Supabase.Client(_supabaseUrl, _supabaseKey);
                await _supabase.InitializeAsync();
            }
            return _supabase;
        }

        public static async Task<(bool success, string message)> SignUp(string email, string password)
        {
            try
            {
                var supabase = await GetClient();
                var response = await supabase.Auth.SignUp(email, password);

                if (response?.User != null)
                {
                    return (true, "Регистрация успешна!");
                }
                return (false, "Ошибка регистрации");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public static async Task<(bool success, string message, string userId)> SignIn(string email, string password)
        {
            try
            {
                var supabase = await GetClient();
                var response = await supabase.Auth.SignIn(email, password);

                if (response?.User != null)
                {
                    return (true, "Вход выполнен!", response.User.Id);
                }
                return (false, "Неверный email или пароль", "");
            }
            catch (Exception ex)
            {
                return (false, ex.Message, "");
            }
        }

        public static async Task SignOut()
        {
            var supabase = await GetClient();
            await supabase.Auth.SignOut();
        }

        public static string GetCurrentUserEmail()
        {
            return _supabase?.Auth?.CurrentUser?.Email;
        }
    }
}