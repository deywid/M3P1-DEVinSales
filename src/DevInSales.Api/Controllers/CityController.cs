using DevInSales.Api.Dtos;
using DevInSales.Core.Entities;
using DevInSales.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevInSales.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly IStateService _stateService;
        private readonly ICityService _cityService;

        public CityController(IStateService stateService, ICityService cityService)
        {
            _stateService = stateService;
            _cityService = cityService;
        }

        [HttpGet("/api/State/{stateId}/city")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetCityByStateId(int stateId, string? name)
        {
            var state = _stateService.GetById(stateId);
            if (state == null)
                return NotFound();

            var citiesList = _cityService.GetAll(stateId, name);
            if (citiesList == null || citiesList.Count == 0)
                return NoContent();

            return Ok(citiesList);
        }

        [HttpGet("/api/State/{stateId}/city/{cityId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetCityById(int stateId, int cityId)
        {
            var state = _stateService.GetById(stateId);
            if (state == null)
                return NotFound();

            var city = _cityService.GetById(cityId);
            if (city == null)
                return NotFound();

            if (state.Id != city.State.Id)
                return BadRequest();

            return Ok(city);
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost("/api/State/{stateId}/city")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult AddCity(int stateId, AddCity model)
        {
            var state = _stateService.GetById(stateId);
            if (state == null)
                return NotFound();

            var city = _cityService.GetAll(stateId, model.Name);
            if (city != null && city.Count > 0)
                return BadRequest();

            var newCity = new City(stateId, model.Name);
            _cityService.Add(newCity);

            return CreatedAtAction(
                nameof(GetCityById),
                new { stateId, cityId = newCity.Id },
                newCity.Id
            );
        }
    }
}
