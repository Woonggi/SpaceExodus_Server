const express = require('express');
const app = express();
const http = require('http');
const server = http.createServer(app);
const WebSocketServer = require('websocket').server;

const port = 8080;

server.listen(port, () => {
    console.log(`Server is listening on port ${port}`);
});

let ws = new WebSocketServer ({
    httpServer: server,
    autoAcceptConnections: false
});
