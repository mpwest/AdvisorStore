using AdvisorStore.DataStore;
using Microsoft.AspNetCore.Mvc;

namespace AdvisorStore
{
    [ApiController]
    [Route("[controller]")]
    public class AdvisorController(ILogger<AdvisorController> logger, AdvisorRepository repository) : ControllerBase
    {
        private readonly ILogger<AdvisorController> log = logger;
        private readonly AdvisorRepository db = repository;

        /// <summary>
        /// Retrieves a list of some or all advisors, optionally filtered by a search term
        /// </summary>
        /// <param name="search">Optional search string searching on name, SIN, address and phone</param>
        /// <param name="limit">Optional integer to limit number of records returned for basic pagination</param>
        /// <param name="skip">Optional integer to skip records, typically used with limit</param>
        /// <returns>List of advisor records</returns>
        /// <response code="200"></response>
        [Route("")]
        [HttpGet]
        public IActionResult List(string? search, int? limit, int? skip)
        {
            var defaultLimit = 100;
            var result = db.GetList(search, limit ?? defaultLimit, skip ?? 0).Select(a => Mask(a));

            return Ok(result);
        }

        /// <summary>
        /// Retrieves an advisor record by id
        /// </summary>
        /// <param name="id">Id of advisor</param>
        /// <returns>Advisor record</returns>
        /// <response code="200">If record returned</response>
        /// <response code="404">If not able to be found</response>
        [Route("{id}")]
        [HttpGet]
        public IActionResult Get(string id)
        {
            var result = db.Get(id);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(Mask(result));
        }

        /// <summary>
        /// Creates a new advisor record
        /// </summary>
        /// <param name="advisor">Advisor record with no id or health status required</param>
        /// <returns>Newly created advisor record, including generated id and health status</returns>
        /// <response code="201">If created successfully</response>
        /// <response code="422">If not able to be created</response>
        /// <response code="400">If record is invalid</response>
        [Route("")]
        [HttpPost]
        public IActionResult Post(Advisor advisor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = Mask(db.Create(advisor));
                    return CreatedAtAction(nameof(Get), new { id = advisor.Id}, advisor);
                }
                catch
                {
                    log.LogError(message: "Could not create advisor {Name}", advisor.Name);
                    return UnprocessableEntity();
                }
            }

            return BadRequest();
        }

        /// <summary>
        /// Updates advisor record
        /// </summary>
        /// <param name="advisor">Advisor record with matching id, and newly updated info</param>
        /// <returns>Updated advisor record</returns>
        /// <response code="200">If updated successfully</response>
        /// <response code="422">If not able to be updated</response>
        /// <response code="400">If record is invalid</response>
        [Route("")]
        [HttpPut]
        public IActionResult Put(Advisor advisor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return Ok(Mask(db.Update(advisor)));
                }
                catch
                {
                    log.LogError(message: "Could not update advisor {Name}", advisor.Name);
                    return UnprocessableEntity();
                }
            }

            return BadRequest();
        }

        /// <summary>
        /// Delete the advisor 
        /// </summary>
        /// <param name="id">Advisor id</param>
        /// <returns>No content, http response error code if advisor was not deleted.</returns>
        /// <response code="200">If deleted successfully</response>
        /// <response code="422">If not able to be deleted</response>
        [Route("{id}")]
        [HttpDelete]
        public IActionResult Delete(string id)
        {
            return db.Delete(id) ? NoContent() : UnprocessableEntity();
        }

        private static Advisor Mask(Advisor advisor)
        {
            advisor.SIN = "*********";
            advisor.Phone = "********";
            return advisor;
        }
    }
}
