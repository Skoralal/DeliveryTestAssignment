using System.Diagnostics;

namespace DeliveryTestAssignment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(DataBaseService.Orders.Count);
            AppSettings appSettings;
            try
            {
                appSettings = new AppSettings();

            } catch (System.UnauthorizedAccessException ex)
            {
                Console.WriteLine("Нет доступа к директории, хотите сбрость настройки y/n?");
                var input = Console.ReadLine();
                while (input != "y" && input != "n")
                {
                    Console.WriteLine("Invalid command");
                    input = Console.ReadLine();
                }
                if(input == "n")
                {
                    Environment.Exit(0);
                }
                if (input == "y")
                {
                    appSettings = new AppSettings(true);
                }
            }
            finally {
            
                appSettings = new AppSettings();
            }
            while (true)
            {
                Console.WriteLine("Новая сессия\n" +
                "1 - Создать новую запись\n" +
                "2 - Сгенерировать случайную запись в нужном количестве\n" +
                "3 - Выгрузить данные с указанными параметрами\n" +
                "_deliveryLog - Изменить расположение файла с логами\n" +
                "_deliveryLogOpen - Открыть файл с логами\n" +
                "_deliveryOrder - Изменить расположение файла с результатами выборки\n" +
                "_deliveryOrderOpen - Открыть файл с выборкой\n" +
                "_resetSettings - Сбросить настройки\n" +
                "_currentSettings - Текущие настройки\n" +
                "0 - Выход");
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
                    Console.WriteLine("Введите вес заказа в килограммах, если значение дробное, используйте запятую");
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
                    Console.WriteLine("Хотите внести данные y/n?");
                    userinput = Console.ReadLine();
                    while (userinput != "y" && userinput != "n")
                    {
                        Console.WriteLine("Invalid command");
                        userinput = Console.ReadLine();
                    }
                    if (userinput == "y")
                    {
                        DataBaseService.Orders.Add(new(weight, districtID, completionTime));
                        Console.WriteLine("Данные добавлены");
                    }
                    else
                    {
                        continue;
                    }
                }
                if (userinput == "_resetSettings")
                {
                    appSettings = new AppSettings(true);
                    Console.WriteLine("Настройки сброшены, изменения будут применены после перезапуска приложения");
                }
                if (userinput == "_deliveryLog")
                {
                    Console.WriteLine("Введите полный путь к папке(если файла нет, он буден создан автоматически)");
                    appSettings.ChangeLogPath(Console.ReadLine());
                    Console.WriteLine("Изменения будут применены после перезапуска приложения");
                }
                if (userinput == "_deliveryLogOpen")
                {
                    Process.Start("notepad.exe", appSettings.LogPath);
                }
                if (userinput == "_deliveryOrderOpen")
                {
                    Process.Start("notepad.exe", appSettings.OutputPath);
                }
                if (userinput == "_deliveryOrder")
                {
                    Console.WriteLine("Введите полный путь к папке(если файла нет, он буден создан автоматически)");
                    appSettings.ChangeOutputPath(Console.ReadLine());
                    Console.WriteLine("Изменения будут применены после перезапуска приложения");
                }
                if (userinput == "_currentSettings")
                {
                    Console.WriteLine($"DataBasePath = {appSettings.DBPath}\nLogPath = {appSettings.LogPath}\nOutputPath = {appSettings.OutputPath}");
                }
                if (userinput == "2")
                {
                    Console.WriteLine("Сколько случайных записей вы хотите создать?");
                    userinput = Console.ReadLine();
                    int number;
                    while (!int.TryParse(userinput, out number))
                    {
                        Console.WriteLine("Invalid command");
                        userinput = Console.ReadLine();
                    }
                    Console.WriteLine("Writing...");
                    for (int i = 0; i < number; i++)
                    {
                        DataBaseService.Orders.Add(new Order().GenerateRandom());
                    }
                    Console.WriteLine("Done");
                }
                if (userinput == "3")
                {
                    Console.WriteLine("Введите ID района");
                    userinput = Console.ReadLine();
                    int districtID;
                    while (!int.TryParse(userinput, out districtID))
                    {
                        Console.WriteLine("Invalid command");
                        userinput = Console.ReadLine();
                    }

                    Console.WriteLine("Введите дату первой доставки в формате yyyy-MM-dd HH:mm:ss");
                    userinput = Console.ReadLine();
                    DateTime firstDelivery;
                    while (!DateTime.TryParseExact(userinput, "yyyy-MM-dd HH:mm:ss",
                            System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.None, out firstDelivery))
                    {
                        Console.WriteLine("Invalid command");
                        userinput = Console.ReadLine();
                    }

                    Console.WriteLine("Введите временной промежуток со времени доставки первого заказа в минутах");
                    userinput = Console.ReadLine();
                    int minutes;
                    while (!int.TryParse(userinput, out minutes))
                    {
                        Console.WriteLine("Invalid command");
                        userinput = Console.ReadLine();
                    }
                    TimeSpan timeSpan = TimeSpan.FromMinutes(minutes);
                    List<Order> toWrite = DataBaseService.Orders.Where(order => order.DistrictID == districtID &&
                        order.CompletionTime <= firstDelivery + timeSpan && order.CompletionTime >= firstDelivery).OrderBy(order=>order.CompletionTime).ToList();
                    DataBaseService.WriteOutput(toWrite);
                    Console.WriteLine($"{toWrite.Count} записей выгружено в результат");
                }
                if (userinput == "0")
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}
