using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Text _coinsText;

    private void Start()
    {
        MAinMenuPanel();//ќтображение главного меню
    }

    private void MAinMenuPanel()//ќтображение главного меню
    {
        int _coins = PlayerPrefs.GetInt("coins");//отображение собраных Coins
        _coinsText.text = "$" + _coins.ToString();
    }

    public void PlayGame()//запуск игры
    {
        SceneManager.LoadScene(1);
    }
}

