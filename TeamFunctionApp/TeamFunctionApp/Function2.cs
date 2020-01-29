using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TeamFunctionApp
{
    public static class GetTeamMatch
    {

        private class TeamMember
        {
            public int branch { get; set; }
            public int position { get; set; }
            public string partner { get; set; }

            public static List<TeamMember> LoadTeam()
            {
                return new List<TeamMember>() {
                    new TeamMember { branch = 622, position = 1, partner = "Marcus Morris Sr." },
                    new TeamMember { branch = 622, position = 1, partner = "RJ Barrett" },
                    new TeamMember { branch = 622, position = 1, partner = "Elfrid Payton" },
                    new TeamMember { branch = 622, position = 1, partner = "Reggie Bullock" },
                    new TeamMember { branch = 622, position = 2, partner = "Mitchell Robinson" },
                    new TeamMember { branch = 622, position = 2, partner = "RJ Barrett" },
                    new TeamMember { branch = 622, position = 2, partner = "Elfrid Payton" },
                    new TeamMember { branch = 622, position = 2, partner = "Reggie Bullock" },
                    new TeamMember { branch = 622, position = 3, partner = "Mitchell Robinson" },
                    new TeamMember { branch = 622, position = 3, partner = "Marcus Morris Sr." },
                    new TeamMember { branch = 622, position = 3, partner = "Elfrid Payton" },
                    new TeamMember { branch = 622, position = 3, partner = "Reggie Bullock" },
                    new TeamMember { branch = 622, position = 4, partner = "Mitchell Robinson" },
                    new TeamMember { branch = 622, position = 4, partner = "Marcus Morris Sr." },
                    new TeamMember { branch = 622, position = 4, partner = "RJ Barrett" },
                    new TeamMember { branch = 622, position = 4, partner = "Reggie Bullock" },
                    new TeamMember { branch = 622, position = 5, partner = "Mitchell Robinson" },
                    new TeamMember { branch = 622, position = 5, partner = "Marcus Morris Sr." },
                    new TeamMember { branch = 622, position = 5, partner = "RJ Barrett" },
                    new TeamMember { branch = 622, position = 5, partner = "Elfrid Payton" },
                };


            }
            public static List<TeamMember> FindMember(int branch, int position, List<TeamMember> team)
            {
                List<TeamMember> suggested = new List<TeamMember>();
                foreach(TeamMember tm in team ){
                    if(tm.branch == branch && tm.position == position){
                        suggested.Add(tm);
                    }
                        
                }
                if (suggested.Count > 0)
                    return suggested;
                else
                    return null;

            }
        }
        [FunctionName("GetTeamMatch")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            string branch = req.Query["branch"];
            string position = req.Query["position"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;

            List<TeamMember> team = TeamMember.FindMember(Convert.ToInt32(branch), Convert.ToInt32(position), TeamMember.LoadTeam());
            if(team != null && team.Count > 0)
                return new OkObjectResult(team);
            else
                return new BadRequestObjectResult("No suggested team match found");
        }
    }
}
