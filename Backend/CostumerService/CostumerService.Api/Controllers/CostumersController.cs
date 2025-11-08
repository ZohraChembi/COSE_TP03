using CostumerService.Api.Dtos;
using CostumerService.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CostumerService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CostumersController : ControllerBase
    {
        private readonly IUserService _userService;

        public CostumersController(IUserService userService)
        {
            _userService = userService;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _userService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(string id, [FromBody] UserPatchRequest request)
        {
            var result = await _userService.PatchAsync(id, request);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("User updated successfully");
        }


        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userService.DeleteAsync(id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("User deleted successfully");
        }
    }
}

