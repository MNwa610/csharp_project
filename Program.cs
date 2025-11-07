using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.Title = "Ресторан: Система бронирования и заказов";
        
        // Приветственное сообщение
        Console.Clear();
        Console.WriteLine("================================================================");
        Console.WriteLine("           ДОБРО ПОЖАЛОВАТЬ В СИСТЕМУ РЕСТОРАНА!");
        Console.WriteLine("        Система бронирования столов и управления заказами");
        Console.WriteLine("================================================================");
        Console.WriteLine();
        
        // Создаем коллекции для хранения данных
        List<Table> tables = new List<Table>();
        List<Reservation> reservations = new List<Reservation>();
        List<Dish> dishes = new List<Dish>();
        List<Order> orders = new List<Order>();

        // Этап 1: Создание столов ресторана
        CreateTables(tables);

        // Этап 2: Запуск основного меню программы
        MainMenuLoop(tables, reservations, dishes, orders);
        
        // Прощальное сообщение при выходе
        Console.WriteLine("\n================================================================");
        Console.WriteLine("            Благодарим за использование системы!");
        Console.WriteLine("                      До свидания!");
        Console.WriteLine("================================================================");
        Console.ReadKey();
    }

    // Метод для создания столов ресторана
    static void CreateTables(List<Table> tables)
    {
        Console.WriteLine("НАСТРОЙКА СТОЛОВ РЕСТОРАНА");
        Console.WriteLine("Выберите способ создания столов:");
        Console.WriteLine("1 - Автоматическое создание");
        Console.WriteLine("2 - Ручное создание");
        Console.Write("Ваш выбор: ");

        string? choice = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(choice))
        {
            ShowError("Необходимо выбрать вариант");
            return;
        }

        int tableCount;
        
        if (choice == "1")
        {
            // Автоматическое создание случайных столов
            Console.Write("Сколько столов создать?: ");
            string? input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out tableCount) || tableCount <= 0)
            {
                ShowError("Нужно ввести положительное число");
                return;
            }

            Random rand = new Random();
            for (int i = 0; i < tableCount; i++)
            {
                // Случайные параметры для стола
                string[] locations = { "у окна", "у прохода", "у выхода", "в центре", "в углу" };
                string location = locations[rand.Next(locations.Length)];
                int seats = rand.Next(2, 7); // от 2 до 6 мест
                
                tables.Add(new Table(location, seats));
            }

            ShowSuccess("Создано столов: " + tableCount);
        }
        else if (choice == "2")
        {
            // Ручное создание каждого стола
            Console.Write("Сколько столов создать?: ");
            string? input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out tableCount) || tableCount <= 0)
            {
                ShowError("Нужно ввести положительное число");
                return;
            }

            for (int i = 0; i < tableCount; i++)
            {
                Console.WriteLine("\n--- Стол №" + (i + 1) + " ---");
                Console.Write("Расположение: ");
                string? location = Console.ReadLine();
                
                if (string.IsNullOrWhiteSpace(location))
                {
                    ShowError("Расположение не может быть пустым");
                    return;
                }

                Console.Write("Количество мест: ");
                string? seatsInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(seatsInput) || !int.TryParse(seatsInput, out int seats) || seats <= 0)
                {
                    ShowError("Нужно ввести положительное число");
                    return;
                }

                tables.Add(new Table(location, seats));
                Console.WriteLine("Стол добавлен!");
            }
            
            ShowSuccess("Все столы созданы успешно!");
        }
        else
        {
            ShowError("Неверный выбор");
            return;
        }

        Console.WriteLine("\nНажмите Enter чтобы продолжить...");
        Console.ReadKey();
    }

    // Главный цикл меню программы
    static void MainMenuLoop(List<Table> tables, List<Reservation> reservations, List<Dish> dishes, List<Order> orders)
    {
        while (true)
        {   
            Console.Clear();
            ShowMainMenu();
            
            string? input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out int choice))
            {
                ShowError("Нужно ввести число");
                WaitForKey();
                continue;
            }

            if (choice == 0)
            {
                Console.WriteLine("Выход из программы...");
                break;
            }

            // Обработка выбора пользователя
            HandleMenuChoice(choice, tables, reservations, dishes, orders);
            WaitForKey();
        }
    }

    // Отображение главного меню
    static void ShowMainMenu()
    {
        Console.WriteLine("================================================================");
        Console.WriteLine("               ГЛАВНОЕ МЕНЮ СИСТЕМЫ");
        Console.WriteLine("================================================================");
        Console.WriteLine("БРОНИРОВАНИЕ СТОЛОВ:");
        Console.WriteLine("  1 - Показать все столы");
        Console.WriteLine("  2 - Создать бронь");
        Console.WriteLine("  3 - Изменить бронь");
        Console.WriteLine("  4 - Отменить бронь");
        Console.WriteLine("  5 - Список броней");
        Console.WriteLine("  6 - Найти бронь");
        Console.WriteLine("------------------------------------------------");
        Console.WriteLine("УПРАВЛЕНИЕ ЗАКАЗАМИ:");
        Console.WriteLine("  7 - Управление блюдами");
        Console.WriteLine("  8 - Управление заказами");
        Console.WriteLine("  9 - Показать меню");
        Console.WriteLine(" 10 - Статистика");
        Console.WriteLine("------------------------------------------------");
        Console.WriteLine("  0 - Выход");
        Console.WriteLine("================================================================");
        Console.Write("Выберите действие: ");
    }

    // Обработчик выбора в меню
    static void HandleMenuChoice(int choice, List<Table> tables, List<Reservation> reservations, 
                                List<Dish> dishes, List<Order> orders)
    {
        switch (choice)
        {
            case 1:
                ShowAllTables(tables);
                break;
            case 2:
                CreateNewReservation(reservations, tables);
                break;
            case 3:
                ChangeReservation(reservations, tables);
                break;
            case 4:
                RemoveReservation(reservations);
                break;
            case 5:
                ShowAllReservations(reservations);
                break;
            case 6:
                FindReservation(reservations);
                break;
            case 7:
                DishManagement(dishes);
                break;
            case 8:
                OrderManagement(orders, dishes, tables);
                break;
            case 9:
                ShowRestaurantMenu(dishes);
                break;
            case 10:
                ShowStats(orders);
                break;
            default:
                ShowError("Неизвестная команда");
                break;
        }
    }

    // Показать сообщение об ошибке
    static void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("ОШИБКА: " + message);
        Console.ResetColor();
    }

    // Показать сообщение об успехе
    static void ShowSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("УСПЕХ: " + message);
        Console.ResetColor();
    }

    // Ожидание нажатия клавиши
    static void WaitForKey()
    {
        Console.WriteLine("\nНажмите любую клавишу...");
        Console.ReadKey();
    }

    // =========================================================================
    // МЕТОДЫ ДЛЯ РАБОТЫ СО СТОЛАМИ И БРОНИРОВАНИЕМ
    // =========================================================================

    // Показать все столы
    static void ShowAllTables(List<Table> tables)
    {
        Console.Clear();
        Console.WriteLine("СПИСОК ВСЕХ СТОЛОВ");
        Console.WriteLine("================================================================");
        
        if (tables.Count == 0)
        {
            Console.WriteLine("Столы не созданы");
            return;
        }

        // Выводим информацию о каждом столе
        foreach (var table in tables)
        {
            table.ShowTableInfo();
            Console.WriteLine("------------------------------------------------");
        }
        
        Console.WriteLine("Всего столов: " + tables.Count);
    }

    // Создать новое бронирование
    static void CreateNewReservation(List<Reservation> reservations, List<Table> tables)
    {
        Console.Clear();
        Console.WriteLine("НОВОЕ БРОНИРОВАНИЕ");
        Console.WriteLine("================================================================");
        
        // Собираем данные от пользователя
        Console.Write("Имя клиента: ");
        string? name = Console.ReadLine();
        Console.Write("Телефон: ");
        string? phone = Console.ReadLine();
        Console.Write("Время начала (ЧЧ:мм): ");
        string? start = Console.ReadLine();
        Console.Write("Время окончания (ЧЧ:мм): ");
        string? end = Console.ReadLine();
        Console.Write("Комментарий: ");
        string? comment = Console.ReadLine();

        // Проверяем что все обязательные поля заполнены
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phone) || 
            string.IsNullOrWhiteSpace(start) || string.IsNullOrWhiteSpace(end))
        {
            ShowError("Заполните все обязательные поля");
            return;
        }

        // Ищем доступные столы
        Console.WriteLine("\nДОСТУПНЫЕ СТОЛЫ:");
        var freeTables = tables.Where(t => t.IsAvailable(start!, end!)).ToList();
        
        if (freeTables.Count == 0)
        {
            ShowError("Нет свободных столов на это время");
            return;
        }

        // Показываем доступные столы
        foreach (var table in freeTables)
        {
            table.ShowTableInfo();
            Console.WriteLine("------------------------------------------------");
        }

        // Выбираем стол
        Console.Write("ID стола: ");
        string? tableInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(tableInput) || !int.TryParse(tableInput, out int tableId))
        {
            ShowError("Неправильный ID стола");
            return;
        }

        Table? selectedTable = tables.FirstOrDefault(t => t.TableID == tableId);

        // Проверяем что стол существует и доступен
        if (selectedTable != null && freeTables.Contains(selectedTable))
        {
            reservations.Add(new Reservation(name!, phone!, start!, end!, comment ?? "", selectedTable));
            ShowSuccess("Бронь создана успешно!");
        }
        else
        {
            ShowError("Стол не найден или занят");
        }
    }

    // Изменить существующую бронь
    static void ChangeReservation(List<Reservation> reservations, List<Table> tables)
    {
        Console.Clear();
        Console.WriteLine("ИЗМЕНЕНИЕ БРОНИ");
        Console.WriteLine("================================================================");
        
        ShowAllReservations(reservations);
        
        Console.Write("ID брони для изменения: ");
        string? reservationInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(reservationInput) || !int.TryParse(reservationInput, out int reservationId))
        {
            ShowError("Неправильный ID брони");
            return;
        }

        Reservation? reservation = reservations.FirstOrDefault(r => r.ReservationID == reservationId);

        if (reservation == null)
        {
            ShowError("Бронь не найдена");
            return;
        }

        // Запрашиваем новые данные
        Console.WriteLine("\nНовые данные (Enter - оставить текущие):");
        Console.Write("Время начала (" + reservation.StartTime + "): ");
        string? newStart = Console.ReadLine();
        Console.Write("Время окончания (" + reservation.EndTime + "): ");
        string? newEnd = Console.ReadLine();
        Console.Write("Комментарий (" + reservation.Comment + "): ");
        string? newComment = Console.ReadLine();

        // Используем текущие значения если новые не введены
        newStart = string.IsNullOrWhiteSpace(newStart) ? reservation.StartTime : newStart;
        newEnd = string.IsNullOrWhiteSpace(newEnd) ? reservation.EndTime : newEnd;

        // Ищем доступные столы для нового времени
        Console.WriteLine("\nДОСТУПНЫЕ СТОЛЫ:");
        var availableTables = tables.Where(t => t.IsAvailable(newStart!, newEnd!) || t == reservation.AssignedTable).ToList();
        
        foreach (var table in availableTables)
        {
            table.ShowTableInfo();
            Console.WriteLine("------------------------------------------------");
        }

        Console.Write("ID нового стола (текущий " + reservation.AssignedTable.TableID + "): ");
        string? newTableInput = Console.ReadLine();
        
        Table? newTable;
        if (string.IsNullOrWhiteSpace(newTableInput))
        {
            // Оставляем текущий стол
            newTable = reservation.AssignedTable;
        }
        else
        {
            if (!int.TryParse(newTableInput, out int newTableId))
            {
                ShowError("Неправильный ID стола");
                return;
            }
            newTable = tables.FirstOrDefault(t => t.TableID == newTableId);
        }

        // Обновляем бронь
        if (newTable != null && availableTables.Contains(newTable))
        {
            reservation.UpdateReservation(newStart!, newEnd!, newComment ?? reservation.Comment, newTable);
            ShowSuccess("Бронь обновлена!");
        }
        else
        {
            ShowError("Стол не найден или занят");
        }
    }

    // Отменить бронирование
    static void RemoveReservation(List<Reservation> reservations)
    {
        Console.Clear();
        Console.WriteLine("ОТМЕНА БРОНИ");
        Console.WriteLine("================================================================");
        
        ShowAllReservations(reservations);
        
        Console.Write("ID брони для отмены: ");
        string? cancelInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(cancelInput) || !int.TryParse(cancelInput, out int cancelId))
        {
            ShowError("Неправильный ID брони");
            return;
        }

        Reservation? reservation = reservations.FirstOrDefault(r => r.ReservationID == cancelId);

        if (reservation != null)
        {
            reservation.CancelReservation();
            reservations.Remove(reservation);
            ShowSuccess("Бронь отменена!");
        }
        else
        {
            ShowError("Бронь не найдена");
        }
    }

    // Показать все бронирования
    static void ShowAllReservations(List<Reservation> reservations)
    {
        Console.Clear();
        Console.WriteLine("ВСЕ БРОНИРОВАНИЯ");
        Console.WriteLine("================================================================");
        
        if (reservations.Count == 0)
        {
            Console.WriteLine("Бронирований нет");
            return;
        }

        foreach (var reservation in reservations)
        {
            reservation.ShowReservationInfo();
            Console.WriteLine("------------------------------------------------");
        }
        
        Console.WriteLine("Всего броней: " + reservations.Count);
    }

    // Поиск бронирования
    static void FindReservation(List<Reservation> reservations)
    {
        Console.Clear();
        Console.WriteLine("ПОИСК БРОНИ");
        Console.WriteLine("================================================================");
        
        Console.Write("Имя клиента: ");
        string? name = Console.ReadLine();
        Console.Write("Телефон (последние 4 цифры): ");
        string? phone = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phone))
        {
            ShowError("Введите имя и телефон");
            return;
        }

        // Ищем брони по имени и телефону
        var found = reservations.Where(r => 
            r.ClientName.Contains(name, StringComparison.OrdinalIgnoreCase) && 
            r.ClientPhone.EndsWith(phone)).ToList();
        
        Console.WriteLine("\nРЕЗУЛЬТАТЫ ПОИСКА:");
        Console.WriteLine("================================================================");
        
        if (found.Count == 0)
        {
            Console.WriteLine("Ничего не найдено");
            return;
        }

        foreach (var reservation in found)
        {
            reservation.ShowReservationInfo();
            Console.WriteLine("------------------------------------------------");
        }
        
        Console.WriteLine("Найдено: " + found.Count);
    }

    // =========================================================================
    // МЕТОДЫ ДЛЯ РАБОТЫ С БЛЮДАМИ И ЗАКАЗАМИ
    // =========================================================================

    // Меню управления блюдами
    static void DishManagement(List<Dish> dishes)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("УПРАВЛЕНИЕ БЛЮДАМИ");
            Console.WriteLine("================================================================");
            Console.WriteLine("1 - Добавить блюдо");
            Console.WriteLine("2 - Изменить блюдо");
            Console.WriteLine("3 - Список блюд");
            Console.WriteLine("4 - Удалить блюдо");
            Console.WriteLine("0 - Назад");
            Console.WriteLine("================================================================");
            Console.Write("Выбор: ");
            
            string? choice = Console.ReadLine();
            if (choice == "0") break;

            switch (choice)
            {
                case "1":
                    AddNewDish(dishes);
                    WaitForKey();
                    break;
                case "2":
                    EditExistingDish(dishes);
                    WaitForKey();
                    break;
                case "3":
                    ShowAllDishes(dishes);
                    WaitForKey();
                    break;
                case "4":
                    DeleteDish(dishes);
                    WaitForKey();
                    break;
                default:
                    ShowError("Неверная команда");
                    WaitForKey();
                    break;
            }
        }
    }

    // Добавить новое блюдо в меню
    static void AddNewDish(List<Dish> dishes)
    {
        Console.Clear();
        Console.WriteLine("НОВОЕ БЛЮДО");
        Console.WriteLine("================================================================");
        
        Console.Write("Название: ");
        string? name = Console.ReadLine();
        Console.Write("Состав: ");
        string? composition = Console.ReadLine();
        Console.Write("Вес (г/мл): ");
        string? weight = Console.ReadLine();
        Console.Write("Цена: ");
        string? priceInput = Console.ReadLine();
        
        // Проверяем что цена - положительное число
        if (string.IsNullOrWhiteSpace(priceInput) || !double.TryParse(priceInput, out double price) || price <= 0)
        {
            ShowError("Цена должна быть числом больше 0");
            return;
        }
        
        // Выбор категории блюда
        Console.WriteLine("\nКАТЕГОРИИ:");
        Console.WriteLine("1 - Напитки");
        Console.WriteLine("2 - Салаты"); 
        Console.WriteLine("3 - Холодные закуски");
        Console.WriteLine("4 - Горячие закуски");
        Console.WriteLine("5 - Супы");
        Console.WriteLine("6 - Горячие блюда");
        Console.WriteLine("7 - Десерты");
        Console.Write("Категория: ");
        string? categoryInput = Console.ReadLine();
        
        // Преобразуем ввод в перечисление категорий
        if (string.IsNullOrWhiteSpace(categoryInput) || !Enum.TryParse<DishCategory>(categoryInput, out DishCategory category) || !Enum.IsDefined(typeof(DishCategory), category))
        {
            ShowError("Неверная категория");
            return;
        }
        
        Console.Write("Время готовки (минут): ");
        string? timeInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(timeInput) || !int.TryParse(timeInput, out int cookingTime) || cookingTime <= 0)
        {
            ShowError("Время должно быть числом больше 0");
            return;
        }
        
        Console.Write("Типы (через запятую): ");
        string? typesInput = Console.ReadLine();
        // Разделяем ввод на массив типов, убираем пробелы
        string[] types = (typesInput ?? "").Split(',').Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t)).ToArray();

        // Проверяем обязательные поля
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(composition) || string.IsNullOrWhiteSpace(weight))
        {
            ShowError("Заполните название, состав и вес");
            return;
        }

        // Создаем новое блюдо и добавляем в список
        Dish newDish = new Dish(name, composition, weight, price, category, cookingTime, types);
        dishes.Add(newDish);
        ShowSuccess("Блюдо добавлено в меню!");
    }

    // Редактировать существующее блюдо
    static void EditExistingDish(List<Dish> dishes)
    {
        ShowAllDishes(dishes);
        Console.Write("ID блюда для изменения: ");
        string? idInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(idInput) || !int.TryParse(idInput, out int id))
        {
            ShowError("Неверный ID");
            return;
        }

        Dish? dish = dishes.FirstOrDefault(d => d.DishId == id);
        if (dish == null)
        {
            ShowError("Блюдо не найдено");
            return;
        }

        Console.WriteLine("ИЗМЕНЕНИЕ БЛЮДА (Enter - оставить текущее значение)");
        
        Console.Write("Название (" + dish.Name + "): ");
        string? name = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(name)) dish.Name = name;
        
        Console.Write("Состав (" + dish.Composition + "): ");
        string? composition = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(composition)) dish.Composition = composition;
        
        Console.Write("Вес (" + dish.Weight + "): ");
        string? weight = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(weight)) dish.Weight = weight;
        
        Console.Write("Цена (" + dish.Price + "): ");
        string? priceInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(priceInput) && double.TryParse(priceInput, out double price))
            dish.Price = price;

        ShowSuccess("Блюдо обновлено!");
    }

    // Показать все блюда
    static void ShowAllDishes(List<Dish> dishes)
    {
        Console.Clear();
        Console.WriteLine("ВСЕ БЛЮДА");
        Console.WriteLine("================================================================");
        
        if (dishes.Count == 0)
        {
            Console.WriteLine("Блюд нет");
            return;
        }

        foreach (var dish in dishes)
        {
            dish.ShowDishInfo();
            Console.WriteLine("------------------------------------------------");
        }
    }

    // Удалить блюдо
    static void DeleteDish(List<Dish> dishes)
    {
        ShowAllDishes(dishes);
        Console.Write("ID блюда для удаления: ");
        string? idInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(idInput) || !int.TryParse(idInput, out int id))
        {
            ShowError("Неверный ID");
            return;
        }

        Dish? dish = dishes.FirstOrDefault(d => d.DishId == id);
        if (dish != null)
        {
            dishes.Remove(dish);
            ShowSuccess("Блюдо удалено!");
        }
        else
        {
            ShowError("Блюдо не найдено");
        }
    }

    // Меню управления заказами
    static void OrderManagement(List<Order> orders, List<Dish> dishes, List<Table> tables)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("УПРАВЛЕНИЕ ЗАКАЗАМИ");
            Console.WriteLine("================================================================");
            Console.WriteLine("1 - Создать заказ");
            Console.WriteLine("2 - Изменить заказ");
            Console.WriteLine("3 - Список заказов");
            Console.WriteLine("4 - Закрыть заказ");
            Console.WriteLine("5 - Показать чек");
            Console.WriteLine("0 - Назад");
            Console.WriteLine("================================================================");
            Console.Write("Выбор: ");
            
            string? choice = Console.ReadLine();
            if (choice == "0") break;

            switch (choice)
            {
                case "1":
                    CreateNewOrder(orders, dishes, tables);
                    WaitForKey();
                    break;
                case "2":
                    EditExistingOrder(orders, dishes);
                    WaitForKey();
                    break;
                case "3":
                    ShowAllOrders(orders);
                    WaitForKey();
                    break;
                case "4":
                    CloseExistingOrder(orders);
                    WaitForKey();
                    break;
                case "5":
                    ShowOrderReceipt(orders);
                    WaitForKey();
                    break;
                default:
                    ShowError("Неверная команда");
                    WaitForKey();
                    break;
            }
        }
    }

    // Создать новый заказ
    static void CreateNewOrder(List<Order> orders, List<Dish> dishes, List<Table> tables)
    {
        Console.Clear();
        Console.WriteLine("НОВЫЙ ЗАКАЗ");
        Console.WriteLine("================================================================");
        
        // Проверяем что есть блюда в меню
        if (dishes.Count == 0)
        {
            ShowError("Нет блюд в меню. Сначала добавьте блюда.");
            return;
        }

        // Ввод основных данных заказа
        Console.Write("Номер стола: ");
        string? tableInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(tableInput) || !int.TryParse(tableInput, out int tableId))
        {
            ShowError("Неверный номер стола");
            return;
        }

        Console.Write("ID официанта: ");
        string? waiterInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(waiterInput) || !int.TryParse(waiterInput, out int waiterId))
        {
            ShowError("Неверный ID официанта");
            return;
        }

        Console.Write("Комментарий к заказу: ");
        string? comment = Console.ReadLine();

        // Выбор блюд для заказа
        List<Dish> selectedDishes = new List<Dish>();
        while (true)
        {
            ShowRestaurantMenu(dishes);
            Console.Write("ID блюда (0 - закончить выбор): ");
            string? dishInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(dishInput) || !int.TryParse(dishInput, out int dishId) || dishId == 0)
                break;

            Dish? dish = dishes.FirstOrDefault(d => d.DishId == dishId);
            if (dish != null)
            {
                selectedDishes.Add(dish);
                Console.WriteLine("Добавлено: " + dish.Name);
            }
            else
            {
                ShowError("Блюдо не найдено");
            }
        }

        // Проверяем что в заказе есть хотя бы одно блюдо
        if (selectedDishes.Count == 0)
        {
            ShowError("Заказ должен содержать блюда");
            return;
        }

        // Создаем и сохраняем заказ
        Order newOrder = new Order(tableId, selectedDishes.ToArray(), comment ?? "", waiterId);
        orders.Add(newOrder);
        ShowSuccess("Заказ создан! Номер заказа: " + newOrder.OrderId);
    }

    // Редактировать заказ
    static void EditExistingOrder(List<Order> orders, List<Dish> dishes)
    {
        ShowAllOrders(orders);
        Console.Write("ID заказа для изменения: ");
        string? idInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(idInput) || !int.TryParse(idInput, out int id))
        {
            ShowError("Неверный ID заказа");
            return;
        }

        Order? orderToEdit = orders.FirstOrDefault(o => o.OrderId == id);
        if (orderToEdit == null || orderToEdit.IsClosed)
        {
            ShowError("Заказ не найден или уже закрыт");
            return;
        }

        Console.WriteLine("ИЗМЕНЕНИЕ ЗАКАЗА");
        Console.Write("Комментарий (" + orderToEdit.Comment + "): ");
        string? comment = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(comment)) orderToEdit.Comment = comment;

        // Показываем текущие блюда в заказе
        Console.WriteLine("\nТекущие блюда:");
        foreach (var dish in orderToEdit.Dishes)
        {
            Console.WriteLine("- " + dish.Name);
        }

        // Предлагаем добавить новые блюда
        Console.Write("Добавить еще блюда? (да/нет): ");
        string? addMore = Console.ReadLine();
        if (addMore?.ToLower() == "да")
        {
            ShowRestaurantMenu(dishes);
            Console.Write("ID блюда для добавления: ");
            string? dishInput = Console.ReadLine();
            if (int.TryParse(dishInput, out int dishId))
            {
                Dish? dish = dishes.FirstOrDefault(d => d.DishId == dishId);
                if (dish != null)
                {
                    orderToEdit.AddDish(dish);
                    ShowSuccess("Добавлено: " + dish.Name);
                }
                else
                {
                    ShowError("Блюдо не найдено");
                }
            }
        }

        ShowSuccess("Заказ обновлен!");
    }

    // Показать все заказы
    static void ShowAllOrders(List<Order> orders)
    {
        Console.Clear();
        Console.WriteLine("ВСЕ ЗАКАЗЫ");
        Console.WriteLine("================================================================");
        
        if (orders.Count == 0)
        {
            Console.WriteLine("Заказов нет");
            return;
        }

        foreach (var order in orders)
        {
            order.ShowOrderInfo();
            Console.WriteLine("------------------------------------------------");
        }
    }

    // Закрыть заказ
    static void CloseExistingOrder(List<Order> orders)
    {
        // Показываем только открытые заказы
        var openOrders = orders.Where(o => !o.IsClosed).ToList();
        if (openOrders.Count == 0)
        {
            ShowError("Нет открытых заказов");
            return;
        }

        Console.WriteLine("ОТКРЫТЫЕ ЗАКАЗЫ:");
        foreach (var openOrder in openOrders)
        {
            openOrder.ShowOrderInfo();
            Console.WriteLine("------------------------------------------------");
        }

        Console.Write("ID заказа для закрытия: ");
        string? idInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(idInput) || !int.TryParse(idInput, out int id))
        {
            ShowError("Неверный ID заказа");
            return;
        }

        Order? orderToClose = orders.FirstOrDefault(o => o.OrderId == id && !o.IsClosed);
        if (orderToClose != null)
        {
            orderToClose.CloseOrder();
            ShowSuccess("Заказ закрыт! Итоговая сумма: " + orderToClose.TotalPrice);
        }
        else
        {
            ShowError("Заказ не найден или уже закрыт");
        }
    }

    // Показать чек для закрытого заказа
    static void ShowOrderReceipt(List<Order> orders)
    {
        // Показываем только закрытые заказы
        var closedOrders = orders.Where(o => o.IsClosed).ToList();
        if (closedOrders.Count == 0)
        {
            ShowError("Нет закрытых заказов");
            return;
        }

        Console.WriteLine("ЗАКРЫТЫЕ ЗАКАЗЫ:");
        foreach (var closedOrder in closedOrders)
        {
            closedOrder.ShowOrderInfo();
            Console.WriteLine("------------------------------------------------");
        }

        Console.Write("ID заказа для чека: ");
        string? idInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(idInput) || !int.TryParse(idInput, out int id))
        {
            ShowError("Неверный ID заказа");
            return;
        }

        Order? orderForReceipt = orders.FirstOrDefault(o => o.OrderId == id && o.IsClosed);
        if (orderForReceipt != null)
        {
            orderForReceipt.PrintReceipt();
        }
        else
        {
            ShowError("Закрытый заказ не найден");
        }
    }

    // Показать меню ресторана
    static void ShowRestaurantMenu(List<Dish> dishes)
    {
        Console.Clear();
        Console.WriteLine("МЕНЮ РЕСТОРАНА");
        Console.WriteLine("================================================================");

        if (dishes.Count == 0)
        {
            Console.WriteLine("Меню пустое");
            return;
        }

        // Группируем блюда по категориям для удобного отображения
        var byCategory = dishes.GroupBy(d => d.Category).OrderBy(g => g.Key);

        foreach (var categoryGroup in byCategory)
        {
            Console.WriteLine("\n" + GetCategoryName(categoryGroup.Key) + ":");
            Console.WriteLine("------------------------------------------------");

            foreach (var dish in categoryGroup)
            {
                Console.WriteLine("ID: " + dish.DishId);
                Console.WriteLine("Название: " + dish.Name);
                Console.WriteLine("Состав: " + dish.Composition);
                Console.WriteLine("Вес: " + dish.Weight);
                Console.WriteLine("Цена: " + dish.Price.ToString("0.00") + " руб.");
                Console.WriteLine("Время готовки: " + dish.CookingTime + " мин.");
                if (dish.Types.Length > 0)
                    Console.WriteLine("Особенности: " + string.Join(", ", dish.Types));
                Console.WriteLine("------------------------------------------------");
            }
        }
    }

    // Показать статистику
    static void ShowStats(List<Order> orders)
    {
        Console.Clear();
        Console.WriteLine("СТАТИСТИКА РЕСТОРАНА");
        Console.WriteLine("================================================================");

        // Общая выручка по закрытым заказам
        double total = 0;
        foreach (var order in orders)
        {
            if (order.IsClosed)
            {
                total += order.TotalPrice;
            }
        }
        Console.WriteLine("Общая выручка: " + total.ToString("0.00") + " руб.");

        // Статистика по официантам
        Console.WriteLine("\nСТАТИСТИКА ПО ОФИЦИАНТАМ:");
        var waiterStats = orders.Where(o => o.IsClosed)
                               .GroupBy(o => o.WaiterId)
                               .Select(g => new { 
                                   Waiter = g.Key, 
                                   Total = g.Sum(o => o.TotalPrice), 
                                   Count = g.Count() 
                               });

        foreach (var stat in waiterStats)
        {
            Console.WriteLine("Официант " + stat.Waiter + ": " + stat.Count + 
                            " заказов на сумму " + stat.Total.ToString("0.00") + " руб.");
        }

        // Статистика по популярности блюд
        Console.WriteLine("\nПОПУЛЯРНОСТЬ БЛЮД:");
        var dishStats = new Dictionary<string, int>();
        foreach (var order in orders.Where(o => o.IsClosed))
        {
            foreach (var dish in order.Dishes)
            {
                if (dishStats.ContainsKey(dish.Name))
                    dishStats[dish.Name]++;
                else
                    dishStats[dish.Name] = 1;
            }
        }

        // Сортируем по популярности (по убыванию)
        foreach (var stat in dishStats.OrderByDescending(s => s.Value))
        {
            Console.WriteLine(stat.Key + ": " + stat.Value + " раз");
        }
    }

    // Вспомогательный метод для получения названия категории
    static string GetCategoryName(DishCategory category)
    {
        switch (category)
        {
            case DishCategory.Drinks: return "НАПИТКИ";
            case DishCategory.Salads: return "САЛАТЫ";
            case DishCategory.ColdAppetizers: return "ХОЛОДНЫЕ ЗАКУСКИ";
            case DishCategory.HotAppetizers: return "ГОРЯЧИЕ ЗАКУСКИ";
            case DishCategory.Soups: return "СУПЫ";
            case DishCategory.MainCourses: return "ОСНОВНЫЕ БЛЮДА";
            case DishCategory.Desserts: return "ДЕСЕРТЫ";
            default: return "ДРУГОЕ";
        }
    }
}

