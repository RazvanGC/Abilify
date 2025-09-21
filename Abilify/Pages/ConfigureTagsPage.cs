using Abilify.PageModels;
using Microsoft.Maui.Controls;

namespace Abilify.Pages
{
    public partial class ConfigureTagsPage : ContentPage
    {
        public ConfigureTagsPage()
        {
            InitializeComponent();
            // Set the BindingContext to the page model for data binding
            BindingContext = new ConfigureTagsPageModel();
        }
    }
}
