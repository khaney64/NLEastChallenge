import js from '@eslint/js';
import react from 'eslint-plugin-react';
import reactHooks from 'eslint-plugin-react-hooks';
import globals from 'globals';

export default [
  { ignores: ['build', 'node_modules', 'coverage'] },
  {
    files: ['**/*.{js,jsx,mjs}'],
    languageOptions: {
      ecmaVersion: 2022,
      sourceType: 'module',
      parserOptions: {
        ecmaFeatures: { jsx: true },
      },
      globals: {
        ...globals.browser,
        ...globals.node,
      },
    },
    plugins: {
      react,
      'react-hooks': reactHooks,
    },
    rules: {
      ...js.configs.recommended.rules,
      ...react.configs.recommended.rules,
      ...reactHooks.configs.recommended.rules,
      'react/prop-types': 'off',
      'react/no-unknown-property': 'warn',
      'no-use-before-define': 'off',
      'no-unused-vars': ['warn', { args: 'none', ignoreRestSiblings: true }],
    },
    settings: {
      react: { version: 'detect' },
    },
  },
  {
    files: ['**/*.test.{js,jsx}'],
    languageOptions: {
      globals: globals.vitest,
    },
    rules: {
      'react/no-render-return-value': 'off',
    },
  },
];
