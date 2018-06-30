using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IQVIA.BadApi.Client;
using IQVIA.BadApi.Client.Models;

namespace IQVIA.BadApi.Tests
{
    /// <summary>
    /// Test that the known, required 2-year range of tweets are retrieved properly
    /// </summary>
    [TestClass]
    public class RequiredTweetsTest
    {
        private static List<Tweet> requiredTweets;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var badApiClient = new BadApiClient();
            requiredTweets = badApiClient.GetAllTweetsAsync(BadApiClient.TestStartDate, BadApiClient.TestEndDate).Result;
        }

        /// <summary>
        /// Test that at least some tweets were found.
        /// </summary>
        [TestMethod]
        public void SomeTweetsAreFound()
        {
            Assert.IsNotNull(requiredTweets, "Tweets list is null");
            Assert.IsTrue(requiredTweets.Any(), "No tweets were found");
        }

        [TestMethod]
        public void AllTweetsHaveID()
        {
            if (requiredTweets == null)
                return;
            var firstTweetWithNoID = requiredTweets.FirstOrDefault(t => string.IsNullOrWhiteSpace(t.ID));
            Assert.IsNull(firstTweetWithNoID, "Tweet has no ID: first Stamp={0}", firstTweetWithNoID?.Stamp);
        }

        [TestMethod]
        public void AllTweetsHaveStampInRange()
        {
            if (requiredTweets == null)
                return;
            var firstTweetWithBadStamp = requiredTweets.FirstOrDefault(t => t.Stamp < BadApiClient.TestStartDate || t.Stamp > BadApiClient.TestEndDate);
            Assert.IsNull(firstTweetWithBadStamp, "Tweet stamp out of range: first ID={0}, Stamp={1}", firstTweetWithBadStamp?.ID, firstTweetWithBadStamp?.Stamp);
        }

        [TestMethod]
        public void AllTweetsHaveText()
        {
            if (requiredTweets == null)
                return;
            var firstTweetWithNoText = requiredTweets.FirstOrDefault(t => string.IsNullOrWhiteSpace(t.Text));
            Assert.IsNull(firstTweetWithNoText, "Empty tweet text: first ID={0}, Stamp={1}", firstTweetWithNoText?.ID, firstTweetWithNoText?.Stamp);
        }

        [TestMethod]
        public void AllTweetsHaveUniqueID()
        {
            if (requiredTweets == null)
                return;
            var tweetIdHashSet = new HashSet<string>();
            foreach (var tweet in requiredTweets)
            {
                if (!tweetIdHashSet.Add(tweet.ID))
                    Assert.Fail("Duplicate tweet ID(s) found: first ID={0}, Stamp={1}", tweet.ID, tweet.Stamp);
            }
        }
    }
}
