import fs from 'node:fs';
import path from 'node:path';
import { env } from 'node:process';
import { defineConfig, configDefaults } from 'vitest/config';
import react from '@vitejs/plugin-react';

// aspnetcore-https.js (prestart) exports the ASP.NET Core dev certificate here, named after the npm package.
function loadDevCert() {
  const certFolder = env.APPDATA ? path.join(env.APPDATA, 'ASP.NET', 'https') : path.join(env.HOME ?? '', '.aspnet', 'https');
  const certName = env.npm_package_name ?? 'nleastchallenge';
  const certFilePath = path.join(certFolder, `${certName}.pem`);
  const keyFilePath = path.join(certFolder, `${certName}.key`);
  if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
    return { certFilePath, https: undefined };
  }
  return { certFilePath, https: { cert: fs.readFileSync(certFilePath), key: fs.readFileSync(keyFilePath) } };
}

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:3078';

const backend = { target, secure: false };

export default defineConfig(({ command }) => {
  const { certFilePath, https } = loadDevCert();
  if (command === 'serve' && !https) {
    console.warn(`ASP.NET Core dev certificate not found at ${certFilePath}; the dev server is running over HTTP, so the SPA proxy (https://localhost:44403) will fail to connect. Run "npm start" so aspnetcore-https.js exports it.`);
  }

  return {
    plugins: [react()],
    build: {
      outDir: 'build',
      sourcemap: false,
      // Explicit target keeps the pre-Vite-7 "modules" browser reach instead of Vite's narrower baseline default.
      // Vite does not read the browserslist block in package.json.
      target: ['es2020', 'edge88', 'firefox78', 'chrome87', 'safari14'],
    },
    server: {
      port: 44403,
      strictPort: true,
      https,
      proxy: {
        '/divisiondata': backend,
        '/gamedata': backend,
      },
    },
    test: {
      environment: 'jsdom',
      globals: true,
      exclude: [...configDefaults.exclude, 'build/**'],
    },
  };
});
