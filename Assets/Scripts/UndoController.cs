using System.Collections.Generic;
using UnityEngine;

public class UndoController : MonoBehaviour
{
    public static UndoController instance;
    private Stack<PlayerAction> stackPlayerActions;
    private PlayerAction newAction, oldAction;

    public Stack<PlayerAction> StackPlayerActions { get { return stackPlayerActions; } }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            stackPlayerActions = new Stack<PlayerAction>();
        }
        else
        {
            Destroy(this);
        }
    }

    //Delete junk player action
    public void DeleteJunkAction()
    {
        if (stackPlayerActions.Count == 1)
        {
            stackPlayerActions.Pop();
            return;
        }
        if (stackPlayerActions.Count > 1)
        {
            PlayerAction backupAction = stackPlayerActions.Pop();
            PlayerAction olderAction = stackPlayerActions.Peek();
            if(backupAction.id == olderAction.id)
            {
                stackPlayerActions.Push(backupAction);
            }
        }
    }

    public void SaveAction(int value, int id, int idRow, int idCol, bool isLock, bool[] notes)
    {
        PlayerAction newAction = new PlayerAction(value, id, idRow, idCol, isLock, notes);
        stackPlayerActions.Push(newAction);
    }

    public void SaveAction(int value, int id, int idRow, int idCol,bool isLock)
    {
        PlayerAction newAction = new PlayerAction(value, id, idRow, idCol, isLock);
        stackPlayerActions.Push(newAction);
    }

    public void SaveFirstAction(int value, int id, int idRow, int idCol, bool isLock)
    {
        if (stackPlayerActions.Count == 0)
        {
            SaveAction(value, id, idRow, idCol, isLock);
        }
        else if(stackPlayerActions.Peek().id != id)
        {
            SaveAction(value, id, idRow, idCol, isLock);
        }
    }

    public PlayerAction UndoAction()
    {
        newAction = stackPlayerActions.Pop();
        oldAction = stackPlayerActions.Peek();
        if(newAction.id != oldAction.id)
        {
            newAction = stackPlayerActions.Peek();
            ProcessUndo(oldAction);
        }
        return oldAction;
    }

    private void ProcessUndo(PlayerAction oldAction)
    {
        stackPlayerActions.Pop();
        oldAction = stackPlayerActions.Peek();
        if(oldAction.id != this.newAction.id) 
        {
            ProcessUndo(oldAction);
        }
    }
}

public struct PlayerAction
{
    public int value;
    public int id;
    public int idRow;
    public int idCol;
    public bool isLock;
    public bool[] notes;

    public PlayerAction(int value, int id, int idRow, int idCol, bool isLock, bool[] notes)
    {
        this.value = value;
        this.id = id;
        this.idRow = idRow;
        this.idCol = idCol;
        this.isLock = isLock;
        this.notes = notes;
    }

    public PlayerAction(int value, int id, int idRow, int idCol, bool isLock)
    {
        this.value = value;
        this.id = id;
        this.idRow = idRow;
        this.idCol = idCol;
        this.isLock = isLock;
        this.notes = new bool[9];
    }
}
