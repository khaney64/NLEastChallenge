import fs from 'node:fs';
import path from 'node:path';
import { env } from 'node:process';
import { defineConfig, configDefaults } from 'vitest/config';
import react from '@vitejs/plugin-react';

// aspnetcore-https.js (prestart) exports the ASP.NET Core dev certificate here.
const certFolder = env.APPDATA ? path.join(env.APPDATA, 'ASP.NET', 'https') : path.join(env.HOME ?? '', '.aspnet', 'https');
const certFilePath = path.join(certFolder, 'nleastchallenge.pem');
const keyFilePath = path.join(certFolder, 'nleastchallenge.key');
const https = fs.existsSync(certFilePath) && fs.existsSync(keyFilePath)
  ? { cert: fs.readFileSync(certFilePath), key: fs.readFileSync(keyFilePath) }
  : undefined;

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:3078';

const backend = { target, secure: false };

export default defineConfig({
  plugins: [react()],
  build: {
    outDir: 'build',
    sourcemap: false,
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
});
