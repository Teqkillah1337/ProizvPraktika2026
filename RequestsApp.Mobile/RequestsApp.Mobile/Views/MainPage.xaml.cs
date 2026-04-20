using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using RequestsApp.Mobile.Models;
using RequestsApp.Mobile.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RequestsApp.Mobile.Views
{
    public partial class MainPage : TabbedPage
    {
        private string _userId;
        private ObservableCollection<Request> _requests = new();
        private Request _selectedRequest;

        public MainPage(string userId, string userEmail)
        {
            InitializeComponent();
            _userId = userId;
            lblEmail.Text = userEmail;

            pickerCategory.Items.Add("Благоустройство");
            pickerCategory.Items.Add("ЖКХ");
            pickerCategory.Items.Add("Социальная помощь");
            pickerCategory.Items.Add("Образование");
            pickerCategory.Items.Add("Здравоохранение");
            pickerCategory.Items.Add("Другое");
            pickerCategory.SelectedIndex = 0;

            btnNewRequest.Clicked += (s, e) => CurrentPage = Children[1];
            btnSubmit.Clicked += OnSubmitRequest;
            btnDelete.Clicked += OnDeleteRequest;
            btnLogout.Clicked += async (s, e) =>
            {
                await SupabaseService.SignOut();
                await Navigation.PopToRootAsync();
            };

            LoadRequests();

            btnDelete.IsEnabled = false;
        }

        private async void LoadRequests()
        {
            var requests = await SupabaseService.GetMyRequests(_userId);
            _requests = new ObservableCollection<Request>(requests);

            cvRequests.ItemTemplate = new DataTemplate(() =>
            {
                var border = new Border
                {
                    Stroke = Colors.Gray,
                    StrokeThickness = 1,
                    StrokeShape = new RoundRectangle { CornerRadius = 10 },
                    Padding = 10,
                    Margin = new Thickness(0, 5)
                };

                var layout = new VerticalStackLayout { Spacing = 4 };

                var numberLabel = new Label { FontAttributes = FontAttributes.Bold, FontSize = 14 };
                numberLabel.SetBinding(Label.TextProperty, "RequestNumber");

                var fioLabel = new Label { FontSize = 13 };
                fioLabel.SetBinding(Label.TextProperty, "CitizenName", stringFormat: "ФИО: {0}");

                var phoneLabel = new Label { FontSize = 13 };
                phoneLabel.SetBinding(Label.TextProperty, "CitizenPhone", stringFormat: "Телефон: {0}");

                var categoryLabel = new Label { FontSize = 13 };
                categoryLabel.SetBinding(Label.TextProperty, "CategoryName", stringFormat: "Категория: {0}");

                var descLabel = new Label { FontSize = 12 };
                descLabel.SetBinding(Label.TextProperty, "Description", stringFormat: "Описание: {0}");

                var statusLabel = new Label { FontSize = 12, FontAttributes = FontAttributes.Bold };
                statusLabel.SetBinding(Label.TextProperty, "StatusName", stringFormat: "Статус: {0}");

                var dateLabel = new Label { FontSize = 11, TextColor = Colors.Gray };
                dateLabel.SetBinding(Label.TextProperty, "CreatedDate", stringFormat: "Дата: {0:dd.MM.yyyy HH:mm}");

                layout.Add(numberLabel);
                layout.Add(fioLabel);
                layout.Add(phoneLabel);
                layout.Add(categoryLabel);
                layout.Add(descLabel);
                layout.Add(statusLabel);
                layout.Add(dateLabel);

                border.Content = layout;
                return border;
            });

            cvRequests.ItemsSource = _requests;
        }

        private void OnRequestSelected(object sender, SelectionChangedEventArgs e)
        {
            _selectedRequest = e.CurrentSelection.FirstOrDefault() as Request;
            btnDelete.IsEnabled = _selectedRequest != null;
        }

        private async void OnSubmitRequest(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCitizenName.Text))
            {
                await DisplayAlert("Ошибка", "Введите ФИО", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                await DisplayAlert("Ошибка", "Введите описание", "OK");
                return;
            }

            var request = new Request
            {
                RequestNumber = $"REQ-{DateTime.Now:yyyyMMddHHmmss}",
                CitizenName = txtCitizenName.Text,
                CitizenPhone = txtPhone.Text,
                CategoryName = pickerCategory.SelectedItem?.ToString(),
                Description = txtDescription.Text,
                StatusName = "Новая",
                CreatedDate = DateTime.Now,
                UserId = _userId
            };

            bool success = await SupabaseService.CreateRequest(request, _userId);
            if (success)
            {
                await DisplayAlert("Успех", "Заявка создана", "OK");
                txtCitizenName.Text = "";
                txtPhone.Text = "";
                txtDescription.Text = "";
                LoadRequests();
                CurrentPage = Children[0];
            }
            else
            {
                await DisplayAlert("Ошибка", "Не удалось создать", "OK");
            }
        }

        private async void OnDeleteRequest(object sender, EventArgs e)
        {
            if (_selectedRequest == null) return;
            bool confirm = await DisplayAlert("Удаление", $"Удалить заявку {_selectedRequest.RequestNumber}?", "Да", "Нет");
            if (confirm)
            {
                bool success = await SupabaseService.DeleteRequest(_selectedRequest.Id);
                if (success)
                {
                    await DisplayAlert("Успех", "Заявка удалена", "OK");
                    LoadRequests();
                }
                else
                {
                    await DisplayAlert("Ошибка", "Не удалось удалить", "OK");
                }
            }
        }
    }
}