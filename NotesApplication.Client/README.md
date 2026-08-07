# Notebook — Notes App (Frontend)

A Vue 3 + TypeScript + Tailwind CSS client for a personal notes app, built against
an ASP.NET Core Web API (Dapper + SQL Server) backend.

## Stack

- **Vue 3** (`<script setup>`, Composition API) + **TypeScript**
- **Vite** for dev/build
- **Tailwind CSS v4**
- **Pinia** for state management
- **Vue Router** for pages + auth guards
- **Axios** for API calls

## Design

The UI takes a "field notebook" identity: warm paper background with a faint
ruled-line texture, a serif display face (Fraunces) for titles, a clean sans
(Inter) for body copy, and a monospace face (JetBrains Mono) for timestamps —
like a date stamped on a notebook page. Each note renders as a folded-corner
index card. Fonts load from Google Fonts in `index.html`; if you're working
offline, swap those `<link>` tags for local font files.

## Project layout

```
src/
├── api/           axios instance + typed API wrappers (noteApi, authApi)
├── components/    Navbar, NoteCard, NoteForm, SearchBar, SortDropdown,
│                  DeleteModal, LoadingSpinner
├── composables/   useAuth, useNotes — thin wrappers over the Pinia stores
├── layouts/       DefaultLayout (Navbar + router-view + footer)
├── pages/         Notes (list), NoteDetail (view/create/edit), Login, Register
├── router/        route table + auth guard
├── store/         Pinia stores: auth.ts, notes.ts
├── types/         note.ts, auth.ts
└── utils/         date.ts (formatting), validator.ts (form validation)
```

## Getting started

```bash
npm install
cp .env.example .env   # then edit the URLs below to match your backend
npm run dev
```

The app runs at `http://localhost:5173`.

### Environment variables

The backend in this brief exposed the Notes API and Auth API on different
ports in different examples (`7057` vs `5096`), so both are configurable
separately rather than hard-coded:

```
VITE_API_BASE_URL=https://localhost:7057/api    # GET/POST/PUT/DELETE /api/Notes
VITE_AUTH_BASE_URL=https://localhost:7057/api   # POST /api/auth/login, /api/auth/register
```

If your Auth API actually runs on a separate port (e.g. 5096), point
`VITE_AUTH_BASE_URL` there instead.

> **HTTPS/dev certs:** ASP.NET Core's local HTTPS cert is self-signed. If the
> browser blocks requests to `https://localhost:7057`, open that URL directly
> once and accept the certificate warning, or run
> `dotnet dev-certs https --trust` on the backend.

## API endpoints this client expects

| Method | Path                | Purpose                          |
| ------ | -------------------- | --------------------------------- |
| GET    | `/api/Notes`          | List the signed-in user's notes   |
| GET    | `/api/Notes/{id}`     | Fetch one note                    |
| POST   | `/api/Notes`          | Create a note (`{ title, content }`) |
| PUT    | `/api/Notes/{id}`     | Update a note                     |
| DELETE | `/api/Notes/{id}`     | Delete a note                     |
| POST   | `/api/auth/login`     | `{ email, password }` → `{ token, expiresAt, user }` |
| POST   | `/api/auth/register`  | `{ fullName, email, password, confirmPassword }` → same shape |

A note is expected to look like:

```ts
{
  id: number
  title: string
  content: string | null
  createdAt: string   // ISO date
  updatedAt: string | null
  userId?: string
}
```

## Auth

Login/Register are wired up and the router guards `/` and `/notes/:id`
behind `isAuthenticated`. Since auth was optional for this brief:

- To ship **without** auth, delete the `meta: { requiresAuth: true }` /
  `meta: { guestOnly: true }` entries in `src/router/index.ts` (or just leave
  the guard — it'll simply always require login).
- The JWT is stored in `localStorage` and attached as a `Bearer` token on
  every request to `VITE_API_BASE_URL` via an Axios interceptor. A `401`
  response clears the token and redirects to `/login`.

## Features implemented

- Create / read / update / delete notes
- Notes list: title + created date, click through to full detail
- Search (title + content, client-side)
- Sort: recently updated, newest/oldest first, title A–Z / Z–A
- Delete confirmation modal
- Responsive layout (mobile → desktop) with Tailwind
- Login / register forms with client-side validation
- Loading and empty/error states throughout

## Scripts

```bash
npm run dev       # start dev server
npm run build     # type-check (vue-tsc) + production build
npm run preview   # preview the production build locally
```

## Notes for the backend team

- CORS must allow `http://localhost:5173` (or wherever this is hosted) with
  credentials if you switch to cookie-based auth instead of a bearer token.
- Notes should be scoped server-side to the authenticated user — the client
  never sends a `userId` on create; it relies entirely on the token.
