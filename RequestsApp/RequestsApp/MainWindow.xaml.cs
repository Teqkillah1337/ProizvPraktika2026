using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace RequestsApp
{
    public partial class MainWindow : Window
    {
        private string _userId;
        private ObservableCollection<RequestItem> _requests = new ObservableCollection<RequestItem>();

        public MainWindow(string userId)
        {
            InitializeComponent();
            _userId = userId;
            txtUserInfo.Text = "Пользователь";

            btnNewRequest.Click += BtnNewRequest_Click;
            btnRefresh.Click += BtnRefresh_Click;
            btnLogout.Click += BtnLogout_Click;

            LoadRequests();
        }

        private async void LoadRequests()
        {
            var requests = await SupabaseService.GetMyRequests(_userId);
            _requests = new ObservableCollection<RequestItem>(requests);
            dgvRequests.ItemsSource = _requests;
        }

        private void BtnNewRequest_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new NewRequestWindow(_userId);
            if (dialog.ShowDialog() == true)
            {
                LoadRequests();
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadRequests();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
    }
}