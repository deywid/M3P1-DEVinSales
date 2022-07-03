using DevInSales.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevInSales.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StateController : ControllerBase
    {
        private readonly IStateService _stateService;

        public StateController(IStateService stateService)
        {
            _stateService = stateService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult GetAll(string? name)
        {
            var statesList = _stateService.GetAll(name);
            if (statesList == null || statesList.Count == 0)
                return NoContent();

            return Ok(statesList);
        }

        [HttpGet("{stateId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetByStateId(int stateId)
        {
            var state = _stateService.GetById(stateId);

            if (state == null)
                return NotFound();

            return Ok(state);
        }
    }
}
