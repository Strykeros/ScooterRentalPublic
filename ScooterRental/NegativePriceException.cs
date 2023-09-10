
namespace ScooterRental
{
    public class NegativePriceException : Exception
    {
        public NegativePriceException() : base("The price can't be negative") 
        { 
        
        }
    }
}
