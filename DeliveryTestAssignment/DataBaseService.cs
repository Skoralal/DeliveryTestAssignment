using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace DeliveryTestAssignment
{
    public static class DataBaseService
    {
        static AppSettings _appSettings = new();
        private static string DBPath = _appSettings.DBPath;
        private static string LogPath = _appSettings.LogPath;
        private static string OutPath = _appSettings.OutputPath;
        private static StreamWriter? DBWriter;
        private static StreamWriter LogWriter = new(LogPath, true);
        private static StreamWriter OutWriter = new(OutPath, false);
        public static ObservableCollection<Order> Orders { get; set; }
        
        static DataBaseService()
        {
            GetData();
            Orders.CollectionChanged += Orders_CollectionChanged;
        }

        private static void Orders_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateData();
            LogWriter.WriteLine($"Время записи = {DateTime.Now.ToString()}");
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Order newItem in e.NewItems)
                {
                    LogWriter.WriteLine($"Добавлена доставка:" +
                        $"\nId = {newItem.Id}" +
                        $"\nВес = {newItem.Weight}" +
                        $"\nРайон доставки = {newItem.DistrictID}" +
                        $"\nДата доставки = {newItem.HumanDate}\n");
                    LogWriter.Flush();
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Order oldItem in e.OldItems)
                {
                    LogWriter.WriteLine($"Удалена доставка:" +
                        $"\nId = {oldItem.Id}" +
                        $"\nВес = {oldItem.Weight}" +
                        $"\nРайон доставки = {oldItem.DistrictID}" +
                        $"\nДата доставки = {oldItem.HumanDate}\n");
                    LogWriter.Flush();
                }
            }
            else if(e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (Order oldItem in e.OldItems)
                {
                    LogWriter.WriteLine($"Удалена доставка:" +
                        $"\nId = {oldItem.Id}" +
                        $"\nВес = {oldItem.Weight} ==> {((Order)e.NewItems[0]).Weight}" +
                        $"\nРайон доставки = {oldItem.DistrictID} ==> {((Order)e.NewItems[0]).DistrictID}" +
                        $"\nДата доставки = {oldItem.HumanDate} ==> {((Order)e.NewItems[0]).HumanDate}\n");
                    LogWriter.Flush();
                }
            }
        }

        private static bool GetData()
        {
            string json = "";
            using (var DBreader = new StreamReader(DBPath))
            {
                json = DBreader.ReadToEnd();

                if (json.Length == 0)
                {
                    DBreader.Dispose();
                    DBWriter = new(DBPath, false);
                    Orders = new ObservableCollection<Order>();
                    return true;
                }
            }
            DBWriter = new(DBPath, false);
            DBWriter.Write(json);
            DBWriter.Flush();
            Orders = JsonSerializer.Deserialize<ObservableCollection<Order>>(json);
            
            return true;
        }
        private static bool UpdateData()
        {
            string json = JsonSerializer.Serialize(Orders);
            DBWriter.BaseStream.SetLength(0);
            DBWriter.Write(json);
            DBWriter.Flush();
            return true;
        }
        public static bool WriteOutput(List<Order> toWrite)
        {
            OutWriter.BaseStream.SetLength(0);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Order order in toWrite) 
            { 
                stringBuilder.Append(order.ToString());
            }
            OutWriter.Write(stringBuilder);
            OutWriter.Flush();
            return true;
        }
    }
}
