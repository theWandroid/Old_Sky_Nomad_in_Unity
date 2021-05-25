using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class JoystickController : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        float movH = CrossPlatformInputManager.GetAxis("Horizontal") * speed;
        float movV = CrossPlatformInputManager.GetAxis("Vertical") * speed;
        _rigidBody.velocity = new Vector2(movH, movV).normalized * speed;
    }
}
