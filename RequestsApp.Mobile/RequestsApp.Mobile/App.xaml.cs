using Microsoft.Maui.Controls;
using RequestsApp.Mobile.Views;

namespace RequestsApp.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new NavigationPage(new LoginPage()));
        }
    }
}