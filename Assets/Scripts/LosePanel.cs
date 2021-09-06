using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LosePanel : MonoBehaviour
{
    [SerializeField] Text _recordText;

    private void Update()
    {
        ViewLosePanel();
    }

    private void ViewLosePanel()
    {
        int _lastRunScore = PlayerPrefs.GetInt("lastRunScore");//очки за текущую игру
        int _recordScore = PlayerPrefs.GetInt("recordScore");//рекорд очков

        if (_lastRunScore > _recordScore)//установка нового рекорда
        {
            _recordScore = _lastRunScore;
            PlayerPrefs.SetInt("recordScore", _recordScore);
            _recordText.text = _recordScore.ToString();
        }
        else
        {
            _recordText.text = _recordScore.ToString();
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(1);//перезапуск игры
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);//выход в главное меню
        Time.timeScale = 1;
    }

}
