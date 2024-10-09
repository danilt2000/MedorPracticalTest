namespace MedorPracticalTest.CnbExchangeService.Abstractions.Services
{
        public interface ICnbExchangeService
        {
                Task<decimal> GetEurCzkExchangeRateAsync(DateTime date);

                Task<Dictionary<DateTime, decimal>> GetEurCzkExchangeRatesAsync(IEnumerable<DateTime> dates);
        }
}
