using UnityEngine;

[System.Serializable]
public class Number : MonoBehaviour
{
    [SerializeField] int value;
    [SerializeField] int id;
    [SerializeField] int idRow;
    [SerializeField] int idCol;
    [SerializeField] bool isLock;

    public int Value
    {
        set
        {
            if (value == 0) isLock = false;
            if (value >= 0 && value <= 9) this.value = value;
        }
        get
        {
            return this.value;
        }
    }
    public int ID { get { return id; } }
    public int IDRow { get { return idRow; } }
    public int IDCol { get { return idCol; } }
    public bool IsLock
    {
        get { return isLock; }
        set { isLock = value; }
    }

    public Number(int value, int id, int idRow, int idCol, bool isLock)
    {
        this.value = value;
        this.id = id;
        this.idRow = idRow;
        this.idCol = idCol;
        this.isLock = isLock;
    }

    public Number(PlayerData data, int id)
    {
        this.value = data.playerBoard[id].Value;
        this.id = data.playerBoard[id].ID;
        this.idRow = data.playerBoard[id].IDRow;
        this.idCol = data.playerBoard[id].IDCol;
        this.isLock = data.playerBoard[id].IsLock;
    }
}
