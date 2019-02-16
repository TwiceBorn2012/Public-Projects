var io = require('socket.io')(process.env.PORT || 3000);

console.log('server started');

var playerCount = 0;

io.on('connection', function (socket){
    console.log('client connected, broadcasting spawn');
    
    
    socket.broadcast.emit('spawn');
    playerCount++;
    
    for(i=0; i< playerCount; i++){
        socket.broadcast.emit('spawn');
        console.log('sending spawn to new player');
    }
    
    socket.on('move', function(data){
        console.log('client moved', JSON.stringify(data));
        socket.broadcast.emit('move', data);
    })
    
    socket.on('disconnect', function () {
        console.log('client disconnected');
        playerCount--;
    })
})