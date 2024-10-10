using MediatR;
using MedorPracticalTest.Application.Requests.Bitcoins.Commands.DeleteBitcoinPriceRequest;
using MedorPracticalTest.Application.Requests.Bitcoins.Commands.SaveBitcoinPriceRequest;
using MedorPracticalTest.Application.Requests.Bitcoins.Commands.UpdateBitcoinNoteRequest;
using MedorPracticalTest.Application.Requests.Bitcoins.Queries.GetCurrentBitcoinPriceRequest;
using MedorPracticalTest.Application.Requests.Bitcoins.Queries.GetHistoricalBitcoinPriceRequest;
using MedorPracticalTest.Application.Requests.Bitcoins.Queries.GetSavedBitcoinsRequest;
using Microsoft.AspNetCore.Mvc;

namespace MedorPracticalTest.WebAPI.Controllers.v1
{
        /// <summary>
        /// Controller responsible for handling requests related to Bitcoin prices, including fetching current prices, 
        /// historical data, saved data, and managing (saving, updating, deleting) stored Bitcoin information.
        /// </summary>
        [ApiController]
        [ApiVersion("1.0")]
        public class BitcoinPriceController : BaseController
        {
                /// <summary>
                /// Initializes a new instance of the <see cref="BitcoinPriceController"/> class.
                /// </summary>
                /// <param name="mediator">Dependency injection for MediatR to handle requests.</param>
                public BitcoinPriceController(IMediator mediator) : base(mediator)
                {
                }

                /// <summary>
                /// Retrieves the current Bitcoin price from an external data source.
                /// </summary>
                /// <returns>The current Bitcoin price.</returns>
                /// <response code="200">Returns the current Bitcoin price.</response>
                [HttpGet("GetCurrentPrice")]
                public async Task<IActionResult> GetCurrentPrice()
                {
                        var bitcoin = await Mediator.Send(new GetCurrentBitcoinPriceRequest());
                        return Ok(bitcoin);
                }

                /// <summary>
                /// Retrieves historical Bitcoin price data starting from a specified date.
                /// </summary>
                /// <param name="startDate">The date from which to start retrieving historical data.</param>
                /// <returns>A list of historical Bitcoin price data.</returns>
                /// <response code="200">Returns the historical Bitcoin price data from the specified start date.</response>
                [HttpGet("GetHistoricalData")]
                public async Task<IActionResult> GetHistoricalData([FromQuery] DateTime startDate)
                {
                        var historicalBitcoinData = await Mediator.Send(new GetHistoricalBitcoinPriceRequest
                        {
                                StartDateTime = startDate
                        });

                        return Ok(historicalBitcoinData);
                }

                /// <summary>
                /// Retrieves saved Bitcoin data that has been stored in the system.
                /// </summary>
                /// <returns>A list of saved Bitcoin data records.</returns>
                /// <response code="200">Returns a list of saved Bitcoin data records.</response>
                [HttpGet("GetSavedData")]
                public async Task<IActionResult> GetSavedData()
                {
                        var bitcoins = await Mediator.Send(new GetSavedBitcoinsRequest());

                        return Ok(bitcoins);
                }

                /// <summary>
                /// Saves the current live Bitcoin price data to the system.
                /// </summary>
                /// <param name="request">The request containing Bitcoin price data to be saved.</param>
                /// <returns>The ID of the saved Bitcoin data record.</returns>
                /// <response code="200">Returns the ID of the saved Bitcoin data record.</response>
                /// <response code="400">Returns if the request data is invalid.</response>
                [HttpPost("SaveLiveData")]
                public async Task<IActionResult> SaveLiveData([FromBody] SaveBitcoinPriceRequest request)
                {
                        var idBitcoin = await Mediator.Send(request);

                        return Ok(idBitcoin);
                }

                /// <summary>
                /// Updates a note associated with a saved Bitcoin data record.
                /// </summary>
                /// <param name="request">The request containing the updated note and associated Bitcoin record ID.</param>
                /// <returns>An HTTP status code indicating the operation result.</returns>
                /// <response code="200">Returns if the update was successful.</response>
                /// <response code="400">Returns if the request data is invalid.</response>
                [HttpPatch("UpdateNote")]
                public async Task<IActionResult> UpdateNote([FromBody] UpdateBitcoinNoteRequest request)
                {
                        await Mediator.Send(request);

                        return Ok();
                }

                /// <summary>
                /// Deletes a saved Bitcoin data record from the system.
                /// </summary>
                /// <param name="request">The request containing the ID of the Bitcoin data record to delete.</param>
                /// <returns>An HTTP status code indicating the operation result.</returns>
                /// <response code="200">Returns if the deletion was successful.</response>
                /// <response code="400">Returns if the request data is invalid.</response>
                [HttpDelete("DeleteSavedData")]
                public async Task<IActionResult> DeleteSavedData([FromBody] DeleteBitcoinPriceRequest request)
                {
                        await Mediator.Send(request);

                        return Ok();
                }
        }
}
