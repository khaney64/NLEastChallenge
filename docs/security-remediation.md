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

### 1. Migrate ClientApp from Create React App to Vite: `done`

Merged in PR #3 and deployed to production on 2026-10-22. Two follow-ups remain open: the alert recount and confirming #239/#240. Both wait on Dependabot rescanning the new lockfile.

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
- **Deploy target:** there is a single site, production App Service `NLEastChallenge` (Linux; runtime stack must match the target framework, see item 5), in resource group `NLEastRG`. Its subscription is `3d6572f6-2e54-4530-a484-2847d25e01fa` in tenant `55117e3a-e992-480d-8f42-095cd729530a`, not the default `az` login. There is no beta or staging site.
- **How to deploy:** the Visual Studio Zip Deploy profile (`NLEastChallenge/Properties/PublishProfiles/`) loses its saved password and then fails with "The ZipDeploy task was not given a value for the required parameter DestinationUsername". The Azure CLI uses your `az` login instead:
  ```
  az login --tenant 55117e3a-e992-480d-8f42-095cd729530a
  dotnet publish NLEastChallenge -c Release -r linux-x64 --self-contained false -o <out>
  # zip the contents of <out> (not the folder itself)
  az webapp deploy -g NLEastRG -n NLEastChallenge --subscription 3d6572f6-2e54-4530-a484-2847d25e01fa --type zip --clean true --src-path <zip>
  ```
  Keep `--clean true`. Without it, files from the previous build stay in `wwwroot`. The first 2026-10-22 deploy left the old CRA source map and `service-worker.js` downloadable.

#### Checklist

- [x] Replace `react-scripts` with `vite` and `@vitejs/plugin-react`. Pin exact versions, as 3H.Admin does. Set `"engines": { "node": ">=24.0.0" }`.
- [x] Move `public/index.html` to `ClientApp/index.html`:
  - Add `<script type="module" src="/src/index.jsx"></script>`.
  - Replace every `%PUBLIC_URL%` (the `<base href>`, manifest and favicon links) with `/`.
  - Keep `<base href="/">`. `src/index.js` reads it to set `<BrowserRouter basename>`. Alternatively switch that to `import.meta.env.BASE_URL`.
  - `public/manifest.json` and `favicon.ico` can stay in `public/`; Vite copies them unchanged.
- [x] JSX lives in `.js` files: `App.js`, `App.test.js`, `index.js`, and `components/Home.js`, `Layout.js`, `Standings.js`, `Games.js`, `UpcomingGames.js`. Rename them to `.jsx` (simplest) and fix the imports.
- [x] Fix the CommonJS image import in `components/Games.js` line 54: `src={require('../images/refresh2.png')}` → `import refreshIcon from '../images/refresh2.png'`, then `src={refreshIcon}`. `Standings.js` already uses an `import`.
- [x] Port `src/setupProxy.js` to `server.proxy` in `vite.config.mjs`:
  - Paths `/divisiondata` and `/gamedata`, with `secure: false`.
  - Keep its target resolution: `ASPNETCORE_HTTPS_PORT` → `https://localhost:<port>`, else the first entry of `ASPNETCORE_URLS`, else `http://localhost:3078`.
  - Delete `setupProxy.js` and drop `http-proxy-middleware` (and its `overrides` entry).
- [x] Dev HTTPS:
  - `server: { port: 44403, strictPort: true, https: { key, cert } }`, reading the `.pem`/`.key` from `%APPDATA%\ASP.NET\https\nleastchallenge.*`.
  - Keep `aspnetcore-https.js` as `prestart`, so the cert exists before Vite starts.
  - Delete `aspnetcore-react.js` and all three env files (`.env`, `.env.development`, `.env.development.local`). Vite config replaces them.
