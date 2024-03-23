using MazeEscape.WebAPI.DTO;
using MazeEscape.WebAPI.Enums;
using MazeEscape.WebAPI.Hypermedia.Definitions;
using MazeEscape.WebAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MazeEscape.WebAPI.Hypermedia
{
    public class HypermediaManager : IHypermediaManager
    {
        
        public HypermediaResponse GetEndpointHypermedia(string endpointName, IUrlHelper url)
        {
            var definitions = EndpointDefinitions.HypermediaDefinitions[endpointName];

            var response = new HypermediaResponse()
            {
                Actions = GetActionsList(definitions.Actions, url),
                Links = GetLinks(definitions.Links, url)
            };

            return response;
        }

        public List<ActionLink> GetActionsList(Dictionary<ActionLinkType, string> actionDefinitions, IUrlHelper urlHelper)
        {
           var actionList = new List<ActionLink>();

           var definitions = actionDefinitions.ToList();

           foreach (var keyValuePair in definitions)
           {
               var action = ActionLinkDefinitions.ActionsMap[keyValuePair.Key];
               action.Href = urlHelper.Action(keyValuePair.Value) + action.QueryParams;

               if (ActionLinkBodyDefinitions.ActionBodyMap.ContainsKey(keyValuePair.Key))
               {
                   action.Body = ActionLinkBodyDefinitions.ActionBodyMap[keyValuePair.Key];
               }
                
               actionList.Add(action);
           }

           return actionList;
        }

        public List<Link> GetLinks(Dictionary<LinkType, string> linkDefinitions, IUrlHelper urlHelper)
        {
            var links = new List<Link>();

            var definitions = linkDefinitions.ToList();

            foreach (var keyValuePair in definitions)
            {
                var link = LinkDefinitions.LinksMap[keyValuePair.Key];
                link.Href = urlHelper.Action(keyValuePair.Value) + link.QueryParams;
                links.Add(link);
            }

            return links;
        }

    }
}
