using Azure.Core;
using MediatR;
using MedorPracticalTest.Application.Requests.Bitcoins.Commands.SaveBitcoinPriceRequest;
using MedorPracticalTest.Application.Requests.Bitcoins.Commands.UpdateBitcoinNoteRequest;
using MedorPracticalTest.Application.Requests.Bitcoins.Queries.GetCurrentBitcoinPriceRequest;
using MedorPracticalTest.Application.Requests.Bitcoins.Queries.GetHistoricalBitcoinPriceRequest;
using MedorPracticalTest.Application.Requests.Bitcoins.Queries.GetSavedBitcoinsRequest;
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
                public async Task<IActionResult> GetHistoricalData([FromQuery] DateTime startDate)
                {
                        var historicalBitcoinData = await Mediator.Send(new GetHistoricalBitcoinPriceRequest
                        {
                                StartDateTime = startDate
                        });

                        return Ok(historicalBitcoinData);
                }

                // POST: api/BitcoinPrice/SaveLiveData
                [HttpPost("SaveLiveData")]
                public async Task<IActionResult> SaveLiveData([FromBody] SaveBitcoinPriceRequest request)
                {
                        await Mediator.Send(request);

                        return Ok();
                }

                // GET: api/BitcoinPrice/GetSavedData
                [HttpGet("GetSavedData")]
                public async Task<IActionResult> GetSavedData()
                {
                        var bitcoins = await Mediator.Send(new GetSavedBitcoinsRequest());

                        return Ok(bitcoins);
                }

                // PUT: api/BitcoinPrice/UpdateSavedData
                [HttpPatch("UpdateNote")]
                public async Task<IActionResult> UpdateNote([FromBody] UpdateBitcoinNoteRequest request)
                {
                        await Mediator.Send(request);

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
