namespace MedorPracticalTest.Application.Exceptions
{
        public class BitcoinNotFoundException(DateTime timestamp)
                : Exception($"Bitcoin on {timestamp} was not found");
}
