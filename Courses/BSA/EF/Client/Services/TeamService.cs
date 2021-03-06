using System.Collections.Generic;
using System.Net.Http;
using System.Text;

using BusinessLayer.Commands;

using Core.DataTransferObjects.Team;

using DataAccessLayer.Entities;

using Newtonsoft.Json;

namespace Client.Services
{
    public class TeamService : ApiServiceBase, Interfaces.ITeamService
    {
        // CONSTRUCTORS
        public TeamService(HttpClient client)
            : base(client) { }
        
        // METHODS
        public IEnumerable<Team> Get()
        {
            string url = GenerateUrl(@"api/teams");

            return JsonConvert.DeserializeObject<IEnumerable<Team>>(client.GetStringAsync(url).Result);
        }

        public IEnumerable<BusinessLayer.DataTransferObjects.TeamUsersDTO> GetTeamByAgeLimit(int participantsAge = 9)
        {
            string url = GenerateUrl($@"api/teams/limited/?participantAge={participantsAge}");

            return JsonConvert.DeserializeObject<IEnumerable<BusinessLayer.DataTransferObjects.TeamUsersDTO>>(client.GetStringAsync(url).Result);
        }

        public IDictionary<int, int> GetTeamUserAmount()
        {
            string url = GenerateUrl($@"api/teams/participants_amount/");

            return JsonConvert.DeserializeObject<IDictionary<int, int>>(client.GetStringAsync(url).Result);
        }

        public CommandResponse RenameTeam(RenameTeamDTO value)
        {
            if (value == null) throw new System.ArgumentNullException(nameof(value));

            string url = GenerateUrl(@"api/teams");
            StringContent content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");

            // result
            return GenerateResponse(client.PatchAsync(url, content).Result);
        }

        public CommandResponse DeleteTeam(int id)
        {
            string url = GenerateUrl($@"api/teams/{id}");

            // result
            return GenerateResponse(client.DeleteAsync(url).Result);
        }

        public CommandResponse CreateTeam(CreateTeamDTO createTeamDTO)
        {
            if (createTeamDTO == null) throw new System.ArgumentNullException(nameof(createTeamDTO));

            string url =  GenerateUrl($@"api/teams");
            StringContent content = new StringContent(JsonConvert.SerializeObject(createTeamDTO), Encoding.UTF8, "application/json");

            // result
            return GenerateResponse(client.PostAsync(url, content).Result);
        }
    }
}
