<!--
Sync Impact Report
Version change: unratified template -> 1.0.0 (initial adoption)
Modified principles: None; this is the initial project constitution.
Added sections: Additional Constraints; Development Workflow and Quality Gates.
Removed sections: None.
Follow-up TODO: Confirm the original ratification date.
-->

# RSS Feed Reader Constitution

## Core Principles

### I. Security and Privacy
Treat user-provided URLs and feed content as untrusted data. In the MVP, URLs are inert
subscription values; do not fetch them. Before implementing server-side feed retrieval,
validate destinations and every redirect to prevent access to loopback, private, link-local,
and metadata addresses; apply timeouts and response-size limits. Render feed content as
text unless it has been safely sanitized. Do not commit credentials or collect data the
application does not need.

### II. MVP Scope and Simplicity
The MVP MUST let a user add a subscription URL and see it in the subscription list.
Subscriptions MAY remain in memory. Feed fetching, parsing, persistence, deletion, and
other enhancements are out of MVP scope and MUST NOT be added without an approved
feature specification. Prefer the smallest implementation that satisfies the active scope.

### III. Maintainable Boundaries
Keep the ASP.NET Core Web API responsible for subscription operations and the Blazor
WebAssembly UI responsible for user interaction and display. Keep API contracts explicit,
configuration external to source code, and components cohesive. Add abstractions only
when a current requirement or a tested seam needs them; do not build speculative
infrastructure for deferred features.

### IV. Verifiable Quality
Changes MUST preserve existing behavior and include focused automated tests for new or
changed behavior where practical. Validate relevant API and UI behavior, and run the
smallest applicable test and build commands before considering work complete. Review
failures explicitly; do not suppress errors or replace them with success-shaped fallbacks.

### V. Cross-Platform, Usable Delivery
The application MUST remain developable on Windows, macOS, and Linux. Keep local service
addresses and frontend/backend configuration consistent and configurable. The MVP UI
MUST make adding a URL and seeing the updated subscription list straightforward; visual
polish MUST NOT displace correct core behavior.

## Additional Constraints

- Use ASP.NET Core Web API and Blazor WebAssembly as the application foundation.
- Keep MVP subscription storage in memory and avoid feed network operations until the
  Extended-MVP is explicitly specified.
- When feed retrieval is approved, use bounded, manual refresh and display only the
  specified feed fields. Do not render untrusted HTML without sanitization.
- Keep local CORS and API base URL configuration aligned with the actual frontend and
  backend origins; do not hardcode environment-specific values in application logic.

## Development Workflow and Quality Gates

- Define work through a feature specification when it adds or changes user-visible
  behavior or expands the agreed MVP scope.
- For each change, identify affected API/UI behavior, add focused tests where practical,
  and run relevant tests and builds. Verify the actual required output rather than relying
  only on compilation.
- Review changes for scope, security, maintainability, and clear error handling before
  integration. Resolve or document test failures; do not silently ignore them.
- Keep documentation that directly governs changed behavior accurate. Avoid unrelated
  refactoring and dependencies.

## Governance

This constitution governs project decisions and supersedes conflicting informal practices.
Amendments MUST be made in this file, state their rationale, and receive project-owner
approval before adoption. Feature work that would change the MVP boundaries MUST first
be specified and approved.

Use semantic versioning for this constitution: MAJOR for incompatible principle or
governance changes, MINOR for new principles or materially expanded requirements, and
PATCH for clarifications that do not change obligations. Every amendment MUST update the
version and last-amended date. Project reviews MUST check proposed work against these
principles; deviations require an explicit rationale and approval.

**Version**: 1.0.0 | **Ratified**: TODO(RATIFICATION_DATE): original ratification date unknown | **Last Amended**: 2026-11-12
