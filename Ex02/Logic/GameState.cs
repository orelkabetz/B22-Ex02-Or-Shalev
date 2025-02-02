﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex02.Logic
{
    public class GameState
    {
        private ShapeWrapper m_playerTurn;
        private int m_boardSize;
        private Piece[,] m_boardArray;
        private List<Move> m_possibleComputerMoves;
        private List<Move> m_possibleComputerEatingMoves;
        private int m_xScore = 0;
        private int m_oScore = 0;

        public GameState(int size)
        {
            m_boardSize = size;
            m_boardArray = new Piece[size, size];
            m_playerTurn = new ShapeWrapper('X');
            m_possibleComputerMoves = new List<Move>();
            m_possibleComputerEatingMoves = new List<Move>();
        }

        public Piece[,] BoardArray
        { 
            get { return m_boardArray; } 
            set { m_boardArray = value; }
        }

        public List<Move> PossibleComputerMoves
        {
            get { return m_possibleComputerMoves; }
            set { m_possibleComputerMoves = value; }
        }

        public List<Move> PossibleComputerEatingMoves
        {
            get { return m_possibleComputerEatingMoves; }
            set { m_possibleComputerEatingMoves = value; }
        }

        public ShapeWrapper playerTurn
        {
            get { return m_playerTurn; }
            set { m_playerTurn = value; }
        }

        public int xScore
        {
            get { return m_xScore; }
            set { m_xScore = value; }
        }

        public int oScore
        {
            get { return m_oScore; }
            set { m_oScore = value; }
        }

        public void SwitchTurn()
        {
            if (m_playerTurn.getShapeChar() == 'X')
            {
                m_playerTurn.Shape = ShapeWrapper.eShape.O;
            }
            else
            {
                m_playerTurn.Shape = ShapeWrapper.eShape.X;
            }
        }

        public bool CheckMove(Move currentMove)
        {
            // Check if the start or end position is in the same row/col - not possible
            if ((currentMove.startPosition.Row == currentMove.endPosition.Row)
                || (currentMove.startPosition.Col == currentMove.endPosition.Col))
            {
                return false;
            }
            if(!isStartPositionValid(currentMove)) 
            {
                return false;
            }
            if (!isEndPositionValid(currentMove))
            {
                return false;
            }
            return true;
        }

        public void SetAllComputerPossibleMoves()
        {
            m_possibleComputerEatingMoves.Clear();
            setPossibleEating();
            if (m_possibleComputerEatingMoves.Count == 0)
            {
                m_possibleComputerMoves.Clear();
                setPossibleMoves();
            }
        }

        private void setPossibleMoves()
        {
            Move currentMove;
            for (int i = m_boardSize - 1; i >= 0; i--)
            {
                for (int j = 0; j < m_boardSize; j++)
                {
                    if ((m_boardArray[i, j].Shape.getShapeChar() == 'O') || (m_boardArray[i, j].Shape.getShapeChar() == 'U'))
                    {
                        if ((i + 1 < m_boardSize) && (j - 1 >= 0) && ((m_boardArray[i + 1, j - 1].Shape.getShapeChar() == ' '))) 
                        {
                            currentMove = new Move(new Position(i, j), new Position(i + 1, j - 1));
                            m_possibleComputerMoves.Add(currentMove);
                        }
                        else if ((i + 1 < m_boardSize) && (j + 1 < m_boardSize) && ((m_boardArray[i + 1, j + 1].Shape.getShapeChar() == ' '))) 
                        {
                            currentMove = new Move(new Position(i, j), new Position(i + 1, j + 1));
                            m_possibleComputerMoves.Add(currentMove);
                        }

                        if (m_boardArray[i, j].IsKing == true)
                        {
                            if ((i - 1 >= 0) && (j - 1 >= 0) && ((m_boardArray[i - 1, j - 1].Shape.getShapeChar() == ' ')))
                            {
                                currentMove = new Move(new Position(i, j), new Position(i - 1, j - 1));
                                m_possibleComputerMoves.Add(currentMove);
                            }
                            else if ((i - 1 >= 0) && (j + 1 < m_boardSize) && ((m_boardArray[i - 1, j + 1].Shape.getShapeChar() == ' ')))
                            {
                                currentMove = new Move(new Position(i, j), new Position(i - 1, j + 1));
                                m_possibleComputerMoves.Add(currentMove);
                            }
                        }
                    }
                }
            }
        }

        private void setPossibleEating()
        {
            Move currentMove;
            for (int i = m_boardSize - 1; i >= 0; i--)
            {
                for (int j = 0; j < m_boardSize; j++)
                {
                    if ((m_boardArray[i, j].Shape.getShapeChar() == 'O') || (m_boardArray[i, j].Shape.getShapeChar() == 'U'))
                    {
                        if ((i + 2 < m_boardSize) && (j - 2 >= 0) && ((m_boardArray[i + 1, j - 1].Shape.getShapeChar() == 'X') || (m_boardArray[i + 1, j - 1].Shape.getShapeChar() == 'K'))
                            &&
                               (m_boardArray[i + 2, j - 2].Shape.getShapeChar() == ' '))
                        {
                            currentMove = new Move(new Position(i, j), new Position(i + 2, j - 2));
                            m_possibleComputerEatingMoves.Add(currentMove);
                        }
                        else if ((i + 2 < m_boardSize) && (j + 2 < m_boardSize) && ((m_boardArray[i + 1, j + 1].Shape.getShapeChar() == 'X') || (m_boardArray[i + 1, j + 1].Shape.getShapeChar() == 'K'))
                                &&
                                (m_boardArray[i + 2, j + 2].Shape.getShapeChar() == ' '))
                        {
                            currentMove = new Move(new Position(i, j), new Position(i + 2, j + 2));
                            m_possibleComputerEatingMoves.Add(currentMove);
                        }

                        if (m_boardArray[i, j].IsKing == true)
                        {
                            if ((i - 2 >= 0) && (j - 2 >= 0) && ((m_boardArray[i - 1, j - 1].Shape.getShapeChar() == 'X') || (m_boardArray[i - 1, j - 1].Shape.getShapeChar() == 'K'))
                             &&
                            (m_boardArray[i - 2, j - 2].Shape.getShapeChar() == ' '))
                            {
                                currentMove = new Move(new Position(i, j), new Position(i - 2, j - 2));
                                m_possibleComputerEatingMoves.Add(currentMove);
                            }
                            else if ((i - 2 >= 0) && (j + 2 < m_boardSize) && ((m_boardArray[i - 1, j + 1].Shape.getShapeChar() == 'X') || (m_boardArray[i - 1, j + 1].Shape.getShapeChar() == 'K'))
                                    &&
                                    (m_boardArray[i - 2, j + 2].Shape.getShapeChar() == ' '))
                            {
                                currentMove = new Move(new Position(i, j), new Position(i - 2, j + 2));
                                m_possibleComputerEatingMoves.Add(currentMove);
                            }
                        }
                    }
                }
            }
        }

        public void PlayMove(Move currentMove)
        {
            int eatenRow;
            int eatenCol;
            char temp = m_boardArray[currentMove.startPosition.Row, currentMove.startPosition.Col].Shape.getShapeChar();
            m_boardArray[currentMove.startPosition.Row, currentMove.startPosition.Col].Shape = new ShapeWrapper(' ');
            m_boardArray[currentMove.endPosition.Row, currentMove.endPosition.Col].Shape = new ShapeWrapper(temp);
            if (temp == 'K')
            {
                m_boardArray[currentMove.endPosition.Row, currentMove.endPosition.Col].IsKing = true;
            }
            if (Math.Abs(currentMove.startPosition.Row-currentMove.endPosition.Row) == 2)
            {
                eatenRow = (currentMove.startPosition.Row + currentMove.endPosition.Row) / 2;
                eatenCol = (currentMove.startPosition.Col + currentMove.endPosition.Col) / 2;
                m_boardArray[eatenRow, eatenCol].Shape = new ShapeWrapper(' ');
            }
            updateKing(currentMove);
        }

        private void updateKing(Move currentMove)
        {
            if (m_playerTurn.getShapeChar() == 'X')
            {
                if (currentMove.endPosition.Row == 0)
                {
                    m_boardArray[currentMove.endPosition.Row,currentMove.endPosition.Col].Shape = new ShapeWrapper('K');
                    m_boardArray[currentMove.endPosition.Row, currentMove.endPosition.Col].IsKing = true;
                }
            }
            else // 'O'
            {
                if (currentMove.endPosition.Row == m_boardSize-1)
                {
                    m_boardArray[currentMove.endPosition.Row, currentMove.endPosition.Col].Shape = new ShapeWrapper('U');
                    m_boardArray[currentMove.endPosition.Row, currentMove.endPosition.Col].IsKing = true;
                }
            }

        }

        public bool CheckGameOver(bool quit)
        {
            return checkWin(quit);
        }

        private bool checkWin(bool quit)
        {
            // if winner is X winnerShape = 'X' , else if winner is O winnerShape is '0' ,else (no winner) winnerShape = ' '
            bool xExists = false;
            int xScore = 0;
            bool oExists = false;
            int oScore = 0;

            for (int i = 0; i < m_boardSize; i++)
            {
                for (int j = 0; j < m_boardSize; j++)
                {
                    if (m_boardArray[i, j].Shape.getShapeChar() == 'X')
                    {
                        xScore += 1;
                        xExists = true;
                    }
                    else if (m_boardArray[i, j].Shape.getShapeChar() == 'K')
                    {
                        xScore += 4;
                        xExists = true;
                    }
                    else if (m_boardArray[i, j].Shape.getShapeChar() == 'O')
                    {
                        oScore += 1;
                        oExists = true;
                    }
                    else if (m_boardArray[i, j].Shape.getShapeChar() == 'U')
                    {
                        oScore += 4;
                        oExists = true;
                    }

                }
            }
            if (xExists && !oExists)
            {
                m_xScore += xScore;
                return xExists;
            }
            else if (!xExists && oExists)
            {
                m_oScore += oScore;
                return oExists;
            }
            else if ((xExists && oExists)&&quit)
            {
                m_xScore += xScore;
                m_oScore += oScore;
                return true;
            }
            return false;
        }

        private bool isStartPositionValid(Move currentMove)
        {
            char startPositionShape = m_boardArray[currentMove.startPosition.Row, currentMove.startPosition.Col].Shape.getShapeChar();
            if (startPositionShape == 'K')
            {
                startPositionShape = 'X';
            }
            else if (startPositionShape == 'U')
            {
                startPositionShape = 'O';
            }
            if (startPositionShape != playerTurn.getShapeChar())
            {
                return false;
            }
            return true;
        }

        private bool isEndPositionValid(Move currentMove)
        {
            bool isEndPositionLegal = true;
            
            if (!isEndPositionEmpty(currentMove))
            {
                return false;
            }
            if (!insideTheBoard(currentMove))
            {
                return false;
            }
            if (m_playerTurn.getShapeChar() == 'X')
            {
                if (!isDiagonalDown(currentMove,1))
                {
                    isEndPositionLegal = false;
                }
            }
            else
            { 
                if (!isDiagonalUp(currentMove,1))
                {
                    isEndPositionLegal = false;
                }
            }
            if (m_boardArray[currentMove.startPosition.Row, currentMove.startPosition.Col].IsKing)
            {
                isEndPositionLegal = true;
                if (m_playerTurn.getShapeChar() == 'X')
                {
                    if (!isDiagonalUp(currentMove,1))
                    {
                        isEndPositionLegal = false;
                    }
                }
                else // 'O'
                {
                    if (!isDiagonalDown(currentMove,1))
                    {
                        isEndPositionLegal = false;
                    }
                }
            }
            return isEndPositionLegal;
        }

        private bool isEndPositionEmpty(Move currentMove)
        {
            char endPositionShape = m_boardArray[currentMove.endPosition.Row, currentMove.endPosition.Col].Shape.getShapeChar();

            if (endPositionShape != ' ')
            {
                return false;
            }

            return true;
        }

        private bool insideTheBoard(Move currentMove)
        {
            bool isStartPositionLegal = true;
            bool isEndPositionLegal = true;
            if (currentMove.startPosition.Row < 0 || currentMove.startPosition.Row >= m_boardSize)
            {
                isStartPositionLegal = false;
            }
            if (currentMove.startPosition.Col < 0 || currentMove.startPosition.Col >= m_boardSize)
            {
                isStartPositionLegal = false;
            }
            if (currentMove.endPosition.Row < 0 || currentMove.endPosition.Row >= m_boardSize)
            {
                isEndPositionLegal = false;
            }
            if (currentMove.endPosition.Col < 0 || currentMove.endPosition.Col >= m_boardSize)
            {
                isEndPositionLegal = false;
            }
            return isStartPositionLegal && isEndPositionLegal;
        }

        private bool isDiagonalDown(Move currentMove, int stepSize)
        {
            if (stepSize == 1)
            {
                if (IsEatingPossible())
                {
                    return checkIfPlayerMoveEating(currentMove);
                }
            }
            
            if (currentMove.startPosition.Row != (currentMove.endPosition.Row + stepSize))
            {
                return false;
            }
            else if (!(currentMove.startPosition.Col != (currentMove.endPosition.Col - stepSize))
                && !(currentMove.startPosition.Col != (currentMove.endPosition.Col + stepSize)))
            {
                return false;
            }
            return true;
        }

        private bool isDiagonalUp(Move currentMove, int stepSize)
        {
            if (stepSize == 1)
            {
                if (IsEatingPossible())
                {
                    return checkIfPlayerMoveEating(currentMove);
                }
            }
            if (currentMove.startPosition.Row != (currentMove.endPosition.Row - stepSize))
            {
                return false;
            }
            else if (!(currentMove.startPosition.Col != (currentMove.endPosition.Col - stepSize))
                && !(currentMove.startPosition.Col != (currentMove.endPosition.Col + stepSize)))
            {
                return false;
            }
            return true;
        }

        private bool checkIfPlayerMoveEating(Move currentMove)
        {
            //Checking if move is eating
            bool isEating = false;

            int eatenRow = (currentMove.startPosition.Row + currentMove.endPosition.Row) /2;
            int eatenCol = (currentMove.startPosition.Col + currentMove.endPosition.Col) / 2;
            // check regular eating
            //check if between there is opponnent
            if (m_playerTurn.getShapeChar() == 'X')
            {
                if (isDiagonalDown(currentMove, 2))
                {
                    if ((m_boardArray[eatenRow,eatenCol].Shape.getShapeChar() == 'O')|| (m_boardArray[eatenRow, eatenCol].Shape.getShapeChar() == 'U'))
                    isEating = true;
                }
            }
            else // 'O'
            {
                if (isDiagonalUp(currentMove, 2))
                {
                    if ((m_boardArray[eatenRow, eatenCol].Shape.getShapeChar() == 'X')|| m_boardArray[eatenRow, eatenCol].Shape.getShapeChar() == 'K')
                    isEating = true;
                }
            }
            //check is king
            // check king eating
            if (m_boardArray[currentMove.startPosition.Row,currentMove.startPosition.Col].IsKing)
            {
                if (m_playerTurn.getShapeChar() == 'X')
                {
                    if (isDiagonalUp(currentMove, 2))
                    {
                        if (m_boardArray[eatenRow, eatenCol].Shape.getShapeChar() == 'O'|| m_boardArray[eatenRow, eatenCol].Shape.getShapeChar() == 'U')
                            isEating = true;
                    }
                }
                else // 'O'
                {
                    if (isDiagonalDown(currentMove, 2))
                    {
                        if (m_boardArray[eatenRow, eatenCol].Shape.getShapeChar() == 'X' || m_boardArray[eatenRow, eatenCol].Shape.getShapeChar() == 'K')
                            isEating = true;
                    }
                }
            }
            return isEating;
        }

        public bool IsEatingPossible()
        {
            if (m_playerTurn.getShapeChar() == 'X')
            {
                return isEatingPossibleForX();
            }
            else // 'O'
            {
                return isEatingPossibleForO();
            }
        }

        private bool isEatingPossibleForX()
        {
            for (int i = m_boardSize-1; i >= 0; i--)
            {
                for (int j = 0; j < m_boardSize; j++)
                {
                    if (m_boardArray[i, j].Shape.getShapeChar() == 'X' || m_boardArray[i, j].Shape.getShapeChar() == 'K')
                    {
                        if ((i - 2 >= 0 ) && (j - 2 >=0) && ((m_boardArray[i - 1, j - 1].Shape.getShapeChar() == 'O')|| (m_boardArray[i - 1, j - 1].Shape.getShapeChar() == 'U'))
                            &&
                           (m_boardArray[i - 2, j - 2].Shape.getShapeChar() == ' '))
                        {
                            return true;
                        }
                        else if ((i - 2 >= 0) && (j + 2 < m_boardSize) && ((m_boardArray[i - 1, j + 1].Shape.getShapeChar() == 'O')|| (m_boardArray[i - 1, j + 1].Shape.getShapeChar() == 'U'))
                                &&
                                (m_boardArray[i - 2, j+2].Shape.getShapeChar() == ' ')) 
                        {
                            return true;
                        }
                        if (m_boardArray[i, j].IsKing == true)
                        {
                            if ((i + 2 < m_boardSize) && (j - 2 >= 0) && ((m_boardArray[i + 1, j - 1].Shape.getShapeChar() == 'O')|| (m_boardArray[i + 1, j - 1].Shape.getShapeChar() == 'U'))
                             &&
                                (m_boardArray[i + 2, j - 2].Shape.getShapeChar() == ' '))
                            {
                                return true;
                            }
                            else if ((i +2 < m_boardSize) && (j + 2 < m_boardSize) && ((m_boardArray[i + 1, j + 1].Shape.getShapeChar() == 'O')|| (m_boardArray[i + 1, j + 1].Shape.getShapeChar() == 'U'))
                                    &&
                                    (m_boardArray[i + 2, j + 2].Shape.getShapeChar() == ' '))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private bool isEatingPossibleForO()
        {
            for (int i = m_boardSize-1; i >= 0; i--)
            {
                for (int j = 0; j < m_boardSize; j++)
                {
                    if ((m_boardArray[i, j].Shape.getShapeChar() == 'O')|| (m_boardArray[i, j].Shape.getShapeChar() == 'U'))
                    {
                        if ((i + 2 < m_boardSize) && (j - 2 >= 0) && ((m_boardArray[i + 1, j - 1].Shape.getShapeChar() == 'X')|| (m_boardArray[i + 1, j - 1].Shape.getShapeChar() == 'K'))
                            &&
                               (m_boardArray[i + 2, j - 2].Shape.getShapeChar() == ' '))
                        {
                            return true;
                        }
                        else if ((i + 2 < m_boardSize) && (j + 2 < m_boardSize) && ((m_boardArray[i + 1, j + 1].Shape.getShapeChar() == 'X')|| (m_boardArray[i + 1, j + 1].Shape.getShapeChar() == 'K'))
                                &&
                                (m_boardArray[i + 2, j + 2].Shape.getShapeChar() == ' '))
                        {
                            return true;
                        }

                        if (m_boardArray[i, j].IsKing == true)
                        {
                            if ((i - 2 >= 0) && (j - 2 >= 0) && ((m_boardArray[i - 1, j - 1].Shape.getShapeChar() == 'X')|| (m_boardArray[i - 1, j - 1].Shape.getShapeChar() == 'K'))
                             &&
                            (m_boardArray[i - 2, j - 2].Shape.getShapeChar() == ' '))
                            {
                                return true;
                            }
                            else if ((i - 2 >= 0) && (j + 2 < m_boardSize) && ((m_boardArray[i - 1, j + 1].Shape.getShapeChar() == 'X')|| (m_boardArray[i - 1, j + 1].Shape.getShapeChar() == 'K'))
                                    &&
                                    (m_boardArray[i - 2, j + 2].Shape.getShapeChar() == ' '))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }     
    }
}
