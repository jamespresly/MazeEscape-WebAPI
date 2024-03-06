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
        private readonly IConfiguration _configuration;


        public MazesController(IMazeManager mazeManager, IWebHostEnvironment environment, IConfiguration configuration)
        {
            _mazeManager = mazeManager;
            _environment = environment;
            _configuration = configuration;
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
            var path = _environment.ContentRootPath + _configuration["PresetsPath"];

            var presets = _mazeManager.GetPresets(path);

            return Ok(presets);
        }

        [HttpPost]
        [Route("")]
        public IActionResult CreateMaze([FromQuery] CreateMode createMode, [FromBody] CreateParams createParams)
        {
            var mazeToken = "";

            try
            {
                var path = _environment.ContentRootPath + _configuration["PresetsPath"];
                var key = _configuration["MazeEncryptionKey"];

                mazeToken = _mazeManager.CreateMaze(createMode, createParams, key, path);
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
