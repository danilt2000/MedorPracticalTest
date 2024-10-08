using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MedorPracticalTest.WebAPI.Controllers
{
        /// <summary>
        /// Base controller all controllers are derived from
        /// Defines rule for api routing
        /// </summary>
        [Route("api/v{version:apiVersion}/[controller]")]
        public class BaseController : Controller
        {
                /// <summary>
                /// Mediator dependency injection
                /// </summary>
                public IMediator Mediator;

                /// <summary>
                /// Constructor for base controller
                /// </summary>
                /// <param name="mediator">Mediator dependency injection</param>
                public BaseController(IMediator mediator)
                {
                        Mediator = mediator;
                }
        }
}
