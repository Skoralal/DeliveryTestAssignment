using System.Text.Json;
using System.Text.Json.Serialization;

namespace DeliveryTestAssignment
{
    class AppSettings
    {
        public string ThisPath { get; set; } = Directory.GetCurrentDirectory();
        public string SettingsFilePath { get; set; } = "AppSettings.json";
        public string DBPath { get; set; } = "DataBase.json";
        public string LogPath { get; set; } = "Log.txt";
        public string OutputPath { get; set; } = "Out.txt";
        public AppSettings(bool deserialization = true)
        {
            if (deserialization)
            {
                return;
            }
            if (!File.Exists(SettingsFilePath))
            {
                using (StreamWriter writer = new StreamWriter(SettingsFilePath))
                {
                    writer.WriteLine(JsonSerializer.Serialize(this));
                }
            }
            else
            {
                using (StreamReader reader = new(SettingsFilePath))
                {
                    string json = reader.ReadToEnd();
                    AppSettings newSettings = JsonSerializer.Deserialize<AppSettings>(json);
                    ThisPath = newSettings.ThisPath;
                    LogPath = newSettings.LogPath;
                    OutputPath = newSettings.OutputPath;
                    DBPath = newSettings.DBPath;
                }
            }
            if (!File.Exists(DBPath))
            {
                File.Create(DBPath).Dispose();
            }
            if (!File.Exists(LogPath))
            {
                File.Create(LogPath).Dispose();
            }
            if (!File.Exists(OutputPath))
            {
                File.Create(OutputPath).Dispose();
            }
        }
        public void ChangeDBPath(string newPath)
        {
            DBPath = newPath;
        }
        public void ChangeLogPath(string newPath)
        {
            LogPath = newPath;
        }
        public void ChangeOutputPath(string newPath)
        {
            OutputPath = newPath;
        }
        }
}
