using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    public bool canMove = true;

    public static bool playerCreated;

    public bool isTalking;

    [Tooltip("This is the speed of the main character")]
    public float speed = 5.0f;

    private bool walking = false;
    public Vector2 lastMovement = Vector2.zero;

    //public float currentSpeed;
    private const string AXIS_H = "Horizontal";
    private const string AXIS_V = "Vertical";
    private const string WALK = "Walking";
    private const string LAST_H = "Last_H";
    private const string LAST_V = "Last_V";
    private SpriteRenderer m_SpriteRenderer;

    //public Vector2 facingDirection = Vector2.zero;

    private Animator _animator;
    // Start is called before the first frame update

    private Rigidbody2D _rigidBody;

    private GameObject joystick;

    public string nextUuid;

    public CapsuleCollider2D playerCapCol;

    void Start()
    {
        this.transform.position = LoadPlayerPosition();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
        playerCapCol = GetComponent<CapsuleCollider2D>();
        playerCreated = true;
        nextUuid = "origin";
        isTalking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTalking)
        {
            _rigidBody.velocity = Vector2.zero;
            _animator.enabled = false;
            return;
        }
        this.walking = false;
        if (!isTalking)
        {
            _animator.enabled = true;
        }
        //S = V*T
#if UNITY_STANDALONE_WIN
        if (Mathf.Abs(Input.GetAxisRaw(AXIS_H)) > 0.2f )
        {
            _rigidBody.velocity = new Vector2(Input.GetAxisRaw(AXIS_H) /** currentSpeed*/, _rigidBody.velocity.y).normalized * speed;
            walking = true;
            lastMovement = new Vector2(Input.GetAxisRaw(AXIS_H), 0);

        }
         if (Mathf.Abs(Input.GetAxisRaw(AXIS_V)) > 0.2f )
        {
            /*
            Vector3 translation = new Vector3(0, Input.GetAxis(AXIS_V) * speed * Time.deltaTime, 0);
            this.transform.Translate(translation);
            */
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, Input.GetAxisRaw(AXIS_V)).normalized * speed /**
                currentSpeed*/ ;
            walking = true;
            lastMovement = new Vector2(0, Input.GetAxisRaw(AXIS_V));

        }
        if (Mathf.Abs(Input.GetAxisRaw(AXIS_V)) < 0.2f)
        {
            /*
            Vector3 translation = new Vector3(0, Input.GetAxis(AXIS_V) * speed * Time.deltaTime, 0);
            this.transform.Translate(translation);
            */
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 0).normalized * speed /**
                currentSpeed*/ ;
        }
        if (Mathf.Abs(Input.GetAxisRaw(AXIS_H)) < 0.2f)
        {
            /*
            Vector3 translation = new Vector3(0, Input.GetAxis(AXIS_V) * speed * Time.deltaTime, 0);
            this.transform.Translate(translation);
            */
            _rigidBody.velocity = new Vector2(0, _rigidBody.velocity.y ).normalized * speed /**
                currentSpeed*/ ;
        }

#endif
        if ( CrossPlatformInputManager.GetAxis(AXIS_H) != 0 )
        {

            _rigidBody.velocity = new Vector2(CrossPlatformInputManager.GetAxis(AXIS_H), _rigidBody.velocity.y).normalized * speed;
            walking = true;
        }

        if ( CrossPlatformInputManager.GetAxis(AXIS_V) != 0)
        {
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, CrossPlatformInputManager.GetAxis(AXIS_V)).normalized * speed;
            walking = true;
        }

        if (Mathf.Abs(CrossPlatformInputManager.GetAxis(AXIS_H)) > Mathf.Abs(CrossPlatformInputManager.GetAxis(AXIS_V)))
        {

            lastMovement = new Vector2(CrossPlatformInputManager.GetAxis(AXIS_H), 0);
        }

        if (Mathf.Abs(CrossPlatformInputManager.GetAxis(AXIS_V)) > Mathf.Abs(CrossPlatformInputManager.GetAxis(AXIS_H)))
        {
            lastMovement = new Vector2(0, CrossPlatformInputManager.GetAxis(AXIS_V));
        }

        if (CrossPlatformInputManager.GetButtonDown("Action"))
        {
            Debug.Log("El jugador ha pulado el boton");
        }

    }

    private void LateUpdate()
    {
        if (!walking)
        {
            _rigidBody.velocity = Vector2.zero;
        }

#if UNITY_STANDALONE_WIN




        _animator.SetFloat(AXIS_H, Input.GetAxisRaw(AXIS_H));
        _animator.SetFloat(AXIS_V, Input.GetAxisRaw(AXIS_V));

        _animator.SetBool(WALK, walking);
        _animator.SetFloat(LAST_H, lastMovement.x);
        _animator.SetFloat(LAST_V, lastMovement.y);
#endif

        _animator.SetFloat(AXIS_H, CrossPlatformInputManager.GetAxis(AXIS_H));
        _animator.SetFloat(AXIS_V, CrossPlatformInputManager.GetAxis(AXIS_V));

        _animator.SetBool(WALK, walking);
        _animator.SetFloat(LAST_H, lastMovement.x);
        _animator.SetFloat(LAST_V, lastMovement.y);

    }

    public void SavePlayerPosition()
    {
        PlayerPrefs.SetFloat("playerPositionX", this.transform.position.x);
        PlayerPrefs.SetFloat("playerPositionY", this.transform.position.y);
        PlayerPrefs.SetFloat("playerPositionZ", this.transform.position.z);
    }

    public Vector3 LoadPlayerPosition()
    {
        return new Vector3(PlayerPrefs.GetFloat("playerPositionX", this.transform.position.x), PlayerPrefs.GetFloat("playerPositionY", this.transform.position.y), PlayerPrefs.GetFloat("playerPositionZ", this.transform.position.z));
    }
}
