using MazeEscape.WebAPI.DTO;
using MazeEscape.WebAPI.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MazeEscape.WebAPI.Controllers
{
    [Route("mazes")]
    [ApiController]
    public class MazesController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public IActionResult GetMazes()
        {
            return Ok();
        }


        [HttpGet]
        [Route("presets")]
        public IActionResult GetPresets()
        {
            var presets = new List<string>
            {
                "",
            };

            return Ok(presets);
        }

        [HttpPost]
        [Route("")]
        public IActionResult CreateMaze([FromQuery] CreateMode createMode, [FromBody] CreateParams createParams)
        {

            return Ok();
        }

        [HttpGet]
        [Route("player")]
        public IActionResult GetPlayer()
        {
            return Ok();
        }

        [HttpPost]
        [Route("player")]
        public IActionResult MovePlayer([FromQuery] PlayerMove playerMove)
        {
            return Ok();
        }
    }
}
