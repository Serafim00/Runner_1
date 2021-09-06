using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] public Text _scoreText;

    void Update()
    {
        ViewScoreTet();//отображение счета
    }

    private void ViewScoreTet()//отображение счета
        {
        _scoreText.text = ((int)(_player.position.z / 2)).ToString();
    }
}
