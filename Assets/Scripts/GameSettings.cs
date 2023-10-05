using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSettings : MonoBehaviour
{
    public static GameSettings instance;
    private FileHandler fileHandler;

    public enum Difficulty { Unknow, Easy, Medium, Hard};
    public Difficulty difficulty;
    public int stage;
    public bool isContinue;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            this.fileHandler = GetComponent<FileHandler>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadNewStage()
    {
        this.stage = +1;
    }

    public void SetStage(int stage)
    {
        this.stage = stage;
    }

    public void SetDifficulty(string difficulty)
    {
        if (difficulty == "Easy")
        {
            this.difficulty = Difficulty.Easy;
            return;
        }
        if(difficulty == "Medium")
        {
            this.difficulty = Difficulty.Medium;
            return;
        }
        if(difficulty == "Hard")
        {
            this.difficulty = Difficulty.Hard;
            return;
        }
        this.difficulty = Difficulty.Unknow;
    }

    public FileHandler GetFileHandler()
    {
        return this.fileHandler;
    }
}
