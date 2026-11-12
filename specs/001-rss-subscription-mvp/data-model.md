# Data Model: RSS Subscription MVP

## Subscription

Represents one non-empty value submitted by the user.

| Field | Type | Rules |
|---|---|---|
| `url` | String | Must contain a non-whitespace character. Preserve the submitted value; do not validate URL syntax, reachability, or feed format. |

### Lifecycle

- A successful submission adds one subscription value to the current process's collection.
- Repeated values are separate subscriptions; there is no uniqueness constraint.
- Empty or whitespace-only values do not change the collection.
- The collection is lost when the application process stops or restarts.

## Subscription List

The current process's collection of subscriptions. It may be empty and contains zero or
more `Subscription` values. No additional metadata, persistence, or feed-item relationship
is part of this MVP.
