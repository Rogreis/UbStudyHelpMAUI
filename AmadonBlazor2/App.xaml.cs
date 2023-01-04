namespace AmadonBlazor2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Application.Current.UserAppTheme = AppTheme.Dark;
            MainPage = new MainPage();
        }
    }
}