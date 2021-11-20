const xmlrpc = require("davexmlrpc");

var config = {
    port: 1417,
    xmlRpcPath: "/rpc2"
}

xmlrpc.startServerOverHttp(config, function (request) {
    switch (request.verb) {
        case "uppercase":
            if (request.params.length > 0) {
                request.returnVal(undefined, request.params[0].toUpperCase());
            }
            else {
                request.returnVal({ message: "There must be at least one parameter." });
            }
            return (true); //we handled it
    }
    return (false); //we didn't handle it
});