// Перечисление для категорий блюд
public enum DishCategory
{
    Drinks = 1,
    Salads = 2,
    ColdAppetizers = 3,
    HotAppetizers = 4,
    Soups = 5,
    MainCourses = 6,
    Desserts = 7
}

// Класс для представления блюда
public class Dish
{
    // Уникальный идентификатор блюда
    public int DishId { get; private set; }
    // Название блюда
    public string Name { get; set; }
    // Ингредиенты и состав
    public string Composition { get; set; }
    // Вес или объем
    public string Weight { get; set; }
    // Цена в рублях
    public double Price { get; set; }
    // Категория из перечисления
    public DishCategory Category { get; set; }
    // Время приготовления в минутах
    public int CookingTime { get; set; }
    // Особенности: острое, вегетарианское и т.д.
    public string[] Types { get; set; }

    // Статическая переменная для генерации уникальных ID
    private static int nextDishId = 1;

    // Конструктор для создания нового блюда
    public Dish(string name, string composition, string weight, double price, 
                DishCategory category, int cookingTime, string[] types)
    {
        DishId = nextDishId++;
        Name = name;
        Composition = composition;
        Weight = weight;
        Price = price;
        Category = category;
        CookingTime = cookingTime;
        Types = types;
    }

    // Метод для отображения информации о блюде
    public void ShowDishInfo()
    {
        Console.WriteLine("ID: " + DishId);
        Console.WriteLine("Название: " + Name);
        Console.WriteLine("Состав: " + Composition);
        Console.WriteLine("Вес: " + Weight);
        Console.WriteLine("Цена: " + Price.ToString("0.00") + " руб.");
        Console.WriteLine("Категория: " + Category);
        Console.WriteLine("Время готовки: " + CookingTime + " мин.");
        Console.WriteLine("Особенности: " + string.Join(", ", Types));
    }
}

