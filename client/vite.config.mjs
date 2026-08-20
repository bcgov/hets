import { defineConfig, loadEnv } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '');

  return {
    plugins: [react()],
    envPrefix: ['VITE_', 'REACT_APP_'],
    server: {
      open: false,
      proxy: {
        '/api': {
          target: env.REACT_APP_API_HOST || 'http://localhost:27239',
          changeOrigin: true,
        },
      },
    },
    build: {
      outDir: 'build',
      sourcemap: env.GENERATE_SOURCEMAP === 'true',
    },
    test: {
      environment: 'jsdom',
      globals: true,
      setupFiles: './src/setupTests.js',
    },
  };
});
