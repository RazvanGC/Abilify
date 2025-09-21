using Microsoft.Maui.Controls;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Abilify.MicroApps
{
	public partial class SimplifyTextPage : ContentPage
	{
		private const string Endpoint = "https://api-inference.huggingface.co/models/facebook/bart-large-cnn";
		private const string ApiKey = "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"; // Replace with your Hugging Face API Key

		public SimplifyTextPage()
		{
			InitializeComponent();
		}

		private async void OnSimplifyClicked(object sender, EventArgs e)
		{
			LoadingIndicator.IsVisible = true;
			LoadingIndicator.IsRunning = true;

			string inputText = InputEditor.Text;
			string simplifiedText = await SimplifyTextAsync(inputText);

			OutputLabel.Text = simplifiedText;

			LoadingIndicator.IsVisible = false;
			LoadingIndicator.IsRunning = false;
		}

		private async Task<string> SimplifyTextAsync(string inputText)
		{
			var client = new HttpClient();
			client.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");

			var requestBody = new
			{
				inputs = inputText
			};

			var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

			var response = await client.PostAsync(Endpoint, content);
			var responseString = await response.Content.ReadAsStringAsync();

			try
			{
				var jsonResponse = JsonDocument.Parse(responseString);
				if (jsonResponse.RootElement.ValueKind == JsonValueKind.Array)
				{
					var simplifiedText = jsonResponse.RootElement[0].GetProperty("summary_text").GetString();
					return simplifiedText;
				}
				return "Error: Unexpected response format.";
			}
			catch (JsonException)
			{
				return $"Error: Could not parse the response. Response was: {responseString}";
			}
		}
	}
}
