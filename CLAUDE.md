# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

NL East Challenge is a prediction game where participants guess NL East team win totals for the MLB season. Actual standings are fetched from the MLB Stats API and compared against predictions, with points awarded based on correct team rankings.

## Tech Stack

- **Backend**: ASP.NET Core 8.0 (C#, nullable reference types enabled)
- **Frontend**: React 17 with React Router 5, Bootstrap 5, Reactstrap
- **HTTP Client**: Flurl.Http 4.0 (for MLB API calls)
- **Caching**: Microsoft.Extensions.Caching.Memory
- **Tests**: NUnit 4.3 with MSTest SDK

## Build & Run Commands

```bash
# Backend (from repo root)
dotnet build                    # Builds backend + triggers frontend build
dotnet run --project NLEastChallenge  # Starts ASP.NET Core server
dotnet test                     # Run NUnit tests in NLEastTests/

# Frontend (from NLEastChallenge/ClientApp/)
npm start                       # React dev server (proxied via SPA proxy at https://localhost:44403)
npm run build                   # Production build
npm test                        # Jest tests
```

The .csproj auto-runs `npm install` and `npm run build` on publish.

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

`DivisionData` contains the scoring: players earn points based on how accurately they predicted team rankings (not exact win totals). Tiebreaker uses actual team win percentage.

## Configuration

Player predictions are configured in `appsettings.json` under each player name with win guesses for all 5 NL East teams (NYM, PHI, ATL, MIA, WAS). The current season year is also set there.
