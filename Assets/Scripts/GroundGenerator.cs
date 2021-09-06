using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGenerator : MonoBehaviour
{
    public GameObject[] _groundPrefabs;
    private List<GameObject> _activeGroundPanel = new List<GameObject>();
    private float _spawnPosition = 0;
    private float _groundLength = 100;//����� ���������

    [SerializeField] private Transform _player;
    private int _startGround = 3; //��� �������� ��� ������������ 3 ���������

    void Start()
    {
        ViewStartPanel();//���������� ��������� ��������
    }
        
    void Update()
    {
        SpawnNextPanel();//����� ����������� ��������
    }

    private void SpawnNextPanel()//����� ����������� �������� , ���� ������ �������� �� 55%
    {
        if (_player.position.z - 55 > _spawnPosition - (_startGround * _groundLength))
        {
            SpawnGroundPanel(Random.Range(1, _groundPrefabs.Length));//����� ��������� ���������
            DeleteGroundPanel();//�������� ��������� ���������
        }
    }

    private void SpawnGroundPanel(int _groungIndex)//����� ��������� 
    {
        GameObject _nextGround = Instantiate(_groundPrefabs[_groungIndex], transform.forward * _spawnPosition, transform.rotation);
        _activeGroundPanel.Add(_nextGround);//������� ������ �� ������ 
        _spawnPosition += _groundLength;//������� ������ = ����� ������� +����� ���������
    }

    private void DeleteGroundPanel()//�������� ���������
    { 
        Destroy(_activeGroundPanel[0]);
        _activeGroundPanel.RemoveAt(0);
    }

    void ViewStartPanel()//����������� ��������� ��������
    {
        for (int i = 0; i < _startGround; i++)
        {
            SpawnGroundPanel(0);
        }
    }
}