// Класс для представления заказа
public class Order
{
    // Уникальный идентификатор заказа
    public int OrderId { get; private set; }
    // Номер стола
    public int TableId { get; set; }
    // Список блюд в заказе
    public Dish[] Dishes { get; private set; }
    // Комментарий к заказу
    public string Comment { get; set; }
    // ID официанта
    public int WaiterId { get; set; }
    // Время создания заказа
    public DateTime OrderTime { get; private set; }
    // Время закрытия заказа (null если еще открыт)
    public DateTime? CloseTime { get; private set; }
    // Общая стоимость заказа
    public double TotalPrice { get; private set; }
    // Флаг закрыт ли заказ
    public bool IsClosed => CloseTime.HasValue;

    // Генератор уникальных ID заказов
    private static int nextOrderId = 1;

    // Конструктор заказа
    public Order(int tableId, Dish[] dishes, string comment, int waiterId)
    {
        OrderId = nextOrderId++;
        TableId = tableId;
        Dishes = dishes;
        Comment = comment;
        WaiterId = waiterId;
        OrderTime = DateTime.Now;
        CalculateTotal(); // Сразу рассчитываем сумму
    }

    // Метод для добавления блюда в заказ
    public void AddDish(Dish dish)
    {
        // Создаем новый массив на 1 элемент больше
        Dish[] newDishes = new Dish[Dishes.Length + 1];
        // Копируем старые блюда
        Array.Copy(Dishes, newDishes, Dishes.Length);
        // Добавляем новое блюдо
        newDishes[Dishes.Length] = dish;
        // Заменяем массив
        Dishes = newDishes;
        // Пересчитываем сумму
        CalculateTotal();
    }

