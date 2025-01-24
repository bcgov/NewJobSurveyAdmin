
# Stage 1: Build
FROM docker.io/node:22-alpine AS builder
ENV NODE_ENV production
WORKDIR /opt/app-root/src

# Copy everything and yarn build
COPY . .
RUN yarn install --frozen-lockfile --production && yarn build

# Stage 2: Build a runtime image
FROM nginxinc/nginx-unprivileged:alpine

COPY --from=builder /opt/app-root/src/build/ /usr/share/nginx/html
COPY --from=builder /opt/app-root/src/config/__ENV.js /usr/share/nginx/html/config/__ENV.js

#RUN yarn global add serve
EXPOSE 8080
CMD ["nginx","-g","daemon off;"]