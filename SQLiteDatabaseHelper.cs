using SQLite;
using LocationTracker.Models;

namespace LocationTracker.Data
{
    public class SQLiteDatabaseHelper
    {
        private readonly string _dbPath;

        public SQLiteDatabaseHelper()
        {
            // Set the database file path (it will be stored in the app's local folder)
            _dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "locations.db");
        }

        // Create the table if it doesn't exist
        private SQLiteConnection GetConnection()
        {
            var connection = new SQLiteConnection(_dbPath);
            connection.CreateTable<LocationModel>(); // Ensure this matches your model class
            return connection;
        }

        // Save location data
        public void SaveLocation(LocationModel location)
        {
            using (var connection = GetConnection())
            {
                connection.Insert(location);
            }
        }

        // Retrieve all saved locations
        public List<LocationModel> GetLocations()
        {
            using (var connection = GetConnection())
            {
                return connection.Table<LocationModel>().ToList();
            }
        }

        // Optional: Asynchronous version of SaveLocation
        public async Task SaveLocationAsync(LocationModel location)
        {
            await Task.Run(() =>
            {
                using (var connection = GetConnection())
                {
                    connection.Insert(location);
                }
            });
        }

        // Optional: Asynchronous version of GetLocations
        public async Task<List<LocationModel>> GetLocationsAsync()
        {
            return await Task.Run(() =>
            {
                using (var connection = GetConnection())
                {
                    return connection.Table<LocationModel>().ToList();
                }
            });
        }
    }
}