    // Закрытие заказа
    public void CloseOrder()
    {
        CloseTime = DateTime.Now;
        CalculateTotal();
    }

    // Расчет общей стоимости заказа
    private void CalculateTotal()
    {
        TotalPrice = Dishes.Sum(d => d.Price);
    }

    // Показать информацию о заказе
    public void ShowOrderInfo()
    {
        Console.WriteLine("Номер заказа: " + OrderId);
        Console.WriteLine("Стол: " + TableId);
        Console.WriteLine("Официант: " + WaiterId);
        Console.WriteLine("Время заказа: " + OrderTime.ToString("dd.MM.yyyy HH:mm"));
        Console.WriteLine("Статус: " + (IsClosed ? "ЗАКРЫТ" : "ОТКРЫТ"));
        if (IsClosed)
            Console.WriteLine("Время закрытия: " + CloseTime.Value.ToString("dd.MM.yyyy HH:mm"));
        Console.WriteLine("Сумма: " + TotalPrice.ToString("0.00") + " руб.");
        Console.WriteLine("Комментарий: " + Comment);
        Console.WriteLine("Блюда:");
        foreach (var dish in Dishes)
        {
            Console.WriteLine("  - " + dish.Name + " (" + dish.Price.ToString("0.00") + " руб.)");
        }
    }

