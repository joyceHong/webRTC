var edge = require('./edge/lib/edge.js');
var helloWorld;
//try {
//    helloWorld = edge.func('ClassLibrary2.dll');
//}
//catch (error) { 
//    console.log(error);
//}

var http = require('http');
fs = require('fs')
var url = require('url');
var requestListener = function (req, res) {
    var path = url.parse(req.url).pathname;    
    switch (path) {
        case '/':
            res.writeHead(200, { 'Content-Type': 'text/html' });
            res.write('<head><meta charset="utf-8"/></head>');
           
            try {
                //var exec = require('child_process').execFile;                
                //var fun = function () {
                //    console.log("fun() start");
                //    exec('ConsoleApplication1.exe', function (err, data) {
                //        console.log(err)
                //        console.log(data.toString());
                //    });
                //}
                //fun();

                var mongo = require('mongodb');
                var Server = mongo.Server;
                var Db = mongo.Db;                
                var server = new Server('192.168.1.63', 27017, { auto_reconnect: true });
                var db = new Db('cooper', server);     
                var htmlCode = "<table border=1><tr><th>病歷編號</th></tr>";
                db.open(function () {
                    db.collection('patient', function (err, collection) {
                        collection.find().toArray(function (err, items) {
                            //res.write(items[0].醫師代號);                                                                
                            for (var i = 0; i <= items.length - 1; i++) {
                                htmlCode+="<tr>";
                                htmlCode +="<td>"+items[i].病歷編號+"</td>";  
                                htmlCode +="</tr>";
                            }
                            htmlCode += "</table>";
                            res.write(htmlCode);
                            res.end();
                        });
                    });                  
                });


                //var filename = "./test.txt";  //讀取檔案時，不需寫檔案.txt                                         
                //fs.readFile(filename,'utf8', function (err, data) {                   
                //    //var jsonObj = JSON.parse(data);
                //    if (err) {
                //        return console.log(err);
                //    }
                //    //var a = '{"STATUS":[{"STATUS":"S","When":1394045650,"Code":17,"Msg":"GPU0","Description":"cgminer 3.7.3"}],"GPU":[{"GPU":0,"Enabled":"Y","Status":"Alive","Temperature":70.00,"Fan Speed":3090,"Fan Percent":70,"GPU Clock":1180,"Memory Clock":1500,"GPU Voltage":1.206,"GPU Activity":99,"Powertune":20,"MHS av":0.4999,"MHS 5s":0.5007,"Accepted":4841,"Rejected":8,"Hardware Errors":0,"Utility":28.0261,"Intensity":"0","Last Share Pool":0,"Last Share Time":1394045638,"Total MH":5181.3734,"Diff1 Work":77548,"Difficulty Accepted":77456.00000000,"Difficulty Rejected":128.00000000,"Last Share Difficulty":16.00000000,"Last Valid Work":1394045638,"Device Hardware%":0.0000,"Device Rejected%":0.1651,"Device Elapsed":10364}],"id":1}';
                //    //var b = new Buffer(a);
                //    //var objectArray = JSON.parse(b.toString());
                //    //console.log(objectArray.STATUS[0].STATUS);
                //    var b = new Buffer(data);
                //    //var json = JSON.strigify(b.toString());
                //    var objectArray = JSON.parse(b.toString());
                //    var keys = Object.keys(objectArray[0]);
                //    console.log(keys);
                //    //console.log(objectArray[0].醫師代號);                    
                //});
                
            } catch (error) {                
                console.log(error);
            }                      
            break;
        case '/a1.html':
            res.writeHead(200);
            fs.readFile('a1.html', 'utf8', function (err, data) {
                if (err) {
                    return console.log(err);
                }
                res.end(data);
            });
            break;
        default:
            res.writeHead(404);
            res.write("opps this doesn't exist - 404");
            res.end();
            break;
     
    }
};

var server = http.createServer(requestListener, function (err) {
    console.log(err);
});

server.listen(1234);


