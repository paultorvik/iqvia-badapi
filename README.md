# IQVIA "Bad API" Take-home Problem Solution

The main console application prompts for start and end date-times (UTC) and also prompts for a file name where it can export the downloaded Tweets from [IQVIA's "bad" API](https://badapi.iqvia.io/swagger/). By default, the dates span all of 2016 and 2017.

## Prerequisites
- [Visual Studio 2017 Community edition or higher](https://www.visualstudio.com/downloads/)

## Solution Structure
My Visual Studio 2017 solution to IQVIA's "Bad API" take-home problem consists of the following .NET Core 2.0 projects:

- [IQVIA.BadApi.Client](https://github.com/paultorvik/iqvia-badapi/tree/master/IQVIA.BadApi.Client) - REST API client library for IQVIA's "bad API", which also accounts for lack of paging
- [IQVIA.BadApi.ConsoleApp](https://github.com/paultorvik/iqvia-badapi/tree/master/IQVIA.BadApi.ConsoleApp) - Console application that uses the API client to download a requested date range of tweets
- [IQVIA.BadApi.Tests](https://github.com/paultorvik/iqvia-badapi/tree/master/IQVIA.BadApi.Tests) - Tests for the "bad API", focused on the 2-year range of known, available tweets

## How to Run Console App

1. Clone this repo locally.
2. Navigate to the root folder and double click the **.sln** file to open the project in Visual Studio 2017.
3. Set the IQVIA.BadApi.ConsoleApp as the StartUp project.
4. Press **F5** to launch the Console app.
5. Respond to the prompts for start date, end date and whether to export the Tweets to a CSV file or to the window.
6. If a CSV file path was entered, go and find the CSV file and review the contents. Otherwise, the tweets are output to the console window.

## To Do
- [ ] Add Docker file
- [ ] Possibly add another UI, e.g. ASP.NET Core MVC web site, that reuses the REST API client wrapper library
- [ ] More error handling
