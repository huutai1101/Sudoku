using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEditor;

public class TitleController : MonoBehaviour
{
    [Header("Continue")]
    [SerializeField] GameObject continueObj;
    [SerializeField] Text detailContinueText;

    [Header("Others")]
    [SerializeField] GameObject titleObj;
    [SerializeField] GameObject difficultyObj;

    private bool isContinue;

    private void Start()
    {
        GameSettings.instance.GetFileHandler().LoadFromJson();
        GameSettings.instance.difficulty = GameSettings.Difficulty.Unknow;
        titleObj.SetActive(true);
        difficultyObj.SetActive(false);
        GameSettings.instance.isContinue = GameSettings.instance.GetFileHandler().GetPlayerData().isContinue;
        if(GameSettings.instance.isContinue)
        {
            float playTime = GameSettings.instance.GetFileHandler().GetPlayerData().playTime;
            string formattedTime = GetFormattedTime(playTime);
            string difficulty = GameSettings.instance.GetFileHandler().GetPlayerData().difficulty;
            detailContinueText.text = formattedTime + " - " + difficulty;
            continueObj.SetActive(true);
        }    
        else
        {
            continueObj.SetActive(false);
        }
    }

    private string GetFormattedTime(float playTime)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(playTime);
        if(timeSpan.Hours > 0)
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }
        return string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }

    public void NewGameButton()
    {
        titleObj.SetActive(false);
        difficultyObj.SetActive(true);
        //SceneManager.LoadScene("Gameplay");
    }

    public void ContinueButton()
    {
        GameSettings.instance.SetDifficulty(GameSettings.instance.GetFileHandler().GetPlayerData().difficulty);
        GameSettings.instance.stage = GameSettings.instance.GetFileHandler().GetPlayerData().stage;
        SceneManager.LoadScene("Gameplay");
    }

    public void DifficultyButton(string Difficulty)
    {
        switch(Difficulty)
        {
            case "Easy":
                GameSettings.instance.difficulty = GameSettings.Difficulty.Easy; break;
            case "Medium":
                GameSettings.instance.difficulty = GameSettings.Difficulty.Medium; break;
            case "Hard":
                GameSettings.instance.difficulty = GameSettings.Difficulty.Hard; break;
            default:
                GameSettings.instance.difficulty = GameSettings.Difficulty.Unknow;break;
        }
        GameSettings.instance.isContinue = false;
        GameSettings.instance.stage = 1;
        SceneManager.LoadScene("Gameplay");
    }

    public void BackButton()
    {
        difficultyObj.SetActive(false);
        titleObj.SetActive(true);
    }
}
