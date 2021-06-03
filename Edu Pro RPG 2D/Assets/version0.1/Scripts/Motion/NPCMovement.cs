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

    private const string LAST_H = "Last_H";
    private const string LAST_V = "Last_V";

    public Vector2 facingDirection = Vector2.zero;
    private int currentDirection;

    public BoxCollider2D villagerZone;

    private DialogueManager dialogueManager;

    private PlayerController playerController;
    private SpriteRenderer _spriteRenderer;

    Vector2 destination = Vector2.zero;
    private CapsuleCollider2D _collider;
    private List<Vector2> permittedDirections = new List<Vector2>{ Vector2.up, Vector2.down, Vector2.right, Vector2.left };

    private Vector3 lastPositon;
    private Vector3 currentPosition;

    private enum directions
    {
        up,
        down,
        left,
        right
    }


    private Vector2 directionChoice;

    float distanciaRayo;
    int direccion;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        waitCounter = waitTime;
        walkCounter = walkTime;
        isTalking = false;
        _collider = this.GetComponent<CapsuleCollider2D>();

        //como solo hay un objeto que contenga el DIalogueManager entonces usamos este metodo, dunciona porque solo hay uno
       dialogueManager = FindObjectOfType<DialogueManager>();
        //Debug.Log("Quiero jugar");
        playerController = FindObjectOfType<PlayerController>();

        destination = this.transform.position;

        float margerRayo = 0.2f;
        distanciaRayo = _collider.size.x / 2 + margerRayo;
    }

    private void FixedUpdate()
    {
        permittedDirections = new List<Vector2> { Vector2.up, Vector2.down, Vector2.right, Vector2.left };
    
        if (playerController.gameObject.transform.position.y < this.transform.position.y)
        {
            _spriteRenderer.sortingOrder = 5;
        }else if (playerController.gameObject.transform.position.y > this.transform.position.y)
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
            if (playerController.lastMovement == new Vector2(1, 0))
            {
                Debug.Log("El jugador esta mirando hacia la derecha");
               facingDirection = new Vector2(-1, 0);
            }
            else if (playerController.lastMovement == new Vector2(-1, 0))
            {
                Debug.Log("El jugador esta mirando hacia la izquierda");

                facingDirection = new Vector2(1, 0);
            }
            else if (playerController.lastMovement == new Vector2(0, -1))
            {
                Debug.Log("El jugador esta mirando hacia arriba");

                facingDirection = new Vector2(0, 1);
            }
            else if (playerController.lastMovement == new Vector2(0, 1))
            {
                Debug.Log("El jugador esta mirando hacia abajo");

                facingDirection = new Vector2(0, -1);
            }
            return;
        }


        if (isWalking)
        {
            if(this.transform.position.x < villagerZone.bounds.min.x +0.3||
                this.transform.position.x > villagerZone.bounds.max.x - 0.3 ||
                this.transform.position.y < villagerZone.bounds.min.y + 0.3||
                this.transform.position.y > villagerZone.bounds.max.y -0.3 )
            {
                permittedDirections.Remove(permittedDirections[currentDirection]);
                currentDirection = Random.Range(0, permittedDirections.Count);
                StopWalking();
            } 
            
           
            
            Vector2 puntoOrigen= new Vector2(this.transform.position.x, this.transform.position.y);
            //creamos un Raycast
            RaycastHit2D hitAbajo = Physics2D.Raycast(transform.position, permittedDirections[currentDirection], 0.2f);
            //con el método Draw se dibuja una línea infinita

            Debug.Log(hitAbajo);


            if (hitAbajo.collider != null)
            {
                _rigidBody.velocity = permittedDirections[currentDirection] * speed;
                facingDirection = permittedDirections[currentDirection];
                Debug.DrawRay(puntoOrigen, permittedDirections[currentDirection], Color.green);

                Debug.Log("Voy a andar");
            } else if (hitAbajo.collider == null)
            {
                StopWalking();
                Debug.Log("Me paro");
                Debug.DrawRay(puntoOrigen, permittedDirections[currentDirection], Color.red);

            }

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
        _animator.SetFloat("Horizontal", permittedDirections[currentDirection].x);
        _animator.SetFloat("Vertical", permittedDirections[currentDirection].y);

        _animator.SetFloat(LAST_H, facingDirection.x);
        _animator.SetFloat(LAST_V, facingDirection.y);
        

    }



    public void StartWalking()
    {

        Debug.Log(permittedDirections.Count);
        currentDirection = Random.Range(0, permittedDirections.Count);
        
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
