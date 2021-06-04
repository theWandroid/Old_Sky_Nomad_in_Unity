using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionPlayerMovement : MonoBehaviour
{

    public float speed = 1.0f;
    private Rigidbody2D _rigidBody;
    private Animator _animator;

    private bool isWalking = false;

    public float walkTime = 1.5f;
    private float walkCounter;


    public float waitTime = 4.0f;
    private float waitCounter;

    private Vector2[] walkingDirections = { Vector2.up, Vector2.down, Vector2.right, Vector2.left };

    private const string LAST_H = "Last_H";
    private const string LAST_V = "Last_V";

    public Vector2 facingDirection = Vector2.zero;
    private int currentDirection;


    private Vector3 lastPositon;
    private Vector3 currentPosition;

    public BoxCollider2D villagerZone;

    // Start is called before the first frame update
    void Start()
    {
        //_spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        waitCounter = waitTime;
        walkCounter = walkTime;
        //_collider = this.GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking)
        {
            if (this.transform.position.x < villagerZone.bounds.min.x + 0.3 ||
                this.transform.position.x > villagerZone.bounds.max.x - 0.3 ||
                this.transform.position.y < villagerZone.bounds.min.y + 0.3 ||
                this.transform.position.y > villagerZone.bounds.max.y - 0.3)
            {
                
                StopWalking();
            }

            //con el método Draw se dibuja una línea infinita

           

            walkCounter -= Time.fixedDeltaTime;
            //Debug.Log("Estoy andando");
            if (walkCounter < 0)
            {
                //Debug.Log("Me voy a parar");
                StopWalking();
            }
            _rigidBody.velocity = walkingDirections[currentDirection] * speed;

        }
        else
        {
            //Debug.Log("Estoy parada");
            _rigidBody.velocity = Vector2.zero;
            waitCounter -= Time.fixedDeltaTime;
            if (waitCounter < 0)
            {
                //Debug.Log("Voy a empezar a andar");
                StartWalking();
            }
        }
    }


    private void LateUpdate()
    {
        _animator.SetBool("Walking", isWalking);
        _animator.SetFloat("Horizontal", walkingDirections[currentDirection].x);
        _animator.SetFloat("Vertical", walkingDirections[currentDirection].y);

        _animator.SetFloat(LAST_H, facingDirection.x);
        _animator.SetFloat(LAST_V, facingDirection.y);


    }



    public void StartWalking()
    {
        currentDirection = Random.Range(0, walkingDirections.Length);

        isWalking = true;
        walkCounter = walkTime;
        currentPosition = transform.position;
        //Debug.Log("Empiezo a andar");
    }

    public void StopWalking()
    {
        //Debug.Log("ME paro");

        isWalking = false;
        waitCounter = waitTime;
        _rigidBody.velocity = Vector2.zero;
        lastPositon = transform.position;
        _animator.SetFloat(LAST_H, facingDirection.x);
        _animator.SetFloat(LAST_V, facingDirection.y);
    }

}
