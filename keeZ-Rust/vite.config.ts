import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";

const host = process.env.TAURI_DEV_HOST;

export default defineConfig(async () => ({
  plugins: [vue()],

  // Prevent Vite from obscuring Rust compilation errors
  clearScreen: false,

  server: {
    port: 1420,
    strictPort: true,
    host: host || false,
    hmr: host
      ? {
          protocol: "ws",
          host,
          port: 1421,
        }
      : undefined,
    watch: {
      // Tell Vite to ignore changes in the Tauri src-tauri directory;
      // cargo handles those rebuilds separately
      ignored: ["**/src-tauri/**"],
    },
  },

  // Expose VITE_ and TAURI_ENV_* env vars to the frontend
  envPrefix: ["VITE_", "TAURI_ENV_*"],

  build: {
    // Tauri requires these specific targets
    target:
      process.env.TAURI_ENV_PLATFORM === "windows"
        ? "chrome105"
        : "safari13",
    // Do not minify for debug builds
    minify: !process.env.TAURI_ENV_DEBUG ? "esbuild" : false,
    sourcemap: !!process.env.TAURI_ENV_DEBUG,
  },
}));
