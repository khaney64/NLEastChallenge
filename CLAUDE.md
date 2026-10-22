# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

NL East Challenge is a prediction game where participants guess NL East team win totals for the MLB season. Actual standings are fetched from the MLB Stats API and compared against predictions, with points awarded based on correct team rankings.

## Tech Stack

- **Backend**: ASP.NET Core 10.0 (C#, nullable reference types enabled)
- **Frontend**: React 17 with React Router 5, Bootstrap 5, Reactstrap, built with Vite 8 (Node >= 24)
- **HTTP Client**: Flurl.Http 4.0 (for MLB API calls)
- **Caching**: Microsoft.Extensions.Caching.Memory
- **Tests**: NUnit 4.3 with MSTest SDK (backend); Vitest with jsdom (frontend)

## Build & Run Commands

```bash
# Backend (from repo root)
dotnet build                    # Builds backend + triggers frontend build
dotnet run --project NLEastChallenge  # Starts ASP.NET Core server
dotnet test                     # Run NUnit tests in NLEastTests/

# Frontend (from NLEastChallenge/ClientApp/)
npm start                       # Vite dev server on https://localhost:44403 (launched by the SPA proxy on dotnet run)
npm run build                   # Production build
npm test                        # Vitest tests
npm run lint                    # ESLint 9 (flat config in eslint.config.mjs)
```

The .csproj auto-runs `npm install` and `npm run build` on publish and copies `ClientApp/build/**` into `wwwroot`. `vite.config.mjs` holds the dev server (HTTPS using the ASP.NET dev cert exported by `aspnetcore-https.js`), the `/divisiondata` and `/gamedata` proxy to the backend, the build settings (no source maps), and the Vitest config.

## Architecture

### Backend

- **Controllers**: `DivisionDataController` (`/divisiondata`), `GameDataController` (`/gamedata/today`, `/gamedata/upcoming`)
- **Services**: `MlbApi` interface / `MlbApiLive` implementation — fetches from `https://statsapi.mlb.com/api/v1/` (NL East = Division ID 204)
- **Models**: `DivisionData` (player + their team predictions), `TeamData` (team record + guess), `GameData` (live/upcoming game info), `DivisionDataVm` (view model for standings table)
- **Caching**: 1-minute TTL for standings/today's games, 12-hour TTL for upcoming games

### Frontend (ClientApp/src/)

- **Components**: Home (standings), Games (today's scores), UpcomingGames, Standings, Layout
- **API calls**: Fetches `/divisiondata`, `/gamedata/today`, `/gamedata/upcoming`

### Scoring Logic

`DivisionData` supports two scoring modes, selected by the `/divisiondata` query string:

- `mode=normal` (default): original exact-rank scoring. Players earn points when predicted team rankings match actual team rankings, not exact win totals.
- `mode=horseshoes`: Horseshoes and Hand Grenades scoring. Players earn rank-distance points plus a pairwise-order bonus.

Horseshoes rank-distance scoring gives each actual standings slot a value of `5 - actualRank`. The predicted team earns `max(0, slotValue - abs(predictedRank - actualRank))`. Pairwise bonus adds 1 point for each pair of teams the player ordered correctly relative to the actual standings; that bonus is stored on the player's first team row. Missing teams are skipped, and duplicate predicted team entries use the first predicted rank.

Ties are resolved by predicted top-team win percentage, then per-team point values. `DivisionDataController` caches each scoring mode separately with cache keys like `division-Normal` and `division-Horseshoes`.

Scoring tests cover normal exact-rank scoring, normal tie sorting, Horseshoes rank-distance and pairwise bonus scoring, Horseshoes tie sorting, missing teams, and duplicate predictions.

## Configuration

Player predictions are configured in `appsettings.json` under each player name with win guesses for all 5 NL East teams (NYM, PHI, ATL, MIA, WAS). The current season year is also set there.
