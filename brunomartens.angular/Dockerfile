# Use a lightweight Node.js image
FROM node:24-alpine AS builder

# Set the working directory inside the container
WORKDIR /app

# Copy package-related files first to leverage Docker's caching mechanism
COPY brunomartens.angular/package.json brunomartens.angular/package-lock.json ./

# Install project dependencies using npm ci (ensures a clean, reproducible install)
RUN --mount=type=cache,target=/root/.npm npm ci
#   RUN npm i -g @angular/cli

# Copy the rest of the application source code into the container
COPY ./brunomartens.angular .

RUN ls -a
# Build the Angular application
RUN npm run build portal

# =========================================
# Stage 2: Prepare Nginx to Serve Static Files
# =========================================

FROM nginx:alpine AS runner

# Use a built-in non-root user for security best practices
# USER nginx

# Copy custom Nginx config
COPY brunomartens.angular/nginx/nginx.conf /etc/nginx/nginx.conf

# Copy the static build output from the build stage to Nginx's default HTML serving directory
COPY --chown=nginx:nginx --from=builder /app/dist/*/browser /usr/share/nginx/html

# Expose port 8080 to allow HTTP traffic
# Note: The default NGINX container now listens on port 8080 instead of 80 
EXPOSE 8080

# Start Nginx directly with custom config
ENTRYPOINT ["nginx", "-c", "/etc/nginx/nginx.conf"]
CMD ["-g", "daemon off;"]