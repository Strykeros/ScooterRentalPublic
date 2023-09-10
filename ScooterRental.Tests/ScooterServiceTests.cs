using FluentAssertions;

namespace ScooterRental.Tests
{
    [TestClass]
    public class ScooterServiceTests
    {
        private ScooterService _scooterService;
        private List<Scooter> scooters;
        [TestInitialize] 
        public void Setup() 
        { 
            scooters = new List<Scooter>();
            _scooterService = new ScooterService(scooters);

            _scooterService.AddScooter("1", 5.2m);
            _scooterService.AddScooter("2", 1.5m);
            _scooterService.AddScooter("3", 6.5m);
        }

        [TestMethod]
        public void AddScooter_ScooterIdAndPrice_ScooterAddedToList()
        {
            _scooterService.AddScooter("4", 7.1m);
            scooters.Count.Should().Be(4);
        }

        [TestMethod]
        public void GetScooterById_ScooterId_ScooterWithThatId()
        {
            ScooterService scooterService = _scooterService.GetScooterById("1");
            Scooter selectedScooter = scooterService._scooters[0];
            selectedScooter.Id.Should().Be("1");
        }

        [TestMethod]
        public void GetScooters_ScooterServices_ListOfScooterServices()
        {
            IList<ScooterService> scooterServices = _scooterService.GetScooters();
            string[] scooterIds = { "1", "2", "3" };
            decimal[] scooterPrices = { 5.2m, 1.5m, 6.5m };

            for (int i = 0; i < scooterIds.Length; i++)
            {
                string expectedId = scooterIds[i];
                decimal expectedPrice = scooterPrices[i];

                Scooter? foundScooter = scooterServices
                    .SelectMany(service => service._scooters)
                    .FirstOrDefault(s => s.Id == expectedId);

                foundScooter.Should().NotBeNull();
                foundScooter.Id.Should().Be(expectedId);
                foundScooter.PricePerMinute.Should().Be(expectedPrice);
            }
        }

        [TestMethod]
        public void RemoveScooter_ScooterToRemove_ScooterRemovedFromList()
        {
            _scooterService.RemoveScooter("2");
            scooters.Count.Should().Be(2);
        }
    }
}
