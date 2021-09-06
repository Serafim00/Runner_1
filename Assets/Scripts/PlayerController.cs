using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController _controller;
    private CapsuleCollider _col;
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

    private int _lineToMove = 1;
    public float _lineDistance = 4;
    private float _maxSpeed = 110;
    
    
    void Start()
    {
        _anim = GetComponentInChildren<Animator>();
        _controller = GetComponent<CharacterController>();
        _col = GetComponent<CapsuleCollider>();
        Time.timeScale = 1;
        _coins = PlayerPrefs.GetInt("coins");
        _coinsText.text = "$" + _coins.ToString();
        StartCoroutine(SpeedIncrease());
      
    }

    private void Update()
    {
        if (SwipeController.swipeRight)
        {
            if (_lineToMove <2)
                _lineToMove++;
        }

        if (SwipeController.swipeLeft)
        {
            if (_lineToMove > 0)
                _lineToMove--;
        }

        if (SwipeController.swipeUp)
        {
            if (_controller.isGrounded)
                Jump();
        }

        if (SwipeController.swipeDown)
        {
            StartCoroutine(Slide());
        }

        if (_controller.isGrounded && !_isRolling)
            _anim.SetBool("isRunning", true);
        else
            _anim.SetBool("isRunning", false);

        Vector3 _targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (_lineToMove == 0)
            _targetPosition += Vector3.left * _lineDistance;
        else if (_lineToMove == 2)
            _targetPosition += Vector3.right * _lineDistance;

        if (transform.position == _targetPosition)
            return;
        Vector3 _diff = _targetPosition - transform.position;
        Vector3 _moveDir = _diff.normalized * 25 * Time.deltaTime;
        if (_moveDir.sqrMagnitude < _diff.sqrMagnitude)
            _controller.Move(_moveDir);
        else
            _controller.Move(_diff);

        //_speed += 0.1f * Time.deltaTime;
    }


    private void Jump()
    {
        _distance.y = _jumpForse;
        _anim.SetTrigger("isJumping");
        _anim.SetBool("isRunning", true);
    }

    void FixedUpdate()
    {
        _distance.z = _speed;
        _distance.y += _gravity * Time.fixedDeltaTime;
        _controller.Move(_distance * Time.fixedDeltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
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
        if(other.gameObject.tag == "coin")
        {
            _coins++;
            PlayerPrefs.SetInt("coins", _coins);
            _audioCoin.Play();
            _coinsText.text = '$'+_coins.ToString();
            Destroy(other.gameObject);
        }
    }

    private IEnumerator SpeedIncrease()
    {
        yield return new WaitForSeconds(10);
        if (_speed < _maxSpeed)
            {
                _speed += 1;
                StartCoroutine(SpeedIncrease());
            }
    }

    private IEnumerator Slide()
    {
        
        _anim.SetTrigger("isRolling");
        _anim.SetBool("isRunning", true);
        _col.center = new Vector3(0, 0.2f, 0);
        _col.height = 0.8f;
        _controller.center = new Vector3(0, 0.2f, 0);
        _controller.height = 0.8f;
        _isRolling = true;
        _anim.SetBool("isRunning", false);
        _anim.SetTrigger("isRolling");
        

        yield return new WaitForSeconds(1);
        _anim.SetBool("isRunning", true);
        _col.center = new Vector3(0, 0.84f, 0);
        _col.height = 1.92f;
        _controller.center = new Vector3(0, 0.84f, 0);
        _controller.height = 1.92f;
        _isRolling = false;
        
    }
}
