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
        int _lastRunScore = PlayerPrefs.GetInt("lastRunScore");
        int _recordScore = PlayerPrefs.GetInt("recordScore");

        if (_lastRunScore > _recordScore)
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
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

}
