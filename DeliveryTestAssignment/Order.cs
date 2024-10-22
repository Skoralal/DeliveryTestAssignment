namespace DeliveryTestAssignment
{
    public class Order
    {
        public Guid Id { get;}
        public double Weight { get; set; }
        public int DistrictID { get; set; }
        public DateTime CompletionTime { get; set; }
        public string HumanDate { get 
            {
                return CompletionTime.ToString("yyyy-MM-dd HH:mm:ss");
            } }
        public Order(double weight, int districtID, DateTime completionTime) 
        {
            Id = Guid.NewGuid();
            Weight = weight;
            DistrictID = districtID;
            CompletionTime = completionTime;
        }
        public Order GenerateRandom()
        {
            var rand = new Random();
            //var weight = Math.Round(0.1 + (rand.NextDouble() * (500 - 0.1)), 2);
            var weight = rand.Next(1, 50001)/100;
            var districtID = rand.Next(1, 201);
            var startDate = new DateTime(2020, 1, 1);
            var endDate = DateTime.Now;
            var timeSpan = endDate - startDate;
            double randomTicks = rand.NextDouble() * timeSpan.Ticks;
            var completionTime = startDate.AddTicks((long)randomTicks);
            return new Order(weight, districtID, completionTime);
        }
    }
}
