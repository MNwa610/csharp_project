using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        List<Table> tables = new List<Table>();
        List<Reservation> reservations = new List<Reservation>();

        Console.WriteLine("Выберите способ создания столов:");
        Console.WriteLine("1 - Случайное создание столов");
        Console.WriteLine("2 - Ручной ввод столов");
        Console.Write("Введите номер действия: ");
        string creationChoice = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(creationChoice))
        {
            Console.WriteLine("Ошибка: Ввод не может быть пустым.");
            return;
        }

        int tableCount;
        if (creationChoice == "1")
        {
            Console.Write("Введите количество столов для случайного создания: ");
            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out tableCount) || tableCount <= 0)
            {
                Console.WriteLine("Ошибка: Введите корректное количество столов.");
                return;
            }

            Random random = new Random();
            for (int i = 0; i < tableCount; i++)
            {
                string location = new[] { "у окна", "у прохода", "у выхода", "в глубине" }[random.Next(4)];
                int seats = random.Next(2, 6);
                tables.Add(new Table(location, seats));
            }

            Console.WriteLine("Случайные столы успешно созданы.");
        }
        else if (creationChoice == "2")
        {
            Console.Write("Введите количество столов для ручного ввода: ");
            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out tableCount) || tableCount <= 0)
            {
                Console.WriteLine("Ошибка: Введите корректное количество столов.");
                return;
            }

            for (int i = 0; i < tableCount; i++)
            {
                Console.Write($"Введите местоположение стола {i + 1}: ");
                string location = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(location))
                {
                    Console.WriteLine("Ошибка: Местоположение не может быть пустым.");
                    return;
                }

                Console.Write($"Введите количество мест за столом {i + 1}: ");
                string seatsInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(seatsInput) || !int.TryParse(seatsInput, out int seats) || seats <= 0)
                {
                    Console.WriteLine("Ошибка: Введите корректное количество мест.");
                    return;
                }

                tables.Add(new Table(location, seats));
            }
        }
        else
        {
            Console.WriteLine("Ошибка: Некорректный выбор.");
            return;
        }

        while (true)
        {   
            Console.WriteLine(new string('-', 80)); 
            Console.WriteLine("Меню:");
            Console.WriteLine("1 - Вывод всех столов");
            Console.WriteLine("2 - Создание брони");
            Console.WriteLine("3 - Изменение брони");
            Console.WriteLine("4 - Отмена брони");
            Console.WriteLine("5 - Вывод всех бронирований");
            Console.WriteLine("6 - Поиск бронирования");
            Console.WriteLine("0 - Выход");
            Console.Write("Введите номер действия: ");
            string choiceInput = Console.ReadLine();
            Console.WriteLine(new string('-', 80)); 
            if (string.IsNullOrWhiteSpace(choiceInput) || !int.TryParse(choiceInput, out int choice))
            {
                Console.WriteLine("Ошибка: Введите корректный номер действия.");
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
                    foreach (var table in tables)
                    {
                        table.DisplayInfo();
                        Console.WriteLine();
                    }
                    break;

                case 2:
                    Console.WriteLine(new string('-', 80)); 
                    Console.Write("Введите имя клиента: ");
                    string clientName = Console.ReadLine();
                    Console.Write("Введите номер телефона клиента: ");
                    string clientPhone = Console.ReadLine();
                    Console.Write("Введите время начала брони (hh:mm): ");
                    string startTime = Console.ReadLine();
                    Console.Write("Введите время окончания брони (hh:mm): ");
                    string endTime = Console.ReadLine();
                    Console.Write("Введите комментарий: ");
                    
                    string comment = Console.ReadLine();

                    Console.WriteLine("Доступные столы:");
                    var availableTables = tables.Where(t => t.IsAvailable(startTime, endTime)).ToList();
                    foreach (var table in availableTables)
                    {
                        table.DisplayInfo();
                    }

                    Console.Write("Введите ID стола для бронирования: ");
                    int tableID = int.Parse(Console.ReadLine());
                    Table assignedTable = tables.FirstOrDefault(t => t.TableID == tableID);

                    if (assignedTable != null)
                    {
                        reservations.Add(new Reservation(clientName, clientPhone, startTime, endTime, comment, assignedTable));
                        Console.WriteLine("Бронирование успешно создано.");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: Стол с указанным ID не найден.");
                    }
                    Console.WriteLine(new string('-', 80)); 
                    break;

                case 3:
                    Console.WriteLine(new string('-', 80)); 
                    Console.Write("Введите ID брони для изменения: ");
                    int reservationID = int.Parse(Console.ReadLine());
                    var reservationToUpdate = reservations.FirstOrDefault(r => r.ReservationID == reservationID);

                    if (reservationToUpdate != null)
                    {
                        Console.Write("Введите новое время начала брони (hh:mm): ");
                        string newStartTime = Console.ReadLine();
                        Console.Write("Введите новое время окончания брони (hh:mm): ");
                        string newEndTime = Console.ReadLine();
                        Console.Write("Введите новый комментарий: ");
                        string newComment = Console.ReadLine();

                        Console.WriteLine("Доступные столы:");
                        availableTables = tables.Where(t => t.IsAvailable(newStartTime, newEndTime)).ToList();
                        foreach (var table in availableTables)
                        {
                            table.DisplayInfo();
                        }

                        Console.Write("Введите ID нового стола: ");
                        int newTableID = int.Parse(Console.ReadLine());
                        Table newTable = tables.FirstOrDefault(t => t.TableID == newTableID);

                        if (newTable != null)
                        {
                            reservationToUpdate.UpdateReservation(newStartTime, newEndTime, newComment, newTable);
                            Console.WriteLine("Бронирование успешно обновлено.");
                        }
                        else
                        {
                            Console.WriteLine("Ошибка: Стол с указанным ID не найден.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: Бронирование с указанным ID не найдено.");
                    }
                    Console.WriteLine(new string('-', 80)); 
                    break;

                case 4:
                    Console.WriteLine(new string('-', 80)); 
                    Console.Write("Введите ID брони для отмены: ");
                    int cancelID = int.Parse(Console.ReadLine());
                    var reservationToCancel = reservations.FirstOrDefault(r => r.ReservationID == cancelID);

                    if (reservationToCancel != null)
                    {
                        reservationToCancel.CancelReservation();
                        reservations.Remove(reservationToCancel);
                        Console.WriteLine("Бронирование успешно отменено.");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: Бронирование с указанным ID не найдено.");
                    }
                    Console.WriteLine(new string('-', 80)); 
                    break;

                case 5:
                    Console.WriteLine(new string('-', 80)); 
                    foreach (var reservation in reservations)
                    {
                        reservation.DisplayInfo();
                        Console.WriteLine();
                    }
                    Console.WriteLine(new string('-', 80)); 
                    break;

                case 6:
                    Console.WriteLine(new string('-', 80)); 
                    Console.Write("Введите имя клиента: ");
                    string searchName = Console.ReadLine();
                    Console.Write("Введите последние 4 цифры номера телефона: ");
                    string searchPhone = Console.ReadLine();
                    Console.WriteLine(new string('-', 80)); 

                    var foundReservations = reservations.Where(r => r.ClientName == searchName && r.ClientPhone.EndsWith(searchPhone)).ToList();
                    foreach (var reservation in foundReservations)
                    {
                        reservation.DisplayInfo();
                        Console.WriteLine();
                    }
                    Console.WriteLine(new string('-', 80)); 
                    break;

                default:
                    Console.WriteLine("Ошибка: Некорректный выбор.");
                    break;
            }
        }
    }
}

