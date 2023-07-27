<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="learn from an ai.aspx.vb" Inherits="NEA_project.lean_from_an_ai" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>

<body>
    <%--<script src="C:/Users/namjo/OneDrive/Documents/Namjote.S.Dulay/A-levels/Computer_Science/Year 13/NEA Namjote Dulay/NEA project/NEA project/Stockfish js/stockfish.js"></script>--%>
    <%--<script src="C:\Users\namjo\OneDrive\Documents\Namjote.S.Dulay\A-levels\Computer_Science\Year 13\NEA Namjote Dulay\NEA project\NEA project\Stockfish js\worker.js"></script>--%>

    <%--<script>
        function startWorker() {
            // Create the Stockfish worker
            var stockfishWorker = new Worker('C:\Users\namjo\OneDrive\Documents\Namjote.S.Dulay\A-levels\Computer_Science\Year 13\NEA Namjote Dulay\NEA project\NEA project\Stockfish js\worker.js');
        
            // Set up the worker message handler
            stockfishWorker.onmessage = function(event) {
                if (event.data.startsWith('bestmove')) {
                    var bestMove = event.data.split(' ')[1];
                    document.getElementById('ResultLabel').innerHTML = 'Best move: ' + bestMove;
                }
            };
        
            // Send the position and depth to the worker
            stockfishWorker.postMessage('position fen ' + document.getElementById('<%=FENTextBox.ClientID %>').value);
            stockfishWorker.postMessage('go depth 20');
        }
    </script>--%>
    <%--<script>
        function startWorker() {
            // Create the Stockfish instance
            var stockfish = STOCKFISH();

            // Set up the Stockfish message handler
            stockfish.onmessage = function (event) {
                if (event.data.startsWith('bestmove')) {
                    var bestMove = event.data.split(' ')[1];
                    document.getElementById('ResultLabel').innerHTML = 'Best move: ' + bestMove;
                }
            };

            // Send the position and depth to Stockfish
            stockfish.postMessage('position fen ' + document.getElementById('<%=FENTextBox.ClientID %>').value);
            stockfish.postMessage('go depth 20');
        }
    </script>
    <form id="form1" runat="server">--%>

    <form runat="server">
    </form>
</body>
</html>
