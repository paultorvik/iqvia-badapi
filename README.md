# IQVIA "Bad API" Take-home Problem Solution

The main console application prompts for start and end date-times (UTC) and also prompts for a file name where it can export the downloaded Tweets from [IQVIA's "bad" API](https://badapi.iqvia.io/swagger/). By default, the dates span 2016 to 2017.

## Prerequisites
- [Visual Studio 2017 Community edition or higher](https://www.visualstudio.com/downloads/)

## Visual Studion Solution Structure
My Visual Studio 2017 solution to IQVIA's "Bad API" take-home problem consists of the following .NET Core 2.0 projects:

- [IQVIA.BadApi.Client](https://github.com/paultorvik/iqvia-badapi/tree/master/IQVIA.BadApi.Client) - REST API client library for IQVIA's "bad API", which also accounts for lack of paging
- [IQVIA.BadApi.ConsoleApp](https://github.com/paultorvik/iqvia-badapi/tree/master/IQVIA.BadApi.ConsoleApp) - Console application that uses the API client to download a requested date range of tweets
- [IQVIA.BadApi.Tests](https://github.com/paultorvik/iqvia-badapi/tree/master/IQVIA.BadApi.Tests) - Tests for the "bad API", focused on the 2-year range of known, available tweets

## To Run the Code

1. Clone this repo locally.
2. Navigate to the root folder and double click the **.sln** file to open the project in Visual Studio 2017.
3. Press **F5** to launch the Console app.
4. Respond to the prompts for start date, end date and whether export the Tweet records to a CSV file or to the window.
5. If a CSV file path was entered, go and find the CSV file and review the contents.
Otherwise, the tweets are output to the console window.

## TODO:
- [ ] Docker
- [ ] Consider other client like web site (ASP.NET Core MVC)
- [ ] Possibly more error handling