public class Table
{
    public int TableID { get; private set; }
    public string Location { get; private set; }
    public int Seats { get; private set; }
    public Dictionary<string, string> Schedule { get; private set; }

    private static int nextTableID = 1;

    public Table(string location, int seats)
    {
        TableID = nextTableID++;
        Location = location;
        Seats = seats;
        Schedule = new Dictionary<string, string>();
    }

    public void UpdateTable(string newLocation, int newSeats)
    {
        Location = newLocation;
        Seats = newSeats;
    }

    public void DisplayInfo()
    {
        Console.WriteLine(new string('-', 80)); 
        Console.WriteLine($"ID стола: {TableID}");
        Console.WriteLine($"Расположение: {Location}");
        Console.WriteLine($"Количество мест: {Seats}");
        Console.WriteLine("Расписание:");

        if (Schedule.Count == 0)
        {
            Console.WriteLine("Нет бронирований.");
        }
        else
        {
            var orderedSchedule = Schedule.OrderBy(e => TimeSpan.Parse(e.Key)).ToList();
            string currentReservation = null;
            string rangeStart = null;

            foreach (var entry in orderedSchedule)
            {
                if (currentReservation == null)
                {
                    // Начало нового диапазона
                    currentReservation = entry.Value;
                    rangeStart = entry.Key;
                }
                else if (currentReservation != entry.Value)
                {
                    // Завершение текущего диапазона
                    Console.WriteLine($"{rangeStart}-{entry.Key} - {currentReservation}");
                    currentReservation = entry.Value;
                    rangeStart = entry.Key;
                }
            }

            // Вывод последнего диапазона
            if (currentReservation != null)
            {
                Console.WriteLine($"{rangeStart}-{orderedSchedule.Last().Key} - {currentReservation}");
            }
        }
        Console.WriteLine(new string('-', 80)); 
    }

