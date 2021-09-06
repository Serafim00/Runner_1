using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController _controller;
    private Animator _anim;
    private Vector3 _distance;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForse;
    [SerializeField] private float _gravity;
    [SerializeField] private int _coins;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private Text _coinsText;
    [SerializeField] private AudioSource _audioCoin;
    [SerializeField] private Score _scoreScript;

    private bool _isRolling;

    private int _lineToMove = 1; //����� ������ ������ (�����)
    public float _lineDistance = 4; //��������� ����� �������
    private float _maxSpeed = 110; //������������ �������� ������


    void Start()
    {
        SceneSettings();//��������� �����
        StartCoroutine(SpeedIncrease());//���������� ��������
    }

    private void Update()
    {
        SwipeProcessing();//��������� �������
    }

   void SceneSettings()
    {
        _anim = GetComponentInChildren<Animator>();//�������� ���������
        _controller = GetComponent<CharacterController>();//���������� ���������
        Time.timeScale = 1;//�����
        _coins = PlayerPrefs.GetInt("coins");//�������� Coins
        _coinsText.text = "$" + _coins.ToString();//����� Coins
    }

    void SwipeProcessing()//��������� �������
    {
        SwipeRight();
        SwipeLeft();
        SwipeUp();
        SwipeDown();

        PlayerLocation();

        UpdatePlayerPosition();//���������� ������� ���������
    }

    private void PlayerLocation()
    {
        if (_controller.isGrounded && !_isRolling)//����������� ��������� ��������� (�� ����� ��� � ������)
            _anim.SetBool("isRunning", true);
        else
            _anim.SetBool("isRunning", false);
    }

    private void SwipeDown()
    {
        if (SwipeController.swipeDown)//����� ����
        {
            StartCoroutine(Slide());
        }
    }

    private void SwipeUp()
    {
        if (SwipeController.swipeUp)//����� �����
        {
            if (_controller.isGrounded)
                Jump();
        }
    }

    private void SwipeLeft()
    {
        if (SwipeController.swipeLeft)//����� �����
        {
            if (_lineToMove > 0)
                _lineToMove--;
        }
    }

    private void SwipeRight()
    {
        if (SwipeController.swipeRight)//����� � �����
        {
            if (_lineToMove < 2)
                _lineToMove++;
        }
    }

    private void UpdatePlayerPosition()//���������� ������� ���������
    {
        Vector3 _targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (_lineToMove == 0)
            _targetPosition += Vector3.left * _lineDistance;
        else if (_lineToMove == 2)
            _targetPosition += Vector3.right * _lineDistance;

        if (transform.position == _targetPosition)
            return;

       Vector3 _dif = _targetPosition - transform.position;
       Vector3 _moveDir = _dif.normalized * 25 * Time.deltaTime;
       if (_moveDir.sqrMagnitude < _dif.sqrMagnitude)
          _controller.Move(_moveDir);
       else
          _controller.Move(_dif);
    }

    void FixedUpdate()
    {
        MovePleyer();
    }

    private void MovePleyer()//����������� ������
    {
        _distance.z = _speed;
        _distance.y += _gravity * Time.fixedDeltaTime;
        _controller.Move(_distance * Time.fixedDeltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        LosePanelActivate(hit);//�������� ���� ��������
    }

    private void LosePanelActivate(ControllerColliderHit hit)//������������� ���� ���������
    {
        if (hit.gameObject.tag == "obstacle")
        {
            _losePanel.SetActive(true);
            int _lastRunScore = int.Parse(_scoreScript._scoreText.text.ToString());
            PlayerPrefs.SetInt("lastRunScore", _lastRunScore);
            Time.timeScale = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CollectoinCoins(other);//���� �����
    }

    private void CollectoinCoins(Collider other)//���� �����
    {
        if (other.gameObject.tag == "coin")
        {
            _coins++;
            PlayerPrefs.SetInt("coins", _coins);
            _audioCoin.Play();
            _coinsText.text = '$' + _coins.ToString();
            Destroy(other.gameObject);
        }
    }

    private IEnumerator SpeedIncrease()//���������� �������� ��������
    {
        yield return new WaitForSeconds(10);
        if (_speed < _maxSpeed)
            {
                _speed += 1;
                StartCoroutine(SpeedIncrease());
            }
    }

    private void Jump()//������ ���������
    {
        _distance.y = _jumpForse;
        _anim.SetTrigger("isJumping");
        _anim.SetBool("isRunning", true);
    }

    private IEnumerator Slide()//������� ���������
    {
        DecreaseColliderSise();

        yield return new WaitForSeconds(1);//����� ������� ����������� � �������� ����������

        IncreaseColliderSise();

    }

    private void DecreaseColliderSise()
    {
        _anim.SetTrigger("isRolling");
        _anim.SetBool("isRunning", true);
        _controller.center = new Vector3(0, 0.2f, 0);
        _controller.height = 0.8f;
        _isRolling = true;
        _anim.SetBool("isRunning", false);
        _anim.SetTrigger("isRolling");
    }

    private void IncreaseColliderSise()
    {
        _anim.SetBool("isRunning", true);
        _controller.center = new Vector3(0, 0.84f, 0);
        _controller.height = 1.92f;
        _isRolling = false;
    }

   
}
