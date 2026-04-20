using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;

namespace RequestsApp
{
    public partial class NewRequestWindow : Window
    {
        private string _currentUserId;

        public class CategoryItem
        {
            public int CategoryId { get; set; }
            public string CategoryName { get; set; }
        }

        public NewRequestWindow(string currentUserId)
        {
            InitializeComponent();
            _currentUserId = currentUserId;
            LoadCategories();

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
        }

        private void LoadCategories()
        {
            // Временно добавим категории вручную
            cmbCategory.Items.Add("Благоустройство");
            cmbCategory.Items.Add("ЖКХ");
            cmbCategory.Items.Add("Социальная помощь");
            cmbCategory.Items.Add("Образование");
            cmbCategory.Items.Add("Здравоохранение");
            cmbCategory.Items.Add("Другое");
            cmbCategory.SelectedIndex = 0;
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCitizenName.Text))
            {
                MessageBox.Show("Введите ФИО заявителя");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Введите описание");
                return;
            }

            var request = new RequestItem
            {
                request_number = $"REQ-{DateTime.Now:yyyyMMddHHmmss}",
                citizen_name = txtCitizenName.Text,
                citizen_phone = txtPhone.Text,
                category_name = cmbCategory.SelectedItem?.ToString(),
                description = txtDescription.Text
            };

            bool success = await SupabaseService.CreateRequest(request, _currentUserId);

            if (success)
            {
                MessageBox.Show($"Заявка {request.request_number} создана!");
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Ошибка при создании");
            }
        }

        private int GetCategoryId(string categoryName)
        {
            switch (categoryName)
            {
                case "Благоустройство": return 1;
                case "ЖКХ": return 2;
                case "Социальная помощь": return 3;
                case "Образование": return 4;
                case "Здравоохранение": return 5;
                default: return 6;
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}