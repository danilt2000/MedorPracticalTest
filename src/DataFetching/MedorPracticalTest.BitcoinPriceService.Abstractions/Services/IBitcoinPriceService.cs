using MedorPracticalTest.Domain.Entities.Bitcoin;

namespace MedorPracticalTest.BitcoinPriceService.Abstractions.Services
{
        public interface IBitcoinPriceService
        {
                Task<Bitcoin> GetCurrentBitcoinPriceAsync();

                Task<IEnumerable<Bitcoin>> GetHistoricalBitcoinPriceAsync(DateTime startDateTime);
        }
}
