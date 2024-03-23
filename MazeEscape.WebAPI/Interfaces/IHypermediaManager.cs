using MazeEscape.WebAPI.DTO;
using Microsoft.AspNetCore.Mvc;

namespace MazeEscape.WebAPI.Interfaces;

public interface IHypermediaManager
{
    HypermediaResponse GetEndpointHypermedia(string endpointName, IUrlHelper url);
}