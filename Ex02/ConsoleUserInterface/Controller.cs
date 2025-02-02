﻿using System;
using Ex02.Logic;
using Ex02.ConsoleUtils;

namespace Ex02.ConsoleUserInterface
{ 
    public class Controller
    {
        Game m_game;
        Messages m_messages;
        private ShapeWrapper m_playerTurn;
        private ShapeWrapper m_previousTurn;

        public Controller()
        {
            m_messages = new Messages();
            m_playerTurn = new ShapeWrapper('X');
            m_previousTurn = new ShapeWrapper('O');
        }

        public void Run() // Maybe should be static and not object referenced
        {
            bool io_finished = false;
            bool io_keepPlaying = true;
            string o_moveString;
            m_messages.Start();
            CreateNewGame(m_messages.BoardSize, m_messages.PlayerOneName, m_messages.PlayerTwoName);
            printBoard();
            while (!io_finished) 
            {
                
                //turn 1
                m_messages.DisplayTurn(m_game.currentState.playerTurn, m_previousTurn);
                turnPlaying(ref io_finished, ref io_keepPlaying);
                if (io_finished)
                {
                    if (m_playerTurn.getShapeChar() == 'X')
                    {
                        m_messages.DisplayWinner(m_playerTurn, m_game.currentState.xScore);
                    }
                    else
                    {
                        m_messages.DisplayWinner(m_playerTurn, m_game.currentState.oScore);
                    }
                    if (m_messages.CheckRestartGame())
                    {
                        io_finished = false;
                        io_keepPlaying = true;
                        restartGame();
                        continue;
                    }
                    break;
                }
                //between turn 1 and turn 2
                switchTurn();
                //init keepPlaying to true before turn 2
                io_keepPlaying = true;

                //turn 2
                m_messages.DisplayTurn(m_game.currentState.playerTurn, m_previousTurn);
                if (m_messages.OneOrTwoPlayers == 2)
                {
                    turnPlaying(ref io_finished, ref io_keepPlaying);
                }
                else
                {
                    m_game.MakeComputerTurn(ref io_finished, ref io_keepPlaying, out o_moveString);
                    m_messages.CurrentMove = o_moveString;
                    printBoard();
                }
                // finished turn 1 and turn 2 
                //init keepPlaying to true before turn 1
                io_keepPlaying = true;
                switchTurn();
                if (io_finished)
                {
                    if (m_playerTurn.getShapeChar() == 'X')
                    {
                        m_messages.DisplayWinner(m_playerTurn, m_game.currentState.xScore);
                    }
                    else
                    {
                        m_messages.DisplayWinner(m_playerTurn, m_game.currentState.oScore);
                    }
                    if (m_messages.CheckRestartGame())
                    {
                        io_finished = false;
                        io_keepPlaying = true;
                        restartGame();
                        continue;
                    }
                }
            }
        }

        private void restartGame()
        {
            m_messages.Restart();
            System.Threading.Thread.Sleep(2000);
            // init the board to initial state
            intializeBoard(m_messages.BoardSize);
            printBoard();
        }

        private void turnPlaying(ref bool io_finished, ref bool io_keepPlaying)
        {
            bool moveIlegal;
            bool quit = true;
            while (io_keepPlaying)
            {
                // is there eating before move
                io_keepPlaying = m_game.currentState.IsEatingPossible();
                setUserMove();
                io_finished = checkIfUserQuit();
                if (io_finished)
                {
                    m_game.currentState.CheckGameOver(quit);
                    switchTurn();
                    if (m_playerTurn.getShapeChar() == 'X')
                    {
                        m_messages.DisplayWinner(m_playerTurn, m_game.currentState.xScore);
                    }
                    else
                    {
                        m_messages.DisplayWinner(m_playerTurn, m_game.currentState.oScore);
                    }
                    if (m_messages.CheckRestartGame())
                    {
                        io_finished = false;
                        io_keepPlaying = true;
                        switchTurn();
                        restartGame();
                        m_messages.DisplayTurn(m_game.currentState.playerTurn, m_previousTurn);
                        continue;
                    }
                    break;
                }
                moveIlegal = m_game.MakeMove(m_messages.CurrentMove, m_playerTurn);
                printBoard();
                if (!moveIlegal)
                {
                    m_messages.PrintInvalidLogicInput();
                    io_keepPlaying = true;
                    continue;
                }
                // is there eating after move
                if (io_keepPlaying)
                {
                    io_keepPlaying = m_game.currentState.IsEatingPossible();
                    m_messages.PrintExtraTurn();
                }
            }
            // Check if the game has ended
            if (m_messages.CurrentMove == "Q")
            {
                io_finished = m_game.currentState.CheckGameOver(quit);
            }
            else
            {
                io_finished = m_game.currentState.CheckGameOver(!quit);
            }
        }

