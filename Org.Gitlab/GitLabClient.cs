﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Org.Gitlab.Models;
using System.Collections;
using Microsoft.Extensions.Options;

namespace Org.Gitlab
{
    public class GitLabClient
    {
        private readonly string GitlabBaseUrl;
        private readonly HttpClient _client;
        private readonly PageInfo _pageInfo;

        public GitLabClient(HttpClient client, PageInfo pInf, IOptions<GitlabServiceOptions> options)
        {
            _client = client;
            _pageInfo = pInf;
            GitlabBaseUrl = options.Value.GitlabBaseUrl;
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {options.Value.AccessToken}");
        }

        private async Task<List<Job>> GetJobsByProjectId(int projectId)
        {
            var streamTask = _client.GetStreamAsync($"{GitlabBaseUrl}/projects/{projectId}/jobs?per_page=10&sort=desc");
            var jobs = await JsonSerializer.DeserializeAsync<List<Job>>(await streamTask);

            return jobs;
        }

        public async Task<(List<Project>, PageInfo)> GetProjectsByGroupId(int groupId, int page)
        {
            var response = await _client.GetAsync($"{GitlabBaseUrl}/groups/{groupId}/projects?per_page=10&include_subgroups=true&page={page}");
            var projects = await JsonSerializer.DeserializeAsync<List<Project>>(await response.Content.ReadAsStreamAsync());

            _pageInfo.PopulatePageInfoFromHeader(response.Headers.ToDictionary(k => k.Key, v => string.Join("", v.Value)));

            return (projects, _pageInfo);
        }

        public async Task<(List<Project>, PageInfo)> GetSubgroupsByGroupId(int groupId, int page)
        {
            var response = await _client.GetAsync($"{GitlabBaseUrl}/groups/{groupId}/subgroups?per_page=10&include_subgroups=true&page={page}");

            var projects = await JsonSerializer.DeserializeAsync<List<Project>>(await response.Content.ReadAsStreamAsync());

            _pageInfo.PopulatePageInfoFromHeader(response.Headers.ToDictionary(k => k.Key, v => string.Join("", v.Value)));

            return (projects, _pageInfo);
        }

        public async Task<bool> IsCheckmarxImplemented(int projectId)
        {
            var jobs = await GetJobsByProjectId(projectId);

            return jobs.FirstOrDefault(j => j.Name.Contains("checkmarx")) != null;
        }

        public async Task<(List<Commit>, PageInfo)> GetCommitsByProjectId(int projectId)
        {
            var response = await _client.GetAsync($"{GitlabBaseUrl}/projects/{projectId}/repository/commits?sort=desc&per_page=50&all=true&page=1");

            var commits = await JsonSerializer.DeserializeAsync<List<Commit>>(await response.Content.ReadAsStreamAsync());

            _pageInfo.PopulatePageInfoFromHeader(response.Headers.ToDictionary(k => k.Key, v => string.Join("", v.Value)));

            return (commits, _pageInfo);
        }

        public async Task<(List<MergeRequest>, PageInfo)> GetMergeRequestByProjectId(int projectId)
        {
            var response = await _client.GetAsync($"{GitlabBaseUrl}/projects/{projectId}/merge_requests?sort=desc&per_page=20&all=true&page=1");

            var mergeRequests = await JsonSerializer.DeserializeAsync<List<MergeRequest>>(await response.Content.ReadAsStreamAsync());

            _pageInfo.PopulatePageInfoFromHeader(response.Headers.ToDictionary(k => k.Key, v => string.Join("", v.Value)));

            return (mergeRequests, _pageInfo);
        }

    }
}
