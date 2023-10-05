using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileHandler : MonoBehaviour
{
    [SerializeField] string jsonPath;
    [SerializeField] string jsonName;
    private string filePath;

    private PlayerData data;

    private void Awake()
    {
        data = new PlayerData();
        //string filePath = Path.Combine(Application.persistentDataPath, "data.json");
        // Đường dẫn tới tệp JSON
        filePath = Path.Combine(jsonPath, jsonName + ".json");
    }

    public void LoadFromJson()
    {
        //Đã có file
        if (File.Exists(filePath))
        {
            // Đọc nội dung từ tệp JSON
            string jsonString = File.ReadAllText(filePath);
            data = LoadFromJson(jsonString);

            // Sử dụng dữ liệu trong biến chuỗi
            Debug.Log("Dữ liệu từ tệp JSON: " + jsonString);
        }
        else //Chưa có file
        {
            data.isContinue = false;
            data.difficulty = "Unknow";
            data.stage = 1;
            data.playTime = 0;
            data.mistake = 0;
            Debug.Log("Application.dataPath là: " + Application.dataPath);
            SaveToJson(data);
        }
    }

    public void SaveToJson(PlayerData data)
    {
        string playerData = JsonUtility.ToJson(data);
        // Ghi nội dung JSON vào tệp
        File.WriteAllText(filePath, playerData);
    }

    //Load from Json
    private PlayerData LoadFromJson(string jsonString)
    {
        return JsonUtility.FromJson<PlayerData>(jsonString);
    }

    public PlayerData GetPlayerData()
    {
        return this.data;
    }
}
