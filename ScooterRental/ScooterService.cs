
using System.Data;

namespace ScooterRental
{
    public class ScooterService : IScooterService
    {
        public List<Scooter> _scooters;
        public ScooterService(List<Scooter> scooterListInput) 
        { 
            _scooters = scooterListInput;
        }

        public void AddScooter(string id, decimal pricePerMinute)
        {

            if(pricePerMinute < 0)
            {
                throw new NegativePriceException();
            }

            Scooter scooter = new Scooter(id, pricePerMinute);
            _scooters.Add(scooter);
        }

        public ScooterService GetScooterById(string scooterId)
        {
            List<Scooter> filteredScooters = new List<Scooter>();
            Scooter scooter = _scooters.FirstOrDefault(s => s.Id == scooterId);

            if (scooter == null)
                throw new NoNullAllowedException("The scooter does not exist");

            if (scooter != null)
            {
                filteredScooters.Add(scooter);
            }

            return new ScooterService(filteredScooters);
        }

        public IList<ScooterService> GetScooters()
        {
            List<ScooterService> scooterServices = new List<ScooterService>();

            foreach (Scooter scooter in _scooters)
            {
                ScooterService scooterService = new ScooterService(new List<Scooter> { scooter });
                scooterServices.Add(scooterService);
            }

            return scooterServices;
        }

        public void RemoveScooter(string id)
        {
            Scooter scooterToRemove = _scooters.FirstOrDefault(s => s.Id == id);

            if (scooterToRemove == null)
                throw new NoNullAllowedException("The scooter does not exist");

            _scooters.Remove(scooterToRemove);
        }
    }
}
