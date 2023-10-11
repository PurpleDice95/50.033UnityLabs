using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject highScoreText;
    public IntVariable gameScore;

    void Start()
    {
        SetHighScore();
    }

    void SetHighScore()
    {
        highScoreText.GetComponent<TextMeshProUGUI>().text = "TOP- " + gameScore.previousHighestValue.ToString("D6");
    }

    public void ResetHighScore()
    {
        GameObject eventSystem = GameObject.Find("EventSystem");
        eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

        gameScore.ResetHighestValue();

        SetHighScore();
    }

    public void GoToLoadScene()
    {
        SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Single);
    }
}
