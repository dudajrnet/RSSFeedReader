# Implementation Plan: RSS Subscription MVP

**Branch**: `001-rss-subscription-mvp` | **Date**: 2026-11-12 | **Spec**: [spec.md](./spec.md)

**Input**: Feature specification from `specs/001-rss-subscription-mvp/spec.md`

## Summary

Deliver the subscription-list MVP: a user submits a non-empty URL and sees it in the
list immediately. Use the planned ASP.NET Core API and Blazor WebAssembly UI with a
process-local in-memory collection. The API accepts URL values as inert data; it does not
validate, fetch, or parse feeds. Blank values are ignored, duplicates remain separate,
and data is lost when the application stops.

## Technical Context

**Language/Version**: C# on .NET 10 LTS (`net10.0`); .NET SDK 10.0.401 is installed.

**Primary Dependencies**: ASP.NET Core Minimal APIs for the backend; Blazor WebAssembly
and its built-in HTTP client for the application API; xUnit and
`Microsoft.AspNetCore.Mvc.Testing` for API integration tests.

**Storage**: Process-local in-memory collection; no persistence.

**Testing**: xUnit API integration tests for empty listing, adding, duplicates, and blank
input; manual browser acceptance for the end-to-end UI flow.

**Target Platform**: Windows, macOS, and Linux for local development; any modern browser
that supports WebAssembly for the UI.

**Project Type**: Web application with a backend API and browser UI.

**Performance Goals**: No throughput or scale target for this single-user local proof of
concept. After a successful add, the updated list is visible in the same interaction.

**Constraints**: Add and list only; accept non-empty URL values without URL or feed
validation; ignore empty or whitespace-only submissions; preserve duplicates; no feed
fetching or parsing; in-memory data is discarded on restart. Keep API and UI origins
aligned with local CORS and frontend configuration.

**Scale/Scope**: One local user and a small, session-only subscription list.

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- **Security and Privacy**: PASS. Treat URLs as inert values; no DNS resolution,
  outbound feed requests, redirects, or rendering of feed content are in scope.
- **MVP Scope and Simplicity**: PASS. Only add and list subscriptions; no persistence,
  fetch, parsing, editing, or removal.
- **Maintainable Boundaries**: PASS. Keep API data operations separate from UI behavior;
  use only the integration needed for this feature.
- **Verifiable Quality**: PASS. Focus API integration tests on specified behavior and
  run the API/UI build plus the browser acceptance flow.
- **Cross-Platform, Usable Delivery**: PASS. Use the documented local origins and
  configuration, and validate the primary flow in a browser.

## Project Structure

### Documentation (this feature)

```text
specs/001-rss-subscription-mvp/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── subscriptions-api.md
└── tasks.md
```

### Source Code (repository root)

```text
backend/
└── RSSFeedReader.Api/

frontend/
└── RSSFeedReader.UI/

tests/
└── RSSFeedReader.Api.Tests/
```

**Structure Decision**: Use the backend and frontend project locations named in
`StakeholderDocuments/TechStack.md`, with a focused API test project under `tests/`.
No solution or source projects currently exist in the repository; implementation tasks
must scaffold these projects before building the feature. Keep the in-memory collection
inside the API boundary and avoid adding a separate persistence or repository layer.

## Post-Design Constitution Check

- **Security and Privacy**: PASS. The API stores URL strings only and makes no requests
  to user-supplied destinations.
- **MVP Scope and Simplicity**: PASS. The contract contains only list and add operations.
- **Maintainable Boundaries**: PASS. The frontend consumes the API; state remains owned
  by the backend process.
- **Verifiable Quality**: PASS. API contract tests and the documented browser flow verify
  the observable requirements.
- **Cross-Platform, Usable Delivery**: PASS. The quickstart uses the documented local
  ports and cross-platform `dotnet` commands.
