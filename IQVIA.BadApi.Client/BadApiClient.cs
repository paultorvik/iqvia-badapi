using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using IQVIA.BadApi.Client.Models;

namespace IQVIA.BadApi.Client
{
    /// <summary>
    /// REST API client that can download tweets from IQVIA's "bad API"
    /// </summary>
    public class BadApiClient
    {
        public const string BaseAddress = "https://badapi.iqvia.io/";

        /// <summary>
        /// Start date for testing the required range of tweets
        /// </summary>
        public readonly static DateTime TestStartDate = DateTime.Parse("2016-01-01T00:00:00.0000000Z", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

        /// <summary>
        /// End date for testing the required range of tweets
        /// </summary>
        public readonly static DateTime TestEndDate = DateTime.Parse("2017-12-31T23:59:59.9999999Z", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

        private const int TweetPageSize = 100;

        private readonly HttpClient _client = new HttpClient();

        /// <summary>
        /// Instantiates API client, with base address configurable to allow for separate deployment environments
        /// </summary>
        /// <param name="baseAddress"></param>
        public BadApiClient(string baseAddress = BaseAddress) : this(new Uri(baseAddress))
        {
        }

        /// <summary>
        /// Instantiates API client, with base address configurable to allow for separate deployment environments
        /// </summary>
        /// <param name="baseAddress"></param>
        public BadApiClient(Uri baseAddress)
        {
            _client.BaseAddress = baseAddress;
        }

        /// <summary>
        /// Requests the tweets in the specified date range, which may be only the first 100 records in that range, ordered by Stamp date.
        /// </summary>
        /// <param name="startDate">Start date (UTC), inclusive</param>
        /// <param name="endDate">End date (UTC), inclusive</param>
        /// <returns>All or a page of tweets in the specified date range</returns>
        private async Task<List<Tweet>> GetTweetsAsync(DateTime startDate, DateTime endDate)
        {
            var url = $"/api/v1/Tweets?startDate={startDate.ToUniversalTime():o}&endDate={endDate.ToUniversalTime():o}";
            var jsonStream = await _client.GetStreamAsync(url);
            var serializer = new DataContractJsonSerializer(typeof(List<Tweet>));
            var tweets = (List<Tweet>)serializer.ReadObject(jsonStream);
            return tweets;
        }

        /// <summary>
        /// Gets all tweats in the specified UTC date range.
        /// Accounts for paging by querying after the last time stamp in each page for the next page of tweets.
        /// This takes advantage of the fact that the tweets are sorted by Stamp date for paging purposes already.
        /// </summary>
        /// <param name="startDate">Start date (UTC), inclusive</param>
        /// <param name="endDate">End date (UTC), inclusive</param>
        /// <returns>All tweets in the specified date range</returns>
        public async Task<List<Tweet>> GetAllTweetsAsync(DateTime startDate, DateTime endDate)
        {
            var allTweets = new List<Tweet>();
            var nextStartDate = startDate;
            List<Tweet> nextTweets;
            do
            {
                nextTweets = await GetTweetsAsync(nextStartDate, endDate);
                if (nextTweets.Count > 0)
                {
                    allTweets.AddRange(nextTweets);
                    nextStartDate = nextTweets[nextTweets.Count - 1].Stamp.AddTicks(1);
                }
            }
            while (nextTweets.Count >= TweetPageSize);
            return allTweets;
        }

        /// <summary>
        /// Returns all tweats in the specified UTC date range.
        /// Accounts for paging by querying after the last time stamp in each page for the next page of tweets.
        /// This takes advantage of the fact that the tweets are sorted by Stamp date for paging purposes already.
        /// </summary>
        /// <param name="startDate">Start date (UTC), inclusive</param>
        /// <param name="endDate">End date (UTC), inclusive</param>
        /// <returns>All tweets in the specified date range</returns>
        public IEnumerable<Tweet> GetAllTweets(DateTime startDate, DateTime endDate)
        {
            var nextStartDate = startDate;
            List<Tweet> nextTweets;
            do
            {
                nextTweets = GetTweetsAsync(nextStartDate, endDate).Result;
                if (nextTweets.Count > 0)
                {
                    foreach (var tweet in nextTweets)
                        yield return tweet;
                    nextStartDate = nextTweets[nextTweets.Count - 1].Stamp.AddTicks(1);
                }
            }
            while (nextTweets.Count >= TweetPageSize);
        }
    }
}
