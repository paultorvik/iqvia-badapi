# IQVIA "Bad API" Take-home Problem Solution

## Overview

My console application simply prompts for start and end date-times (UTC) and a file name where it can export all Tweets from [IQVIA's "bad" API](https://badapi.iqvia.io/swagger/) in that date range. By default, the dates span all of 2016 and 2017.

Because the tweets are returned from the API in ascending time stamp order, we can simply move the requested start date for each page to just after the highest time stamp in the last page we downloaded, in order to download the next page of Tweets. To get all Tweets, we do this until the next returned page has fewer Tweets in it than the page size (100), or none. That means there are no more Tweets, or a full page would have been returned. The same, original end date is used for all requests to always potentially encapsulate all remaining records.

No duplicate Tweets are downloaded because the starting date for each requested page of Tweets is always just after whatever range of time stamps was returned for the previous page.

## Solution Structure
My Visual Studio 2017 solution to IQVIA's "Bad API" take-home problem consists of the following .NET Core 2.0 projects:

- [IQVIA.BadApi.Client](IQVIA.BadApi.Client/) - REST API client library for IQVIA's "bad API", which also works around the paging issues in order to download all Tweets
- [IQVIA.BadApi.ConsoleApp](IQVIA.BadApi.ConsoleApp/) - Console application that uses **IQVIA.BadApi.Client** to download a requested date range of tweets
- [IQVIA.BadApi.Tests](IQVIA.BadApi.Tests/) - Tests for "bad API" using **IQVIA.BadApi.Client** to download 2-year range of known, available tweets

## How to Run App

1. Clone this repo locally.
2. Navigate to the root folder and double click the **.sln** file to open the solution in Visual Studio.
3. Set **IQVIA.BadApi.ConsoleApp** as the StartUp project.
4. Press **F5** to launch the Console app.
5. Respond to the prompts for start date, end date and whether to export the Tweets to a CSV file or to the console window.
6. If a CSV file path was entered, go and find the CSV file and review the contents. Otherwise, the tweets are output to the console window.

## Possible Enhancements
- [ ] Add Dockerfile
- [ ] Add another UI, e.g. ASP.NET Core MVC web site, that reuses the **IQVIA.BadApi.Client** library
- [ ] More error handling
