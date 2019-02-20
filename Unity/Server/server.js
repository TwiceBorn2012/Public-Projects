var io = require('socket.io')(process.env.PORT || 3000);
var shortid = require('shortid');
var $ = require('jQuery');
var Request = require("request");
var ARequest = require("async-request");
var http = require('http');
var rdata = null;
console.log('server started');

var players = [];

io.on('connection', function (socket){
    console.log('connected');
    
    var thisplayerId = shortid.generate();
    
    players.push(thisplayerId);
    
    socket.on('addPlayerItemToInv', function (data) {
        Request('http://btsdev.azurewebsites.net/WebService.asmx/AddItemToPlayerInventory?itemHash=' + data['id'] + '&playerAccount=' + data['aid'], { json: true }, (err, res, body) => {
          if (err) { return console.log(err); }
            console.log("Added " + data['id'] + " to player " + data['aid']);
            //socket.emit('updatePlayerInventory', body);
        });
        
    })
    
    socket.on('spawn', function (data) { //FAKE
        var thisplayerId = shortid.generate();
    
        players.push(thisplayerId);

        console.log('new client connected, broadcasting spawn ' + thisplayerId + " to " + (players.length - 1));

        socket.broadcast.emit('spawn', {id: thisplayerId });
        socket.broadcast.emit('requestPosition');

        players.forEach(function (playerId){
            if (thisplayerId === playerId){
                return;
            }

            socket.emit('spawn', {id: playerId });
            console.log('sending spawn of old player: ' + playerId);

        });
    })
    
    socket.on('move', function(data){
        //data.id = thisplayerId;
        //console.log('client moved', JSON.stringify(data));
        socket.broadcast.emit('move', data);
    })

    socket.on('updatePosition', function (data) {
        data.id = thisplayerId;
        socket.broadcast.emit('updatePosition', data);
    })
    
    socket.on('disconnect', function () {
       console.log('disconnected'); 
    });
    //socket.on('disconnect', function () {
        //console.log('client disconnected: ' + thisplayerId);
        //for( var i = 0; i < players.length; i++){ 
          // if ( players[i] === thisplayerId) {
             //players.splice(i,1)
           //}
        //}
        //socket.broadcast.emit('disconnected', {id: thisplayerId });
    //})
})