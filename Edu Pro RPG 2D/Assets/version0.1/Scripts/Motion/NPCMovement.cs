using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float speed = 1.0f;
    private Rigidbody2D _rigidBody;
    private Animator _animator;

    private bool isWalking = false;
    public bool isTalking = false;

    public float walkTime = 1.5f;
    private float walkCounter;


    public float waitTime = 4.0f;
    private float waitCounter;

    private Vector2[] walkingDirections = { Vector2.up, Vector2.down, Vector2.right, Vector2.left };
    private Vector2 forbiddenDirection;
    private int currentDirection;

    public BoxCollider2D villagerZone;

    private DialogueManager dialogueManager;

    public GameObject player;
    private SpriteRenderer _spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        waitCounter = waitTime;
        walkCounter = walkTime;
        isTalking = false;
        //como solo hay un objeto que contenga el DIalogueManager entonces usamos este metodo, dunciona porque solo hay uno
       dialogueManager = FindObjectOfType<DialogueManager>();
        //Debug.Log("Quiero jugar");
    }

    private void FixedUpdate()
    {

        if (player.transform.position.y < this.transform.position.y)
        {
            _spriteRenderer.sortingOrder = 5;
        }else if (player.transform.position.y > this.transform.position.y)
        {
            _spriteRenderer.sortingOrder = 15;
        }
       
        //si estoy hablando y el manager dice que ya hemos acabado de hablar, así reducimos las veces que tenemos que pasar por aqui
        if (isTalking && !dialogueManager.dialogueActive)
        {
            isTalking = false;
        }
        

        if (isTalking || dialogueManager.dialogueActive)
        {
            // si el DialogueManager idica que el dialogo esta activo isTalking será igual true, sino será false
            isTalking = dialogueManager.dialogueActive;
            StopWalking();
            return;
        }


        if (isWalking)
        {
            if(this.transform.position.x < villagerZone.bounds.min.x ||
                this.transform.position.x > villagerZone.bounds.max.x ||
                this.transform.position.y < villagerZone.bounds.min.y ||
                this.transform.position.y > villagerZone.bounds.max.y)
            {
                StopWalking();

            }
            _rigidBody.velocity = walkingDirections[currentDirection] * speed;
            walkCounter -= Time.fixedDeltaTime;
            //Debug.Log("Estoy andando");
            if (walkCounter < 0)
            {
                //Debug.Log("Me voy a parar");
                StopWalking();
            }

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
    }

   

    public void StartWalking()
    {
        currentDirection = Random.Range(0, walkingDirections.Length);
        
        isWalking = true;
        walkCounter = walkTime;
        //Debug.Log("Empiezo a andar");
    }

    public void StopWalking()
    {
        //Debug.Log("ME paro");
        isWalking = false;
        waitCounter = waitTime;
        _rigidBody.velocity = Vector2.zero;
    }
}
