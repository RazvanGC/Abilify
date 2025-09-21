using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace Abilify.MicroApps
{
    public partial class FocusTimerPage : ContentPage
    {
        private int pomodoroMinutes = 25; // Pomodoro duration in minutes
        private int breakMinutes = 5; // Break duration in minutes
        private int longBreakMinutes = 15; // Long break after 4 Pomodoros
        private int cycleCount = 0; // To track the number of Pomodoros completed
        private CancellationTokenSource cancellationTokenSource; // To manage the cancellation of the timer

        public FocusTimerPage()
        {
            InitializeComponent();
        }

        // Method to start the Pomodoro timer
        private async void OnStartPomodoroClicked(object sender, EventArgs e)
        {
            // Disable the Start button while the timer is running
            ((Button)sender).IsEnabled = false;

            // Start the Pomodoro timer
            await StartPomodoroTimer(pomodoroMinutes);
        }

        // Method to stop the Pomodoro timer
        private void OnStopPomodoroClicked(object sender, EventArgs e)
        {
            cancellationTokenSource?.Cancel();
            TimerLabel.Text = "25:00"; // Reset the timer display
            StatusMessageLabel.Text = "Timer stopped"; // Display stop message
            cycleCount = 0; // Reset the Pomodoro cycle count
            ((Button)FindByName("StartButton")).IsEnabled = true;
        }

        // Method to start a Pomodoro session and then the break
        private async Task StartPomodoroTimer(int minutes)
        {
            cancellationTokenSource = new CancellationTokenSource();

            int totalSeconds = minutes * 60;
            int secondsRemaining = totalSeconds;

            // Set initial status to "Focus Time"
            StatusMessageLabel.Text = "Time to focus!";

            while (secondsRemaining > 0)
            {
                if (cancellationTokenSource.Token.IsCancellationRequested)
                    break;

                // Update the timer label with the remaining time
                TimerLabel.Text = TimeSpan.FromSeconds(secondsRemaining).ToString(@"mm\:ss");
                await Task.Delay(1000);
                secondsRemaining--;
            }

            // After the Pomodoro session, take a break
            if (secondsRemaining == 0)
            {
                cycleCount++;

                if (cycleCount >= 4)
                {
                    await DisplayAlert("Focus time finished", "Congrats! Now you deserve a long break!", "OK");
                    // Long break after 4 Pomodoros
                    await StartBreak(longBreakMinutes);
                    cycleCount = 0; // Reset Pomodoro count after long break
                }
                else
                {
                    await DisplayAlert("Focus time finished", "Time to take a break!", "OK");
                    // Short break after each Pomodoro
                    await StartBreak(breakMinutes);
                }
            }
        }

        // Method to handle the break
        private async Task StartBreak(int minutes)
        {
            int totalSeconds = minutes * 60;
            int secondsRemaining = totalSeconds;

            // Set status to "Break Time"
            StatusMessageLabel.Text = "Break Time";

            while (secondsRemaining > 0)
            {
                if (cancellationTokenSource.Token.IsCancellationRequested)
                    break;

                // Update the timer label with the break time
                TimerLabel.Text = TimeSpan.FromSeconds(secondsRemaining).ToString(@"mm\:ss");
                await Task.Delay(1000);
                secondsRemaining--;
            }

            // After the break, prompt to start the next Pomodoro session
            if (secondsRemaining == 0)
            {
                await DisplayAlert("Break Finished", "Time to start the next timer!", "OK");
                //((Button)FindByName("StartButton")).IsEnabled = true; // Re-enable the Start button
                // Start the Pomodoro timer
                await StartPomodoroTimer(pomodoroMinutes);
            }
        }
    }
}
