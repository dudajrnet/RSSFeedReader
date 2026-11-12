# Tasks: RSS Subscription MVP

**Input**: Design documents from `specs/001-rss-subscription-mvp/`

**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/subscriptions-api.md`

**Organization**: Tasks are grouped by user story so the P1 MVP can be implemented and
validated as an independent increment.

## Phase 1: Setup

**Purpose**: Create the .NET solution, application projects, and test project required
by the implementation plan.

- [ ] T001 [P] Create the .NET solution file `RSSFeedReader.sln` at the repository root for the `net10.0` projects.
- [ ] T002 [P] Scaffold the ASP.NET Core Minimal API project targeting `net10.0` in `backend/RSSFeedReader.Api/RSSFeedReader.Api.csproj`; do not add feed-fetching or feed-parsing dependencies.
- [ ] T003 [P] Scaffold the Blazor WebAssembly project targeting `net10.0` in `frontend/RSSFeedReader.UI/RSSFeedReader.UI.csproj`.
- [ ] T004 [P] Scaffold the xUnit test project targeting `net10.0` in `tests/RSSFeedReader.Api.Tests/RSSFeedReader.Api.Tests.csproj` and add a compatible `Microsoft.AspNetCore.Mvc.Testing` package reference.
- [ ] T005 Add the API, UI, and test projects to `RSSFeedReader.sln`, and add a project reference from `tests/RSSFeedReader.Api.Tests/RSSFeedReader.Api.Tests.csproj` to `backend/RSSFeedReader.Api/RSSFeedReader.Api.csproj`.

---

## Phase 2: Foundational

**Purpose**: Complete shared setup that must be in place before the subscription story.

- [ ] T006 [P] Remove the template demo pages `frontend/RSSFeedReader.UI/Pages/Home.razor`, `frontend/RSSFeedReader.UI/Pages/Counter.razor`, and `frontend/RSSFeedReader.UI/Pages/Weather.razor`; remove their links from `frontend/RSSFeedReader.UI/Layout/NavMenu.razor`.
- [ ] T007 [P] Configure the API and UI local origins and cross-origin access in `backend/RSSFeedReader.Api/Properties/launchSettings.json`, `backend/RSSFeedReader.Api/Program.cs`, `frontend/RSSFeedReader.UI/Properties/launchSettings.json`, and `frontend/RSSFeedReader.UI/wwwroot/appsettings.json`; align with the documented API port `5151` and UI port `5213`.
- [ ] T008 Expose the web entry point as `public partial class Program` in `backend/RSSFeedReader.Api/Program.cs` so `WebApplicationFactory` can host the API in integration tests.
- [ ] T009 Build `RSSFeedReader.sln` and verify the scaffold compiles and the removed demo routes do not leave duplicate or conflicting routes in `frontend/RSSFeedReader.UI/Pages/`.

**Checkpoint**: Solution projects build, shared local configuration is aligned, and template route conflicts are cleared.

---

## Phase 3: User Story 1 - Add and View Subscriptions (Priority: P1) 🎯 MVP

**Goal**: A user submits a non-empty URL and sees it in the subscription list immediately;
duplicates remain separate, blank values are ignored, and values last only for the current
application run.

**Independent Test**: Start with an empty list, submit a non-empty URL through the UI, and
verify that the same value appears immediately. Also verify a second submission, a duplicate,
blank input, and that restarting clears the list.

### Tests for User Story 1

- [ ] T010 [US1] Add API integration tests in `tests/RSSFeedReader.Api.Tests/SubscriptionsApiTests.cs` for an initially empty `GET`, successful `POST` followed by `GET`, duplicate preservation, and blank or missing `url` returning `400` without changing the list; run the tests and confirm they fail on the unimplemented endpoints.

### Implementation for User Story 1

- [ ] T011 [P] [US1] Implement the process-local subscription collection in `backend/RSSFeedReader.Api/Services/SubscriptionStore.cs` and register it as a singleton in `backend/RSSFeedReader.Api/Program.cs`; the `url` field rule is: “Must contain a non-whitespace character. Preserve the submitted value; do not validate URL syntax, reachability, or feed format.” Preserve duplicates and do not make network requests.
- [ ] T012 [P] [US1] Implement `SubscriptionApiClient` in `frontend/RSSFeedReader.UI/Services/SubscriptionApiClient.cs` for the contracted `GET` and `POST` operations, and register it in `frontend/RSSFeedReader.UI/Program.cs` using the configured API base URL.
- [ ] T013 [P] [US1] Map `GET /api/subscriptions` and `POST /api/subscriptions` in `backend/RSSFeedReader.Api/Program.cs` using `backend/RSSFeedReader.Api/Services/SubscriptionStore.cs`; return `200` with the string list, `204` for an accepted add, and `400` for a missing or whitespace-only value, preserving non-empty values and duplicates without URL validation or feed requests.
- [ ] T014 [P] [US1] Create the root subscription page in `frontend/RSSFeedReader.UI/Pages/Subscriptions.razor` with a URL input and list; ignore empty or whitespace-only submissions, preserve submitted values, refresh the list after successful adds, render values as text, and show API failures instead of silently swallowing them.
- [ ] T015 [US1] Add a `Subscriptions` navigation link to the root page in `frontend/RSSFeedReader.UI/Layout/NavMenu.razor`.
- [ ] T016 [US1] Run `dotnet test tests/RSSFeedReader.Api.Tests` and build `RSSFeedReader.sln`; confirm the API integration tests pass for empty list, add/list, duplicate, and blank-input behavior.

**Checkpoint**: User Story 1 is independently functional and testable through the API and UI.

---

## Phase 4: Polish and Cross-Cutting Validation

**Purpose**: Verify the complete quickstart and the MVP's security and scope boundaries.

- [ ] T017 Run the build, test, API, and browser scenarios in `specs/001-rss-subscription-mvp/quickstart.md`; verify restart clears subscriptions and add/list operations make no requests to submitted feed URLs.

---

## Dependencies and Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No prerequisite. T001–T004 create independent project files and can run in parallel; T005 depends on all four.
- **Foundational (Phase 2)**: Depends on T005. T006 and T007 can run in parallel; T008 follows T007 because both update `backend/RSSFeedReader.Api/Program.cs`; T009 follows T006–T008 and blocks story work.
- **User Story 1 (Phase 3)**: Depends on the foundational checkpoint. T010 writes and runs the failing integration tests before behavior is implemented. T011 and T012 can then run in parallel. T013 depends on T011; T014 depends on T012. T013 and T014 can run in parallel once their respective prerequisites are complete. T015 depends on T014. T016 depends on T010–T015.
- **Polish (Phase 4)**: T017 depends on T016 and validates the complete story.

### User Story Dependencies

- **User Story 1 (P1)**: No dependencies on other user stories; it is the entire MVP.

### Parallel Opportunities

- Setup: T001, T002, T003, and T004 work in separate solution/project files.
- Foundational: T006 edits frontend template pages/navigation while T007 configures launch settings, API CORS, and app settings.
- User Story 1: T011 (API store) and T012 (frontend client) can proceed independently after T010. After both are complete, T013 (API endpoints) and T014 (subscription page) can proceed in parallel; T015 follows the page.

## Parallel Example: User Story 1

```text
First, complete T010 and confirm its integration tests fail against the unimplemented API.
Then run these independent tasks in parallel:

Task: T011 Implement the process-local store in backend/RSSFeedReader.Api/Services/SubscriptionStore.cs
Task: T012 Implement the API client in frontend/RSSFeedReader.UI/Services/SubscriptionApiClient.cs

After T011 and T012 complete, run these independent tasks in parallel:

Task: T013 Map the API endpoints in backend/RSSFeedReader.Api/Program.cs
Task: T014 Create the subscription page in frontend/RSSFeedReader.UI/Pages/Subscriptions.razor
```

## Implementation Strategy

### MVP First

1. Complete Setup and Foundational phases.
2. Complete User Story 1, writing and running its API tests before implementation.
3. Validate add/list, duplicate, blank-input, and restart behavior.
4. Run the quickstart and browser flow; stop after User Story 1 because it is the full MVP.

### Incremental Delivery

This feature has one user story. Deliver it as a complete increment across the API and UI;
do not add feed retrieval, parsing, persistence, editing, or removal as part of this plan.
