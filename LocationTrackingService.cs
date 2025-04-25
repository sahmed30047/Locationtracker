using LocationTracker.Data;
using LocationTracker.Models;

namespace LocationTracker.Services
{
    public class LocationTrackingService
    {
        public async Task StartTrackingAsync()
        {
            // Request for location permissions
            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            var location = await Geolocation.GetLocationAsync(request);

            if (location != null)
            {
                // Create a new location model
                var locationModel = new LocationModel
                {
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    Timestamp = DateTime.UtcNow
                };

                // Save location to SQLite database
                var dbHelper = new SQLiteDatabaseHelper();
                dbHelper.SaveLocation(locationModel);
            }
        }
    }
}
