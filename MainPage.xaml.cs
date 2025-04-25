using Microsoft.Maui.Controls.Maps;
using LocationTracker.Models; // Ensure this points to your LocationModel
using LocationTracker.Data;
using LocationTracker.Services;
using Microsoft.Maui.Maps;   // Ensure this points to your SQLiteDatabaseHelper

namespace LocationTracker
{
    public partial class MainPage : ContentPage
    {
        private readonly LocationTrackingService _locationTrackingService;
        private readonly SQLiteDatabaseHelper _dbHelper;
        private bool _isTracking = false;

        public MainPage()
        {
            InitializeComponent();
            _locationTrackingService = new LocationTrackingService();
            _dbHelper = new SQLiteDatabaseHelper();
        }

        // When the Start Tracking button is clicked
        private async void OnStartTrackingClicked(object sender, EventArgs e)
        {
            // Start tracking the location
            await StartTrackingAsync();

            // Retrieve locations from SQLite database
            var locations = await _dbHelper.GetLocationsAsync(); // Use async method

            // Clear existing circles if necessary
            map.MapElements.Clear();

            // Plot locations as blue circles on the map
            foreach (var location in locations)
            {
                AddCircleToMap(new Location(location.Latitude, location.Longitude));
            }
        }

        // Method to start tracking user location
        private async Task StartTrackingAsync()
        {
            if (_isTracking) return;

            _isTracking = true;

            while (_isTracking)
            {
                var location = await GetCurrentLocationAsync();
                if (location != null)
                {
                    var locationData = new LocationModel
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        Timestamp = DateTime.UtcNow
                    };
                    await _dbHelper.SaveLocationAsync(locationData); // Use async method
                    AddCircleToMap(location);
                }

                await Task.Delay(5000); // Delay for 5 seconds before tracking again
            }
        }

        // Method to get the current location asynchronously
        private async Task<Location> GetCurrentLocationAsync()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                return await Geolocation.GetLocationAsync(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to get location: {ex.Message}");
                return null;
            }
        }

        // Method to add a circle to the map at the specified location
        private void AddCircleToMap(Location location)
        {
            double radiusInMeters = 100; // Specify radius in meters

            var circle = new Circle
            {
                Center = location,
                Radius = Distance.FromMeters(radiusInMeters), // Convert double to Distance
                StrokeColor = Colors.Blue, // Circle border color
                StrokeWidth = 2,           // Circle border width
                FillColor = Color.FromArgb("#800000FF") // Semi-transparent blue fill (ARGB format)
            };

            // Add the circle to the map's MapElements collection
            map.MapElements.Add(circle);
        }
    }
}
