using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public QuestionClass[] questions;
    public CiscoClass[] ciscoquestions;
    public Text txtquestion, wintxt;
    public List<object> qList;
    public List<object> qCisco;
    public QuestionClass currentQ;
    public CiscoClass currentCiscoQ;
    public Text[] answerstxt;
    private int rand;
    public Button[] buttons = new Button[3];
    public GameObject toMenu;
    public GameObject winText;
    private int count;
    private int score, globalscore;

    public void Start()
    {
        qList = new List<object>(questions);
        qCisco = new List<object>(ciscoquestions);
        GenerateQuestion();
        wintxt = winText.GetComponent<Text>();
        globalscore = PlayerPrefs.GetInt("score");
        score = 0;
    }

    public void GenerateQuestion()
    {
        count += 1;
        if (count % 4 == 0 && qCisco.Count > 0)
        {
            rand = Random.Range(0, qCisco.Count);
            currentCiscoQ = qCisco[rand] as CiscoClass;
            OpenCisco(currentCiscoQ.path);
            txtquestion.text = currentCiscoQ.question;
            List<string> answersQ = new List<string>(currentCiscoQ.answers);
            for (int i = 0; i < currentCiscoQ.answers.Length; i++)
            {
                int randomindex = Random.Range(0, answersQ.Count);
                answerstxt[i].text = answersQ[randomindex];
                answersQ.RemoveAt(randomindex);
            }
        }
        else if (qList.Count > 0)
        {
            rand = Random.Range(0, qList.Count);
            currentQ = qList[rand] as QuestionClass;
            txtquestion.text = currentQ.question;
            List<string> answers = new List<string>(currentQ.answers);
            for (int i = 0; i < currentQ.answers.Length; i++)
            {
                int randomindex = Random.Range(0, answers.Count);
                answerstxt[i].text = answers[randomindex];
                answers.RemoveAt(randomindex);
            }
        }
    }

    IEnumerator CheckAnswer(bool check)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        yield return new WaitForSeconds(1f);
        if (!check)
        {
            txtquestion.gameObject.SetActive(false);
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].gameObject.SetActive(false);
            }
            if (score > globalscore)
            {
                PlayerPrefs.SetInt("score", score);
            }
            winText.SetActive(true);
            wintxt.text = "Вы проиграли :(\nНе отчаивайтесь!\nПопробуйте\nеще раз!!!";
            toMenu.SetActive(true);
        }
        if (count % 4 == 0 && qCisco.Count > 0)
        {
            qCisco.RemoveAt(rand);
        }
        else
        {
            qList.RemoveAt(rand);
        }
        if (qList.Count > 0 || qCisco.Count > 0)
        {
            GenerateQuestion();
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].GetComponent<Graphic>().color = Color.white;
                buttons[i].interactable = true;
            }
        }
        else
        {
            txtquestion.gameObject.SetActive(false);
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].gameObject.SetActive(false);
            }
            if (score > globalscore)
            {
                PlayerPrefs.SetInt("score", score);
            }
            wintxt.text = "Поздравляем!!!\nВы прошли игру и знаете протокол SIP на 100% ";
            winText.SetActive(true);
            toMenu.SetActive(true);
        }
    }

    public void ClickAnswer(int index)
    {
        if (answerstxt[index].text.ToString() == currentQ.answers[0] || 
            answerstxt[index].text.ToString() == currentCiscoQ.answers[0])
        {
            StartCoroutine(CheckAnswer(true));
            buttons[index].GetComponent<Graphic>().color = Color.green;
            score += 1;
        }
        else
        {
            StartCoroutine(CheckAnswer(false));
            buttons[index].GetComponent<Graphic>().color = Color.red;
        }
    }

    public void OpenCisco(string path)
    {
        Application.OpenURL(path);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene(0);
    }
}

[System.Serializable]
public class QuestionClass
{
    public string question;
    public string[] answers = new string[3];
}

[System.Serializable]
public class CiscoClass
{
    public string question;
    public string[] answers = new string[3];
    public string path;
}
