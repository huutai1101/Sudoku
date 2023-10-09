using UnityEngine;
using UnityEngine.UI;

public class HighlightManager : MonoBehaviour
{
    [Header("BG Color")]
    [SerializeField] private Color pressCellColor;
    [SerializeField] private Color wrongCellColor;
    [SerializeField] private Color highlightAreaColor;
    [SerializeField] private Color refreshColor;

    [Header("Font Color")]
    [SerializeField] private Color lockColor;
    [SerializeField] private Color playerColor;
    [SerializeField] private Color wrongColor;
    [SerializeField] private Color noteColor;

    [Header("Sprite X")]
    [SerializeField] private Color sameColor;

    public Color PressCellColor { get { return pressCellColor;}}
    public Color WrongCellColor { get { return wrongCellColor;}}
    public Color HighlightAreaColor { get { return highlightAreaColor; } }
    public Color RefreshColor { get { return refreshColor; } }
    public Color LockColor { get { return lockColor; } }
    public Color PlayerColor { get { return playerColor; } }
    public Color WrongColor { get { return wrongColor;}}
    public Color NoteColor { get { return noteColor; } }
    public Color SameColor { get { return sameColor; }}

    #region Methods
    public void DoHighlights(Transform target, int idChosing, Number[,] numbers)
    {
        int idRowChosing = GameManager.instance.numberBehaviour.FindIdRow(GameManager.instance.playerBoard, idChosing);
        int idColChosing = GameManager.instance.numberBehaviour.FindIdCol(GameManager.instance.playerBoard, idChosing);
        RefreshHighlight(target);
        HighlightRow(target,idChosing);
        HighlightCollumn(target, idChosing);
        HighlightArea3x3(target, idRowChosing, idColChosing);
        HighlightCell(target, idChosing);
        HighlightWrongCells(numbers, target);
    }
    private void HighlightCell(Transform target, int idChosing)
    {
        target.GetChild(idChosing).GetComponent<Image>().color = PressCellColor;
    }

    private void HighlightWrongCells(Number[,] numbers, Transform target)
    {
        for (int i = 0; i < numbers.GetLength(0); i++)
        {
            for (int j = 0; j < numbers.GetLength(1); j++)
            {
                if (!numbers[i,j].IsLock && numbers[i,j].Value != 0)
                {
                    HighlightWrongCell(target, numbers[i, j].ID);
                }
            }
        }
    }

    private void HighlightWrongCell(Transform target, int id)
    {
        target.GetChild(id).GetComponent<Image>().color = WrongCellColor;
    }

    public void RefreshHighlight(Transform target)
    {
        for (int i = 0; i < target.childCount; i++)
        {
            target.GetChild(i).GetComponent<Image>().color = RefreshColor;
        }
    }

    private void HighlightRow(Transform target, int idChosing)
    {
        int idBegin = idChosing / 9 * 9;
        int idEnd = idBegin + 9;
        for (int i = idBegin; i < idEnd; i++)
        {
            target.GetChild(i).GetComponent<Image>().color = HighlightAreaColor;
        }
    }

    private void HighlightCollumn(Transform target, int idChosing)
    {
        int idBegin = idChosing % 9;
        int idEnd = idBegin + 9 * 9;
        for (int i = idBegin; i < idEnd; i += 9)
        {
            target.GetChild(i).GetComponent<Image>().color = HighlightAreaColor;
        }
    }

    private void HighlightArea3x3(Transform target, int idRow, int idCol)
    {
        int idRowBegin = idRow / 3 * 3;
        int idRowEnd = idRowBegin + 2;
        int idColBegin = idCol / 3 * 3;
        int idColEnd = idColBegin + 2;
        //Debug.Log("idRow = " + idRowBegin + " , idCol = " + idColBegin);
        for (int i = idRowBegin; i <= idRowEnd; i++)
            for (int j = idColBegin; j <= idColEnd; j++)
            {
                int id = GameManager.instance.playerBoard[i, j].ID;
                target.GetChild(id).GetComponent<Image>().color = HighlightAreaColor;
            }
    }
    #endregion
}
