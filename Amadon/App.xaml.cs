using Amadon.Services;

namespace Amadon
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();

            this.PageDisappearing += App_PageDisappearing;
        }

        private void App_PageDisappearing(object sender, Page e)
        {
            SettingsService.Store();
        }


    }
}