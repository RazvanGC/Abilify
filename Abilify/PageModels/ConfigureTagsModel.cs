using Abilify.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;

namespace Abilify.PageModels
{
    public partial class ConfigureTagsPageModel : ObservableObject
    {
        // List of available tags
        public ObservableCollection<UserTag> UserTags { get; set; } = new ObservableCollection<UserTag>();

        // Command to save the selected tags
        public IRelayCommand SaveTagsCommand { get; }

        public ConfigureTagsPageModel()
        {
            // Load the tags when the page is created
            LoadTags();

            // Command binding to save selected tags
            SaveTagsCommand = new RelayCommand(SaveTags);
        }

        private void LoadTags()
        {
            // Load the selected tags from Preferences (if they exist)
            var selectedTagsJson = Preferences.Get("SelectedTags", "[]");
            var selectedTags = JsonSerializer.Deserialize<List<string>>(selectedTagsJson);

            // In a real use case, you could load tags from a database or service
            var availableTags = new List<string> { "ADHD", "Autism", "Anxiety", "Depression", "PTSD", "Dyslexia", "Memory Loss", "Learning Difficulties", "Blind", "Low Vision", "Colour Blind", "Deaf", "Hard of Hearing", "Non-verbal", "Speech Impairment", "Communication Support", "Low Hand Dexterity", "Mobility Impairment", "Wheelchair User", "Tremors", "Paralysis", "Elderly Support" }; // List of all available tags

            foreach (var tag in availableTags)
            {
                UserTags.Add(new UserTag
                {
                    Name = tag,
                    IsSelected = selectedTags.Contains(tag)
                });
            }
        }

        private void SaveTags()
        {
            // Get the selected tags
            var selectedTags = UserTags.Where(tag => tag.IsSelected).Select(tag => tag.Name).ToList();

            // Serialize the selected tags to JSON
            var selectedTagsJson = JsonSerializer.Serialize(selectedTags);

            // Save to Preferences using the JSON string
            Preferences.Set("SelectedTags", selectedTagsJson);

            // Show a message, or navigate away
            System.Diagnostics.Debug.WriteLine($"Selected Tags: {string.Join(", ", selectedTags)}");

            NavigateTo("main");
        }
        private async void NavigateTo(string route)
        {
            await Shell.Current.GoToAsync($"//{route}");
        }
    }
}