    // Печать чека для закрытого заказа
    public void PrintReceipt()
    {
        if (!IsClosed)
        {
            Console.WriteLine("Чек можно распечатать только для закрытых заказов");
            return;
        }

        Console.WriteLine("================================================================");
        Console.WriteLine("                             ЧЕК");
        Console.WriteLine("================================================================");
        Console.WriteLine("Стол: " + TableId);
        Console.WriteLine("Официант: " + WaiterId);
        Console.WriteLine("Время: " + OrderTime.ToString("dd.MM.yyyy HH:mm"));
        Console.WriteLine("================================================================");

        // Группируем блюда по категориям для красивого чека
        var byCategory = Dishes.GroupBy(d => d.Category).OrderBy(g => g.Key);

        double total = 0;

        foreach (var categoryGroup in byCategory)
        {
            Console.WriteLine(GetCategoryName(categoryGroup.Key) + ":");
            double categoryTotal = 0;

            // Группируем одинаковые блюда и считаем количество
            var dishCounts = categoryGroup.GroupBy(d => d.Name)
                                         .Select(g => new { 
                                             Name = g.Key, 
                                             Count = g.Count(), 
                                             Price = g.First().Price 
                                         });

            foreach (var dish in dishCounts)
            {
                double dishTotal = dish.Count * dish.Price;
                categoryTotal += dishTotal;
                Console.WriteLine("  " + dish.Name.PadRight(30) + 
                                 dish.Count + " x " + dish.Price.ToString("0.00") + 
                                 " = " + dishTotal.ToString("0.00") + " руб.");
            }

            Console.WriteLine("Итого по категории: " + categoryTotal.ToString("0.00") + " руб.");
            Console.WriteLine("------------------------------------------------");
            total += categoryTotal;
        }

        Console.WriteLine("ОБЩАЯ СУММА: " + total.ToString("0.00") + " руб.");
        Console.WriteLine("================================================================");
    }

