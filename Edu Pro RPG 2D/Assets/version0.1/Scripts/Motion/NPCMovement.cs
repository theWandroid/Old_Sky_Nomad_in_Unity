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

    private GameObject player;
    private SpriteRenderer _spriteRenderer;

    Vector2 destination = Vector2.zero;


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
        player = FindObjectOfType<PlayerController>().gameObject;

        destination = this.transform.position;
    }

    private void FixedUpdate()
    {

        /*

        //Calculamos el nuevo punto  donde ahy que ir en base a la variable destino
        Vector2 newPos = Vector2.MoveTowards(this.transform.position, destination, speed);
        //USamos el rigidbody para transportar a Pacman hasta dicha posicion
        GetComponent<Rigidbody2D>().MovePosition(newPos);

        float distanceToDestination = Vector2.Distance((Vector2)this.transform.position, destination);

        //tenemos hacer casting a Vector2 ya que los Vectores de posición por defecto son Vector3
        if (distanceToDestination < 1)
        {
            if (Input.GetKey(KeyCode.UpArrow) && CanMoveTo(Vector2.up))
            {
                destination = (Vector2)this.transform.position + Vector2.up;
            }

            if (Input.GetKey(KeyCode.RightArrow) && CanMoveTo(Vector2.right))
            {
                destination = (Vector2)this.transform.position + Vector2.right;
            }

            if (Input.GetKey(KeyCode.DownArrow) && CanMoveTo(Vector2.down))
            {
                destination = (Vector2)this.transform.position + Vector2.down;
            }

            if (Input.GetKey(KeyCode.LeftArrow) && CanMoveTo(Vector2.left))
            {
                destination = (Vector2)this.transform.position + Vector2.left;
            }
        }
       

        Vector2 dir = destination - (Vector2)this.transform.position;
 */
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

    //metodo que dada una posible direccion de movimiento
    //devuelve true si podemos ir en dicha dirección y false si algo nos impode avanzar
    bool CanMoveTo(Vector2 dir)
    {
        //hacemos que se dibuje una linea desde la posicion de Pacman hacia la posicion donde quiere ir
        Vector2 pacmanPos = this.transform.position;
        //de esta manera se chequera desde el punto al que nos  dirgimos hacia el interior de pacman, la linea sale des de donde quiero ir hacia pacman, trazamos una linea desde donde quiero ir hacia pacman
        //lo hacemos de esta manera para ver que el raycast no choque contra el de pacman al principio, de esta manera solo chocara contra el collider de pacman si no hay ningún otro collider  delante
        RaycastHit2D hit = Physics2D.Linecast(pacmanPos + dir, pacmanPos);

        Collider2D pacmanCollider = GetComponent<Collider2D>();
        Collider2D hitCollider = hit.collider;

        if (hitCollider == pacmanCollider)
        {
            //no tengo nada más en medio --> puedo moverme allí
            return true;
        }
        else
        {
            //tengo un collider delante que NO es el de pacman --> no puedo moverme
            return false;
        }
        // return hit.collider == GetComponent<Collider2D>();

    }
}
