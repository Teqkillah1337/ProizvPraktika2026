using System;
using System.Windows;
using SupabaseAuthLib;

namespace RequestsApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            btnAuth.Click += BtnAuth_Click;
        }

        private async void BtnAuth_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                txtStatus.Text = "Заполните email и пароль";
                return;
            }

            btnAuth.IsEnabled = false;
            btnAuth.Content = "Подождите...";

            try
            {
                if (rbRegister.IsChecked == true)
                {
                    var (success, message) = await SupabaseAuth.SignUp(email, password);
                    if (success)
                    {
                        txtStatus.Text = "Регистрация успешна! Теперь войдите.";
                        rbLogin.IsChecked = true;
                        txtPassword.Password = "";
                    }
                    else
                    {
                        txtStatus.Text = $"Ошибка: {message}";
                    }
                }
                else
                {
                    var (success, message, userId) = await SupabaseAuth.SignIn(email, password);
                    if (success)
                    {
                        txtStatus.Text = "Вход выполнен!";
                        var mainWindow = new MainWindow(userId);
                        mainWindow.Show();
                        Close();
                    }
                    else
                    {
                        txtStatus.Text = $"Ошибка: {message}";
                    }
                }
            }
            catch (Exception ex)
            {
                txtStatus.Text = $"Ошибка: {ex.Message}";
            }
            finally
            {
                btnAuth.IsEnabled = true;
                btnAuth.Content = rbRegister.IsChecked == true ? "Зарегистрироваться" : "Войти";
            }
        }
    }
}