        private bool checkIfUserQuit()
        {
            if ((m_messages.CurrentMove == "Q")|| (m_messages.CurrentMove == "q"))
            {
                return true;
            }
            return false;
        }

        public void CreateNewGame(int boardSize, string playerOneName, string playerTwoName)
        {
            m_game = new Game(boardSize, playerOneName, playerTwoName);
            intializeBoard(boardSize);
        }

        private void intializeBoard(int boardSize)
        {
            genereateOPieces(boardSize);
            generateEPieces(boardSize);
            genereateXPieces(boardSize);
        }

        private void genereateOPieces(int boardSize)
        {
            for (int i = 0; i < (boardSize / 2) - 1; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (i % 2 == 0)
                    {
                        if (j % 2 == 0)
                        {
                            m_game.currentState.BoardArray[i, j] = generateNewEmptyPiece();
                        }
                        else
                        {
                            m_game.currentState.BoardArray[i, j] = generateNewOPiece();
                        }
                    }
                    else
                    {
                        if (j % 2 == 0)
                        {
                            m_game.currentState.BoardArray[i, j] = generateNewOPiece();
                        }
                        else
                        {
                            m_game.currentState.BoardArray[i, j] = generateNewEmptyPiece();
                        }

                    }
                }
            }
        }

        private Piece generateNewOPiece()
        {
            return new Piece(new ShapeWrapper('O'));
        }

