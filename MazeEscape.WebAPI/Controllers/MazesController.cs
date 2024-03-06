using MazeEscape.WebAPI.DTO;
using MazeEscape.WebAPI.Enums;
using MazeEscape.WebAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MazeEscape.WebAPI.Controllers
{
    [Route("mazes")]
    [ApiController]
    public class MazesController : ControllerBase
    {
        private readonly IMazeManager _mazeManager;
        private readonly IWebHostEnvironment _environment;


        public MazesController(IMazeManager mazeManager, IWebHostEnvironment environment)
        {
            _mazeManager = mazeManager;
            _environment = environment;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetMazes()
        {
            // todo hypermedia
            return Ok();
        }


        [HttpGet]
        [Route("presets")]
        public IActionResult GetPresets()
        {
            var path = _environment.ContentRootPath;

            var x = _environment.WebRootPath;

            return Ok(_mazeManager.GetPresets(path));
        }

        [HttpPost]
        [Route("")]
        public IActionResult CreateMaze([FromQuery] CreateMode createMode, [FromBody] CreateParams createParams)
        {
            var mazeToken = "";

            try
            {
                mazeToken = _mazeManager.CreateMaze(createMode, createParams);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (FileNotFoundException e)
            {
                return NotFound(e.Message);
            }

            return Created("", new { mazeToken = mazeToken });

        }

  

        [HttpPost]
        [Route("player")]
        public IActionResult MovePlayer([FromQuery] PlayerMove playerMove, [FromBody] MazeState? mazeState)
        {
            PlayerInfo? playerInfo = null;
            try
            {
                playerInfo = _mazeManager.GetPlayerInfo(mazeState);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }


            return Ok(playerInfo);
        }
    }
}
