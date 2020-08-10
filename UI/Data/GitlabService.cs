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

        public async Task<(Project[], PageInfo)> GetProjectsByGroupId(int groupId, int page)
        {
            var response = await gitlabClient.GetProjectsByGroupId(groupId, page);

            return (response.Item1.ToArray(), response.Item2);

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

        public async Task<float> PercentageOfCommitsWithMergeRequests(int projectId)
        {
            var mergerequests = await gitlabClient.GetMergeRequestByProjectId(projectId);

            var commits = await gitlabClient.GetCommitsByProjectId(projectId);

            float commitsWithMr = 0f;
            foreach (var c in commits.Item1)
            {
                if (mergerequests.Item1.Exists(mr => mr.CommitId == c.Id))
                    commitsWithMr++;
            }            

            return commits.Item1.Count > 0 ? (commitsWithMr / commits.Item1.Count) : 0f;

        }
    }
}