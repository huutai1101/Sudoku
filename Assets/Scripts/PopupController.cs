using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupController : MonoBehaviour
{
    [SerializeField] GameObject pauseObj;
    [SerializeField] GameObject gameoverObj;
    [SerializeField] GameObject winObj;

    private void Start()
    {
        pauseObj.SetActive(false);
        gameoverObj.SetActive(false);
        winObj.SetActive(false);
    }

    public void NewGameButton()
    {
        if(GameSettings.instance != null)
        {
            GameSettings.instance.stage += 1;
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
