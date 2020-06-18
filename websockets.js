
var oldLog = console.log;
console.log = function (message) {
    if(message.indexOf("label = [debug]") !== -1)
        return;
    oldLog.apply(console, arguments);
};

var  connection = {
	
	ws: null, 
	
	loginInClient: false,
	loginInSocket: false,
	socketOpen: false,
	initSocket: function(){
		
		connection.ws = new WebSocket("ws://127.0.0.1:8181");
		connection.ws.binaryType= 'arraybuffer';

		connection.ws.onmessage = function(evt){
			var bytes = new Uint8Array(event.data);    
			var serverPacket = new ServerPacket(bytes);
			switch(serverPacket._header)
			{
				case 1:
				{
					connection.loginInSocket = true;
					console.log('Login in socket');
					break;
				}
				case 2: // the same packet that used to be "123|Keiz|Hi y'all"
				{
					$('#exampleModal #exampleModalLabel').text(serverPacket.popString());
					$('#exampleModal .modal-body').text(serverPacket.popString());
					$('#exampleModal').modal('toggle');
					
					$(document).on('click', '#exampleModal #lokoshon', function(){
						var clientPacket = new ClientPacket(2);
						clientPacket.writeInteger(324123434);
						connection.sendMessage(clientPacket);
					});
				   break;
				}
			}
			
			
			
		}
		connection.ws.onopen = function(){
			console.log('connection open');
			connection.socketOpen = true;
		}
		connection.ws.onclose = function(){
			console.log('close');
			connection.loginInSocket = false;
			
			connection.socketOpen = false;
		}

		connection.ws.onerror = function(){
			console.log('error');
			connection.loginInSocket = false;
			
			connection.socketOpen = false;
		}

	},
	
	sendMessage: function(packet){
		console.log('send packet');
		if(connection.socketOpen){
			connection.ws.send(packet.getBytes());
		console.log('send packet');
		}
	}
}

$(document).ready(function(){
	connection.initSocket();
});

window.FlashExternalInterface.legacyTrack = function(n, e, a) {
	console.log('otro log');
	if(n === 'authentication' && e === 'authok'){
	console.log('otro log');
		//cuando hace login el usuario
		connection.loginInClient = true;
		var clientPacket = new ClientPacket(1);
		clientPacket.writeInteger(1);
		clientPacket.writeString('customekizd');
		connection.sendMessage(clientPacket);
	}

	window.HabboFlashClient.started = !0, window.HabboTracking.track(n, e, a)
}