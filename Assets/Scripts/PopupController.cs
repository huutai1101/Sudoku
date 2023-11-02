using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupController : MonoBehaviour
{
    [SerializeField] GameObject pauseObj;
    [SerializeField] GameObject gameoverObj;
    [SerializeField] GameObject winObj;
    [SerializeField] GameObject mainObj;

    private void Start()
    {
        pauseObj.SetActive(false);
        gameoverObj.SetActive(false);
        winObj.SetActive(false);
        mainObj.SetActive(false);
    }

    public void NewGameButton()
    {
        if(GameSettings.instance != null)
        {
            GameSettings.instance.stage += 1;
            GameSettings.instance.isContinue = false;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void HomeButton()
    {
        if (GameSettings.instance != null)
        {
            PlayerData data = new PlayerData();
            data.isContinue = false;
            data.difficulty = GameSettings.Difficulty.Unknow.ToString();
            data.stage = GameSettings.instance.stage;
            GameSettings.instance.GetFileHandler().SaveToJson(data);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void RestartButton()
    {
        GameSettings.instance.isContinue = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResumeButton()
    {
        pauseObj.SetActive(false);
        GameManager.instance.ChangePauseStatus();
    }

    public void ShowMainPopup()
    {
        StartCoroutine(DisplayMainPopup());
    }

    private IEnumerator DisplayMainPopup()
    {
        mainObj.SetActive(true);
        yield return new WaitForSeconds(2f);
        mainObj.SetActive(false);
    }

    public GameObject GetPauseObj()
    {
        return pauseObj;
    }

    public GameObject GetGameoverbj() 
    {
        return gameoverObj;
    }

    public GameObject GetWinObj()
    {
        return winObj;
    }
}
