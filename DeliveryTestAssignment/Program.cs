namespace DeliveryTestAssignment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(DataBaseService.Orders.Count);
            Console.WriteLine("Hello, World!");
            while (true)
            {
                var userinput = Console.ReadLine();
                while (userinput == null)
                {
                    Console.WriteLine("Invalid command");
                    userinput = Console.ReadLine();
                }
                if (userinput == "1")
                {
                    double weight;
                    int districtID;
                    DateTime completionTime;
                    Console.WriteLine("Введите вес заказа в килограммах");
                    bool weightParseSuccess = double.TryParse(Console.ReadLine(), out weight);
                    while (!weightParseSuccess)
                    {
                        Console.WriteLine("Некорректный ввод, введите корректное значение");
                        weightParseSuccess = double.TryParse(Console.ReadLine(), out weight);
                    }

                    Console.WriteLine("Введите ID района доставки");
                    bool districtIDParseSuccess = int.TryParse(Console.ReadLine(), out districtID);
                    while (!districtIDParseSuccess)
                    {
                        Console.WriteLine("Некорректный ввод, введите корректное значение");
                        districtIDParseSuccess = int.TryParse(Console.ReadLine(), out districtID);
                    }

                    Console.WriteLine("Введите время доставки формата yyyy-MM-dd HH:mm:ss");
                    bool completionTimeParseSuccess = DateTime.TryParseExact(
                            Console.ReadLine(),
                            "yyyy-MM-dd HH:mm:ss",
                            System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.None,
                            out completionTime
                        );
                    while (!completionTimeParseSuccess)
                    {
                        Console.WriteLine("Некорректный ввод, введите корректное значение");
                        completionTimeParseSuccess = DateTime.TryParseExact(
                            Console.ReadLine(),
                            "yyyy-MM-dd HH:mm:ss",
                            System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.None,
                            out completionTime
                        );
                    }
                    Console.WriteLine($"Вес заказа = {weight}");
                    Console.WriteLine($"ID района доставки = {districtID}");
                    Console.WriteLine($"Дата доставки = {completionTime:yyyy-MM-dd HH:mm:ss}");
                    Console.WriteLine("Всё правильно y/n?");
                    var check = Console.ReadLine();
                    while (check != "y" && check != "n")
                    {
                        Console.WriteLine("Invalid command");
                        userinput = Console.ReadLine();
                    }
                    if (check == "y")
                    {
                        DataBaseService.Orders.Add(new(weight, districtID, completionTime));
                    }
                }
                if(userinput == "_deliveryLog")
                {
                    AppSettings appSettings = new AppSettings();
                    Console.WriteLine("Введите полный путь к папке(если файла нет, он буден создан автоматически)");
                    appSettings.ChangeLogPath(Console.ReadLine());
                    Console.WriteLine("Изменения будут применены после перезапуска приложения");
                }
                if (userinput == "0")
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}
