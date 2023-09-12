
namespace ScooterRental
{
    public class RentalCompany : IRentalCompany
    {
        public string Name { get; }
        public IScooterService _scooterService;
        public List<RentedScooter> _rentedScooterList;
        private EventJournal _eventJournal = new EventJournal();
        private decimal _currentIncome = 0;
        
        public RentalCompany(string companyName, IScooterService service, List<RentedScooter> rentedScooters)
        {
            Name = companyName;
            _scooterService = service;
            _rentedScooterList = rentedScooters;
        }

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            decimal totalIncome = 0;

            foreach (RentedScooter rentedScooter in _rentedScooterList)
            {
                if(year.HasValue && rentedScooter.RentStart.Year != year.Value)
                    continue;

                DateTime endDate = !includeNotCompletedRentals && rentedScooter.RentEnd.HasValue ? rentedScooter.RentEnd.Value : DateTime.Now;

                TimeSpan difference = endDate - rentedScooter.RentStart;
                decimal minutes = (decimal)difference.TotalMinutes;

                ScooterService service = _scooterService.GetScooterById(rentedScooter.Id);
                Scooter scooter = service._scooters[0];
                decimal scooterPricePerMinute = scooter.PricePerMinute;
                decimal rentalIncome = scooterPricePerMinute * minutes;
                totalIncome += rentalIncome;
            }

            string totalIncomeStr = totalIncome.ToString().Replace("-", "");

            return decimal.Parse(totalIncomeStr);
        }

        // starts the rental
        public void StartRent(string id)
        {
            var scooter = _scooterService.GetScooterById(id);

            if (scooter._scooters[0].IsRented)
            {
                _eventJournal.LogEvent($"Exception error: Scooter is already rented");
                throw new ScooterAlreadyRentedException();
            }
                
            scooter._scooters[0].IsRented = true;
            _rentedScooterList.Add(new RentedScooter(scooter._scooters[0].Id, DateTime.Now));
        }

        public decimal EndRent(string id)
        {
            ScooterService scooter = _scooterService.GetScooterById(id);

            if (!scooter._scooters[0].IsRented)
            {
                _eventJournal.LogEvent($"Exception error: Scooter is not rented.");
                throw new ScooterNotRentedException();
            }

            scooter._scooters[0].IsRented = false;
            RentedScooter rental = _rentedScooterList
                .FirstOrDefault(sc => sc.Id == scooter._scooters[0].Id && !sc.RentEnd.HasValue);
            rental.RentEnd = DateTime.Now;

            TimeSpan difference = (TimeSpan)(rental.RentEnd - rental.RentStart);
            decimal minutes = (decimal)difference.TotalMinutes;
            _currentIncome += scooter._scooters[0].PricePerMinute * minutes;
            return _currentIncome;
        }
    }
}
