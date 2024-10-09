using MedorPracticalTest.Domain.Entities.Bitcoin;
using MedorPracticalTest.Persistence.Abstractions.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MedorPracticalTest.Persistence.Repositories
{
        internal class BitcoinRepository : IBitcoinRepository
        {
                private readonly string _connectionString;

                public BitcoinRepository(string connectionString)
                {
                        _connectionString = connectionString;
                }

                public async Task SaveBitcoinAsync(Bitcoin bitcoin)
                {
                        await using var connection = new SqlConnection(_connectionString);
                        await using var command = new SqlCommand("dbo.SaveBitcoinData", connection);

                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@BitcoinPriceUSD", bitcoin.BitcoinPriceUSD);
                        command.Parameters.AddWithValue("@BitcoinPriceEUR", bitcoin.BitcoinPriceEUR);
                        command.Parameters.AddWithValue("@BitcoinPriceCZK", bitcoin.BitcoinPriceCZK);
                        command.Parameters.AddWithValue("@Timestamp", bitcoin.Timestamp);
                        command.Parameters.AddWithValue("@Note", bitcoin.Note ?? (object)DBNull.Value);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                }

                public async Task<IEnumerable<Bitcoin>> GetBitcoinsAsync()
                {
                        var bitcoins = new List<Bitcoin>();

                        await using var connection = new SqlConnection(_connectionString);
                        await using var command = new SqlCommand("dbo.GetAllBitcoins", connection);
                        command.CommandType = CommandType.StoredProcedure;

                        await connection.OpenAsync();
                        await using var reader = await command.ExecuteReaderAsync();
                        while (await reader.ReadAsync())
                        {
                                bitcoins.Add(new Bitcoin(
                                        id: reader.GetInt32(reader.GetOrdinal("Id")),
                                        timestamp: reader.GetDateTime(reader.GetOrdinal("Timestamp")),
                                        bitcoinPriceUSD: reader.GetDecimal(reader.GetOrdinal("BitcoinPriceUSD")),
                                        bitcoinPriceEUR: reader.GetDecimal(reader.GetOrdinal("BitcoinPriceEUR")),
                                        bitcoinPriceCZK: reader.GetDecimal(reader.GetOrdinal("BitcoinPriceCZK")),
                                        note: reader.IsDBNull(reader.GetOrdinal("Note")) ? null : reader.GetString(reader.GetOrdinal("Note"))
                                ));
                        }

                        return bitcoins;
                }

                public async Task UpdateBitcoinNoteAsync(int id, string note)
                {
                        await using var connection = new SqlConnection(_connectionString);
                        await using var command = new SqlCommand("dbo.UpdateBitcoinNote", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Note", note);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                }

                public async Task DeleteBitcoinAsync(int id)
                {
                        await using var connection = new SqlConnection(_connectionString);
                        await using var command = new SqlCommand("dbo.DeleteBitcoinData", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                }
        }
}
