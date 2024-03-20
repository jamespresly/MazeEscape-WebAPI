using MazeEscape.WebAPI.DTO;
using MazeEscape.WebAPI.Enums;
using MazeEscape.WebAPI.Hypermedia;
using MazeEscape.WebAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MazeEscape.WebAPI.Controllers
{
    [Route("mazes")]
    [ApiController]
    public class MazesController : ControllerBase
    {
        private readonly IMazeAppManager _mazeAppManager;

        public MazesController(IMazeAppManager mazeAppManager)
        {
            _mazeAppManager = mazeAppManager;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetMazes()
        {
            var response = new HypermediaResponse()
            {
                Actions = new List<Link>()
                {
                    CreateLink(LinkType.CreatePresetMaze, nameof(CreateMaze)),
                    CreateLink(LinkType.CreateCustomMaze, nameof(CreateMaze)),
                    CreateLink(LinkType.CreateRandomMaze, nameof(CreateMaze)),
                },
                Links = new List<Link>
                {
                    CreateLink(LinkType.GetMazeRoot, nameof(GetMazes)),
                    CreateLink(LinkType.GetPresetsList, nameof(GetPresets))
                }
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("presets")]
        public IActionResult GetPresets()
        {
            var response = new HypermediaResponse()
            {
                Actions = new List<Link>()
                {
                    CreateLink(LinkType.CreatePresetMaze, nameof(CreateMaze))
                },
                Links = new List<Link>
                {
                    CreateLink(LinkType.GetMazeRoot, nameof(GetMazes)),
                }

            };

            var presets = _mazeAppManager.GetPresets();

            response.Data = presets;

            return Ok(response);
        }

        [HttpPost]
        [Route("")]
        public IActionResult CreateMaze([FromQuery] CreateMode createMode, [FromBody] CreateParams createParams)
        {
            var response = new HypermediaResponse()
            {
                Actions = new List<Link>()
                {
                    CreateLink(LinkType.PostPlayer, nameof(PostPlayer))
                },
                Links = new List<Link>
                {
                    CreateLink(LinkType.GetMazeRoot, nameof(GetMazes))
                }
            };

            var mazeToken = "";

            try
            {
                mazeToken = _mazeAppManager.CreateMaze(createMode, createParams);
            }
            catch (ArgumentException e)
            {
                response.Error = e.Message;
                response.Actions = new List<Link>()
                {
                    CreateLink(LinkType.CreatePresetMaze, nameof(CreateMaze))
                };
                return BadRequest(response);
            }
            catch (FileNotFoundException e)
            {
                response.Error = e.Message;
                response.Actions = new List<Link>()
                {
                    CreateLink(LinkType.CreatePresetMaze, nameof(CreateMaze))
                };
                return NotFound(response);
            }
            
            response.Data = new { mazeToken = mazeToken };

            return Created("", response);

        }



        [HttpPost]
        [Route("player")]
        public IActionResult PostPlayer([FromQuery] PlayerMove? playerMove, [FromBody] MazeState mazeState)
        {
            var response = new HypermediaResponse()
            {
                Actions = new List<Link>()
                {
                    CreateLink(LinkType.PostPlayer, nameof(PostPlayer)),
                    CreateLink(LinkType.PlayerTurnLeft, nameof(PostPlayer)),
                    CreateLink(LinkType.PlayerTurnRight, nameof(PostPlayer)),
                    CreateLink(LinkType.PlayerMoveForward, nameof(PostPlayer)),
                },
                Links = new List<Link>
                {
                    CreateLink(LinkType.GetMazeRoot, nameof(GetMazes))
                }
            };

            PlayerInfo? playerInfo = null;
            try
            {
                playerInfo = _mazeAppManager.GetPlayerInfo(mazeState, playerMove);

                if (playerInfo.IsEscaped)
                    response.Actions = null;

            }
            catch (ArgumentException e)
            {
                response.Error = e.Message;
                return BadRequest(response);
            }

            response.Data = playerInfo;

            return Ok(response);
        }

        private Link CreateLink(LinkType linkType, string name, object? values = null)
        {
            var link = HypermediaDefinitions.LinksMap[linkType];
            link.Href = Url.Action(name, values) + link.QueryParams;

            if (HypermediaDefinitions.BodyMap.ContainsKey(linkType))
            {
                link.Body = HypermediaDefinitions.BodyMap[linkType];
            }

            return link;
        }
    }
}
