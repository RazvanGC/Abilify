using Microsoft.Maui.Controls;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Abilify.MicroApps
{
    public partial class ReadItPage : ContentPage
    {
        private bool isSpeaking = false;  // Flag to track if TTS is speaking
        private CancellationTokenSource cancellationTokenSource;  // Token for cancellation

        public ReadItPage()
        {
            InitializeComponent();
        }

        // Start reading the text
        private async void OnSpeakClicked(object sender, EventArgs e)
        {
            var text = TextEditor.Text;

            if (!string.IsNullOrEmpty(text))
            {
                // If speech is already in progress, cancel it
                if (isSpeaking)
                {
                    cancellationTokenSource?.Cancel(); // Cancel ongoing speech
                    isSpeaking = false;
                }

                // Create a new CancellationTokenSource
                cancellationTokenSource = new CancellationTokenSource();

                try
                {
                    isSpeaking = true;

                    // Start reading the new text with cancellation token
                    await TextToSpeech.SpeakAsync(text, cancellationTokenSource.Token);

                    isSpeaking = false;
                }
                catch (OperationCanceledException)
                {
                    // Handle the cancellation
                    isSpeaking = false;
                    Console.WriteLine("Speech was canceled.");
                }
            }
            else
            {
                // Display an error message if the text is empty
                await DisplayAlert("Error", "Please enter some text!", "OK");
            }
        }

        // Stop reading the text completely
        private void OnStopClicked(object sender, EventArgs e)
        {
            // Cancel the ongoing speech
            cancellationTokenSource?.Cancel();
            isSpeaking = false;
        }
    }
}
