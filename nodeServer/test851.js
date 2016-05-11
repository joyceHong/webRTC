//node.js server code
//assume ws.io installed in ./ws.io

var app = require('http').createServer(handler),
	io = require('./ws.io').listen(app),
	fs = require('fs'),
  	url = require('url');

app.listen(8443);

function handler (req, res) {
	var filename = '';
	var resource = url.parse(req.url).pathname;
	switch(resource) {
		case '/ws.io/ws.io.js':
			res.setHeader('Content-Type', 'text/javascript');
			filename = __dirname + resource;
			break;
		case '/js/jquery-1.8.2.js':
			res.setHeader('Content-Type', 'text/javascript');
			filename = __dirname + resource;
			break;
		default:
			res.setHeader('Content-Type', 'text/html');
			filename = __dirname + '/test851.html';
			break
	}
	
	fs.readFile(filename, function (err, data) {
		if (err) {
			res.writeHead(500);
			return res.end('Error loading index.html');
		}
		res.writeHead(200);
		res.end(data);
	});
}

io.sockets.on('connection', function (socket) {
	socket.on('offer', function(data) {
		console.log('offer', data);
		socket.broadcast.emit('offer', {sdp: data.sdp});
	});
  
	socket.on('answer', function(data) {
		console.log('answer', data);
		socket.broadcast.emit('answer', {sdp: data.sdp});
	});
	
	socket.on('ice', function(data) {
		socket.broadcast.emit('ice', {candidate: data.candidate});
	});
	
	socket.on('startice', function(data) {
		socket.broadcast.emit('startice', {});
	});

	socket.on('hangup', function() {
		socket.broadcast.emit('hangup', {});
	});
});

io.of('/echo').on('connection', function(socket) {
	socket.on('echo', function(data) {
		socket.emit('echo', data);
	});
});