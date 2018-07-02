using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using IQVIA.BadApi.Client;
using IQVIA.BadApi.Client.Models;

namespace IQVIA.BadApi.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get desired start date, defaulting to test value
            Console.Write("Enter Start Date (default={0:o}): ", BadApiClient.TestStartDate);
            var startInput = Console.ReadLine();
            DateTime startDate;
            if (!DateTime.TryParse(startInput,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out startDate))
            {
                startDate = BadApiClient.TestStartDate;
            }

            // Get desired end date, defaulting to test value
            Console.Write("Enter End Date (default={0:o}): ", BadApiClient.TestEndDate);
            var endInput = Console.ReadLine();
            DateTime endDate;
            if (!DateTime.TryParse(endInput,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out endDate))
            {
                endDate = BadApiClient.TestEndDate;
            }

            // Adjust end date to last moment of that day if no time of day was provided
            if (endDate == endDate.Date)
                endDate = endDate.AddDays(1).AddTicks(-1);

            // Get path of CSV file where tweets should be exported for review, verification
            Console.Write("Enter CSV File Path to Export Tweets (none=view tweets): ");
            var csvFilePath = Console.ReadLine();

            Console.WriteLine();

            // Download and write tweets to file or console
            var apiClient = new BadApiClient();
            var stopWatch = new Stopwatch();
            Console.WriteLine("Downloading tweets...");
            stopWatch.Start();
            if (!string.IsNullOrWhiteSpace(csvFilePath))
            {
                int tweetCount = 0;
                using (var csvStream = File.Open(csvFilePath, FileMode.Create, FileAccess.Write))
                {
                    using (var csvWriter = new StreamWriter(csvStream, System.Text.Encoding.UTF8))
                    {
                        csvWriter.WriteLine("\"ID\",\"Stamp\",\"Text\"");
                        foreach (var tweet in apiClient.GetAllTweets(startDate, endDate))
                        {
                            csvWriter.WriteLine("\"{0}\",\"{1:o}\",\"{2}\"", tweet.ID, tweet.Stamp, tweet.Text.Replace("\"", "\"\""));
                            tweetCount++;
                        }
                        csvWriter.Close();
                    }
                }
                stopWatch.Stop();
                Console.WriteLine("Exported {0} tweets in {1} seconds", tweetCount, stopWatch.Elapsed.TotalSeconds);
            }
            else
            {
                var tweets = apiClient.GetAllTweetsAsync(startDate, endDate).Result;
                stopWatch.Stop();
                Console.WriteLine("Downloaded {0} tweets in {1} seconds", tweets.Count, stopWatch.Elapsed.TotalSeconds);
                Console.WriteLine();
                Console.WriteLine("Press a key to see next tweet. Esc to stop pausing between tweets.");
                bool waitForKeyBetweenTweets = true;
                foreach (var tweet in tweets)
                {
                    Console.WriteLine();
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("ID:\t{0}", tweet.ID);
                    Console.WriteLine("Stamp:\t{0:o}", tweet.Stamp);
                    Console.ResetColor();
                    Console.WriteLine(tweet.Text);
                    if (waitForKeyBetweenTweets || Console.KeyAvailable)
                    {
                        var keyInfo = Console.ReadKey(true);
                        waitForKeyBetweenTweets = (keyInfo.Key != ConsoleKey.Escape);
                    }
                }
            }

            // Terminate after key press
            Console.ReadKey(true);
        }
    }
}