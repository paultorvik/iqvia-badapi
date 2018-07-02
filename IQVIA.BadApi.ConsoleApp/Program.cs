using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using IQVIA.BadApi.Client;

namespace IQVIA.BadApi.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input loops to get start date, end date and CSV file path for exported tweets (empty=view tweets)
                DateTime startDate;
                DateTime endDate;
                do
                {
                    startDate = GetStartDate();
                    endDate = GetEndDate();
                    if (endDate <= startDate)
                    {
                        Console.WriteLine("The ending date and time must be after the starting date and time");
                        Console.WriteLine();
                    }
                } while (endDate <= startDate);
                string csvFilePath = GetCsvFilePath();

                // Download tweets and export them to CSV file or view them in console window
                if (!string.IsNullOrWhiteSpace(csvFilePath))
                    ExportTweets(startDate, endDate, csvFilePath);
                else
                    ViewTweets(startDate, endDate);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error:\n{0}", ex);
            }
            finally
            {
                // Terminate after key press
                Console.ReadKey(true);
            }
        }

        private static void ExportTweets(DateTime startDate, DateTime endDate, string csvFilePath)
        {
            // Export tweets to file
            int tweetCount = 0;
            var apiClient = new BadApiClient();
            var stopWatch = new Stopwatch();
            Console.WriteLine("Downloading tweets...");
            Console.WriteLine();
            stopWatch.Start();
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
            Console.WriteLine("Exported {0} tweets in {1} seconds to file:\n{2}", tweetCount, stopWatch.Elapsed.TotalSeconds, csvFilePath);
        }

        private static void ViewTweets(DateTime startDate, DateTime endDate)
        {
            // No CSV path = view tweets one at a time
            var apiClient = new BadApiClient();
            var stopWatch = new Stopwatch();
            Console.WriteLine("Downloading tweets...");
            Console.WriteLine();
            stopWatch.Start();
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

        private static DateTime GetStartDate()
        {
            // Get desired start date, defaulting to test value
            DateTime startDate;
            while (true)
            {
                Console.WriteLine("Enter Starting Date-Time (default={0:o}):", BadApiClient.TestStartDate);
                var startInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(startInput))
                {
                    startDate = BadApiClient.TestStartDate;
                    break;
                }
                else if (DateTime.TryParse(startInput,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out startDate))
                {
                    // Valid date entered
                    Console.WriteLine();
                    break;
                }
                else
                {
                    Console.WriteLine("Please enter a valid date, or no date to use the default");
                    Console.WriteLine();
                }
            }
            return startDate;
        }

        private static DateTime GetEndDate()
        {
            // Get desired end date, defaulting to test value
            DateTime endDate;
            while (true)
            {
                Console.WriteLine("Enter Ending Date-Time (default={0:o}):", BadApiClient.TestEndDate);
                var endInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(endInput))
                {
                    endDate = BadApiClient.TestEndDate;
                    break;
                }
                else if (DateTime.TryParse(endInput,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out endDate))
                {
                    // Valid date entered, adjust end date to last moment of that day if no time of day was provided
                    if (endDate == endDate.Date)
                        endDate = endDate.AddDays(1).AddTicks(-1);
                    Console.WriteLine();
                    break;
                }
                else
                {
                    // Invalid date entered
                    Console.WriteLine("Please enter a valid date, or no date to use the default");
                    Console.WriteLine();
                }
            }
            return endDate;
        }

        private static string GetCsvFilePath()
        {
            // Get path of CSV file where tweets should be exported for review, verification
            string csvFilePath;
            while (true)
            {
                Console.WriteLine("Enter CSV File Path to Export Tweets (none=view tweets):");
                csvFilePath = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(csvFilePath))
                {
                    break;
                }
                else
                {
                    try
                    {
                        // Try to convert entered path to full path, handling any exceptions due to bad characters, length etc.
                        csvFilePath = Path.GetFullPath(csvFilePath);

                        // Ensure that the directory exists
                        var csvDirectoryName = Path.GetDirectoryName(csvFilePath);
                        if (!Directory.Exists(csvDirectoryName))
                        {
                            Console.WriteLine("Please enter path to an existing directory");
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine();
                            break;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Please enter a valid file name and path");
                        Console.WriteLine();
                    }
                }
            }
            return csvFilePath;
        }
    }
}