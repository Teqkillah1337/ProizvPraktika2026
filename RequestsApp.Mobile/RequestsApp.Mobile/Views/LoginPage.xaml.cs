using System;
using Microsoft.Maui.Controls;
using RequestsApp.Mobile.Services;

namespace RequestsApp.Mobile.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            btnAuth.Clicked += OnAuthClicked;
        }

        private async void OnAuthClicked(object sender, EventArgs e)
        {
            string email = txtEmail.Text?.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Ошибка", "Заполните email и пароль", "OK");
                return;
            }

            btnAuth.IsEnabled = false;
            btnAuth.Text = "Подождите...";

            if (rbRegister.IsChecked)
            {
                var (success, message) = await SupabaseService.SignUp(email, password);
                await DisplayAlert(success ? "Успех" : "Ошибка", message, "OK");
                if (success)
                {
                    rbLogin.IsChecked = true;
                    txtPassword.Text = "";
                }
            }
            else
            {
                var (success, message, userId) = await SupabaseService.SignIn(email, password);
                if (success)
                {
                    await Navigation.PushAsync(new MainPage(userId, email));
                }
                else
                {
                    await DisplayAlert("Ошибка", message, "OK");
                }
            }

            btnAuth.IsEnabled = true;
            btnAuth.Text = rbRegister.IsChecked ? "Зарегистрироваться" : "Войти";
        }
    }
}