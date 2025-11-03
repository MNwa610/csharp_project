using System;
using System.Collections.Generic;
using System.Linq; 

class Program
{   
    class Table
    {   
        public int TableID { get; private set; }
        public string Location { get; private set; }
        public int NO_seats { get; private set; }
        public List<string> Reservations { get; private set; } 

        private static Dictionary<int, string> tableLocations = new Dictionary<int, string>
        {
            { 1, "у окна" },
            { 2, "у прохода" },
            { 3, "у выхода" },
            { 4, "в глубине" }
        };

        private static int nextTableID = 0;

        public Table()
        {
            Random random = new Random();
            TableID = nextTableID++;
            Location = tableLocations[random.Next(1, 5)];
            NO_seats = random.Next(2, 5);
            Reservations = GenerateRandomReservations(random);
        }


        public void UpdateTableData(string location = "", int no_seats = 0, List<string>? newReservations = null)
        {   
            if (!string.IsNullOrWhiteSpace(location)) 
            {
                try
                {
                    if (!tableLocations.ContainsValue(location))
                        throw new ArgumentException("Недопустимое местоположение стола.");
                    Location = tableLocations.FirstOrDefault(x => x.Value == location).Value;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
            
            if (no_seats > 0)
            {
                NO_seats = no_seats;
            }
            
            if (newReservations != null && newReservations.Any(r => !string.IsNullOrWhiteSpace(r)))
            {
                Reservations = newReservations.Where(r => !string.IsNullOrWhiteSpace(r)).ToList();
            }
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"ID стола: {TableID}, Местоположение: {Location}, Кол. мест: {NO_seats}");
            Console.WriteLine("Резервы:");
            foreach (var reservation in Reservations)
            {
                Console.WriteLine($"- {reservation}");
            }
        }
    }

    static void Main()
    {
        List<Table> tables = new List<Table>();
        Console.WriteLine("Введите количество столов в ресторане:");
        string number_tables = Console.ReadLine();
        int n;
        try
        {
            if (!int.TryParse(number_tables, out n) || n <= 0)
            {
                throw new FormatException("Ошибка: Введите положительное число.");
            }
        }
        catch (FormatException ex)
        {
            Console.WriteLine(ex.Message);
            return;
        }

        for (int i = 0; i < n; i++)
        {
            tables.Add(new Table());
        }

        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1 - Вывод всех столиков");
            Console.WriteLine("2 - Изменение данных стола");
            Console.WriteLine("0 - Выход");
            Console.Write("Введите номер действия: ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Ошибка: Введите корректное число.");
                continue;
            }

            if (choice == 0)
            {
                Console.WriteLine("Выход из программы.");
                break;
            }

            switch (choice)
            {
                case 1:
                    Console.WriteLine("\nИнформация о всех столах:");
                    foreach (var table in tables)
                    {
                        table.DisplayInfo();
                        Console.WriteLine();
                    }
                    break;

                case 2:
                    Console.Write("Введите ID стола для изменения: ");
                    string idInput = Console.ReadLine();
                    if (int.TryParse(idInput, out int tableID))
                    {
                        Table tableToUpdate = tables.FirstOrDefault(t => t.TableID == tableID);
                        if (tableToUpdate != null)
                        {
                            Console.Write("Введите новое местоположение: ");
                            string newLocation = Console.ReadLine();

                            Console.Write("Введите новое количество мест: ");
                            string seatsInput = Console.ReadLine();
                            int newSeats = int.TryParse(seatsInput, out int seats) ? seats : 0;

                            Console.WriteLine("Введите новые резервы (через запятую, например: 10:00-12:00,14:00-16:00): ");
                            string reservationsInput = Console.ReadLine();
                            List<string>? newReservations = null;
                            if (!string.IsNullOrWhiteSpace(reservationsInput))
                            {
                                newReservations = reservationsInput.Split(',') .Select(r => r.Trim()).Where(r => !string.IsNullOrWhiteSpace(r)).ToList();
                            }

                            tableToUpdate.UpdateTableData(newLocation, newSeats, newReservations);
                        }
                        else
                        {
                            Console.WriteLine($"Стол с ID {tableID} не найден.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: Введите корректный ID.");
                    }
                    break;

                default:
                    Console.WriteLine("Ошибка: Выберите корректный пункт меню.");
                    break;
            }
        }
    }
}