- [x] Build: `build.outDir: 'build'`, so the csproj `DistFiles` (`$(SpaRoot)build\**`) keeps working, and `build.sourcemap: false` (closes item 2).
- [x] Service worker: `index.js` already calls `serviceWorkerRegistration.unregister()`. Replace `serviceWorkerRegistration.js` with a minimal unregister so browsers that installed the old worker drop it. Delete `service-worker.js` and every `workbox-*` dependency. These are the only files that use `process.env`.
- [x] `reportWebVitals()` is called with no callback, so it does nothing. Delete `reportWebVitals.js`, its call, and `web-vitals`.
- [x] Tests: switch `npm test` to Vitest (`environment: 'jsdom'`, `globals: true`, `exclude` as in 3H.Admin).
  - `App.test.js` calls `jest.fn` twice; change both to `vi.fn` (with `globals: true`, `vi` is global).
  - Everything else (`react-dom/test-utils` `act`/`Simulate`, `ReactDOM.render`) works unchanged on React 17.
  - All 8 tests must pass.
- [x] Lint: replace `eslint-config-react-app` / eslint 7 / `eslint-plugin-flowtype` with eslint 9 flat config based on 3H.Admin's `eslint.config.mjs`, or drop the `lint` script. Either is fine; record which in the PR.
  - 2026-10-22: kept lint on eslint 9 flat config (`eslint.config.mjs`). Existing `class=` attributes in `Games.jsx` surface as `react/no-unknown-property` warnings, left unfixed (out of scope).
- [x] Remove unused dependencies: `oidc-client`, `jquery`, `merge`, `rimraf`, `web-vitals`, `nan`, `ajv`, `typescript`, `cross-env`, `@babel/plugin-proposal-private-property-in-object`, and the `overrides` block. None appear in the live bundle; confirm with a project-wide search before deleting.
- [x] Scripts: `"start": "vite"`, `"build": "vite build"`, `"test": "vitest run"`. `SpaProxyLaunchCommand` stays `npm start`.
- [x] Update CLAUDE.md and AGENTS.md: the Tech Stack and Build & Run sections say "Jest tests" and describe CRA.
- [x] Verify locally:
  - `npm run build` produces `build/index.html` and `build/assets/*`, with no `.map` files.
  - `npm test` passes.
  - `dotnet run --project NLEastChallenge` opens the app through the SPA proxy on 44403, and Home, Games (including the refresh icon) and Upcoming all render with live data.
  - `dotnet publish` output has `wwwroot/index.html`.
- [x] Deploy to production and check the same three pages, plus that `/static/js/*.js.map` and `/assets/*.map` no longer download.
  - 2026-10-22: deployed with `az webapp deploy --clean true`. Standings render with live data, Games Today and Upcoming render (empty in the offseason), the refresh icon loads, and the console is clean. The old and new `.map` URLs and `/service-worker.js` return 404.
- [x] After merge, re-count open Dependabot alerts and record the number here:
  `MSYS_NO_PATHCONV=1 gh api "repos/khaney64/NLEastChallenge/dependabot/alerts?state=open&per_page=100" --paginate --jq length`.
  - 2026-10-22, after deploy: still 86, but every alert was last updated 2026-10-21. GitHub's dependency graph had not picked up the merged lockfile (its SBOM still listed `react-scripts`), and `npm audit` on it reports 0 vulnerabilities.
  - 2026-10-22, after the dependency graph caught up: **10 open** (down from 86). See item 3.
- [x] After merge, confirm alerts **#239** (CVE-2026-111755, GHSA-8r9p-f939-6h3c) and **#240** (CVE-2026-111756, GHSA-q3jh-wrc6-jgq8) show as fixed. Both closed as fixed 2026-10-22 20:37 UTC. Both are high-severity `follow-redirects` 1.16.0 credential-header leaks, fixed in 1.16.1. Added 2026-10-22. `follow-redirects` came in only through the CRA dev server's proxy chain (`http-proxy`); the `vite-migration` lockfile no longer contains it (`npm ls follow-redirects` is empty), so the migration closes both. If they stay open, add `"follow-redirects": ">=1.16.1"` to `overrides` (item 3).

### 2. Stop publishing the JavaScript source map: `done`

Covered by `build.sourcemap: false` in item 1. Since the 2026-10-22 deploy, `/static/js/main.51a12831.js.map` and `/assets/*.map` return 404 on production.

### 3. Clear remaining Dependabot alerts: `in progress`, waiting for Dependabot to auto-close

Triage whatever is left after the migration. Bump direct dependencies, use npm `overrides` for transitives, and dismiss with a written reason anything dev-only that has no fix.

