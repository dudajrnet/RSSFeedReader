# Research: RSS Subscription MVP

## Decisions

### Target .NET version

- **Decision**: Target .NET 10 LTS (`net10.0`) and use the latest available servicing
  patch.
- **Rationale**: .NET 10 is the current LTS choice for a new application and the installed
  SDK is 10.0.401. An LTS target fits a small project better than adopting a new STS
  release solely for recency.
- **Alternatives considered**: .NET 8 is an older LTS line and is near the end of its
  support window as of the planning date. A newer STS release would shorten the support
  horizon. Neither is needed for this MVP.
- **Source**: [Microsoft .NET support policy](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core)
  and [ASP.NET Core Minimal API tutorial](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-10.0).

### Backend API style and contract

- **Decision**: Use two ASP.NET Core Minimal API operations: `GET /api/subscriptions` and
  `POST /api/subscriptions`.
- **Rationale**: The app exposes only collection listing and creation. A small endpoint
  surface avoids controllers and abstractions that do not serve current requirements.
- **Alternatives considered**: Controllers are supported by ASP.NET Core but add ceremony
  for this two-operation proof of concept. A browser-only list would not meet the
  stakeholder requirement for backend-managed subscriptions and frontend/backend API
  communication.
- **Source**: [Microsoft Minimal API tutorial](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-10.0).

### Storage and URL handling

- **Decision**: Keep subscription strings in a process-local collection. Do not fetch,
  parse, resolve, validate, or follow redirects for submitted URLs.
- **Rationale**: This preserves the explicit in-memory MVP behavior and avoids creating
  a server-side request path for user-controlled destinations. Check only for empty or
  whitespace-only values as required by the spec; preserve every accepted value.
- **Alternatives considered**: Persistent storage, URL validation that performs network
  checks, and feed fetching are outside scope. In particular, fetching or resolving a
  user-controlled URL would introduce SSRF risk and requires a separately specified
  security design.
- **Source**: [OWASP SSRF Prevention Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/Server_Side_Request_Forgery_Prevention_Cheat_Sheet.html).

### Verification approach

- **Decision**: Use xUnit API integration tests with `Microsoft.AspNetCore.Mvc.Testing`,
  plus a manual browser acceptance run. Test empty listing, adding and listing a value,
  duplicate preservation, and rejection of blank input.
- **Rationale**: Integration tests prove the API boundary and in-memory state behavior.
  The simple form/list interaction can be checked in the end-to-end browser flow without
  adding a separate UI test framework.
- **Alternatives considered**: Unit tests for a trivial string collection would provide
  little value by themselves. Component or browser automation may be added if later UI
  behavior becomes more complex.
- **Source**: [ASP.NET Core integration testing](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-10.0)
  and [Blazor testing guidance](https://learn.microsoft.com/en-us/aspnet/core/blazor/test?view=aspnetcore-10.0).

## Resolved Unknowns

- The MVP targets .NET 10 LTS; the matching SDK is installed.
- The project is a cross-platform local web application with an API and Blazor
  WebAssembly UI, as specified by stakeholder documentation.
- There are no existing solution or source project files. Scaffolding them is a prerequisite
  for implementation.
- No scale or throughput target is required for this single-user proof of concept.