    // Вспомогательный метод для получения названия категории
    private string GetCategoryName(DishCategory category)
    {
        switch (category)
        {
            case DishCategory.Drinks: return "Напитки";
            case DishCategory.Salads: return "Салаты";
            case DishCategory.ColdAppetizers: return "Холодные закуски";
            case DishCategory.HotAppetizers: return "Горячие закуски";
            case DishCategory.Soups: return "Супы";
            case DishCategory.MainCourses: return "Основные блюда";
            case DishCategory.Desserts: return "Десерты";
            default: return "Прочее";
        }
    }
}

// Класс для представления стола в ресторане
public class Table
{
    // Уникальный ID стола
    public int TableID { get; private set; }
    // Расположение стола в зале
    public string Location { get; private set; }
    // Количество мест
    public int Seats { get; private set; }
    // Расписание бронирований (время -> информация о брони)
    public Dictionary<string, string> Schedule { get; private set; }

    // Генератор уникальных ID столов
    private static int nextTableID = 1;

    // Конструктор стола
    public Table(string location, int seats)
    {
        TableID = nextTableID++;
        Location = location;
        Seats = seats;
        Schedule = new Dictionary<string, string>();
    }

    // Обновление информации о столе
    public void UpdateTable(string newLocation, int newSeats)
    {
        Location = newLocation;
        Seats = newSeats;
    }

