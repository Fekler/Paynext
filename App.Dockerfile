# Etapa 1: Build
FROM node:20-alpine AS builder
WORKDIR /app
COPY src/Frontend/package.json src/Frontend/package-lock.json ./
RUN npm ci
COPY src/Frontend .
RUN npm run build

# Etapa 2: Servir o build
FROM node:20-alpine AS runner
WORKDIR /app
COPY --from=builder /app/package.json ./
COPY --from=builder /app/node_modules ./node_modules
COPY --from=builder /app/dist ./dist
COPY --from=builder /app/vite.config.ts ./vite.config.ts
EXPOSE 4173
CMD ["npm", "run", "preview"]
