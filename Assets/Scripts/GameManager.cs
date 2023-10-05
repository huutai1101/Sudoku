using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Scripts")]
    private SolveSudoku solveSudoku;
    [HideInInspector] public NumbersBehaviour numberBehaviour;

    [Header ("Scriptable Objects")]
    [SerializeField] SudokuSO[] easySudokuSO;
    [SerializeField] SudokuSO[] mediumSudokuSO;
    [SerializeField] SudokuSO[] hardSudokuSO;
    private SudokuSO playSudokuSO;

    private int[,] initBoard;
    private int[,] board;
    public Number[,] playerBoard;

    private float playTime;
    private int mistake;

    public int idChosing;
    public int idRow;
    public int idCol;

    public float PlayTime { get { return playTime; } }
    public int Mistake
    {
        set { if (value > mistake) mistake = value; }
        get { return mistake; }
    }

    [SerializeField] private int limitMistake;
    public int LimitMistake
    {
        get { return limitMistake; }
    }

    private bool isGameOver;
    public bool IsGameOver
    {         
        get { return isGameOver = mistake >= limitMistake ? true : false; }
    }

    private bool noteModeOn;
    public bool NoteModeOn
    {
        get { return noteModeOn; }
    }

    private bool pause;
    public bool Pause
    {
        set { pause = value; }
        get { return pause; }
    }


    private void Awake()
    {
        MakeInstance();
        solveSudoku = GetComponent<SolveSudoku>();
        numberBehaviour = GetComponent<NumbersBehaviour>();
        playerBoard = new Number[9, 9];
        initBoard = new int[9, 9];
        board = new int[9, 9];
    }

    private void MakeInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        isGameOver = false;
        idChosing = -1;
        idRow = -1;
        idCol = -1;
        noteModeOn = false;
        playSudokuSO = GetSudokuSO();
        AddDataToBoard(playSudokuSO);
        LoadContinueData(GameSettings.instance.GetFileHandler().GetPlayerData());
        solveSudoku.solveSudoku(board);
        Time.timeScale = 1f;
/*        numberBehaviour.ShowAvailableCells(playerBoard);*/
    }

    private void Update()
    {
        playTime += Time.deltaTime;
    }

    private void LoadContinueData(PlayerData data)
    {
        if(data == null)
        {
            return;
        }
        if (GameSettings.instance.isContinue)
        {
            numberBehaviour.LoadPlayerBoardFromJson(data, playerBoard);
            playTime = data.playTime;
            mistake = data.mistake;
        }
        else
        {
            numberBehaviour.LoadPlayerBoard(playerBoard, initBoard);
            playTime = 0f;
            mistake = 0;
        }
    }

    public SudokuSO GetSudokuSO()
    {
        if(GameSettings.instance == null)
        {
            Debug.LogWarning("Missing Game Settings instance");
            return null;
        }
        if(GameSettings.instance.difficulty == GameSettings.Difficulty.Easy)
        {
            if(GameSettings.instance.stage > easySudokuSO.Length) //Kiem tra day co phai la stage cuoi chua?
            {
                GameSettings.instance.stage = 1;
            }
            return easySudokuSO[GameSettings.instance.stage - 1];
        }
        if (GameSettings.instance.difficulty == GameSettings.Difficulty.Medium)
        {
            return mediumSudokuSO[GameSettings.instance.stage - 1];
        }
        return hardSudokuSO[GameSettings.instance.stage - 1];
    }

    public void ShowResult(int[,] board)
    {
        string result = "";
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                result += board[i, j].ToString();
                if (j == 8)
                {
                    result += "\n";
                }
            }
        }
        Debug.Log(result);
    }

    private void AddDataToBoard(SudokuSO sudokuSO)
    {
        for (int i = 0; i < this.board.GetLength(0); i++)
        {
            for (int j = 0; j < this.board.GetLength(1); j++)
            {
                this.board[i, j] = sudokuSO.Row[i].Column[j];
                this.initBoard[i, j] = sudokuSO.Row[i].Column[j];
            }
        }
    }

    public int GetValueFromInitBoard(int idRow, int idCol)
    {
        return this.initBoard[idRow, idCol];
    }

    public void AddMistake(int rowId,int colId)
    {
        if (!IsCorrect(rowId, colId))
        {
            mistake += 1;
        }
    }

    public bool IsWin()
    {
        for (int i = 0; i < this.board.GetLength(0); i++)
        {
            for (int j = 0; j < this.board.GetLength(1); j++)
            {
                if (!IsCorrect(i, j))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool IsCorrect(int rowId, int colId)
    {
/*        Debug.Log("người chơi điền: " + this.playerBoard[rowId, colId].Value);
        Debug.Log("Đáp án: " + this.board[rowId, colId]);*/
        if (this.playerBoard[rowId, colId].Value == this.board[rowId, colId])
        {
            return true;
        }
        return false;
    }

    public int GetCorrectValue(int idRow, int idCol)
    {
        return board[idRow, idCol];
    }

    public int[,] GetInitBoard()
    {
        return this.initBoard;
    }

    public int[,] GetSolvedBoard()
    {
        return this.board;
    }

    public bool DoesPlayerChooseCell()
    {
        if(idChosing == -1 || idCol == -1 || idRow == -1)
        {
            return false;
        }
        return true;
    }

    public void ChangeStatusNoteMode()
    {
        this.noteModeOn = !this.noteModeOn;
    }

    public void ChangePauseStatus()
    {
        this.pause = !this.pause;
        if(pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
