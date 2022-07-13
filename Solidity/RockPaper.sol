pragma solidity ^0.8.6;

import "./Randomer.sol";

contract Game is Rand {
    enum Move {
        None, Rock, Paper, Scisor
    }

    enum Result {
        Lost, Won, Draw
    }

    struct State {
        address player1;
        address player2;
        uint256 onTable;
        uint8 movesRemaining;
    }
    mapping(uint256 => mapping(address => Move)) gameMoves;
    uint256 nonce = 0;

    constructor() Rand(block.number) {
        balances[0xE573eEF9d644Ad481e2E2E16d84EA6a2F838066F] = 256;
    }

    mapping(address => uint256) public balances;
    mapping(uint256 => State) public games;

    event GameStarted(address player, address opponant, uint256 gameId);
    event GameEnded(address player, address opponant, Result _result, uint amount, Move pMove, Move oMove);

    function PlayAgaintBlochchain(address _player, uint256 _bet, Move _move) public payable {
        Result chance = Result(random() % 3);
        if(chance == Result.Won) {
            balances[_player] += _bet;
        } else if (chance == Result.Lost) {
            balances[_player] -= _bet;
        }
        emit GameEnded(_player, address(0), chance, _bet, Move.None, Move.None);

    }

    function StartPVP(address player1, address player2, uint256 bet) public {
        require(player1 != player2, "cannot play againt yourself");
        require(balances[player1] > bet && balances[player2] > bet);
        require(bet > 0, "bet must positive");
        balances[player1] -= bet;
        balances[player2] -= bet;
        games[nonce++] = State(player1, player2, bet * 2, 2);
    }

    function PlayePVP(uint256 gameId, address player, Move move) public {
        require(gameMoves[gameId][player] == Move.None, "player already made a move");
        require(games[gameId].movesRemaining > 0);
        games[gameId].movesRemaining--;
        gameMoves[gameId][player] = move;

        if(games[gameId].movesRemaining == 0) {
            EndPVP(gameId, games[gameId].player1, games[gameId].player2);
        }
    }

    function EndPVP(uint256 gameId, address player1, address player2) public {
        require(games[gameId].movesRemaining == 0);
        Move player1Move = gameMoves[gameId][player1];
        Move player2Move = gameMoves[gameId][player2];
        Result result = CheckEndGame(player1Move, player2Move);
        if(result == Result.Won) {
            balances[player1] += games[gameId].onTable;
        }

        if(result == Result.Lost) {
            balances[player2] += games[gameId].onTable;
        }

        if(result == Result.Draw) {
            uint256 originalBet = games[gameId].onTable / 2;
            balances[player2] += originalBet;
            balances[player1] += originalBet;
        }
        emit GameEnded(player1, player2, result, games[gameId].onTable, player1Move, player2Move);

    }

    function CheckEndGame(Move _move, Move move) private returns (Result) {
        if(     move == Move.Paper  && _move == Move.Scisor 
            ||  move == Move.Scisor && _move == Move.Rock
            ||  move == Move.Rock   && _move == Move.Paper) {
                return Result.Won;
        } else if(move == _move){
                return Result.Draw;
        } else {
                return Result.Lost;
        }
    }

    function getMove(uint256 _moveId) private returns(Move) {
        require(_moveId < 3);
        if(_moveId == 0) return Move.Rock;
        if(_moveId == 1) return Move.Paper;
        if(_moveId == 2) return Move.Scisor;
    } 


}