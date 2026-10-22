# Security remediation tracker

Started 2026-10-21. Prompted by the Node.js October 2026 security releases and 86 open Dependabot alerts on `khaney64/NLEastChallenge`.

## Exposure assessment (2026-10-21)

**Production (`nleastchallenge.azurewebsites.net`) is not exposed to the Node.js runtime CVEs.** The site is served by Kestrel (ASP.NET Core 8). Node.js is not running in production; it is only used to build the React bundle.

**The 86 Dependabot alerts are almost all build/dev tooling.** Create React App (`react-scripts` 5.0.1) pulls in webpack, webpack-dev-server, babel, svgo, postcss and friends. Dependabot labels them "runtime" only because CRA lists `react-scripts` under `dependencies`. None of the flagged packages ship in the browser bundle.

What actually ships, read from the live `main.51a12831.js.map`: react / react-dom 17, react-router(-dom) 5, reactstrap 8, popper.js, react-transition-group and small helpers. None of them has an open alert.

Where the risk does sit:

| Where | Risk | Notes |
|---|---|---|
| Developer machine | Real but limited | `npm start` runs webpack-dev-server with known flaws (alerts include `ws`, `shell-quote`, `launch-editor`, `webpack-dev-middleware`). Only matters while the dev server is running. |
| Build | Supply chain / DoS | Unmaintained toolchain; several packages have no fix (`braces`, `node-forge`, `sprintf-js`). |
| Production | Low | Public source map: `/static/js/*.js.map` is downloadable and exposes the 20 app source files. No secrets found in it; it is information disclosure only. |
| .NET | Clean | `dotnet list package --vulnerable --include-transitive` against nuget.org reports none. |

Not verified: the App Service configuration (runtime stack, HTTPS-only, minimum TLS, FTPS). The app lives in a subscription other than the default `az` login; see item 6.

## Work items

Status values: `todo`, `in progress`, `done`, `wontfix`.

### 1. Migrate ClientApp from Create React App to Vite: `todo`

This is the root-cause fix and should clear most of the 86 alerts. Scope is the build toolchain only: keep React 17, react-router 5 and reactstrap 8 as they are (upgrades are item 4).

#### Start here (context for a fresh session)

- **Repo:** `C:\development\home\NLEastChallenge`, GitHub `khaney64/NLEastChallenge`, default branch `main`. There is no CI. Work on a branch such as `vite-migration` and open a PR.
- **Dependency changes are authorized** for this item. Add `vite`, `@vitejs/plugin-react`, `vitest` and `jsdom` (plus eslint 9 packages if linting is kept), and remove CRA and the unused packages listed below.
- **Prior art:** 3H.Admin made this exact move (ticket HHH-80). Copy its patterns:
  - `C:\development\3h\3H.Admin\3H.Admin\clientapp\vite.config.mjs`: `outDir: 'build'`, `sourcemap: false`, HTTPS proxy, vitest config in the same file
  - `C:\development\3h\3H.Admin\3H.Admin\clientapp\package.json`: exact-pinned versions
  - `C:\development\3h\3H.Admin\3H.Admin\clientapp\eslint.config.mjs`: eslint 9 flat config
  - Versions current as of 2026-10-22: `vite` 8.2.4, `@vitejs/plugin-react` 6.0.5, `vitest` 4.1.12, `jsdom` ^26.1.0. Vite 8 needs Node `^20.19 || >=22.12`; this PC has 24.22.1.
- **How dev mode works today:**
  - `dotnet run` (launch profile `NLEastChallenge`: `https://localhost:7009`, with `ASPNETCORE_HOSTINGSTARTUPASSEMBLIES=Microsoft.AspNetCore.SpaProxy`) starts `SpaProxyLaunchCommand` (`npm start`) and waits for `SpaProxyServerUrl` (`https://localhost:44403`).
  - `npm start` runs `prestart`. That calls `aspnetcore-https.js`, which exports the ASP.NET dev cert to `%APPDATA%\ASP.NET\https\nleastchallenge.pem`/`.key`, and then `aspnetcore-react.js`, which writes those paths into `.env.development.local`.
  - The CRA dev server then serves HTTPS on 44403 (`.env.development`: `PORT=44403`, `HTTPS=true`).
  - `src/setupProxy.js` forwards `/divisiondata` and `/gamedata` to the backend.
- **How production works:** `PublishRunWebpack` in the csproj runs `npm install` and `npm run build`, then copies `ClientApp/build/**` into `wwwroot`. `Program.cs` serves it with `UseStaticFiles` and `MapFallbackToFile("index.html")`.
- **Deploy targets:** Visual Studio Zip Deploy profiles in `NLEastChallenge/Properties/PublishProfiles/`. **Deploy to `NLEastChallengeBeta` (`https://nleastchallengebeta.azurewebsites.net`) first**, then production. Both are in resource group `NLEastRG`, in a subscription that is not the default `az` login.

#### Checklist

- [ ] Replace `react-scripts` with `vite` and `@vitejs/plugin-react`. Pin exact versions, as 3H.Admin does. Set `"engines": { "node": ">=24.0.0" }`.
- [ ] Move `public/index.html` to `ClientApp/index.html`:
  - Add `<script type="module" src="/src/index.jsx"></script>`.
  - Replace every `%PUBLIC_URL%` (the `<base href>`, manifest and favicon links) with `/`.
  - Keep `<base href="/">`. `src/index.js` reads it to set `<BrowserRouter basename>`. Alternatively switch that to `import.meta.env.BASE_URL`.
  - `public/manifest.json` and `favicon.ico` can stay in `public/`; Vite copies them unchanged.
