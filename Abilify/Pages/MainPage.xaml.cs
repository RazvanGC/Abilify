using Abilify.Models;
using Abilify.PageModels;
using Abilify.Services;

namespace Abilify.Pages
{
    public partial class MainPage : ContentPage
    {

        public MainPage(MainPageModel model)
        {
            InitializeComponent();

            BindingContext = model;
        }

        private void OnViewAllClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("alltools");
        }

        private void OnImageButtonClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var route = (string)button.CommandParameter;

            Shell.Current.GoToAsync($"//{route}");
        }
    }
}