using System.Text.RegularExpressions;

namespace ScooterRental
{
    internal class Program
    {
        private static List<Scooter> _scooters;
        private static List<RentedScooter> _rentedScooters;
        private static RentalCompany _company;
        private static ScooterService _scooterService;
        private static EventJournal _eventJournal;

        static void Main(string[] args)
        {
            string companyName = "Bolt";
            _eventJournal = new EventJournal();
            _scooters = new List<Scooter>();
            _rentedScooters = new List<RentedScooter>();
            _scooterService = new ScooterService(_scooters);
            _company = new RentalCompany(companyName, _scooterService, _rentedScooters);
            _eventJournal.LogEvent("Program initialized");
            AddScooters();
            InitializeMenu();
        }

        private static void AddScooters()
        {
            _scooterService.AddScooter("1", 30);
            _scooterService.AddScooter("2", 25);
            _scooterService.AddScooter("3", 15);
            _scooterService.AddScooter("4", 20);
            _eventJournal.LogEvent("Scooters Added");
        }

        private static void InitializeMenu()
        {
            List<string> options = new List<string>() { "Rent a scooter", "Stop scooter rent", "Get yearly income", "Exit program" };
            char[] userAvailableOptions = { '1', '2', '3', '4' };
            List<Action> menuFunctions = new List<Action>() { RentScooter, StopScooterRent, GetYearlyIncome, ExitProgram };
            Console.WriteLine("===============Scooter Rental====================");
            Console.WriteLine("");
            _eventJournal.LogEvent("Menu initialized");

            while (true)
            {
                char userOption = PrintOptions(options, $"Select option from 1 to {options.Count}:");

                for (int i = 0; i < options.Count; i++)
                {
                    if (userAvailableOptions[i] == userOption)
                        menuFunctions[i]();
                }
            }
        }

        private static char PrintOptions(List<string> options, string message)
        {
            Console.WriteLine(message);

            int no = 1;
            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{no}. {options[i]}");
                no++;
            }

            return SelectOption(options.Count);
        }

        private static char SelectOption(int optionCount)
        {
            bool optionIsSelected = false;
            char pressedKey = ' ';

            while (!optionIsSelected)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                pressedKey = key.KeyChar;

                if(int.Parse(pressedKey.ToString()) <= 0 || int.Parse(pressedKey.ToString()) > optionCount)
                {
                    _eventJournal.LogEvent($"Exception error: The selected option is invalid or does not exist.");
                    throw new InvalidOptionException();
                }
                optionIsSelected = true;
            }

            return pressedKey;
        }


        private static string GetUserYearInput(string inputTxt)
        {
            string outputYear = "";
            string regexPattern = "[^0-9]";

            Console.WriteLine(inputTxt);
            string numberStr = Regex.Replace(Console.ReadLine(), regexPattern, "").Replace(" ", "");

            if (numberStr != "")
                outputYear = numberStr;

            return outputYear;
        }

        private static char ListScooters()
        {
            var scooterServiceList = _scooterService.GetScooters();
            int scooterCount = 0;

            Console.WriteLine();
            Console.WriteLine($"Select option from 1 to {scooterServiceList.Count}:");

            foreach (var scooterService in scooterServiceList)
            {
                scooterCount += scooterService._scooters.Count;
                for (int i = 0; i < scooterService._scooters.Count; i++)
                {
                    Scooter scooter = scooterService._scooters[i];
                    Console.WriteLine($"Scooter {scooter.Id}, Price per minute: {scooter.PricePerMinute}, Is already rented: {scooter.IsRented}");
                }
            }

            _eventJournal.LogEvent("Scooters Listed");
            return SelectOption(scooterCount);
        }

        private static char ListRentedScooters()
        {
            Console.WriteLine();
            Console.WriteLine($"Select option from 1 to {_rentedScooters.Count}:");

            foreach (var rentedScooter in _rentedScooters)
            {
                for (int i = 0; i < _rentedScooters.Count; i++)
                {
                    Console.WriteLine($"Scooter {rentedScooter.Id}, Rent start date: {rentedScooter.RentStart}, Rent end date: {rentedScooter.RentEnd}");
                }
            }

            _eventJournal.LogEvent("rented Scooters listed");
            return SelectOption(_rentedScooters.Count);
        }

        private static void RentScooter()
        {
            char userSelect = ListScooters();
            _company.StartRent(userSelect.ToString());
            _eventJournal.LogEvent($"Scooter rented with id {userSelect}");
        }

        private static void StopScooterRent()
        {
            char userSelect = ListRentedScooters();
            _company.EndRent(userSelect.ToString());
            _eventJournal.LogEvent($"Scooter rent stopped with id {userSelect}");
        }

        private static void GetYearlyIncome()
        {
            List<string> options = new List<string>() { "Yes", "No" };
            string userYearString = GetUserYearInput("Enter a year:");
            bool includeIncompleteRentals = PrintOptions(options, "Include not completed rentals?") == 1 ? true : false;

            if(string.IsNullOrEmpty(userYearString))
                Console.WriteLine($"\nTotal income: {_company.CalculateIncome(null, includeIncompleteRentals).ToString("0.00")}");
            else
                Console.WriteLine($"\nTotal income: {_company.CalculateIncome(int.Parse(userYearString), includeIncompleteRentals).ToString("0.00")}");

            _eventJournal.LogEvent($"Yearly income printed");
        }

        private static void ExitProgram()
        {
            _eventJournal.LogEvent($"program exit");
            Environment.Exit(0);
        }
    }
}