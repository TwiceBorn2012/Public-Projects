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

    init: function (id, name,  combatState, resCharge, maxCharge, positionx, positiony, positionz, spositionx, spositiony, spositionz) {
        this.id = id;
        this.name = name;
        this.combatState = combatState;
        this.resCharge = resCharge;
        this.maxCharge = maxCharge;
        this.positionx = positionx;
        this.positiony = positiony;
        this.positionz = positionz;
        this.spositionx = spositionx;
        this.spositiony = spositiony;
        this.spositionz = spositionz;
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
    enemy.init(i, thisTurtle.name, "Server", "A", thisTurtle.maxCharge, thisTurtle.spawnPointx, thisTurtle.spawnPointy, thisTurtle.spawnPointz, thisTurtle.spawnPointx, thisTurtle.spawnPointy, thisTurtle.spawnPointz);
    enemies.push(enemy);
    
}


setInterval(function () {
    
    for( var i = 0; i < enemies.length; i++){ 
    
        var thisTurtle = enemies[i];
        
        
        if(thisTurtle.combatState == 'Server'){
            
            var pos = RandomTurtleMovement();
            
            var newX = parseFloat(thisTurtle.positionx) + parseFloat(pos[0]);
            var newY = parseFloat(thisTurtle.positiony) + parseFloat(pos[1]);
            var newZ = parseFloat(thisTurtle.positionz) + parseFloat(pos[2]);
            
            thisTurtle.positionx = newX;
            thisTurtle.positiony = newY;
            thisTurtle.positionz = newZ;
            
            
    
            // socket broadcast Emit new positions for each enemy full Array 
        }        
    }
    
}, 5000)
 




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
        
        var enemiesArray = enemies;
        
        for( var i = 0; i < enemiesArray.length; i++){ 
        
            // if the enemy value for resCharge is "A",
            // spawn the enemy at its position
            var e = enemiesArray[i]
            if(e.resCharge == "A"){
                socket.emit('spawnEnemy', { id: e.id, name: e.name, combatState: e.combatState, posX: e.positionx, posY: e.positiony, posZ: e.positionz });
            }
            
        }
        
    })
    
    socket.on('requestStateEnemy', function(data){
        var turtleID = data['id'];
        var e = enemies[turtleID];
        socket.emit('requestStateEnemy', { id: e.id, name: e.name, combatState: e.combatState, posX: e.positionx, posY: e.positiony, posZ: e.positionz });
    })
    
    socket.on('requestPositionEnemy', function(data){
        var turtleID = data['id'];
        var e = enemies[turtleID];
        socket.emit('requestPositionEnemy', { id: e.id, name: e.name, combatState: e.combatState, posX: e.positionx, posY: e.positiony, posZ: e.positionz });
    })
    
    socket.on('requestTargetEnemy', function(data){
        var turtleID = data['id'];
        var e = enemies[turtleID];
        socket.emit('requestTargetEnemy', { id: e.id, name: e.name, combatState: e.combatState, posX: e.positionx, posY: e.positiony, posZ: e.positionz });
    })
    
    socket.on('updatePositionEnemy', function(data){
        var turtleID = data['id'];
        var e = enemies[turtleID];
        e.positionx = data['posX'];
        e.positiony = data['posY'];
        e.positionz = data['posZ'];
        
    })
    
    socket.on('updateStateEnemy', function(data){
        var turtleID = data['id'];
        var e = enemies[turtleID];
        e.combatState = 'Aggro';
    })
    
    socket.on('updateTargetEnemy', function(data){
        var turtleID = data['id'];
        var e = enemies[turtleID];

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

function RandomTurtleMovement(){
    
    var i = getRandomInt(5);
    
    var pos = new Array;
    
    if (i == 0){
        pos.push("0");
        pos.push("0");
        pos.push("0");
        return pos
    }
    if (i == 1){
        pos.push("2");
        pos.push("0");
        pos.push("0");
        return pos
    }
    if (i == 2){
        pos.push("-2");
        pos.push("0");
        pos.push("0");
        return pos
    }
    if (i == 3){
        pos.push("0");
        pos.push("0");
        pos.push("2");
        return pos
    }
    if (i == 4){
        pos.push("0");
        pos.push("0");
        pos.push("-2");
        return pos
    }
 
}

function getRandomInt(max) {
    return Math.floor(Math.random() * Math.floor(max));
}