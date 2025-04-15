import { defineConfig } from "vite";
import react from "@vitejs/plugin-react-swc";
import path from "node:path";
import os from "node:os";
import fs from "fs";

const homedir = os.homedir();

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5002,
    https: {
      key: fs.readFileSync(path.join(homedir, ".ssl/localhost/localhost-key.pem")),
      cert: fs.readFileSync(path.join(homedir, ".ssl/localhost/localhost.pem")),
    },
  },
});
