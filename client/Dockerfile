# Use the official Node.js image
FROM node:18 AS build
WORKDIR /app
COPY client/package.json ./
COPY client/package-lock.json ./
RUN npm install
COPY client/ ./
RUN npm run build

# Serve the application
FROM nginx:alpine
COPY --from=build /app/build /usr/share/nginx/html
EXPOSE 80
