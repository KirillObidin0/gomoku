using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gomoku
{
    public class Game
    {
        public List<List<char>> Board { get; set; }
        private int TurnCounter;
        private Random rnd;

        public Game()
        {
            Board = new List<List<char>>();
            for (int i = 0; i < 15; i++)
            {
                Board.Add(new List<char>());
                for (int j = 0; j < 15; j++)
                {
                    Board[i].Add('-');
                }
            }
            TurnCounter = 0;
            rnd = new Random();
        }

        public bool Turn(int x, int y, char playerChar)
        {
            if (Board[x][y] != '-')
                throw new ArgumentException("Wrong board coordinates: field is not empty");
            else
            {
                Board[x][y] = playerChar;
                TurnCounter++;
            }
            if (VictoryConditionCheck(playerChar))
                return true;
            else return false;
        }
        public bool AITurn(char playerChar, char opponentChar)
        {
            int x = 0;
            int y = 0;

            if (TurnCounter == 0)
            {
                x = rnd.Next(5, 10);
                y = rnd.Next(5, 10);
            }
            else if (TurnCounter == 1)
            {
                x = rnd.Next(5, 10);
                y = rnd.Next(5, 10);
                if (Board[x][y] != '-')
                    x++;
            }
            else
            {
                var coordinates = SeekValuableField(playerChar, opponentChar);
                x = coordinates.X;
                y = coordinates.Y;
            }

            return Turn(x, y, playerChar);
        }
        public bool FourInARowH(char opponentChar, out Coordinates result)
        {
            int charsInARow = 0;
            var firstOption = new Coordinates();
            var secondOption = new Coordinates();
            for (int i = 0; i < Board.Count; i++)
            {
                for (int j = 0; j < Board[i].Count; j++)
                {
                    if (Board[i][j] == opponentChar)
                    {
                        if (charsInARow == 0)
                        {
                            firstOption.X = i;
                            firstOption.Y = j - 1;
                        }
                        charsInARow++;
                    }
                    else charsInARow = 0;
                    if (charsInARow == 4)
                    {
                        secondOption.X = i;
                        secondOption.Y = j + 1;
                        if (firstOption.Y < 0)
                            firstOption = null;
                        else if (Board[firstOption.X][firstOption.Y] != '-')
                            firstOption = null;
                        if (secondOption != null)
                        {
                            if (secondOption.Y >= Board.Count && secondOption != null)
                                secondOption = null;
                            else if (Board[secondOption.X][secondOption.Y] != '-')
                                secondOption = null;
                        }
                        if (firstOption is null && secondOption != null)
                            result = secondOption;
                        else if (secondOption is null && firstOption != null)
                            result = firstOption;
                        else if (firstOption is null && secondOption is null)
                            result = null;
                        else result = rnd.Next(0, 101) % 2 == 0 ? firstOption : secondOption;
                        return result is null ? false : true;
                    }
                }
            }
            result = null;
            return false;
        }
        public bool FourInARowV(char opponentChar, out Coordinates result)
        {
            int charsInARow = 0;
            var firstOption = new Coordinates();
            var secondOption = new Coordinates();
            for (int i = 0; i < Board.Count; i++)
            {
                for (int j = 0; j < Board[i].Count; j++)
                {
                    if (Board[i][j] == opponentChar)
                    {
                        for (int k = i; k < Board.Count; k++)
                        {
                            if (Board[k][j] == opponentChar)
                            {
                                if (charsInARow == 0)
                                {
                                    firstOption.X = k - 1;
                                    firstOption.Y = j;
                                }
                                charsInARow++;
                            }
                            else
                            {
                                charsInARow = 0;
                                break;
                            }
                            if (charsInARow == 4)
                            {
                                secondOption.X = k + 1;
                                secondOption.Y = j;
                                if (firstOption.X < 0)
                                    firstOption = null;
                                else if (Board[firstOption.X][firstOption.Y] != '-')
                                    firstOption = null;
                                if (secondOption != null)
                                {
                                    if (secondOption.X >= Board.Count && secondOption != null)
                                        secondOption = null;
                                    else if (Board[secondOption.X][secondOption.Y] != '-')
                                        secondOption = null;
                                }
                                if (firstOption is null && secondOption != null)
                                    result = secondOption;
                                else if (secondOption is null && firstOption != null)
                                    result = firstOption;
                                else if (firstOption is null && secondOption is null)
                                    result = null;
                                else result = rnd.Next(0, 101) % 2 == 0 ? firstOption : secondOption;
                                return result is null ? false : true;
                            }
                        }
                    }
                }
            }
            result = null;
            return false;
        }
        public bool FourInARowD(char playerChar, out Coordinates result)
        {
            int charsInARow = 0;
            int dc; // diagonalCounter
            var firstOption = new Coordinates();
            var secondOption = new Coordinates();
            for (int i = 0; i < Board.Count; i++)
            {
                for (int j = 0; j < Board[i].Count; j++)
                {
                    if (Board[i][j] == playerChar)
                    {
                        dc = j;
                        for (int k = i; k < Board.Count; k++)
                        {
                            if (Board[k][dc] == playerChar)
                            {
                                if (charsInARow == 0)
                                {
                                    firstOption.X = k - 1;
                                    firstOption.Y = dc - 1;
                                }
                                charsInARow++;
                                if (charsInARow == 5)
                                {
                                    secondOption.X = k + 1;
                                    secondOption.Y = dc + 1;
                                    if (firstOption.X < 0 || firstOption.Y < 0)
                                        firstOption = null;
                                    else if (Board[firstOption.X][firstOption.Y] != '-')
                                        firstOption = null;
                                    if (secondOption != null)
                                    {
                                        if (secondOption.X >= Board.Count || secondOption.Y >= Board.Count)
                                            secondOption = null;
                                        else if (Board[secondOption.X][secondOption.Y] != '-')
                                            secondOption = null;
                                    }
                                    if (firstOption is null && secondOption != null)
                                        result = secondOption;
                                    else if (secondOption is null && firstOption != null)
                                        result = firstOption;
                                    else if (firstOption is null && secondOption is null)
                                        result = null;
                                    else result = rnd.Next(0, 101) % 2 == 0 ? firstOption : secondOption;
                                    return result is null ? false : true;
                                }
                                if (dc < Board[i].Count - 1)
                                    dc++;
                                else break;
                            }
                            else
                            {
                                charsInARow = 0;
                                break;
                            }
                        }
                    }
                }
            }
            result = null;
            return false;
        }
        public Coordinates SeekValuableField(char playerChar, char opponentChar)
        {
            Coordinates result = null;
            if (FourInARowH(opponentChar, out Coordinates coordinates))
                result = coordinates;
            else if (FourInARowV(opponentChar, out coordinates))
                result = coordinates;
            else if (FourInARowD(opponentChar, out coordinates))
                result = coordinates;
            else
            {
                Coordinates horizontalField;
                Coordinates verticalField;
                Coordinates diagonalField;
                int num1 = SeekFieldH(playerChar, out horizontalField);
                int num2 = SeekFieldV(playerChar, out verticalField);
                int num3 = SeekFieldD(playerChar, out diagonalField);
                if (horizontalField != null && verticalField != null && diagonalField != null)
                {
                    if (num1 >= num2 && num1 >= num3) result = horizontalField;
                    else if (num2 > num1 && num2 >= num3) result = verticalField;
                    else if (num3 > num1 && num3 > num2) result = diagonalField;
                }
                if (horizontalField is null && verticalField != null && diagonalField != null)
                {
                    if (num2 >= num3) result = verticalField;
                    else result = diagonalField;
                }
                if (horizontalField != null && verticalField != null && diagonalField is null)
                {
                    if (num1 > num2) result = horizontalField;
                    else result = verticalField;
                }
                if (horizontalField != null && verticalField is null && diagonalField != null)
                {
                    if (num1 > num3) result = horizontalField;
                    else result = diagonalField;
                }
                if (horizontalField != null && verticalField is null && diagonalField is null)
                {
                    result = horizontalField;
                }
                if (horizontalField is null && verticalField != null && diagonalField is null)
                {
                    result = verticalField;
                }
                if (horizontalField is null && verticalField is null && diagonalField != null)
                {
                    result = diagonalField;
                }
            }
            return result;
        }
        public int SeekFieldH(char playerChar, out Coordinates result)
        {
            int charsInARow = 0;
            int maxChars = 0;
            Coordinates firstOption = null;
            Coordinates secondOption = null;
            var tmp1 = new Coordinates();
            var tmp2 = new Coordinates();
            for (int i = 0; i < Board.Count; i++)
            {
                for (int j = 0; j < Board[i].Count; j++)
                {
                    if (Board[i][j] == playerChar)
                    {
                        if (charsInARow == 0)
                        {
                            tmp1.X = i;
                            tmp1.Y = j - 1;
                        }
                        charsInARow++;
                        if (j + 1 == Board[i].Count)
                        {
                            tmp2 = null;
                            if (charsInARow > maxChars)
                            {
                                maxChars = charsInARow;
                                firstOption = tmp1;
                                secondOption = tmp2;
                            }
                        }
                    }
                    else
                    {
                        if (charsInARow != 0)
                        {
                            if (tmp2 != null)
                            {
                                tmp2.X = i;
                                tmp2.Y = j;
                            }
                            if (charsInARow > maxChars)
                            {
                                maxChars = charsInARow;
                                firstOption = tmp1;
                                secondOption = tmp2;
                            }
                        }
                        charsInARow = 0;
                    }
                }
            }
            if (firstOption.Y < 0)
                firstOption = null;
            else if (Board[firstOption.X][firstOption.Y] != '-')
                firstOption = null;
            if (secondOption != null)
            {
                if (secondOption.Y >= Board.Count)
                    secondOption = null;
                else if (Board[secondOption.X][secondOption.Y] != '-')
                    secondOption = null;
            }
            if (firstOption is null && secondOption != null)
                result = secondOption;
            else if (secondOption is null && firstOption != null)
                result = firstOption;
            else if (firstOption is null && secondOption is null)
                result = null;
            else result = rnd.Next(0, 101) % 2 == 0 ? firstOption : secondOption;
            return maxChars;
        }
        public int SeekFieldV(char playerChar, out Coordinates result)
        {
            int charsInARow = 0;
            int maxChars = 0;
            Coordinates firstOption = null;
            Coordinates secondOption = null;
            var tmp1 = new Coordinates();
            var tmp2 = new Coordinates();
            for (int i = 0; i < Board.Count; i++)
            {
                for (int j = 0; j < Board[i].Count; j++)
                {
                    if (Board[i][j] == playerChar)
                    {
                        for (int k = i; k < Board.Count; k++)
                        {
                            if (Board[k][j] == playerChar)
                            {
                                if (charsInARow == 0)
                                {
                                    tmp1.X = k - 1;
                                    tmp1.Y = j;
                                }
                                charsInARow++;
                                if (k + 1 == Board.Count)
                                {
                                    tmp2 = null;
                                    if (charsInARow > maxChars)
                                    {
                                        maxChars = charsInARow;
                                        firstOption = tmp1;
                                        secondOption = tmp2;
                                    }
                                }
                            }
                            else
                            {
                                if (charsInARow != 0)
                                {
                                    if (tmp2 != null)
                                    {
                                        tmp2.X = k;
                                        tmp2.Y = j;
                                    }
                                    if (charsInARow > maxChars)
                                    {
                                        maxChars = charsInARow;
                                        firstOption = tmp1;
                                        secondOption = tmp2;
                                    }
                                }
                                charsInARow = 0;
                                break;
                            }
                        }
                    }
                }
            }
            if (firstOption.X < 0)
                firstOption = null;
            else if (Board[firstOption.X][firstOption.Y] != '-')
                firstOption = null;
            if (secondOption != null)
            {
                if (secondOption.X >= Board.Count)
                    secondOption = null;
                else if (Board[secondOption.X][secondOption.Y] != '-')
                    secondOption = null;
            }
            if (firstOption is null && secondOption != null)
                result = secondOption;
            else if (secondOption is null && firstOption != null)
                result = firstOption;
            else if (firstOption is null && secondOption is null)
                result = null;
            else result = rnd.Next(0, 101) % 2 == 0 ? firstOption : secondOption;
            return maxChars;
        }
        public int SeekFieldD(char playerChar, out Coordinates result)
        {
            int charsInARow = 0;
            int dc; // diagonalCounter
            int maxChars = 0;
            Coordinates firstOption = null;
            Coordinates secondOption = null;
            var tmp1 = new Coordinates();
            var tmp2 = new Coordinates();
            for (int i = 0; i < Board.Count; i++)
            {
                for (int j = 0; j < Board[i].Count; j++)
                {
                    if (Board[i][j] == playerChar)
                    {
                        dc = j;
                        for (int k = i; k < Board.Count; k++)
                        {
                            if (Board[k][dc] == playerChar)
                            {
                                if (charsInARow == 0)
                                {
                                    tmp1.X = k - 1;
                                    tmp1.Y = dc - 1;
                                }
                                charsInARow++;
                                if (dc < Board[i].Count - 1)
                                    dc++;
                                else
                                {
                                    tmp2 = null;
                                    if (charsInARow > maxChars)
                                    {
                                        maxChars = charsInARow;
                                        firstOption = tmp1;
                                        secondOption = tmp2;
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                if (charsInARow != 0)
                                {
                                    if (tmp2 != null)
                                    {
                                        tmp2.X = k;
                                        tmp2.Y = dc;
                                    }
                                    if (charsInARow > maxChars)
                                    {
                                        maxChars = charsInARow;
                                        firstOption = tmp1;
                                        secondOption = tmp2;
                                    }
                                }
                                charsInARow = 0;
                                break;
                            }
                        }
                    }
                }
            }
            if (firstOption.X < 0 || firstOption.Y < 0)
                firstOption = null;
            else if (Board[firstOption.X][firstOption.Y] != '-')
                firstOption = null;
            if (secondOption != null)
            {
                if (secondOption.X >= Board.Count || secondOption.Y >= Board.Count)
                    secondOption = null;
                else if (Board[secondOption.X][secondOption.Y] != '-')
                    secondOption = null;
            }
            if (firstOption is null && secondOption != null)
                result = secondOption;
            else if (secondOption is null && firstOption != null)
                result = firstOption;
            else if (firstOption is null && secondOption is null)
                result = null;
            else result = rnd.Next(0, 101) % 2 == 0 ? firstOption : secondOption;
            return maxChars;
        }
        public bool VictoryConditionCheck(char playerChar)
        {
            if (HorizontalVictoryCheck(playerChar) || VerticalVictoryCheck(playerChar) || DiagonalVictoryCheck(playerChar))
                return true;
            else return false;
        }
        private bool HorizontalVictoryCheck(char playerChar)
        {
            int charsInARow = 0;
            for (int i = 0; i < Board.Count; i++)
            {
                for (int j = 0; j < Board[i].Count; j++)
                {
                    if (Board[i][j] == playerChar)
                        charsInARow++;
                    else charsInARow = 0;
                    if (charsInARow == 5)
                        return true;
                }
            }
            return false;
        }
        private bool VerticalVictoryCheck(char playerChar)
        {
            int charsInARow = 0;
            for (int i = 0; i < Board.Count; i++)
            {
                for (int j = 0; j < Board[i].Count; j++)
                {
                    if (Board[i][j] == playerChar)
                    {
                        for (int k = i; k < Board.Count; k++)
                        {
                            if (Board[k][j] == playerChar)
                                charsInARow++;
                            else
                            {
                                charsInARow = 0;
                                break;
                            }
                            if (charsInARow == 5)
                                return true;
                        }
                    }
                }
            }
            return false;
        }
        private bool DiagonalVictoryCheck(char playerChar)
        {
            int charsInARow = 0;
            int dc; // diagonalCounter
            for (int i = 0; i < Board.Count; i++)
            {
                for (int j = 0; j < Board[i].Count; j++)
                {
                    if (Board[i][j] == playerChar)
                    {
                        dc = j;
                        for (int k = i; k < Board.Count; k++)
                        {
                            if (Board[k][dc] == playerChar)
                            {
                                charsInARow++;
                                if (charsInARow == 5)
                                    return true;
                                if (dc < Board[i].Count - 1)
                                    dc++;
                                else break;
                            }
                            else
                            {
                                charsInARow = 0;
                                break;
                            }
                        }
                    }
                }
            }
            return false;
        }
        public void StartGame()
        {
            char player1 = 'X';
            char player2 = 'O';
            bool gameFinished = false;

            ShowGmaeBoard();
            while (!gameFinished)
            {
                if (AITurn(player1, player2))
                {
                    ShowGmaeBoard();
                    Console.WriteLine("Player 1 wins");
                    gameFinished = true;
                    continue;
                }
                ShowGmaeBoard();
                Thread.Sleep(200);
                if (AITurn(player2, player1))
                {
                    ShowGmaeBoard();
                    Console.WriteLine("Player 2 wins");
                    gameFinished = true;
                    continue;
                }
                ShowGmaeBoard();
                Thread.Sleep(200);
            }
            Console.ReadKey();
        }
        public void ShowGmaeBoard()
        {
            Console.Clear();
            for (int i = 0; i < Board.Count; i++)
            {
                for (int j = 0; j < Board[i].Count; j++)
                {
                    Console.Write(Board[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
