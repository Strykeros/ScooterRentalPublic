using FluentAssertions;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScooterRental.Tests
{
    [TestClass]
    public class RentalCompanyTests
    {
        private AutoMocker _mocker;
        private List<Scooter> _scooters;
        private List<RentedScooter> _rentedScooters;
        private IRentalCompany _company;
        private IScooterService _scooterService;
        private const string _defaultCompanyName = "default";

        [TestInitialize]
        public void Setup()
        {
            _mocker = new AutoMocker();
            _rentedScooters = new List<RentedScooter>();
            _scooters = new List<Scooter>();
            _scooterService = new ScooterService(_scooters);
            _company = new RentalCompany(_defaultCompanyName, _scooterService, _rentedScooters);
            _scooterService.AddScooter("1", 4);
            _scooterService.AddScooter("2", 2);
            _scooterService.AddScooter("3", 3);
            _scooterService.AddScooter("4", 2);
            _rentedScooters.Add(new RentedScooter("1", DateTime.Now.AddHours(1)));
            _rentedScooters.Add(new RentedScooter("2",DateTime.Now.AddHours(3)));
            _rentedScooters.Add(new RentedScooter("3", DateTime.Now.AddYears(1)));
            _rentedScooters.Add(new RentedScooter("4", DateTime.Now.AddMonths(6)));
        }

        [TestMethod]
        public void StartRent_StartScooterRent_ScooterRentStarted()
        {
            _company.StartRent("1");
            _scooters[0].IsRented.Should().BeTrue();
        }


        [TestMethod]
        public void EndRent_EndScooterRent_ScooterRentEnded()
        {
            _company.StartRent("1");
            _company.EndRent("1");
            _scooters[0].IsRented.Should().BeFalse();
        }

        [TestMethod]
        public void CalculateIncome_YearAndIncompleteRentals_TotalIncome()
        {
            _company.StartRent("1");
            _company.StartRent("4");
            string income = _company.CalculateIncome(null, true).ToString("0.00");

            _company.EndRent("1");
            string income2 = _company.CalculateIncome(2023, true).ToString("0.00");
            string income3 = _company.CalculateIncome(2023, false).ToString("0.00");
            income.Should().Be("2105880.00");
            income2.Should().Be("600.00");
            income3.Should().Be("600.00");
        }
    }
}
