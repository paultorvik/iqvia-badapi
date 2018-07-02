using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IQVIA.BadApi.Client;

namespace IQVIA.BadApi.Tests
{
    [TestClass]
    public class OtherTweetTests
    {
        /// <summary>
        /// Test case expecting no returned tweets
        /// </summary>
        [TestMethod]
        public void NoTweets()
        {
            var badApiClient = new BadApiClient();
            var tweets = badApiClient.GetAllTweetsAsync(
                DateTime.MinValue.ToUniversalTime(),
                DateTime.MinValue.ToUniversalTime()).Result;
            Assert.IsNotNull(tweets, "Tweets list is null");
            Assert.IsFalse(tweets.Any(), "Not expecting any tweets to be returned");
        }
    }
}
