using System;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    [Header ("Main")]
    [SerializeField] Text difficultyText;
    [SerializeField] Text mistakeText;
    [SerializeField] Text timeText;
    [SerializeField] Text noteModeText;

    [Header ("Pause")]
    [SerializeField] Text timePauseText;
    [SerializeField] Text mistakePauseText;
    [SerializeField] Text difficultPauseText;

    [Header("Win")]
    [SerializeField] Text timeWinText;
    [SerializeField] Text difficultWinText;

    [Header("Game Over")]
    [SerializeField] Text GameoverNoticeText;

/*    public void LoadWinUI(float time, string difficulty)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        timeWinText.text = formattedTime;
        difficultWinText.text = 
    }*/

    public void LoadDifficultyTexts(string difficulty)
    {
        string text = "Difficulty:\n<b>" + difficulty + "</b>";
        difficultyText.text = text;
        difficultPauseText.text = text;
        difficultWinText.text = difficulty;
    }

    public void LoadMistakeTexts(int mistake, int limitMistake)
    {
        string text = "Mistake:\n<b>" + mistake.ToString() + "/" + limitMistake.ToString() + "</b>";
        mistakeText.text = text;
        mistakePauseText.text = text;
    }

    public void LoadTimeText(int time)
    {
        string formattedTime = GetFormattedTime(time);
        string text = "Time:\n<b>" + formattedTime + "</b>";
        timeText.text = text;
        timePauseText.text = text;
        timeWinText.text = formattedTime;
    }

    public void LoadNoteModeText(bool noteMode)
    {
        noteModeText.text = "Note Mode:\n<b>" + IsUseNoteMode(noteMode) + "</b>";
    }

    private string IsUseNoteMode(bool noteMode)
    {
        if (noteMode) 
        {
            return "On";
        }
        return "Off";
    }

    public void SetGameoverNotice(int mistake)
    {
        GameoverNoticeText.text = "You lost the game beause you made " + mistake + " mistakes";
    }

    private string GetFormattedTime(float playTime)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(playTime);
        if (timeSpan.Hours > 0)
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }
        return string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }
}
