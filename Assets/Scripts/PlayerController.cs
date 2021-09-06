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

    private int _lineToMove = 1; //линия старта игрока (центр)
    public float _lineDistance = 4; //дистанция между линиями
    private float _maxSpeed = 110; //максимальная скорость игрока


    void Start()
    {
        SceneSettings();//настройка сцены
        StartCoroutine(SpeedIncrease());//увеличение скорости
    }

    private void Update()
    {
        SwipeProcessing();//обработка свайпов
    }

   void SceneSettings()
    {
        _anim = GetComponentInChildren<Animator>();//аниматор персонажа
        _controller = GetComponent<CharacterController>();//контроллер персонажа
        Time.timeScale = 1;//время
        _coins = PlayerPrefs.GetInt("coins");//собраные Coins
        _coinsText.text = "$" + _coins.ToString();//вывод Coins
    }

    void SwipeProcessing()//обработка свайпов
    {
        SwipeRight();
        SwipeLeft();
        SwipeUp();
        SwipeDown();

        PlayerLocation();

        UpdatePlayerPosition();//обновление позиции персонажа
    }

    private void PlayerLocation()
    {
        if (_controller.isGrounded && !_isRolling)//определение положение персонажа (на земле или в полете)
            _anim.SetBool("isRunning", true);
        else
            _anim.SetBool("isRunning", false);
    }

    private void SwipeDown()
    {
        if (SwipeController.swipeDown)//свайп вниз
        {
            StartCoroutine(Slide());
        }
    }

    private void SwipeUp()
    {
        if (SwipeController.swipeUp)//свайп вверх
        {
            if (_controller.isGrounded)
                Jump();
        }
    }

    private void SwipeLeft()
    {
        if (SwipeController.swipeLeft)//свайп влево
        {
            if (_lineToMove > 0)
                _lineToMove--;
        }
    }

    private void SwipeRight()
    {
        if (SwipeController.swipeRight)//свайп в право
        {
            if (_lineToMove < 2)
                _lineToMove++;
        }
    }

    private void UpdatePlayerPosition()//обновление позиции персонажа
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

    private void MovePleyer()//передвижеие игрока
    {
        _distance.z = _speed;
        _distance.y += _gravity * Time.fixedDeltaTime;
        _controller.Move(_distance * Time.fixedDeltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        LosePanelActivate(hit);//открытие окна проигрша
    }

    private void LosePanelActivate(ControllerColliderHit hit)//активирование окна проигрыша
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
        CollectoinCoins(other);//сбор монет
    }

    private void CollectoinCoins(Collider other)//сбор монет
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

    private IEnumerator SpeedIncrease()//увеличение скорости персонжа
    {
        yield return new WaitForSeconds(10);
        if (_speed < _maxSpeed)
            {
                _speed += 1;
                StartCoroutine(SpeedIncrease());
            }
    }

    private void Jump()//прыжок персонажа
    {
        _distance.y = _jumpForse;
        _anim.SetTrigger("isJumping");
        _anim.SetBool("isRunning", true);
    }

    private IEnumerator Slide()//кувырок персонажа
    {
        DecreaseColliderSise();

        yield return new WaitForSeconds(1);//через секунду возвращение к исходным настройкам

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
