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
        private readonly IMazeAppManager _mazeAppManager;
        private readonly IHypermediaManager _hypermediaManager;

        public MazesController(IMazeAppManager mazeAppManager, IHypermediaManager hypermediaManager)
        {
            _mazeAppManager = mazeAppManager;
            _hypermediaManager = hypermediaManager;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetMazes()
        {
            var response = _hypermediaManager.GetEndpointHypermedia(nameof(GetMazes), Url);

            return Ok(response);
        }

        [HttpGet]
        [Route("presets")]
        public IActionResult GetPresets()
        {
            var response = _hypermediaManager.GetEndpointHypermedia(nameof(GetPresets), Url);

            response.Data = _mazeAppManager.GetPresets();

            return Ok(response);
        }

        [HttpPost]
        [Route("")]
        public IActionResult CreateMaze([FromBody] CreateParams createParams)
        {
            var response = _hypermediaManager.GetEndpointHypermedia(nameof(CreateMaze), Url);

            try
            {
                response.Data = _mazeAppManager.CreateMaze(createParams);
            }
            catch (ArgumentException e)
            {
                response = _hypermediaManager.GetEndpointHypermedia(nameof(GetMazes), Url);
                response.Error = e.Message;

                return BadRequest(response);
            }
            catch (FileNotFoundException e)
            {
                response = _hypermediaManager.GetEndpointHypermedia(nameof(GetMazes), Url);
                response.Error = e.Message;
                
                return NotFound(response);
            }

            return Created("", response);
        }

        [HttpPost]
        [Route("player")]
        public IActionResult PostPlayer([FromBody] PlayerParams playerParams)
        {
            var response = _hypermediaManager.GetEndpointHypermedia(nameof(PostPlayer), Url);

            try
            {
                var playerInfo = _mazeAppManager.GetPlayerInfo(playerParams);

                if (playerInfo.IsEscaped)
                    response.Actions = null;

                response.Data = playerInfo;
            }
            catch (ArgumentException e)
            {
                response.Error = e.Message;
                return BadRequest(response);
            }

            return Ok(response);
        }

    
    }
}
