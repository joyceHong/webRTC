var express = require('express');
var app = express();
app.get('/patients', function (req, res) {
    
    //允許跨源處理的協定
    res.header('Access-Control-Allow-Origin', '*');
    res.header('Access-Control-Allow-Methods', 'GET,PUT,POST,DELETE');
    res.header('Access-Control-Allow-Headers', 'Content-Type');

    var mongo = require('mongodb');
    var Server = mongo.Server;
    var Db = mongo.Db;
    var server = new Server('192.168.1.63', 27017, { auto_reconnect: true });
    var db = new Db('cooper', server);
    db.open(function () {
        db.collection('patient', function (err, collection) {
            collection.find().toArray(function (err, items) {                                
                res.send(items);
            });
        });
    });
});
app.listen(3000);
console.log('Listening on port 3000...');