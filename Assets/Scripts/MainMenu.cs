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
        MAinMenuPanel();//����������� �������� ����
    }

    private void MAinMenuPanel()//����������� �������� ����
    {
        int _coins = PlayerPrefs.GetInt("coins");//����������� �������� Coins
        _coinsText.text = "$" + _coins.ToString();
    }

    public void PlayGame()//������ ����
    {
        SceneManager.LoadScene(1);
    }
}

