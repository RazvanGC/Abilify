using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace Abilify.MicroApps
{
    public partial class AssistantToolPage : ContentPage
    {
        public AssistantToolPage()
        {
            InitializeComponent();
        }

        // Handle RadioButton CheckedChanged event to toggle visibility of the custom message TextBox
        private void OnRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            // Show or hide the custom message TextBox based on the selected RadioButton
            CustomMessageEntry.IsVisible = CustomMessageRadioButton.IsChecked;
            WantMessageEntry.IsVisible = WantMessageRadioButton.IsChecked;
        }

        // Handle button click to send the current location via WhatsApp
        private async void OnSendWhatsAppMessageClicked(object sender, EventArgs e)
        {
            try
            {
                // Get the phone number from the Entry
                var phoneNumber = PhoneNumberEntry.Text;

                // Check if the phone number is empty
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    await DisplayAlert("Error", "Please enter a valid phone number.", "OK");
                    return;
                }

                // Request the user's current location
                var location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));

                // Check if location was retrieved successfully
                if (location == null)
                {
                    await DisplayAlert("Error", "Unable to retrieve location.", "OK");
                    return;
                }

                // Format the latitude and longitude with a period as the decimal separator
                var latitude = location.Latitude.ToString("F6", CultureInfo.InvariantCulture); // Ensures 6 decimal places and period
                var longitude = location.Longitude.ToString("F6", CultureInfo.InvariantCulture); // Ensures 6 decimal places and period

                // Create the location URL using the format https://maps.google.com/?q=latitude,longitude
                var locationUrl = $"https://maps.google.com/?q={latitude},{longitude}";

                // Get the selected message
                string message = string.Empty;

                if (HelpRadioButton.IsChecked)
                {
                    message = $"Come help me: {locationUrl}";
                }
                else if (CustomMessageRadioButton.IsChecked)
                {
                    var customMessage = CustomMessageEntry.Text;

                    if (string.IsNullOrEmpty(customMessage))
                    {
                        await DisplayAlert("Error", "Please enter a custom message.", "OK");
                        return;
                    }

                    message = $"I need: {customMessage}.";
                    //message = $"I need: {customMessage}. My location: {locationUrl}";
                }
                else if (WantMessageRadioButton.IsChecked)
                {
                    var wantMessage = WantMessageEntry.Text;

                    if (string.IsNullOrEmpty(wantMessage))
                    {
                        await DisplayAlert("Error", "Please enter a message for 'I want to...'.", "OK");
                        return;
                    }

                    message = $"I want to: {wantMessage}.";
                    //message = $"I want to: {wantMessage}. My location: {locationUrl}";
                }
                else if (!HelpRadioButton.IsChecked && !CustomMessageRadioButton.IsChecked && !WantMessageRadioButton.IsChecked)
                {
                    DisplayAlert("Error", "Choose one of the options", "OK");
                    return;
                }

                // Construct the WhatsApp URL scheme with the message
                var whatsappUrl = $"https://wa.me/{phoneNumber}?text={Uri.EscapeDataString(message)}";

                // Open WhatsApp with the location link
                await Launcher.OpenAsync(whatsappUrl);
            }
            catch (Exception ex)
            {
                // Handle errors (e.g., if WhatsApp isn't installed or location couldn't be retrieved)
                await DisplayAlert("Error", "Unable to open WhatsApp: " + ex.Message, "OK");
            }
        }
    }
}
