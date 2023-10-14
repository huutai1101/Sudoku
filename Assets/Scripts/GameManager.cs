using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Scripts")]
    //private SolveSudoku solveSudoku; Test To Get New Solved Sudoku
    [HideInInspector] public NumbersBehaviour numberBehaviour;

    [Header ("Scriptable Objects Base Board")]
    [SerializeField] SudokuSO[] easySudokuSO;
    [SerializeField] SudokuSO[] mediumSudokuSO;
    [SerializeField] SudokuSO[] hardSudokuSO;

    [Header("Scriptable Objects Solved Board")]
    [SerializeField] SudokuSO[] easySolvedSudokuSO;
    [SerializeField] SudokuSO[] mediumSolvedSudokuSO;
    [SerializeField] SudokuSO[] hardSolvedSudokuSO;

    private SudokuSO playSudokuSO;
    private SudokuSO solvedSudokuSO;
    //public SudokuSO testSudokuSO; Test To Get New Solved Sudoku

    private int[,] initBoard;
    private int[,] board;
    public Number[,] playerBoard;

    private float playTime;
    private int mistake;

    [HideInInspector] public int idChosing;
    [HideInInspector] public int idRow;
    [HideInInspector] public int idCol;

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
        //solveSudoku = GetComponent<SolveSudoku>();
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
        SetSudokuSO();
        AddDataToBoards(playSudokuSO,solvedSudokuSO);
        LoadContinueData(GameSettings.instance.GetFileHandler().GetPlayerData());
        Time.timeScale = 1f;
    }

/*    #region Debug
    //Test To Get New Solved Sudoku
    private void DebugSudokuBoard(SudokuSO testSudokuSO)
    {
        Debug.Log("Ten " + testSudokuSO.name);
        AddDataToTest(testSudokuSO);
        numberBehaviour.LoadPlayerBoard(playerBoard, initBoard);
        solveSudoku.solveSudoku(board);
    }

    private void AddDataToTest(SudokuSO testSudokuSO)
    {
        for (int i = 0; i < this.board.GetLength(0); i++)
        {
            for (int j = 0; j < this.board.GetLength(1); j++)
            {
                this.initBoard[i, j] = testSudokuSO.Row[i].Column[j];
                this.board[i, j] = testSudokuSO.Row[i].Column[j];
            }
        }
    }
    #endregion*/


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

    private void SetSudokuSO()
    {
        if (GameSettings.instance == null)
        {
            Debug.LogWarning("Missing Game Settings instance");
            return;
        }
        if (GameSettings.instance.difficulty == GameSettings.Difficulty.Easy)
        {
            if (GameSettings.instance.stage > easySudokuSO.Length) //Kiem tra day co phai la stage cuoi chua?
            {
                GameSettings.instance.stage = 1;
            }
            this.playSudokuSO = easySudokuSO[GameSettings.instance.stage - 1];
            this.solvedSudokuSO = easySolvedSudokuSO[GameSettings.instance.stage - 1];
            return;
        }
        if (GameSettings.instance.difficulty == GameSettings.Difficulty.Medium)
        {
            if (GameSettings.instance.stage > mediumSudokuSO.Length) //Kiem tra day co phai la stage cuoi chua?
            {
                GameSettings.instance.stage = 1;
            }
            this.playSudokuSO = mediumSudokuSO[GameSettings.instance.stage - 1];
            this.solvedSudokuSO = mediumSolvedSudokuSO[GameSettings.instance.stage - 1];
            return;
        }
        if (GameSettings.instance.difficulty == GameSettings.Difficulty.Hard)
        {
            if (GameSettings.instance.stage > hardSudokuSO.Length) //Kiem tra day co phai la stage cuoi chua?
            {
                GameSettings.instance.stage = 1;
            }
            this.playSudokuSO = hardSudokuSO[GameSettings.instance.stage - 1];
            this.solvedSudokuSO = hardSolvedSudokuSO[GameSettings.instance.stage - 1];
        }
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

    private void AddDataToBoards(SudokuSO sudokuSO, SudokuSO solvedSudokuSO)
    {
        for (int i = 0; i < this.board.GetLength(0); i++)
        {
            for (int j = 0; j < this.board.GetLength(1); j++)
            {
                this.initBoard[i, j] = sudokuSO.Row[i].Column[j];
                this.board[i, j] = solvedSudokuSO.Row[i].Column[j];
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
