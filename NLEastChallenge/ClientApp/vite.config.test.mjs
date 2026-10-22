// @vitest-environment node
import fs from 'node:fs';
import path from 'node:path';
import { afterEach, describe, expect, it, vi } from 'vitest';
import configFn from './vite.config.mjs';

const resolve = (command, mode = 'test') => configFn({ command, mode });

describe('vite.config', () => {
  const originalName = process.env.npm_package_name;

  afterEach(() => {
    vi.restoreAllMocks();
    if (originalName === undefined) {
      delete process.env.npm_package_name;
    } else {
      process.env.npm_package_name = originalName;
    }
  });

  it('sets an explicit build target so browser support is not narrowed to the Vite default', () => {
    expect(resolve('build').build.target).toEqual(['es2020', 'edge88', 'firefox78', 'chrome87', 'safari14']);
  });

  it('derives the dev cert file name from npm_package_name like aspnetcore-https.js', () => {
    process.env.npm_package_name = 'renamed-app';
    const exists = vi.spyOn(fs, 'existsSync').mockReturnValue(true);
    vi.spyOn(fs, 'readFileSync').mockImplementation((p) => `contents of ${path.basename(p)}`);

    const config = resolve('serve');

    expect(exists).toHaveBeenCalledWith(expect.stringMatching(/renamed-app\.pem$/));
    expect(config.server.https).toEqual({ cert: 'contents of renamed-app.pem', key: 'contents of renamed-app.key' });
  });

  it('falls back to the nleastchallenge cert name when npm_package_name is unset', () => {
    delete process.env.npm_package_name;
    const exists = vi.spyOn(fs, 'existsSync').mockReturnValue(false);
    vi.spyOn(console, 'warn').mockImplementation(() => {});

    resolve('serve');

    expect(exists).toHaveBeenCalledWith(expect.stringMatching(/nleastchallenge\.pem$/));
  });

  it('warns when serving without the dev cert', () => {
    vi.spyOn(fs, 'existsSync').mockReturnValue(false);
    const warn = vi.spyOn(console, 'warn').mockImplementation(() => {});

    const config = resolve('serve', 'development');

    expect(config.server.https).toBeUndefined();
    expect(warn).toHaveBeenCalledWith(expect.stringContaining('dev certificate not found'));
  });

  it('does not warn about a missing dev cert when Vitest resolves the config in test mode', () => {
    vi.spyOn(fs, 'existsSync').mockReturnValue(false);
    const warn = vi.spyOn(console, 'warn').mockImplementation(() => {});

    const config = resolve('serve', 'test');

    expect(config.server.https).toBeUndefined();
    expect(warn).not.toHaveBeenCalled();
  });

  it('does not warn about a missing dev cert during build', () => {
    vi.spyOn(fs, 'existsSync').mockReturnValue(false);
    const warn = vi.spyOn(console, 'warn').mockImplementation(() => {});

    const config = resolve('build');

    expect(config.server.https).toBeUndefined();
    expect(warn).not.toHaveBeenCalled();
  });
});
