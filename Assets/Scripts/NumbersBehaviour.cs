using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumbersBehaviour : MonoBehaviour
{
    public void SavePlayerBoardToJson(Number[,] fromArray, List<Square> toArray)
    {
        for (int i = 0; i < fromArray.GetLength(0); i++)
        {
            for (int j = 0; j < fromArray.GetLength(1); j++)
            {
                Square number = new Square(fromArray[i, j].ID, fromArray[i, j].IDRow, fromArray[i, j].IDCol, fromArray[i, j].Value, fromArray[i, j].IsLock);
                toArray.Add(number);
            }
        }
    }

    public void LoadPlayerBoardFromJson(PlayerData jsonData, Number[,] array)
    {
        if(array == null)
        {
            array = new Number[9, 9];
        }
        int id = 0;
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                Number number = new Number(jsonData, id);
                array[i, j] = number;
                id++;
            }
        }
    }

    public void LoadPlayerBoard(Number[,] array, int[,] board)
    {
        if (array == null)
        {
            array = new Number[9, 9];
        }
        int id = 0;
        bool isLock = false;
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] != 0)
                {
                    isLock = true;
                }
                else
                {
                    isLock = false;
                }
                Number number = new Number(board[i, j], id, i, j, isLock);
                array[i, j] = number;
                id++;
            }
        }
    }

    public int FindIdRow(Number[,] array, int idChosing)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                if (array[i, j].ID == idChosing)
                {
                    return array[i, j].IDRow;
                }
            }
        }
        return -1;
    }
    public int FindIdCol(Number[,] array, int idChosing)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                if (array[i, j].ID == idChosing)
                {
                    return array[i, j].IDCol;
                }
            }
        }
        return -1;
    }

    public void SetLock(int rowId, int colId)
    {
        GameManager.instance.playerBoard[rowId, colId].IsLock = true;
    }

}