        private void generateEPieces(int boardSize)
        {
            for (int i = (boardSize / 2) - 1; i < (boardSize / 2) + 1; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    m_game.currentState.BoardArray[i, j] = generateNewEmptyPiece();
                }
            }
        }

        private Piece generateNewEmptyPiece()
        {
            return new Piece(new ShapeWrapper(' '));
        }

        private void genereateXPieces(int boardSize)
        {
            for (int i = (boardSize / 2) + 1; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (i % 2 == 1)
                    {
                        if (j % 2 == 0)
                        {
                            m_game.currentState.BoardArray[i, j] = generateNewXPiece();
                        }
                        else
                        {
                            m_game.currentState.BoardArray[i, j] = generateNewEmptyPiece();
                        }
                    }
                    else
                    {
                        if (j % 2 == 0)
                        {
                            m_game.currentState.BoardArray[i, j] = generateNewEmptyPiece();
                        }
                        else
                        {
                            m_game.currentState.BoardArray[i, j] = generateNewXPiece();
                        }

                    }
                }
            }
        }

        private Piece generateNewXPiece()
        {
            return new Piece(new ShapeWrapper('X'));
        }

        private void printBoard()
        {
            Screen.Clear();
            object[] printableArray = CreatePrintableArray();
            string board6 = string.Format(
@"   A   B   C   D   E   F  
==========================
a| {0} | {1} | {2} | {3} | {4} | {5} |
==========================
b| {6} | {7} | {8} | {9} | {10} | {11} |
==========================
c| {12} | {13} | {14} | {15} | {16} | {17} |
==========================
d| {18} | {19} | {20} | {21} | {22} | {23} |
==========================
e| {24} | {25} | {26} | {27} | {28} | {29} |
==========================
f| {30} | {31} | {32} | {33} | {34} | {35} |
==========================
", printableArray);

        string board8 = string.Format(
@"   A   B   C   D   E   F   G   H  
==================================
a| {0} | {1} | {2} | {3} | {4} | {5} | {6} | {7} |
==================================
b| {8} | {9} | {10} | {11} | {12} | {13} | {14} | {15} |
==================================
c| {16} | {17} | {18} | {19} | {20} | {21} | {22} | {23} |
==================================
d| {24} | {25} | {26} | {27} | {28} | {29} | {30} | {31} |
==================================
e| {32} | {33} | {34} | {35} | {36} | {37} | {38} | {39} |
==================================
f| {40} | {41} | {42} | {43} | {44} | {45} | {46} | {47} |
==================================
g| {48} | {49} | {50} | {51} | {52} | {53} | {54} | {55} |
==================================
h| {56} | {57} | {58} | {59} | {60} | {61} | {62} | {63} |
==================================
", printableArray);
            string board10 = string.Format(
@"   A   B   C   D   E   F   G   H   I   J  
==========================================
a| {0} | {1} | {2} | {3} | {4} | {5} | {6} | {7} | {8} | {9} |
==========================================
b| {10} | {11} | {12} | {13} | {14} | {15} | {16} | {17} | {18} | {19} |
==========================================
c| {20} | {21} | {22} | {23} | {24} | {25} | {26} | {27} | {28} | {29} |
==========================================
d| {30} | {31} | {32} | {33} | {34} | {35} | {36} | {37} | {38} | {39} |
==========================================
e| {40} | {41} | {42} | {43} | {44} | {45} | {46} | {47} | {48} | {49} |
==========================================
f| {50} | {51} | {52} | {53} | {54} | {55} | {56} | {57} | {58} | {59} |
==========================================
g| {60} | {61} | {62} | {63} | {64} | {65} | {66} | {67} | {68} | {69} |
==========================================
h| {70} | {71} | {72} | {73} | {74} | {75} | {76} | {77} | {78} | {79} |
==========================================
i| {80} | {81} | {82} | {83} | {84} | {85} | {86} | {87} | {88} | {89} |
==========================================
j| {90} | {91} | {92} | {93} | {94} | {95} | {96} | {97} | {98} | {99} |
==========================================
", printableArray);


            switch (m_messages.BoardSize)
            {
                case 6:
                    Console.WriteLine(board6);
                    break;
                case 8:
                    Console.WriteLine(board8);
                    break;
                case 10:
                    Console.WriteLine(board10);
                    break;
            }
            //Console.WriteLine(board6);
        }

        public object[] CreatePrintableArray()
        {
            const int maxSize = 10;
           // object[] o_printableArray = new object[m_messages.BoardSize * m_messages.BoardSize];
            object[] o_printableArray = new object[maxSize * maxSize]; // need to define MAX
            for (int i = 0; i < m_messages.BoardSize; i++)
            {
                for (int j = 0; j < m_messages.BoardSize; j++)
                {
                    o_printableArray[i * m_messages.BoardSize + j] = m_game.currentState.BoardArray[i,j].Shape.getShapeChar();
                }
            }
            return o_printableArray;
        }

        private void setUserMove()
        {
            m_messages.CurrentMove = Console.ReadLine();
            if (!isUserMoveValid(m_messages.CurrentMove))
            {
                while (!isUserMoveValid(m_messages.CurrentMove))
                {
                    Screen.Clear();
                    printBoard();
                    System.Console.SetCursorPosition(0, m_messages.BoardSize+2);
                    m_messages.PrintInvalidInput();
                    m_messages.CurrentMove = Console.ReadLine();
                }
            }
        }

        private bool isUserMoveValid(string move) //Not finished!!
        {
            // string
            if (m_messages.CurrentMove == "Q")
            {
                return true;
            }
            else if (m_messages.CurrentMove.Length != 5)
            {
                m_messages.PrintInvalidInput();
                return false;
            }
            else if (m_messages.CurrentMove[2] != '>')
            {
                m_messages.PrintInvalidInput();
                return false;
            }
            else if ((m_messages.CurrentMove[0] < 'A') || (m_messages.CurrentMove[0] > m_messages.BoardSize + 'A'))
            {
                m_messages.PrintInvalidInput();
                return false;
            }
            else if ((m_messages.CurrentMove[3] < 'A') || (m_messages.CurrentMove[3] > m_messages.BoardSize + 'A'))
            {
                m_messages.PrintInvalidInput();
                return false;
            }
            else if ((m_messages.CurrentMove[1] < 'a') || (m_messages.CurrentMove[1] > m_messages.BoardSize + 'a'))
            {
                m_messages.PrintInvalidInput();
                return false;
            }
            else if ((m_messages.CurrentMove[4] < 'a') || (m_messages.CurrentMove[4] > m_messages.BoardSize + 'a'))
            {
                m_messages.PrintInvalidInput();
                return false;
            }
            else
            {
                return true;
            }
        }

        private void switchTurn()
        {
            if (m_playerTurn.getShapeChar() == 'X')
            {
                m_previousTurn =  new ShapeWrapper(m_playerTurn.getShapeChar());
                m_playerTurn.Shape = ShapeWrapper.eShape.O;
            }
            else
            {
                m_previousTurn = new ShapeWrapper(m_playerTurn.getShapeChar());
                m_playerTurn.Shape = ShapeWrapper.eShape.X;
            }
            m_game.currentState.SwitchTurn();
        }
    }
}



