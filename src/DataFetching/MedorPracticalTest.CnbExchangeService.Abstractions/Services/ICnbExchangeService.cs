namespace MedorPracticalTest.CnbExchangeService.Abstractions.Services
{
        public interface ICnbExchangeService
        {
                Task<decimal> GetEurCzkExchangeRateAsync(DateTime date);
        }
}
