namespace MedorPracticalTest.CnbExchangeService.Abstractions.Services
{
        public interface ICnbExchangeService
        {
                Task<Dictionary<DateTime, decimal>> GetEurCzkExchangeRatesForDatesAsync(IEnumerable<DateTime> dates);
        }
}
