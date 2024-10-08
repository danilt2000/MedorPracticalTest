using System.Data;
using MedorPracticalTest.Domain.Entities.Bitcoin;
using MedorPracticalTest.Persistence.Abstractions.Repositories;
using Microsoft.Data.SqlClient;

namespace MedorPracticalTest.Persistence.Repositories
{
        internal class BitcoinRepository : IBitcoinRepository
        {
                private readonly string _connectionString;

                public BitcoinRepository(string connectionString)
                {
                        _connectionString = connectionString;
                }

                public async Task SaveBitcoinDataAsync(Bitcoin bitcoin)
                {
                        await using SqlConnection connection = new SqlConnection(_connectionString);
                        await using SqlCommand command = new SqlCommand("dbo.SaveBitcoinData", connection);
                        
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@BitcoinPriceUSD", bitcoin.BitcoinPriceUSD);
                        command.Parameters.AddWithValue("@BitcoinPriceEUR", bitcoin.BitcoinPriceEUR);
                        command.Parameters.AddWithValue("@BitcoinPriceCZK", bitcoin.BitcoinPriceCZK);
                        command.Parameters.AddWithValue("@Timestamp", bitcoin.Timestamp);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                }

                public async Task<IEnumerable<Bitcoin>> GetAllBitcoinsAsync()
                {
                        List<Bitcoin> bitcoins = new List<Bitcoin>();

                        //using (SqlConnection connection = new SqlConnection(_connectionString))
                        //{
                        //        using (SqlCommand command = new SqlCommand("dbo.GetAllBitcoins", connection))
                        //        {
                        //                command.CommandType = CommandType.StoredProcedure;

                        //                await connection.OpenAsync();
                        //                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        //                {
                        //                        while (await reader.ReadAsync())
                        //                        {
                        //                                bitcoins.Add(new Bitcoin
                        //                                {
                        //                                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        //                                        Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                        //                                        Timestamp = reader.GetDateTime(reader.GetOrdinal("Timestamp"))
                        //                                });
                        //                        }
                        //                }
                        //        }
                        //}

                        return bitcoins;
                }

                public async Task DeleteBitcoinDataAsync(int id)
                {
                        using (SqlConnection connection = new SqlConnection(_connectionString))
                        {
                                using (SqlCommand command = new SqlCommand("dbo.DeleteBitcoinData", connection))
                                {
                                        command.CommandType = CommandType.StoredProcedure;
                                        command.Parameters.AddWithValue("@Id", id);

                                        await connection.OpenAsync();
                                        await command.ExecuteNonQueryAsync();
                                }
                        }
                }

                public async Task UpdateBitcoinNoteAsync(int id, string note)
                {
                        using (SqlConnection connection = new SqlConnection(_connectionString))
                        {
                                using (SqlCommand command = new SqlCommand("dbo.UpdateBitcoinNote", connection))
                                {
                                        command.CommandType = CommandType.StoredProcedure;
                                        command.Parameters.AddWithValue("@Id", id);
                                        command.Parameters.AddWithValue("@Note", note);

                                        await connection.OpenAsync();
                                        await command.ExecuteNonQueryAsync();
                                }
                        }
                }
        }
}
