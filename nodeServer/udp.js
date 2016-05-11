var dgram = require("dgram");
var http = require('http');
var fs = require('fs');
var port = "8000";
var ip = "192.168.1.63";
var socket_io = require("socket.io");



function onRequest(request, response) {
    console.log("Request received.");    
    response.writeHead(200, { 'Content-Type': 'text/html' });
    response.write('<head><meta charset="utf-8"/></head>');           
    fs.readFile('index3.html', 'utf8', function (err, data) {
        if (err) {
            return console.log(err);
        }
        response.end(data);
    });
}

var server =http.createServer(onRequest).listen(port, ip);
var io = socket_io.listen(server);
io.sockets.on('connection', function (socket) {
    
    socket.on('offer', function (data) {
        console.log('offer');
        socket.broadcast.emit('offer', { sdp: data.sdp });
        console.log('send offer');
    });

    socket.on('answer', function (data) {
        console.log('answer', data);
        socket.broadcast.emit('answer', { sdp: data.sdp });
    });
	
    socket.on('ice', function (data) {
        console.log('receive ice');
        socket.broadcast.emit('ice', { candidate: data.candidate });
        console.log('complete ice');
    });
    
    socket.on('startice', function (data) {
        socket.broadcast.emit('startice', {});
    });
    
    socket.on('hangup', function () {
        socket.broadcast.emit('hangup', {});
    });
});











