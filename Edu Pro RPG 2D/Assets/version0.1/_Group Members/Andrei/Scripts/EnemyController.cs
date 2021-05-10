using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(HealthManager))]
public class EnemyController : MonoBehaviour
{
    [Tooltip("Velocidad de movimiento del enemigo")]
    public float speed = 1.0f;
    private Rigidbody2D _rigidBody;
    [Tooltip("Is a variable to know when is moving the enemy")]
    private bool isMoving;

    [Tooltip("Tiempo que tarda el enemigo entre pasos sucesivos")]
    public float timeBetweenSteps;
    private float timeBetweenStepsCounter;

    [Tooltip("Tiempo que tarda el enemigo en dar un paso")]
    public float timeToMakeStep;
    [Tooltip("Is the counter to make steps")]
    private float timeToMakeStepCounter;

    [Tooltip("Is the direction to move of the enemy")]
    public Vector2 directionToMove;


    // Start is called before the first frame update
    void Start()
    {
        //we instantiete the rigidBody of the enemy
        _rigidBody = GetComponent<Rigidbody2D>();

        //timeBetweenStepsCounter = timeBetweenSteps;
        //the timeBettweenStepsCounter is equals to timeBetweenSteps multiply by a random number between 0.5 and 1.5, to make more random
        timeBetweenStepsCounter = timeBetweenSteps*Random.Range(0.5f,1.5f);

        //timeToMakeStepCounter = timeToMakeStep;
        timeToMakeStepCounter = timeToMakeStep*Random.Range(0.5f, 1.5f);

    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            //we initiate the timeToMakeStepCounter when the enemy isMoving
            timeToMakeStepCounter -= Time.deltaTime;
            //_rigidBody.velocity is a vector2, so we have to give them the direction and the speed
            _rigidBody.velocity = directionToMove * speed;
            //cuando me quedo sin tiempo de movimiento paramos al enemigo
            if(timeToMakeStepCounter < 0)
            {//when the enemy is without time to make the steps, we equeals the variable isMoving to false 
                isMoving = false;
                //equals the time of the counter to the time between steps
                timeBetweenStepsCounter = timeBetweenSteps;
                _rigidBody.velocity = Vector2.zero;
            }
        }
        else
        {
            timeBetweenStepsCounter -= Time.deltaTime;

            //cuando me quedo sin tiempo de estar parado
            //arranca al enemigo para que de el paso
            if(timeBetweenStepsCounter < 0)
            {//the variable isMoving becomes true
                isMoving = true;
                timeToMakeStepCounter = timeToMakeStep;
                directionToMove = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
            }
            
        }
    }
}
