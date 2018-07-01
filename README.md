# IQVIA "Bad API" Take-home Problem Solution

My Visual Studio 2017 solution to IQVIA's "Bad API" take-home problem consists of the following .NET Core 2.0 projects:

- IQVIA.BadApi.Client - REST API client wrapper for IQVIA's "bad API", which also accounts for lack of paging
- IQVIA.BadApi.ConsoleApp - Console application that uses the API client to download a requested date range of tweets
- IQVIA.BadApi.Tests - Unit tests for the "bad API", focused on the 2-year range of known, available tweets
