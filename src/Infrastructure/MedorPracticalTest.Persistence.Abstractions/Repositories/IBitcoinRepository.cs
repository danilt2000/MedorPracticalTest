using MedorPracticalTest.Domain.Entities.Bitcoin;

namespace MedorPracticalTest.Persistence.Abstractions.Repositories
{
        public interface IBitcoinRepository
        {
                Task SaveBitcoinDataAsync(Bitcoin bitcoin);

                Task<IEnumerable<Bitcoin>> GetSavedBitcoinsAsync();

                Task DeleteBitcoinDataAsync(int id);

                Task UpdateBitcoinNoteAsync(int id, string note);
        }
}
