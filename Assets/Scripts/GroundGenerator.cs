using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGenerator : MonoBehaviour
{
    public GameObject[] _groundPrefabs;
    private List<GameObject> _activeGround = new List<GameObject>();
    private float _spawnPosition = 0;
    private float _groundLength = 100;

    [SerializeField] private Transform _player;
    private int _startGround = 3;

    void Start()
    {
        for (int i = 0; i < _startGround; i++)
        {
            SpawnGround(0);
        }
    }

    
    void Update()
    {
        if (_player.position.z -55 > _spawnPosition - (_startGround * _groundLength))
        {
            SpawnGround(Random.Range(1, _groundPrefabs.Length));
            DeleteGround();
        }
    }

    private void SpawnGround(int _groungIndex)
    {
        GameObject _nextGround = Instantiate(_groundPrefabs[_groungIndex], transform.forward * _spawnPosition, transform.rotation);
        _activeGround.Add(_nextGround);
        _spawnPosition += _groundLength;
    }

    private void DeleteGround()
    {
        Destroy(_activeGround[0]);
        _activeGround.RemoveAt(0);
    }
}
