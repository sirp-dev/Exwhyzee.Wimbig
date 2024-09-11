"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/raffleHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on(, function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});
