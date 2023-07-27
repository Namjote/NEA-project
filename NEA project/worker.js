importScripts('C:\Users\namjo\OneDrive\Documents\Namjote.S.Dulay\A-levels\Computer_Science\Year 13\NEA Namjote Dulay\NEA project\NEA project\Stockfish js\stockfish.js');

// Create the Stockfish instance
var stockfish = STOCKFISH();

// Set up the worker message handler
onmessage = function (event) {
    stockfish.postMessage(event.data);
};

stockfish.onmessage = function (event) {
    postMessage(event.data);
};