using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject btnPrefabs;
    [SerializeField] GameObject board;

    //Scripts References
    [SerializeField] HighlightManager highlightManager;
    [SerializeField] TextManager textManager;
    [SerializeField] PopupController popupCtrl;

    //Variables References
    private int idChosing;
    private int idRow;
    private int idCol;

    private void Awake()
    {
        highlightManager = GetComponent<HighlightManager>();
        textManager = GetComponent<TextManager>();
        popupCtrl = GetComponent<PopupController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadFunctionBoardUI(GameManager.instance.playerBoard);
        textManager.LoadDifficultyTexts(GameSettings.instance.difficulty.ToString());
        textManager.LoadMistakeTexts(GameManager.instance.Mistake,GameManager.instance.LimitMistake);
        textManager.LoadNoteModeText(GameManager.instance.NoteModeOn);
        LoadInitBoardUI(board.transform);
        if (GameSettings.instance.isContinue)
        {
            LoadBoardUIFromJson(GameManager.instance.playerBoard, GameManager.instance.GetInitBoard(), GameManager.instance.GetSolvedBoard());
            LoadNotesUIFromJson(GameSettings.instance.GetFileHandler().GetPlayerData());
        }
    }

    void Update()
    {
        textManager.LoadTimeText((int)GameManager.instance.PlayTime);
    }

    private void LoadFunctionBoardUI(Number[,] array)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                int id = GameManager.instance.playerBoard[i, j].ID;
                int idRow = i;
                int idCol = j;
                GameObject obj = Instantiate(btnPrefabs, this.board.transform);
                obj.name = "(" + i +"," + j +")".ToString();
                obj.GetComponent<Button>().onClick.AddListener(() => SetIdWhenClick(id,idRow,idCol));
                obj.GetComponent<Button>().onClick.AddListener(() => highlightManager.DoHighlights(board.transform, id, GameManager.instance.playerBoard));
                obj.GetComponent<Button>().onClick.AddListener(() => HighlightTheSameNumbers(GameManager.instance.playerBoard[idRow,idCol].Value));
                obj.GetComponent<Button>().onClick.AddListener(() => LoadLockColor());
                if (array[i,j].Value == 0)
                {
                    obj.transform.GetChild(0).GetComponent<Text>().text = "";
                }
                else
                {
                    obj.transform.GetChild(0).GetComponent<Text>().text = array[i, j].Value.ToString();
                }
                //LoadUIWhetherCorrect(idRow, idCol);
            }
        }
    }

    private void LoadInitBoardUI(Transform board)
    {
        for (int i = 0; i < board.childCount; i++)
        {
            board.GetChild(i).GetChild(0).GetComponent<Text>().color = highlightManager.LockColor;
        }
    }

    private void LoadLockColor()
    {
        if(GameManager.instance.GetValueFromInitBoard(idRow,idCol) != 0)
        {
            board.transform.GetChild(idChosing).GetChild(0).GetComponent<Text>().color = highlightManager.LockColor;
        }
    }

    private void LoadUI(int number)
    {
        if(number == 0)
        {
            board.transform.GetChild(idChosing).GetChild(0).GetComponent<Text>().text = "";
            return;
        }
        else
        {
            board.transform.GetChild(idChosing).GetChild(0).GetComponent<Text>().text = number.ToString();
        }
        highlightManager.DoHighlights(board.transform, idChosing, GameManager.instance.playerBoard);
        HighlightTheSameNumbers(number);
    }

    private void LoadUIWhetherCorrect(int idRow, int idCol)
    {
        if (GameManager.instance.playerBoard[idRow,idCol].Value == 0)
        {
            return;
        }
        //Kiểm tra người chơi có điền giá trị đúng không, từ đó hiển thị UI tương ứng
        if (GameManager.instance.IsCorrect(idRow, idCol))
        {
            Debug.Log("Ban da dien dung");
            GameManager.instance.numberBehaviour.SetLock(idRow, idCol);
            board.transform.GetChild(idChosing).GetComponent<Image>().color = highlightManager.PressCellColor;
            board.transform.GetChild(idChosing).GetChild(0).GetComponent<Text>().color = highlightManager.PlayerColor;
        }
        else
        {
            Debug.Log("Ban da dien sai");
            textManager.LoadMistakeTexts(GameManager.instance.Mistake, GameManager.instance.LimitMistake);
            board.transform.GetChild(idChosing).GetComponent<Image>().color = highlightManager.WrongCellColor;
            board.transform.GetChild(idChosing).GetChild(0).GetComponent<Text>().color = highlightManager.WrongColor;
        }
    }

    private void HighlightTheSameNumbers(int number)
    {
        int id = 0;
        for (int i = 0; i < GameManager.instance.GetSolvedBoard().GetLength(0); i++)
        {
            for (int j = 0; j < GameManager.instance.GetSolvedBoard().GetLength(1); j++)
            {
                if (number == GameManager.instance.playerBoard[i, j].Value && number == GameManager.instance.GetCorrectValue(i, j))
                {
                    board.transform.GetChild(id).GetComponent<Image>().color = highlightManager.SameColor;
                }
                if (id == idChosing)
                {
                    LoadUIWhetherCorrect(idRow, idCol);
                }
                id++;
            }
        }
    }

    private void SetIdWhenClick(int id, int idRow, int idCol)
    {
        GameManager.instance.idChosing = id;
        GameManager.instance.idRow = idRow;
        GameManager.instance.idCol = idCol;
        SetReferenceIDs();
        Debug.Log("idChosing = " + idChosing + ", idRow = " + idRow + ", idCol = " + idCol);
    }

    private void SetReferenceIDs()
    {
        if (GameManager.instance != null)
        {
            this.idChosing = GameManager.instance.idChosing;
            this.idRow = GameManager.instance.idRow;
            this.idCol = GameManager.instance.idCol;
        }
    }

    #region Fill Numbers (1 -> 9)
    public void FillNumbersButton(int number)
    {
        if (!GameManager.instance.DoesPlayerChooseCell())
        {
            Debug.Log("Hay chon o de dien");
            return;
        }
        if(GameManager.instance.playerBoard[idRow, idCol].IsLock)
        {
            Debug.Log("Khong the dien");
            return;
        }
        if(GameManager.instance.NoteModeOn)
        {
            WriteNote(number);
            return;
        }
        UndoController.instance.SaveFirstAction(GameManager.instance.playerBoard[idRow, idCol].Value, idChosing, idRow, idCol, GameManager.instance.playerBoard[idRow, idCol].IsLock);
        ClearNote(idChosing);
        GameManager.instance.playerBoard[idRow, idCol].Value = number;
        GameManager.instance.AddMistake(idRow, idCol);
        LoadUI(number);
        UndoController.instance.SaveAction(GameManager.instance.playerBoard[idRow, idCol].Value, idChosing, idRow, idCol, GameManager.instance.playerBoard[idRow, idCol].IsLock);
        if(GameManager.instance.IsWin())
        {
            GameManager.instance.ChangePauseStatus();
            popupCtrl.GetWinObj().SetActive(true);
            return;
        }
        if(GameManager.instance.IsGameOver)
        {
            GameManager.instance.ChangePauseStatus();
            textManager.SetGameoverNotice(GameManager.instance.Mistake);
            popupCtrl.GetGameoverbj().SetActive(true);
            return;
        }
    }
    #endregion

    #region Undo Function
    public void UndoButton()
    {
        if (UndoController.instance.StackPlayerActions.Count > 1)
        {
            Debug.Log("Before Undo Count: " + UndoController.instance.StackPlayerActions.Count);
            PlayerAction previousAction = UndoController.instance.UndoAction();
            Debug.Log("After Undo Count: " + UndoController.instance.StackPlayerActions.Count);
            GameManager.instance.idChosing = previousAction.id;
            GameManager.instance.idRow = previousAction.idRow;
            GameManager.instance.idCol = previousAction.idCol;
            SetReferenceIDs();
            GameManager.instance.playerBoard[idRow, idCol].Value = previousAction.value;
            GameManager.instance.playerBoard[idRow, idCol].IsLock = previousAction.isLock;
            for (int i = 0; i < previousAction.notes.Length; i++)
            {
                board.transform.GetChild(this.idChosing).GetChild(1).GetChild(i).gameObject.SetActive(previousAction.notes[i]);
                board.transform.GetChild(this.idChosing).GetChild(1).GetChild(i).GetComponent<Text>().color = highlightManager.NoteColor;
            }
            Debug.Log("After Notes Count: " + UndoController.instance.StackPlayerActions.Count);
            Debug.Log("Undo: ID Row = " + GameManager.instance.idRow + ", ID Col = " + GameManager.instance.idCol + ", Value = " + GameManager.instance.playerBoard[idRow, idCol].Value);
            LoadUI(previousAction.value);
            highlightManager.DoHighlights(board.transform, idChosing, GameManager.instance.playerBoard);
            UndoController.instance.DeleteJunkAction();
        }
    }
    #endregion
    #region Note Function
    public void NoteModeButton()
    {
        GameManager.instance.ChangeStatusNoteMode();
        textManager.LoadNoteModeText(GameManager.instance.NoteModeOn);
    }

    private void WriteNote(int number)
    {
        UndoController.instance.SaveFirstAction(GameManager.instance.playerBoard[idRow, idCol].Value, idChosing, idRow, idCol, GameManager.instance.playerBoard[idRow, idCol].IsLock);
        int id = number - 1;
        GameManager.instance.playerBoard[idRow, idCol].Value = 0;
        board.transform.GetChild(this.idChosing).GetChild(0).GetComponent<Text>().text = "";
        bool noteActive = board.transform.GetChild(this.idChosing).GetChild(1).GetChild(id).gameObject.activeSelf;
        board.transform.GetChild(this.idChosing).GetChild(1).GetChild(id).gameObject.SetActive(!noteActive);
        board.transform.GetChild(this.idChosing).GetChild(1).GetChild(id).GetComponent<Text>().color = highlightManager.NoteColor;
        board.transform.GetChild(this.idChosing).GetComponent<Image>().color = highlightManager.PressCellColor;
        bool[] notes = new bool[9];
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i] = board.transform.GetChild(this.idChosing).GetChild(1).GetChild(i).gameObject.activeSelf;
        }
        UndoController.instance.SaveAction(GameManager.instance.playerBoard[idRow, idCol].Value, idChosing, idRow, idCol, GameManager.instance.playerBoard[idRow, idCol].IsLock, notes);
    }

    private void ClearNote(int id)
    {
        for (int i = 0; i < 9; i++)
        {
            board.transform.GetChild(id).GetChild(1).GetChild(i).gameObject.SetActive(false);
        }
    }

    private bool HasNote(int id)
    {
        for (int i = 0; i < 9; i++)
        {
            if (board.transform.GetChild(id).GetChild(1).GetChild(i).gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }
    #endregion
    #region Erase Function
    public void EraseButton()
    {
        if(GameManager.instance.IsGameOver)
        {
            return;
        }
        if (GameManager.instance.playerBoard[idRow,idCol].IsLock)
        {
            Debug.Log("So da dung, ko the xoa");
            return;
        }
        if(HasNote(idChosing))
        {
            Debug.Log("Co Note");
            ClearNote(idChosing);
            UndoController.instance.SaveAction(GameManager.instance.playerBoard[idRow, idCol].Value, idChosing, idRow, idCol, GameManager.instance.playerBoard[idRow, idCol].IsLock);
            return;
        }
        if (GameManager.instance.playerBoard[idRow, idCol].Value == 0)
        {
            return;
        }
        UndoController.instance.SaveFirstAction(GameManager.instance.playerBoard[idRow,idCol].Value, idChosing, idRow, idCol, GameManager.instance.playerBoard[idRow, idCol].IsLock);
        GameManager.instance.playerBoard[idRow, idCol].Value = 0;
        board.transform.GetChild(idChosing).GetChild(0).GetComponent<Text>().text = "";
        board.transform.GetChild(idChosing).GetComponent<Image>().color = highlightManager.PressCellColor;
        UndoController.instance.SaveAction(GameManager.instance.playerBoard[idRow, idCol].Value, idChosing, idRow, idCol, GameManager.instance.playerBoard[idRow, idCol].IsLock);
    }
    #endregion

    #region Home Function
    public void HomeButton()
    {
        SaveDataToJson();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    private void SaveDataToJson()
    {
        PlayerData data = new PlayerData();
        data.isContinue = true;
        data.difficulty = GameSettings.instance.difficulty.ToString();
        data.stage = GameSettings.instance.stage;
        data.playTime = GameManager.instance.PlayTime;
        data.mistake = GameManager.instance.Mistake;
        GameManager.instance.numberBehaviour.SavePlayerBoardToJson(GameManager.instance.playerBoard, data.playerBoard);
        SaveListNoteToJson(data.listNotes);
        GameSettings.instance.GetFileHandler().SaveToJson(data);
    }
    #endregion

    #region Pause Function
    public void PauseButton()
    {
        GameManager.instance.ChangePauseStatus();
        popupCtrl.GetPauseObj().SetActive(true);
    }
    #endregion

    #region Hint Function
    public void HintButton()
    {
        if(!GameManager.instance.DoesPlayerChooseCell())
        {
            Debug.Log("Xin hãy chọn ô để giải");
            return;
        }
        if (GameManager.instance.IsCorrect(idRow,idCol))
        {
            Debug.Log("Ô này đã đúng, hãy chọn ô khác");
            return;
        }
        UndoController.instance.SaveFirstAction(GameManager.instance.playerBoard[idRow, idCol].Value, idChosing, idRow, idCol, GameManager.instance.playerBoard[idRow, idCol].IsLock);
        ClearNote(idChosing);
        GameManager.instance.playerBoard[idRow, idCol].Value = GameManager.instance.GetCorrectValue(idRow, idCol);
        LoadUI(GameManager.instance.playerBoard[idRow, idCol].Value);
        UndoController.instance.SaveAction(GameManager.instance.playerBoard[idRow, idCol].Value, idChosing, idRow, idCol, GameManager.instance.playerBoard[idRow, idCol].IsLock);
        if (GameManager.instance.IsWin())
        {
            GameManager.instance.ChangePauseStatus();
            popupCtrl.GetWinObj().SetActive(true);
        }
    }
    #endregion

    #region Load UI From Json
    void LoadBoardUIFromJson(Number[,] playerBoard, int[,] initBoard, int[,] solvedBoard)
    {
        int id = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (playerBoard[i, j].Value == initBoard[i, j])
                {
                    //Ve mau board mac dinh
                    board.transform.GetChild(id).GetChild(0).GetComponent<Text>().color = highlightManager.LockColor;
                }
                else
                {
                    if (playerBoard[i,j].Value == solvedBoard[i,j])
                    {
                        //Ve mau board nguoi choi tra loi dung
                        board.transform.GetChild(id).GetChild(0).GetComponent<Text>().color = highlightManager.PlayerColor;
                    }
                    else
                    {
                        //Ve mau board nguoi choi tra loi sai
                        board.transform.GetChild(id).GetComponent<Image>().color = highlightManager.WrongCellColor;
                        board.transform.GetChild(id).GetChild(0).GetComponent<Text>().color = highlightManager.WrongColor;
                    }
                }
                id++;
            }
        }
    }

    private void LoadNotesUIFromJson(PlayerData data)
    {
        for (int i = 0; i < data.listNotes.Count; i++)
        {
            if (data.listNotes[i].noteNumber.Count > 0)
            {
                for (int j = 0; j < data.listNotes[i].noteNumber.Count; j++)
                {
                    LoadNoteUI(data.listNotes[i].noteNumber[j], i);
                }
            }
        }
    }

    private void LoadNoteUI(int number, int index)
    {
        int idNumber = number - 1;
        board.transform.GetChild(index).GetChild(1).GetChild(idNumber).gameObject.SetActive(true);
        board.transform.GetChild(index).GetChild(1).GetChild(idNumber).GetComponent<Text>().color = highlightManager.NoteColor;
    }

    private void SaveListNoteToJson(List<Note> listNotes)
    {
        for (int i = 0; i < board.transform.childCount; i++)
        {
            Note note = new Note();
            note.id = i;
            for (int j = 0; j < 9; j++)
            {
                List<int> noteNumber = new List<int>();
                if (board.transform.GetChild(i).GetChild(1).GetChild(j).gameObject.activeSelf)
                {
                    note.noteNumber.Add(j + 1);
                }
            }
            listNotes.Add(note);
        }
    }
    #endregion
}
