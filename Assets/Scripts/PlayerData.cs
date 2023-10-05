using System.Collections.Generic;
[System.Serializable]
public class PlayerData
{
    public bool isContinue;
    public string difficulty;
    public int stage;
    public float playTime;
    public int mistake;
    public List<Square> playerBoard;
    public List<Note> listNotes;

    public PlayerData()
    {
        playerBoard = new List<Square>();
        listNotes = new List<Note>();
    }
}
[System.Serializable]
public class Square
{
    public int ID;
    public int IDRow;
    public int IDCol;
    public int Value;
    public bool IsLock;

    public Square(int id, int idRow, int idCol, int value, bool isLock)
    {
        this.ID = id;
        this.IDRow = idRow;
        this.IDCol = idCol;
        this.Value = value;
        this.IsLock = isLock;
    }
}
[System.Serializable]
public class Note
{
    public int id;
    public List<int> noteNumber;

    public Note()
    {
        noteNumber = new List<int>();
    }
}
