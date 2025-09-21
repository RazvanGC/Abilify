using Abilify.Models;
using Abilify.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using Microsoft.Maui.Storage; // For Preferences

namespace Abilify.PageModels
{
    public partial class MainPageModel : ObservableObject
    {
        [ObservableProperty]
        private bool _showAllTools = false;

        public ObservableCollection<MicroApp> Microapps { get; set; } = new();

        public string TitleText => ShowAllTools ? "All Tools" : "Your Tools";
        public string ShowMoreText => ShowAllTools ? "Show Your Tools" : "Show All Tools";

        public MainPageModel()
        {
            // Load micro apps on initialization
            LoadMicroapps();
        }

        private void LoadMicroapps()
        {
            // Load user tags dynamically from preferences (JSON)
            var userTagsJson = Preferences.Get("SelectedTags", "[]");  // Default to empty list if not found

            var userTags = JsonSerializer.Deserialize<List<string>>(userTagsJson);

            // If the list is empty, handle the empty case (you can program the list empty case here)
            if (userTags == null || !userTags.Any())
            {
                // Handle empty list case here by navigating to configure tags page
                System.Diagnostics.Debug.WriteLine("User tags list is empty.");
                // Navigation here is delayed until the page is fully loaded
                NavigateTo("configtags");
            }
            else
            {
                // Load micro apps based on the selected tags
                if (ShowAllTools)
                {
                    Microapps = new ObservableCollection<MicroApp>(MicroAppRegistry.All);
                }
                else
                {
                    Microapps = new ObservableCollection<MicroApp>(
                        MicroAppRegistry.All.Where(app => app.Tags.Any(tag => userTags.Contains(tag)))
                    );
                }
            }

            OnPropertyChanged(nameof(Microapps));
        }

        private async void NavigateTo(string route)
        {
            await Shell.Current.GoToAsync($"//{route}");
        }

        [RelayCommand]
        private void ToggleShowAllTools()
        {
            ShowAllTools = !ShowAllTools;
            OnPropertyChanged(nameof(TitleText));
            OnPropertyChanged(nameof(ShowMoreText));
            LoadMicroapps(); // Reload the micro apps after toggling
        }

        [RelayCommand]
        private void NavigatedTo() { }

        [RelayCommand]
        private void NavigatedFrom() { }

        [RelayCommand]
        private void Appearing()
        {
            // Only navigate to ConfigureTagsPage if userTags is empty
            LoadMicroapps();
        }
    }
}
