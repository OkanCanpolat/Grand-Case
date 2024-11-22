using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BFS_MinMoveCalculator : IMinMoveCalculator
{
    [Inject] private Board board;
   
    public int GetMinimumMoveToMatch()
    {
        Queue<(int[,], int)> queue = new Queue<(int[,], int)>();
        HashSet<string> visited = new HashSet<string>();
        List<(Vector2Int, Vector2Int)> possibleMoves = GetPossibleMoves();

        int[,] initialBoard = BoardToInt();
        queue.Enqueue((initialBoard, 0));
        visited.Add(BoardToString(initialBoard));

        while (queue.Count > 0)
        {
            var (currentBoard, moveCount) = queue.Dequeue();

            foreach (var move in possibleMoves)
            {
                var (start, end) = move;

                int temp = currentBoard[start.x, start.y];
                currentBoard[start.x, start.y] = currentBoard[end.x, end.y];
                currentBoard[end.x, end.y] = temp;

                string boardString = BoardToString(currentBoard);

                if (!visited.Contains(boardString))
                {
                    if (FindMatch(currentBoard))
                    {
                        return moveCount + 1;
                    }

                    int[,] clone = (int[,])currentBoard.Clone();
                    queue.Enqueue((clone, moveCount + 1));
                    visited.Add(boardString);
                }

                currentBoard[end.x, end.y] = currentBoard[start.x, start.y];
                currentBoard[start.x, start.y] = temp;
            }
        }
        return -1;
    }
    private List<(Vector2Int, Vector2Int)> GetPossibleMoves()
    {
        List<(Vector2Int, Vector2Int)> moves = new List<(Vector2Int, Vector2Int)>();

        for (int col = 0; col < board.Width; col++)
        {
            for (int row = 0; row < board.Height; row++)
            {
                if (col > 0)
                {
                    moves.Add((new Vector2Int(col, row), new Vector2Int(col - 1, row)));
                }
                if (col < board.Width - 1)
                {
                    moves.Add((new Vector2Int(col, row), new Vector2Int(col + 1, row)));
                }
                if (row > 0)
                {
                    moves.Add((new Vector2Int(col, row), new Vector2Int(col, row - 1)));
                }
                if (row < board.Height - 1)
                {
                    moves.Add((new Vector2Int(col, row), new Vector2Int(col, row + 1)));
                }
            }
        }

        return moves;
    }
    private string BoardToString(int[,] board)
    {
        string result = "";

        for (int col = 0; col < board.GetLength(0); col++)
        {
            for (int row = 0; row < board.GetLength(1); row++)
            {
                result += board[col, row].ToString();
            }
        }

        return result;
    }
    private int[,] BoardToInt()
    {
        int[,] result = new int[(int)board.Width, (int)board.Height];

        for (int col = 0; col < board.Width; col++)
        {
            for (int row = 0; row < board.Height; row++)
            {
                int typeIndex = (int)board.Containers[col, row].Tile.Type;
                result[col, row] = typeIndex;
            }
        }

        return result;
    }
    private bool FindMatch(int[,] board)
    {
        for (int col = 0; col < board.GetLength(0); col++)
        {
            for (int row = 0; row < board.GetLength(1); row++)
            {
                if (col <= board.GetLength(0) - 3 && board[col, row] == board[col + 1, row] && board[col, row] == board[col + 2, row])
                {
                    return true;
                }

                if (row <= board.GetLength(1) - 3 && board[col, row] == board[col, row + 1] && board[col, row] == board[col, row + 2])
                {
                    return true;
                }
            }
        }

        return false;
    }
}
