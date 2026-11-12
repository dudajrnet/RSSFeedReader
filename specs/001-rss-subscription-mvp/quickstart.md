# Quickstart: RSS Subscription MVP

## Prerequisites

- .NET 10 SDK
- A modern browser
- The API and UI projects scaffolded as described in [plan.md](./plan.md)
- Local API and UI origins configured consistently with
  [subscriptions-api.md](./contracts/subscriptions-api.md)

## Build and Test

From the repository root:

```powershell
dotnet restore tests\RSSFeedReader.Api.Tests
dotnet test tests\RSSFeedReader.Api.Tests
dotnet build backend\RSSFeedReader.Api
dotnet build frontend\RSSFeedReader.UI
```

The API integration tests must cover an initially empty collection, successful add/list,
duplicate preservation, and blank-value rejection.

## Run the Application

Start the backend and frontend in separate terminals:

```powershell
dotnet run --project backend\RSSFeedReader.Api
```

```powershell
dotnet run --project frontend\RSSFeedReader.UI
```

Use the configured UI origin (the stakeholder documents currently specify
`http://localhost:5213`) and API origin (`http://localhost:5151`).

## Verify the API

In PowerShell, inspect the initial list, add a subscription, then inspect the list again:

```powershell
Invoke-RestMethod -Uri 'http://localhost:5151/api/subscriptions' -Method Get

$body = '{"url":"https://example.com/feed.xml"}'
Invoke-RestMethod -Uri 'http://localhost:5151/api/subscriptions' `
  -Method Post -ContentType 'application/json' -Body $body

Invoke-RestMethod -Uri 'http://localhost:5151/api/subscriptions' -Method Get
```

Expected outcomes: the first list is empty; the add returns `204 No Content`; the final
list contains the submitted value.

## Verify the UI

1. Open the UI in a browser and confirm the subscription list is initially empty.
2. Submit `https://example.com/feed.xml`; confirm that it appears immediately.
3. Submit the same value again; confirm a second entry appears.
4. Submit empty or whitespace-only input; confirm no entry is added.
5. Restart the application and confirm previous values are gone.
6. Confirm no request is made to any feed URL during add or list operations.
