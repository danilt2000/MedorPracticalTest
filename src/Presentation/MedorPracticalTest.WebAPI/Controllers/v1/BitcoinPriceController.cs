using MediatR;
using MedorPracticalTest.Application.Requests.Bitcoins.Queries.GetCurrentBitcoinPriceRequest;
using MedorPracticalTest.Application.Requests.Bitcoins.Queries.GetHistoricalBitcoinPriceRequest;
using Microsoft.AspNetCore.Mvc;

namespace MedorPracticalTest.WebAPI.Controllers.v1
{
        [ApiController]
        [ApiVersion("1.0")]
        public class BitcoinPriceController : BaseController
        {
                /// <summary>
                /// Constructor
                /// </summary>
                /// <param name="mediator">Dependency injection of Mediator</param>
                public BitcoinPriceController(IMediator mediator) : base(mediator)
                {
                }

                // GET: api/v1/BitcoinPrice/GetCurrentPrice
                [HttpGet("GetCurrentPrice")]
                public async Task<IActionResult> GetCurrentPrice()
                {
                        var bitcoin = await Mediator.Send(new GetCurrentBitcoinPriceRequest());
                        return Ok(bitcoin);
                }

                // GET: api/v1/BitcoinPrice/GetHistoricalData
                [HttpGet("GetHistoricalData")]
                public async Task<IActionResult> GetHistoricalData(
                        [FromQuery] DateTime startDate)
                {
                        var historicalBitcoinData = await Mediator.Send(new GetHistoricalBitcoinPriceRequest
                        {
                                StartDateTime = startDate
                        });

                        return Ok(historicalBitcoinData);
                }

                // POST: api/BitcoinPrice/SaveLiveData
                [HttpPost("SaveLiveData")]
                public IActionResult SaveLiveData(/* parameters */)
                {
                        // Method to save the fetched live data into the database
                        // Implementation here
                        return Ok();
                }

                // GET: api/BitcoinPrice/GetSavedData
                [HttpGet("GetSavedData")]
                public IActionResult GetSavedData()
                {
                        // Method to retrieve saved data from the database
                        // Implementation here
                        return Ok(/* saved data */);
                }

                // PUT: api/BitcoinPrice/UpdateSavedData
                [HttpPut("UpdateSavedData")]
                public IActionResult UpdateSavedData(/* parameters */)
                {
                        // Method to update saved data (e.g., editing notes)
                        // Implementation here
                        return Ok();
                }

                // DELETE: api/BitcoinPrice/DeleteSavedData
                [HttpDelete("DeleteSavedData")]
                public IActionResult DeleteSavedData(/* parameters */)
                {
                        // Method to delete selected saved data from the database
                        // Implementation here
                        return Ok();
                }

                // GET: api/BitcoinPrice/GetBitcoinPriceHistory
                [HttpGet("GetBitcoinPriceHistory")]
                public IActionResult GetBitcoinPriceHistory()
                {
                        // Method to get the history of Bitcoin prices for both table and graph views
                        // Implementation here
                        return Ok(/* history data */);
                }
        }
}
