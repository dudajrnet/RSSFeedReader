# Subscriptions API Contract

Base path: `/api/subscriptions`

The API stores submitted URL values as inert strings for the lifetime of the running
application. It MUST NOT resolve, validate, fetch, or parse the submitted feed address.

## `GET /api/subscriptions`

Returns all subscription values in the current process.

- **Success**: `200 OK`
- **Content type**: `application/json`
- **Response body**: JSON array of strings; an empty collection is `[]`.

Example:

```json
[
  "https://example.com/feed.xml",
  "https://example.org/rss"
]
```

## `POST /api/subscriptions`

Adds one subscription value.

- **Request content type**: `application/json`
- **Request body**:

  ```json
  {
    "url": "https://example.com/feed.xml"
  }
  ```

- **Success**: `204 No Content`
- **Blank or missing `url`**: `400 Bad Request`; do not change the collection.
- **Duplicates**: Accept and append each duplicate as a separate value.
- **Accepted value**: Preserve the submitted string without trimming or URL/feed validation.

After a successful response, the client can retrieve the updated list with `GET`.
The list is process-local and is empty after an application restart.

## Local Development

Allow the configured local Blazor origin through CORS. Stakeholder documentation currently
specifies the API at `http://localhost:5151` and the UI at `http://localhost:5213`; keep
these values aligned with the actual launch settings and frontend configuration.
