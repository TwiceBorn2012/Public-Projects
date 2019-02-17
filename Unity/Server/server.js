var io = require('socket.io')(process.env.PORT || 3000);
var shortid = require('shortid');

console.log('server started');

var players = [];

io.on('connection', function (socket){
    console.log('connected');
    socket.on('auth', function (data) {
        console.log(data);
        data.message = "Nice try authenticating";
        socket.emit('auth', data);
    })
    
    socket.on('trigger', function (data) { //FAKE
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
        data.id = thisplayerId;
        console.log('client moved', JSON.stringify(data));
        socket.broadcast.emit('move', data);
    })
    
    socket.on('updatePosition', function (data) {
        data.id = thisplayerId;
        socket.broadcast.emit('updatePosition', data);
    })
    
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