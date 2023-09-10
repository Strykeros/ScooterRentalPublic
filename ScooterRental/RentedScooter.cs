using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScooterRental
{
    public class RentedScooter
    {

        public RentedScooter(string id, DateTime startTime) 
        { 
            Id = id;
            RentStart = startTime;
        }
        public string Id { get; set; }
        public DateTime RentStart { get; set; }
        public DateTime? RentEnd { get; set; }
    }
}
