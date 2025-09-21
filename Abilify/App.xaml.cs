namespace Abilify
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Set the default theme to light
            UserAppTheme = AppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}