- [ ] JSX lives in `.js` files: `App.js`, `App.test.js`, `index.js`, and `components/Home.js`, `Layout.js`, `Standings.js`, `Games.js`, `UpcomingGames.js`. Rename them to `.jsx` (simplest) and fix the imports.
- [ ] Fix the CommonJS image import in `components/Games.js` line 54: `src={require('../images/refresh2.png')}` → `import refreshIcon from '../images/refresh2.png'`, then `src={refreshIcon}`. `Standings.js` already uses an `import`.
- [ ] Port `src/setupProxy.js` to `server.proxy` in `vite.config.mjs`:
  - Paths `/divisiondata` and `/gamedata`, with `secure: false`.
  - Keep its target resolution: `ASPNETCORE_HTTPS_PORT` → `https://localhost:<port>`, else the first entry of `ASPNETCORE_URLS`, else `http://localhost:3078`.
  - Delete `setupProxy.js` and drop `http-proxy-middleware` (and its `overrides` entry).
- [ ] Dev HTTPS:
  - `server: { port: 44403, strictPort: true, https: { key, cert } }`, reading the `.pem`/`.key` from `%APPDATA%\ASP.NET\https\nleastchallenge.*`.
  - Keep `aspnetcore-https.js` as `prestart`, so the cert exists before Vite starts.
  - Delete `aspnetcore-react.js` and all three env files (`.env`, `.env.development`, `.env.development.local`). Vite config replaces them.
- [ ] Build: `build.outDir: 'build'`, so the csproj `DistFiles` (`$(SpaRoot)build\**`) keeps working, and `build.sourcemap: false` (closes item 2).
- [ ] Service worker: `index.js` already calls `serviceWorkerRegistration.unregister()`. Replace `serviceWorkerRegistration.js` with a minimal unregister so browsers that installed the old worker drop it. Delete `service-worker.js` and every `workbox-*` dependency. These are the only files that use `process.env`.
- [ ] `reportWebVitals()` is called with no callback, so it does nothing. Delete `reportWebVitals.js`, its call, and `web-vitals`.
- [ ] Tests: switch `npm test` to Vitest (`environment: 'jsdom'`, `globals: true`, `exclude` as in 3H.Admin).
  - `App.test.js` calls `jest.fn` twice; change both to `vi.fn` (with `globals: true`, `vi` is global).
  - Everything else (`react-dom/test-utils` `act`/`Simulate`, `ReactDOM.render`) works unchanged on React 17.
  - All 8 tests must pass.
- [ ] Lint: replace `eslint-config-react-app` / eslint 7 / `eslint-plugin-flowtype` with eslint 9 flat config based on 3H.Admin's `eslint.config.mjs`, or drop the `lint` script. Either is fine; record which in the PR.
- [ ] Remove unused dependencies: `oidc-client`, `jquery`, `merge`, `rimraf`, `web-vitals`, `nan`, `ajv`, `typescript`, `cross-env`, `@babel/plugin-proposal-private-property-in-object`, and the `overrides` block. None appear in the live bundle; confirm with a project-wide search before deleting.
- [ ] Scripts: `"start": "vite"`, `"build": "vite build"`, `"test": "vitest run"`. `SpaProxyLaunchCommand` stays `npm start`.
- [ ] Update CLAUDE.md and AGENTS.md: the Tech Stack and Build & Run sections say "Jest tests" and describe CRA.
- [ ] Verify locally:
  - `npm run build` produces `build/index.html` and `build/assets/*`, with no `.map` files.
  - `npm test` passes.
  - `dotnet run --project NLEastChallenge` opens the app through the SPA proxy on 44403, and Home, Games (including the refresh icon) and Upcoming all render with live data.
  - `dotnet publish` output has `wwwroot/index.html`.
- [ ] Deploy to `NLEastChallengeBeta` and check the same three pages, plus that `/static/js/*.js.map` and `/assets/*.map` no longer download. Then deploy to production.
- [ ] After merge, re-count open Dependabot alerts and record the number here:
  `MSYS_NO_PATHCONV=1 gh api "repos/khaney64/NLEastChallenge/dependabot/alerts?state=open&per_page=100" --paginate --jq length`.

### 2. Stop publishing the JavaScript source map: `todo`

Covered by `build.sourcemap: false` in item 1. If item 1 slips, set `GENERATE_SOURCEMAP=false` for the CRA build and redeploy.

### 3. Clear remaining Dependabot alerts: `todo`, blocked by 1

Triage whatever is left after the migration. Bump direct dependencies, use npm `overrides` for transitives, and dismiss with a written reason anything dev-only that has no fix.

### 4. Upgrade React 17 → 18/19 and react-router 5 → 7: `todo`, after 1

React 17 and react-router 5 are out of active support. Reactstrap 8 also needs a major bump (9.x) to work with React 18 and Bootstrap 5.

### 5. .NET maintenance: `todo`

- .NET 8 LTS support ends 2026-11-10. Retarget to `net10.0`.
- Remove `Microsoft.AspNetCore.Http.Features` 5.0.17. It is an out-of-support 5.0-era package; the types come from the shared framework on net8+.
- Fix the `Microsoft.Extensions.*` 9.0.2 references, which resolve to 9.0.3 with NU1603 warnings. Move to versions matching the target framework.

### 6. Verify App Service configuration: `todo`

Log in to the subscription that hosts `nleastchallenge` and confirm:

- runtime stack matches the target framework after item 5
- `httpsOnly: true`
- `minTlsVersion` is 1.2 or higher
- `ftpsState` is `Disabled` or `FtpsOnly`
- no unexpected app settings or secrets

### 7. Developer machine Node.js: `done`

2026-10-21: workstation upgraded to Node 24.22.1 (patched LTS) and nvm-windows removed.
