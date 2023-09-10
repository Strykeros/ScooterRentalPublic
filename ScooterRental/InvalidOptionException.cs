using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScooterRental
{
    public class InvalidOptionException : Exception
    {
        public InvalidOptionException() : base("The selected option is invalid or does not exist.")
        { 
        
        }
    }
}
