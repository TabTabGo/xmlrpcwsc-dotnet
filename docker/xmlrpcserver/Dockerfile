FROM node:16
WORKDIR /app
COPY server.js /app/
RUN npm install davexmlrpc
ENTRYPOINT [ "node", "server.js" ]