    // Показать информацию о столе
    public void ShowTableInfo()
    {
        Console.WriteLine("ID стола: " + TableID);
        Console.WriteLine("Расположение: " + Location);
        Console.WriteLine("Мест: " + Seats);
        Console.WriteLine("Бронирования:");

        if (Schedule.Count == 0)
        {
            Console.WriteLine("  Нет бронирований");
        }
        else
        {
            // Сортируем расписание по времени
            var sortedSchedule = Schedule.OrderBy(e => TimeSpan.Parse(e.Key)).ToList();
            string? currentReservation = null;
            string? rangeStart = null;

            // Объединяем последовательные интервалы с одинаковой броньью
            foreach (var entry in sortedSchedule)
            {
                if (currentReservation == null)
                {
                    // Начало нового интервала
                    currentReservation = entry.Value;
                    rangeStart = entry.Key;
                }
                else if (currentReservation != entry.Value)
                {
                    // Конец текущего интервала, начало нового
                    Console.WriteLine("  " + rangeStart + " - " + entry.Key + " : " + currentReservation);
                    currentReservation = entry.Value;
                    rangeStart = entry.Key;
                }
            }

            // Выводим последний интервал
            if (currentReservation != null && rangeStart != null)
            {
                Console.WriteLine("  " + rangeStart + " - " + sortedSchedule.Last().Key + " : " + currentReservation);
            }
        }
    }

