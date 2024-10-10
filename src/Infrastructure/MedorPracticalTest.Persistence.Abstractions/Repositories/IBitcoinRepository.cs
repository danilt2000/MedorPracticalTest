using MedorPracticalTest.Domain.Entities.Bitcoin;

namespace MedorPracticalTest.Persistence.Abstractions.Repositories
{
        public interface IBitcoinRepository
        {
                Task<int> SaveBitcoinAsync(Bitcoin bitcoin);

                Task<IEnumerable<Bitcoin>> GetBitcoinsAsync();

                Task DeleteBitcoinAsync(int id);

                Task UpdateBitcoinNoteAsync(int id, string note);
        }
}
