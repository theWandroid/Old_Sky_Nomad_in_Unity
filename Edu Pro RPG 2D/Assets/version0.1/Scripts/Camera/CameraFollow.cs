using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("Here goes the GameObject we want to follow with the Camera")]
    public GameObject target;
    [Tooltip("Here goes the position of the GameObject that we have described before")]
    private Vector3 targetPosition;
    [Tooltip("This is a variable for the speed of the camera")]
    public float cameraSpeed;
    private Camera theCamera;
    private Vector3 minLimits, maxLimits;
    private float halfHeight, halfWidth;

    private void Start()
    {
        this.transform.position = LoadCameraPosition();

    }

    public void ChangeLimits(BoxCollider2D cameraLimits)
    {

        cameraLimits = GameObject.Find("Limits").GetComponent<BoxCollider2D>();

        minLimits = cameraLimits.bounds.min;
        maxLimits = cameraLimits.bounds.max;
        theCamera = GetComponent<Camera>();
        halfHeight = theCamera.orthographicSize;
        halfWidth = (halfHeight / Screen.height) * Screen.width;

    }

    // Update is called once per frame
    void Update()
    {
        float posX = Mathf.Clamp(this.target.transform.position.x, minLimits.x + halfWidth, maxLimits.x - halfWidth);
        float posY = Mathf.Clamp(this.target.transform.position.y, minLimits.y + halfWidth, maxLimits.y - halfWidth);

        //actualizra la posicion del personaje en todo moento
        targetPosition = new Vector3(posX, posY, this.transform.position.z);



    }

    private void LateUpdate()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * cameraSpeed);
    }

    public void SaveCameraPosition()
    {
        PlayerPrefs.SetFloat("cameraPositionX", this.transform.position.x);
        PlayerPrefs.SetFloat("cameraPositionY", this.transform.position.y);
        PlayerPrefs.SetFloat("cameraPositionZ", this.transform.position.z);
    }

    public Vector3 LoadCameraPosition()
    {
        return new Vector3(PlayerPrefs.GetFloat("cameraPositionX", this.transform.position.x), PlayerPrefs.GetFloat("cameraPositionY", this.transform.position.y), PlayerPrefs.GetFloat("cameraPositionZ", this.transform.position.z));
    }
}