    // Проверить доступность стола на указанное время
    public bool IsAvailable(string startTime, string endTime)
    {
        // Проверяем каждое время в расписании
        foreach (var time in Schedule.Keys)
        {
            // Если время попадает в интервал брони - стол занят
            if (string.Compare(time, startTime) >= 0 && string.Compare(time, endTime) < 0)
            {
                return false;
            }
        }
        return true;
    }

    // Добавить бронирование в расписание
    public void AddReservation(string startTime, string endTime, string reservationInfo)
    {
        // Добавляем каждый час в интервале брони
        for (var time = TimeSpan.Parse(startTime); time < TimeSpan.Parse(endTime); time += TimeSpan.FromHours(1))
        {
            Schedule[time.ToString(@"hh\:mm")] = reservationInfo;
        }
    }

    // Удалить бронирование из расписания
    public void RemoveReservation(string startTime, string endTime)
    {
        // Удаляем каждый час в интервале брони
        for (var time = TimeSpan.Parse(startTime); time < TimeSpan.Parse(endTime); time += TimeSpan.FromHours(1))
        {
            Schedule.Remove(time.ToString(@"hh\:mm"));
        }
    }
}

// Класс для представления бронирования стола
public class Reservation
{
    // Уникальный ID бронирования
    public int ReservationID { get; private set; }
    // Имя клиента
    public string ClientName { get; private set; }
    // Телефон клиента
    public string ClientPhone { get; private set; }
    // Время начала брони
    public string StartTime { get; private set; }
    // Время окончания брони
    public string EndTime { get; private set; }
    // Комментарий к брони
    public string Comment { get; private set; }
    // Стол который забронирован
    public Table AssignedTable { get; private set; }

    // Генератор уникальных ID бронирований
    private static int nextReservationID = 1;

    // Конструктор бронирования
    public Reservation(string clientName, string clientPhone, string startTime, 
                      string endTime, string comment, Table assignedTable)
    {
        ReservationID = nextReservationID++;
        ClientName = clientName;
        ClientPhone = clientPhone;
        StartTime = startTime;
        EndTime = endTime;
        Comment = comment;
        AssignedTable = assignedTable;

        // Добавляем бронь в расписание стола
        AssignedTable.AddReservation(StartTime, EndTime, 
                                   "Бронь #" + ReservationID + ", " + ClientName + ", " + ClientPhone);
    }

    // Обновление данных бронирования
    public void UpdateReservation(string newStartTime, string newEndTime, string newComment, Table newTable)
    {
        // Удаляем старую бронь из расписания
        AssignedTable.RemoveReservation(StartTime, EndTime);

        // Обновляем данные
        StartTime = newStartTime;
        EndTime = newEndTime;
        Comment = newComment;
        AssignedTable = newTable;

        // Добавляем обновленную бронь
        AssignedTable.AddReservation(StartTime, EndTime, 
                                   "Бронь #" + ReservationID + ", " + ClientName + ", " + ClientPhone);
    }

    // Отмена бронирования
    public void CancelReservation()
    {
        // Удаляем бронь из расписания стола
        AssignedTable.RemoveReservation(StartTime, EndTime);
    }

    // Показать информацию о бронировании
    public void ShowReservationInfo()
    {
        Console.WriteLine("ID брони: " + ReservationID);
        Console.WriteLine("Клиент: " + ClientName);
        Console.WriteLine("Телефон: " + ClientPhone);
        Console.WriteLine("Время: " + StartTime + " - " + EndTime);
        Console.WriteLine("Комментарий: " + Comment);
        Console.WriteLine("Стол: " + AssignedTable.TableID);
    }
}