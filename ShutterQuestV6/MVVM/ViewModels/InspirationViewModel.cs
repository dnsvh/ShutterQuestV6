using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;

namespace ShutterQuestV6.MVVM.ViewModels
{
    public class InspirationViewModel : BaseViewModel
    {
        private const string UnsplashApiKey = "1-zXNtrDFCYbeXq6Q3MReVxNYi-5DaREo-pwlBbG8dM";
        private const string UnsplashEndpoint = "https://api.unsplash.com/photos/random";

        public ObservableCollection<Photo> Photos { get; set; } = new ObservableCollection<Photo>();
        public ICommand LoadCategoryCommand { get; }

        public InspirationViewModel(string category)
        {
            LoadCategoryCommand = new Command<string>(async cat => await LoadPhotosAsync(cat));
            LoadPhotosAsync(category);
        }

        private async Task LoadPhotosAsync(string category)
        {
            try
            {
                IsBusy = true;

                // make API Request
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Client-ID {UnsplashApiKey}");
                var requestUri = $"{UnsplashEndpoint}?query={category}&count=4";
                System.Diagnostics.Debug.WriteLine($"Requesting: {requestUri}");

                // Fetch data
                var response = await client.GetStringAsync(requestUri);
                System.Diagnostics.Debug.WriteLine($"Raw Response: {response}");

              
                var result = JsonSerializer.Deserialize<UnsplashPhoto[]>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

             
                Photos.Clear();
                if (result != null && result.Length > 0)
                {
                    foreach (var photo in result)
                    {
                        if (!string.IsNullOrEmpty(photo?.Urls?.Regular))
                        {
                            Photos.Add(new Photo { Url = photo.Urls.Regular });
                            System.Diagnostics.Debug.WriteLine($"Photo added: {photo.Urls.Regular}");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Skipped a photo due to missing or invalid Urls.");
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No photos found in the response.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error fetching photos: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

 
        public class UnsplashPhoto
        {
            public UnsplashUrls Urls { get; set; }
        }

        public class UnsplashUrls
        {
            public string Regular { get; set; }
        }

        public class Photo
        {
            public string Url { get; set; }
        }
    }
}