2026-10-22 triage of the 10 open alerts: none needs a code change. The alerts were last updated at 20:37 UTC. The dependency graph rebuilt at 20:39 is correct (378 npm packages, `vite` 8.2.4, no `react-scripts`), and `npm audit` reports 0.

| Alerts | Package | Current lockfile |
|---|---|---|
| #226 | `brace-expansion` | 1.1.21 (fixed version), via `eslint-plugin-react` |
| #220, #221 | `js-yaml` | 4.3.2 (fixed version), via `eslint` |
| #217 | `baseline-browser-mapping` | 2.11.30 (fix is 2.11.0), via `eslint-plugin-react-hooks` |
| #216, #218, #219, #222, #223, #225 | `colord`, `svgo` ×2, `fast-uri`, `postcss-selector-parser`, `compression` | not installed (CRA only) |

Next: if they are still open after Dependabot's next pass, dismiss each as "Inaccurate" with the reason from the table. Not dismissed yet, by choice.

### 4. Upgrade React 17 → 18/19 and react-router 5 → 7: `todo`, after 1

React 17 and react-router 5 are out of active support. Reactstrap 8 also needs a major bump (9.x) to work with React 18 and Bootstrap 5.

### 5. .NET maintenance: `in progress` (branch `dotnet10`; production deploy remaining)

- [x] .NET 8 LTS support ends 2026-11-10. Retarget to `net10.0` (both `NLEastChallenge` and `NLEastTests`).
- [x] Remove `Microsoft.AspNetCore.Http.Features` 5.0.17. It is an out-of-support 5.0-era package; the types come from the shared framework on net8+.
- [x] Fix the `Microsoft.Extensions.*` 9.0.2 references, which resolve to 9.0.3 with NU1603 warnings. Move to versions matching the target framework.
  - 2026-10-22: removed `Microsoft.Extensions.Caching.Memory` and `Microsoft.Extensions.Primitives` entirely. The ASP.NET Core shared framework ships both at the framework's version, so the explicit references were only pinning them out of step. The NU1603 warnings are gone.
- [x] `Microsoft.AspNetCore.SpaProxy` 8.0.13 → 10.0.13, to match the framework.
- [x] Verify locally (2026-10-22):
  - `dotnet build`: 0 errors. The 6 remaining warnings are the existing nullable warnings in the controllers.
  - `dotnet test`: 11/11 pass on net10.0.
  - `dotnet list package --vulnerable --include-transitive --source https://api.nuget.org/v3/index.json`: none.
  - `dotnet run`: the SPA proxy serves on 44403, and `/divisiondata` (both modes) and `/gamedata/*` return 200.
  - `dotnet publish -r linux-x64`: runtimeconfig targets `net10.0`.
- [ ] Deploy. Switch the App Service runtime to .NET 10 **in the same step as the deploy**. Each Linux runtime image ships only its own .NET version: the net10 build won't start on `DOTNETCORE|8.0`, and the running net8 build won't start on `DOTNETCORE|10.0`. Change both together:
  ```
  az webapp config set -g NLEastRG -n NLEastChallenge --subscription 3d6572f6-2e54-4530-a484-2847d25e01fa --linux-fx-version "DOTNETCORE|10.0"
  ```
  then `az webapp deploy … --clean true` as in item 1. Rollback: set `DOTNETCORE|8.0` and redeploy `main` from before this change.

### 6. Verify App Service configuration: `todo`

Log in to the subscription that hosts `nleastchallenge` (`az login --tenant 55117e3a-e992-480d-8f42-095cd729530a`; see item 1) and confirm:

- runtime stack matches the target framework after item 5
- `httpsOnly: true`
- `minTlsVersion` is 1.2 or higher
- `ftpsState` is `Disabled` or `FtpsOnly`
- no unexpected app settings or secrets

Seen 2026-10-22 while deploying: runtime is `DOTNETCORE|8.0`, and both SCM and FTP basic-auth publishing credentials are allowed. SCM basic auth is what the Visual Studio profile uses, but `az webapp deploy` doesn't need it. If nothing else needs them, consider disabling both.

### 7. Developer machine Node.js: `done`

2026-10-21: workstation upgraded to Node 24.22.1 (patched LTS) and nvm-windows removed.
