var io = require('socket.io')(process.env.PORT || 3000);
var shortid = require('shortid');
var $ = require('jQuery');
var Request = require("request");
var ARequest = require("async-request");
var http = require('http');
var rdata = null;

var fs = require('fs');
var enemiesJSON = JSON.parse(fs.readFileSync('\Enemies.json', 'utf8'));

console.log('server started');


var Player = {

    init: function (hash, username, positionx, positiony, positionz) {
        this.hash = hash;
        this.username = username;
        this.positionx = positionx;
        this.positiony = positiony;
        this.positionz = positionz;
    }
};

var Resource = {

    init: function (id, charge, positionx, positiony, positionz) {
        this.id = id;
        this.charge = charge;
        this.positionx = positionx;
        this.positiony = positiony;
        this.positionz = positionz;
    }
};

var Enemy = {

    init: function (id, isInCombat, resCharge, maxCharge, positionx, positiony, positionz) {
        this.id = id;
        this.isInCombat = isInCombat;
        this.resCharge = resCharge;
        this.maxCharge = maxCharge;
        this.positionx = positionx;
        this.positiony = positiony;
        this.positionz = positionz;
    }
};


var players = new Array();

var resources = new Array();

var enemies = new Array();

// Load JSON into Array
var turtles = enemiesJSON.turtles;

for( var i = 0; i < turtles.length; i++){ 

    var thisTurtle = turtles[i];

    var enemy = Object.create(Enemy);
    // Loads enemies into Array in the A state which is alive
    enemy.init(thisTurtle.name, "0", "A", thisTurtle.maxCharge, thisTurtle.spawnPointx, thisTurtle.spawnPointy, thisTurtle.spawnPointz);
    enemies.push(enemy);

}

console.log(enemies);

setInterval(function () {
    
    for( var i = 0; i < enemies.length; i++){ 
    
        console.log(enemies[i].name);
        
    // Each second, check the resCharge for each enemy
    // If enemy's resCharge = Max charge,
    // Get enemy's info from JSON for starting position OR get it from Array
    // Spawn enemy in starting position, change resCharge to "A"
    
    //for( var i = 0; i < turtles.length; i++){ 
        
    //}
    
}, 1000)



io.on('connection', function (socket){
    
    var pH = '';
    
    socket.emit('isConnected', function(data){})
    
    socket.on('playerRegister', function (data){
        pH = data['Hash'];

        var playerHash = data['Hash'];
        var playerUN = data['UserName'];
        var playerx = data['Positionx'];
        var playery = data['Positiony'];
        var playerz = data['Positionz'];

        var player = Object.create(Player);
        // Add player to Array
        player.init(playerHash, playerUN, playerx, playery, playerz);

        players.push(player);
        // Broadcast to all other clients
        socket.broadcast.emit('spawn', { hash: playerHash, username: playerUN, posX: playerx, posY: playery, posZ: playerz });
        
        // Spawn all existing players to client
        players.forEach(function (player){

            if (player.hash === pH){
                
                return;
            }

            socket.emit('spawn', { hash: player.hash, username: player.username, posX: player.positionx, posY: player.positiony, posZ: player.positionz });
            

        });
        
        console.log("Players connected count: " + players.length);
        
        // Get array for enemies
        
        // Spawn each enemy at current position
    })
    
    socket.on('addPlayerItemToInv', function (data) {
        Request('http://btsdev.azurewebsites.net/WebService.asmx/AddItemToPlayerInventory?itemHash=' + data['id'] + '&sGUID=' + data['sGUID'], { json: true }, (err, res, body) => {
          if (err) { return console.log(err); }
            console.log("Added " + data['id'] + " to player " + data['sGUID']);
        });
        
    })

    socket.on('move', function(data){
        var playerHash = data['hash'];
        var playerx = data['positionx'];
        var playery = data['positiony'];
        var playerz = data['positionz'];

        var obj = players.find(x => x.hash === playerHash)
        
        obj.positionx = playerx;
        obj.positiony = playery;
        obj.positionz = playerz;
        
        socket.broadcast.emit('move', { hash: playerHash, posX: playerx, posY: playery, posZ: playerz });
    })
    
    socket.on('disconnect', function () {
        
        for( var i = 0; i < players.length; i++){ 
           if ( players[i].hash === pH) {
             players.splice(i,1)
           }
        }
        
        socket.broadcast.emit('disconnected', { hash: pH });
        Request('http://btsdev.azurewebsites.net/WebService.asmx/RemoveSession?sGUID=' + pH, { json: true }, (err, res, body) => {
          if (err) { return console.log(err); }
            console.log("Ended session: " + pH);
        });
        console.log("Players connected count: " + players.length);
    })
})