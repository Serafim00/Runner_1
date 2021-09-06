using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGenerator : MonoBehaviour
{
    public GameObject[] _groundPrefabs;
    private List<GameObject> _activeGroundPanel = new List<GameObject>();
    private float _spawnPosition = 0;
    private float _groundLength = 100;//длина платформы

    [SerializeField] private Transform _player;
    private int _startGround = 3; //при загрузке игр отображается 3 платформы

    void Start()
    {
        ViewStartPanel();//отображеие стартовых платформ
    }
        
    void Update()
    {
        SpawnNextPanel();//спавн последующих платформ
    }

    private void SpawnNextPanel()//спавн последующих платформ , если первая пройдена на 55%
    {
        if (_player.position.z - 55 > _spawnPosition - (_startGround * _groundLength))
        {
            SpawnGroundPanel(Random.Range(1, _groundPrefabs.Length));//спавн рандомной платформы
            DeleteGroundPanel();//удаление пройденой платформы
        }
    }

    private void SpawnGroundPanel(int _groungIndex)//спавн платформы 
    {
        GameObject _nextGround = Instantiate(_groundPrefabs[_groungIndex], transform.forward * _spawnPosition, transform.rotation);
        _activeGroundPanel.Add(_nextGround);//создаем панель из списка 
        _spawnPosition += _groundLength;//позиция панели = спавн позиция +длина платформы
    }

    private void DeleteGroundPanel()//удаление платформы
    { 
        Destroy(_activeGroundPanel[0]);
        _activeGroundPanel.RemoveAt(0);
    }

    void ViewStartPanel()//отображение стартовых платформ
    {
        for (int i = 0; i < _startGround; i++)
        {
            SpawnGroundPanel(0);
        }
    }
}
