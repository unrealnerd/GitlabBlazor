using System;
using System.Linq;
using System.Threading.Tasks;
using Org.Gitlab;
using Org.Gitlab.Models;

namespace Inspector.Data
{
    public class GitlabService
    {
        public readonly GitLabClient gitlabClient;

        public GitlabService(GitLabClient c)
        {
            gitlabClient = c;
        }

        public async Task<Project[]> GetProjectsByGroupId(int groupId)
        {
            var projects = await gitlabClient.GetProjectsByGroupId(groupId);

            return projects.ToArray();

        }

        public async Task<(Project[], PageInfo)> GetSubGroups(int groupId, int page)
        {
            var response = await gitlabClient.GetSubgroupsByGroupId(groupId, page);

            return (response.Item1.ToArray(), response.Item2);

        }

        public async Task<bool> IsCheckmarxImplemented(int projectId)
        {
            var result = await gitlabClient.IsCheckmarxImplemented(projectId);

            return result;

        }
    }
}