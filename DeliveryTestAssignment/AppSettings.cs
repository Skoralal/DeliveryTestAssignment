using System.Text.Json;
using System.Text.Json.Serialization;

namespace DeliveryTestAssignment
{
    class AppSettings
    {
        public void ResetSettings()
        {
            Patch newSettings = new Patch();
            ChangeDBPath(newSettings.DBPath);
            ChangeLogPath(newSettings.LogPath);
            ChangeOutputPath(newSettings.OutputPath);
        }
        private class Patch
        {
            public string DBPath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase.json");
            public string LogPath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log.txt");
            public string OutputPath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Out.txt");
            public Patch()
            {

            }
            public Patch(AppSettings appSettings)
            {
                DBPath = appSettings.DBPath;
                LogPath = appSettings.LogPath;
                OutputPath = appSettings.OutputPath;
            }
        }
        public string ThisPath { get; set; } = Directory.GetCurrentDirectory();
        public string SettingsFilePath { get; set; } = "AppSettings.json";
        public string DBPath { get; set; } = "DataBase.json";
        public string LogPath { get; set; } = "Log.txt";
        public string OutputPath { get; set; } = "Out.txt";
        public AppSettings(bool reset = false)
        {
            if (reset)
            {
                Patch newSettings = new Patch();
                ChangeDBPath(newSettings.DBPath);
                ChangeLogPath(newSettings.LogPath);
                ChangeOutputPath(newSettings.OutputPath);
                return;
            }
            if (!File.Exists(SettingsFilePath))
            {
                using (StreamWriter writer = new StreamWriter(SettingsFilePath))
                {
                    writer.WriteLine(JsonSerializer.Serialize(new Patch(this)));
                }
            }
            else
            {
                using (StreamReader reader = new(SettingsFilePath))
                {
                    string json = reader.ReadToEnd();
                    Patch newSettings = JsonSerializer.Deserialize<Patch>(json);
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
            using (StreamWriter writer = new StreamWriter(SettingsFilePath))
            {
                writer.WriteLine(JsonSerializer.Serialize(new Patch(this)));
            }
        }
        public void ChangeLogPath(string newPath)
        {
            LogPath = newPath;
            using (StreamWriter writer = new StreamWriter(SettingsFilePath))
            {
                writer.WriteLine(JsonSerializer.Serialize(new Patch(this)));
            }
        }
        public void ChangeOutputPath(string newPath)
        {
            OutputPath = newPath;
            using (StreamWriter writer = new StreamWriter(SettingsFilePath))
            {
                writer.WriteLine(JsonSerializer.Serialize(new Patch(this)));
            }
        }
    }
}
