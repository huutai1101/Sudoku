using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolveSudoku : MonoBehaviour
{
    public void solveSudoku(int[,] board)
    {
        if (board == null || board.Length == 0)
            return;
        Solve(board);
    }
    private bool Solve(int[,] board)
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] == 0)
                {
                    for (int currentNumber = 1; currentNumber <= 9; currentNumber++)
                    {
                        if (IsValid(board, i, j, currentNumber))
                        {
                            board[i, j] = currentNumber;

                            if (Solve(board))
                                return true;
                            else
                                board[i, j] = 0;
                        }
                    }
                    return false;
                }
            }
        }
        return true;
    }
    private bool IsValid(int[,] board, int row, int col, int currentNumber)
    {
        for (int i = 0; i < 9; i++)
        {
            //check row  
            if (board[i, col] != 0 && board[i, col] == currentNumber)
                return false;
            //check column  
            if (board[row, i] != 0 && board[row, i] == currentNumber)
                return false;
            //check 3*3 block  
            if (board[3 * (row / 3) + i / 3, 3 * (col / 3) + i % 3] != 0 && board[3 * (row / 3) + i / 3, 3 * (col / 3) + i % 3] == currentNumber)
                return false;
        }
        return true;
    }
}
