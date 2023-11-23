var connection = new signalR.HubConnectionBuilder().withUrl("/hubs/chatHub").build();
connection.on("userCount", (count) => {
    $("#count").text(count);
});

function newWindowLoaded() {
    connection.send("UserCount");
}

function success() {
    console.log("Connected");
    newWindowLoaded();
}

function error() {

}
connection.start().then(success,error);