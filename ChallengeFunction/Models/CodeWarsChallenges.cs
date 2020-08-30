using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ChallengeFunction.Models
{
    public class CodeWarsChallenges
    {
        [JsonProperty("totalPages")]
        public long TotalPages { get; set; }

        [JsonProperty("totalItems")]
        public long TotalItems { get; set; }

        [JsonProperty("data")]
        public List<CodeWarsChallenge> Challenges { get; set; }
    }
    public class CodeWarsChallenge
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("completedLanguages")]
        public List<string> CompletedLanguages { get; set; }

        [JsonProperty("completedAt")]
        public DateTimeOffset CompletedAt { get; set; }
    }
    public partial class CwChallenges
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("publishedAt")]
        public DateTimeOffset PublishedAt { get; set; }

        [JsonProperty("approvedAt")]
        public object ApprovedAt { get; set; }

        [JsonProperty("languages")]
        public List<string> Languages { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("rank")]
        public Rank Rank { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("createdBy")]
        public CreatedBy CreatedBy { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("totalAttempts")]
        public long TotalAttempts { get; set; }

        [JsonProperty("totalCompleted")]
        public long TotalCompleted { get; set; }

        [JsonProperty("totalStars")]
        public long TotalStars { get; set; }

        [JsonProperty("voteScore")]
        public long VoteScore { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("contributorsWanted")]
        public bool ContributorsWanted { get; set; }

        [JsonProperty("unresolved")]
        public Unresolved Unresolved { get; set; }
    }

    public partial class CreatedBy
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

    public partial class Rank
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }
    }

    public partial class Unresolved
    {
        [JsonProperty("issues")]
        public long Issues { get; set; }

        [JsonProperty("suggestions")]
        public long Suggestions { get; set; }
    }
}