    public bool IsAvailable(string startTime, string endTime)
    {
        foreach (var time in Schedule.Keys)
        {
            if (string.Compare(time, startTime) >= 0 && string.Compare(time, endTime) < 0)
            {
                return false;
            }
        }
        return true;
    }

    public void AddReservation(string startTime, string endTime, string reservationInfo)
    {
        for (var time = TimeSpan.Parse(startTime); time < TimeSpan.Parse(endTime); time += TimeSpan.FromHours(1))
        {
            Schedule[time.ToString(@"hh\:mm")] = reservationInfo;
        }
    }

    public void RemoveReservation(string startTime, string endTime)
    {
        for (var time = TimeSpan.Parse(startTime); time < TimeSpan.Parse(endTime); time += TimeSpan.FromHours(1))
        {
            Schedule.Remove(time.ToString(@"hh\:mm"));
        }
    }
}

public class Reservation
{
    public int ReservationID { get; private set; }
    public string ClientName { get; private set; }
    public string ClientPhone { get; private set; }
    public string StartTime { get; private set; }
    public string EndTime { get; private set; }
    public string Comment { get; private set; }
    public Table AssignedTable { get; private set; }

    private static int nextReservationID = 1;

    public Reservation(string clientName, string clientPhone, string startTime, string endTime, string comment, Table assignedTable)
    {
        ReservationID = nextReservationID++;
        ClientName = clientName;
        ClientPhone = clientPhone;
        StartTime = startTime;
        EndTime = endTime;
        Comment = comment;
        AssignedTable = assignedTable;

        AssignedTable.AddReservation(StartTime, EndTime, $"ID {ReservationID}, {ClientName}, {ClientPhone}");
    }

    public void UpdateReservation(string newStartTime, string newEndTime, string newComment, Table newTable)
    {
        AssignedTable.RemoveReservation(StartTime, EndTime);

        StartTime = newStartTime;
        EndTime = newEndTime;
        Comment = newComment;
        AssignedTable = newTable;

        AssignedTable.AddReservation(StartTime, EndTime, $"ID {ReservationID}, {ClientName}, {ClientPhone}");
    }

    public void CancelReservation()
    {
        AssignedTable.RemoveReservation(StartTime, EndTime);
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"ID брони: {ReservationID}");
        Console.WriteLine($"Имя клиента: {ClientName}");
        Console.WriteLine($"Телефон клиента: {ClientPhone}");
        Console.WriteLine($"Время: {StartTime} - {EndTime}");
        Console.WriteLine($"Комментарий: {Comment}");
        Console.WriteLine($"Назначенный стол: {AssignedTable.TableID}");
    }
}