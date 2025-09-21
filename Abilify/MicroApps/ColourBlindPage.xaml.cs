using Microsoft.Maui.Controls;
using SkiaSharp;
using SkiaSharp.Views.Maui.Controls;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Abilify.MicroApps
{
    public partial class ColourBlindPage : ContentPage
    {
        private string _loadedImagePath;
        private SKData _processedImageData;

        public ColourBlindPage()
        {
            InitializeComponent();
        }

        private async void OnLoadPhotoClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Select an Image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    _loadedImagePath = result.FullPath;
                    FilteredImage.Source = ImageSource.FromFile(_loadedImagePath);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load image: {ex.Message}", "OK");
            }
        }

        private async void OnApplyFilterClicked(object sender, EventArgs e)
        {
            await ApplyFilter("Protanopia");
        }

        private async Task ApplyFilter(string filterType)
        {
            if (string.IsNullOrEmpty(_loadedImagePath))
            {
                await DisplayAlert("Error", "Please load a photo first.", "OK");
                return;
            }

            try
            {
                LoadingIndicator.IsRunning = true;
                LoadingIndicator.IsVisible = true;
                LoadingLabel.IsVisible = true;

                await Task.Delay(200); // Small delay to ensure UI updates

                var stream = File.OpenRead(_loadedImagePath);
                var bitmap = SKBitmap.Decode(stream);
                stream.Close();

                if (bitmap == null || bitmap.Width == 0 || bitmap.Height == 0)
                {
                    await DisplayAlert("Error", "Failed to decode image or image is empty.", "OK");
                    return;
                }

                var processedBitmap = ApplyFilterProtanopia(bitmap);

                if (processedBitmap == null)
                {
                    await DisplayAlert("Error", "Image processing failed. The processed bitmap is null.", "OK");
                    return;
                }

                var image = SKImage.FromBitmap(processedBitmap);

                if (image == null)
                {
                    await DisplayAlert("Error", "Failed to convert bitmap to image.", "OK");
                    return;
                }

                _processedImageData = image.Encode(SKEncodedImageFormat.Jpeg, 100);

                if (_processedImageData == null)
                {
                    await DisplayAlert("Error", "Failed to encode image data.", "OK");
                    return;
                }

                // Save to app's internal storage (IMPORTANT)
                var newPath = await SaveProcessedImageToFile();

                if (newPath != null)
                {
                    _loadedImagePath = newPath; // Update the path to the new saved file
                    FilteredImage.Source = ImageSource.FromFile(_loadedImagePath);

                    await DisplayAlert("Success", $"Filter applied successfully and saved!", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "Failed to save the processed image.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to apply filter: {ex.Message}", "OK");
            }
            finally
            {
                LoadingIndicator.IsRunning = false;
                LoadingIndicator.IsVisible = false;
                LoadingLabel.IsVisible = false;
            }
        }

        private async Task<string> SaveProcessedImageToFile()
        {
            try
            {
                string appStoragePath = FileSystem.AppDataDirectory;
                string newFileName = Path.Combine(appStoragePath, $"processed_{DateTime.Now:yyyyMMdd_HHmmss}.jpg");

                using var stream = File.Create(newFileName);
                _processedImageData.SaveTo(stream);
                await stream.FlushAsync();
                stream.Close();

                return newFileName; // Return the new file path
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save processed image: {ex.Message}", "OK");
                return null;
            }
        }

        private SKBitmap ApplyFilterProtanopia(SKBitmap originalBitmap)
        {
            int width = originalBitmap.Width;
            int height = originalBitmap.Height;

            var resultBitmap = new SKBitmap(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var pixel = originalBitmap.GetPixel(x, y);
                    var filteredPixel = ApplyProtanopiaFilter(pixel);
                    resultBitmap.SetPixel(x, y, filteredPixel);
                }
            }

            return resultBitmap;
        }

        private SKColor ApplyProtanopiaFilter(SKColor color)
        {
            int originalR = color.Red;
            int originalG = color.Green;
            int originalB = color.Blue;

            int r = Math.Min(255, (int)(originalR * 1.3));
            int g = Math.Min(255, (int)(originalG * 1.3));
            int b = Math.Min(255, (int)(originalB * 1.3));

            r = Math.Min(255, (int)((r * 0.9) + (g * 0.1)));
            g = Math.Min(255, (int)((g * 0.8) + (b * 0.2)));
            b = Math.Min(255, (int)((b * 0.7) + (r * 0.3)));

            r = Math.Abs(255 - r) / 2 + originalR / 2;
            g = Math.Abs(255 - g) / 2 + originalG / 2;
            b = Math.Abs(255 - b) / 2 + originalB / 2;

            return new SKColor((byte)r, (byte)g, (byte)b, color.Alpha);
        }
    }
}
