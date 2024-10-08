namespace MedorPracticalTest.Domain.Entities.Bitcoin
{
        public class Bitcoin : BaseEntity
        {
                public DateTime Timestamp { get; private set; }

                public decimal BitcoinPriceUSD { get; private set; }

                public decimal BitcoinPriceEUR { get; private set; }

                public decimal BitcoinPriceCZK { get; private set; }

                public string? Note { get; private set; }

                public Bitcoin(int id, DateTime timestamp, decimal bitcoinPriceUSD, decimal bitcoinPriceEUR, decimal bitcoinPriceCZK, string? note) : base(id)
                {
                        Timestamp = timestamp;

                        BitcoinPriceUSD = bitcoinPriceUSD;

                        BitcoinPriceEUR = bitcoinPriceEUR;

                        BitcoinPriceCZK = bitcoinPriceCZK;

                        Note = note;
                }

                public Bitcoin(int id, DateTime timestamp, decimal bitcoinPriceUSD, decimal bitcoinPriceEUR, string? note) : base(id)
                {
                        Timestamp = timestamp;

                        BitcoinPriceUSD = bitcoinPriceUSD;

                        BitcoinPriceEUR = bitcoinPriceEUR;

                        Note = note;
                }
        }
}
