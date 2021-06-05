using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] int score;
    public Text txtscore;

    public void Start()
    {
        score = PlayerPrefs.GetInt("score");
        txtscore.text = "Ваш рекорд : " + score.ToString();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
   
}
