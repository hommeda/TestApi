using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resturant.Core;
using System.Security.Claims;

namespace Resturant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LookUpController : ControllerBase
    {
        #region Private Members

        private readonly ILookUpService _iface;

        #endregion

        #region Public Members
        public string LoggedInUserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        #endregion

        #region Contsructors

        public LookUpController(ILookUpService iface)
        {
            _iface = iface;
        }

        #endregion

        #region HttpGet

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(await _iface.Get(id));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _iface.Get());
        }

        #endregion

        #region HttpPost
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] LookUpVm vm)
        {
            vm.LoggedInUserId = long.Parse(LoggedInUserId);
            return Ok(await _iface.Save(vm));
        }
        #endregion

        #region HttpDelete
        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Remove(long id)
        {
            return Ok(await _iface.Remove(id));
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            return Ok(await _iface.Delete(id));
        }
        #endregion
    }
}
