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
            public string Team { get; set; }
            public int branch { get; set; }
            public int City { get; set; }
            public string Specialty { get; set; }
            public int specialtyNo { get; set; }
            public string Name { get; set; }

            public static List<TeamMember> LoadTeam()
            {
                return new List<TeamMember>() {
                    new TeamMember { Team = "Team 1", branch = 622, specialtyNo = 1, Specialty = "Insurance", Name = "Dinesh" },
                    new TeamMember { Team = "Team 1", branch = 622, specialtyNo = 2, Specialty = "Annuity", Name = "Kai" },
                    new TeamMember { Team = "Team 2", branch = 622, specialtyNo = 3, Specialty = "Alternative Investment", Name = "Jason" },
                    new TeamMember { Team = "Team 2", branch = 622, specialtyNo = 4, Specialty = "UIT", Name = "Frank" },
                    new TeamMember { Team = "Team 3", branch = 658, specialtyNo = 1, Specialty = "Insurance", Name = "Vivian" },
                    new TeamMember { Team = "Team 3", branch = 658, specialtyNo = 2, Specialty = "Annuity", Name = "Abhishek" },
                    new TeamMember { Team = "Team 4", branch = 658, specialtyNo = 3, Specialty = "Alternative Investment", Name = "Jolly" },
                    new TeamMember { Team = "Team 4", branch = 658, specialtyNo = 4, Specialty = "UIT", Name = "Luz" },
                    new TeamMember { Team = "Team 5", branch = 358, specialtyNo = 1, Specialty = "Insurance", Name = "Melanie" },
                    new TeamMember { Team = "Team 1", branch = 358, specialtyNo = 3, Specialty = "Alternative Investment", Name = "Jeff" },
                    new TeamMember { Team = "Team 1", branch = 358, specialtyNo = 4, Specialty = "UIT", Name = "Kevin" }
                };


            }
            public static List<TeamMember> FindMember(int branch, int specialtyNo, List<TeamMember> team)
            {
                List<TeamMember> suggested = new List<TeamMember>();
                foreach(TeamMember tm in team ){
                    if(tm.branch == branch && tm.specialtyNo == specialtyNo){
                        suggested.Add(tm);
                    }
                        
                }
                if (suggested.Count > 0)
                    return suggested;
                else
                    return null;

            }

            public static List<TeamMember> FindMemberbyName(string name, List<TeamMember> team)
            {
                List<TeamMember> suggested = new List<TeamMember>();
                foreach (TeamMember tm in team)
                {
                    if (tm.Name == name)
                    {
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

            string name = req.Query["name"];
            //string branch = req.Query["branch"];
            //string specialtyNo = req.Query["specialtyNo"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;

            //List<TeamMember> team = TeamMember.FindMember(Convert.ToInt32(branch), Convert.ToInt32(specialtyNo), TeamMember.LoadTeam());
            List<TeamMember> team = TeamMember.FindMemberbyName(name, TeamMember.LoadTeam());
            if (team != null && team.Count > 0)
                return new OkObjectResult(team);
            else
                return new BadRequestObjectResult("No suggested team match found");
        }
    }
}
