using UnityEngine;

[CreateAssetMenu (fileName = "New Sudoku Board", menuName = "Scriptable Objects/New Sudoku Board")]
public class SudokuSO : ScriptableObject
{
    [SerializeField] private Cell[] row = new Cell[9];
    public Cell[] Row { get { return row; } }
}

[System.Serializable]
public class Cell
{
    [SerializeField] private int[] column = new int[9];
    public int[] Column { get { return column;} }
}
