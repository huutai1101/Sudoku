using System.IO;
using UnityEngine;

public class Test : MonoBehaviour
{
    private string filePath;

    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "mycustomdata.json");
        Debug.Log("FilePath là: " + filePath);
    }

    private void Start()
    {
        int[,] my2DArray = new int[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 9 }
        };

        // Lưu trữ mảng
        Save2DArray(my2DArray);

        // Tải mảng
        int[,] loaded2DArray = Load2DArray();

        // In ra mảng đã tải
        if (loaded2DArray != null)
        {
            Debug.Log("In ra la");
            int rowCount = loaded2DArray.GetLength(0);
            int colCount = loaded2DArray.GetLength(1);

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    Debug.Log(loaded2DArray[i, j]);
                }
            }
        }
        else
        {
            Debug.Log("Ko load dc 2D array");
        }
    }

    public void Save2DArray(int[,] array)
    {
        // Chuyển đổi mảng thành định dạng JSON
        string json = JsonUtility.ToJson(new ArrayWrapper(array));

        // Ghi dữ liệu JSON vào file
        File.WriteAllText(filePath, json);
    }

    public int[,] Load2DArray()
    {
        if (File.Exists(filePath))
        {
            // Đọc dữ liệu từ file
            string json = File.ReadAllText(filePath);

            // Chuyển đổi từ JSON thành mảng
            ArrayWrapper wrapper = JsonUtility.FromJson<ArrayWrapper>(json);
            return wrapper.array;
        }
        else
        {
            Debug.LogWarning("File not found: " + filePath);
            return null;
        }
    }

    // Lớp wrapper để chứa mảng 2D và tạo JSON
    [System.Serializable]
    private class ArrayWrapper
    {
        public int[,] array;

        public ArrayWrapper(int[,] array)
        {
            this.array = array;
        }
    }
}
