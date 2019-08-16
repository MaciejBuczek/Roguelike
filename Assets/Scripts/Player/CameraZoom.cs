using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {

    public float zoomSpeed = 0.5f;
    public float dragSpeed = 1.5f;
    private int rows, cols;
    private Camera playerCamera;
    private float cameraSize;
    private Vector3 dragOrigin;
    private Vector3 lastMouseCoordinate;
    Vector3 mouseDelta = Vector3.zero;
    private bool shouldLerp = false;
    private float lerpStartTime;
    private float lerpTime;
    Vector3 lerpTargetPosition;
    // Use this for initialization
    void Start()
    {
        playerCamera = GetComponent<Camera>();
        rows = GameObject.Find("GameManager").GetComponent<DungeonGenerator>().rows;
        cols = GameObject.Find("GameManager").GetComponent<DungeonGenerator>().cols;
    }
	// Update is called once per frame
	void Update () {
        zoomCamera();
        dragCamera();
        if (shouldLerp)
        {
            transform.position = startLerping();
            if (transform.position == lerpTargetPosition)
                shouldLerp = false;
        }
    }
    void zoomCamera()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            cameraSize -= zoomSpeed;
            cameraSize = Mathf.Clamp(cameraSize, 5f, 15f);
            playerCamera.orthographicSize = cameraSize;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            cameraSize += zoomSpeed;
            cameraSize = Mathf.Clamp(cameraSize, 5f, 15f);
            playerCamera.orthographicSize = cameraSize;
        }       
    }
    void dragCamera()
    {
        
        if (Input.GetMouseButtonDown(2))
        {
            lastMouseCoordinate = Input.mousePosition;
            dragOrigin = Input.mousePosition;
            return;
        }
        if (mouseDelta.x == 0 && mouseDelta.y == 0)
            dragOrigin = Input.mousePosition;
        mouseDelta = Input.mousePosition - lastMouseCoordinate;
        if (!Input.GetMouseButton(2) || (mouseDelta.x == 0 && mouseDelta.y == 0)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(-pos.x, -pos.y, 0);
        move *= dragSpeed;
        move.x = Mathf.Clamp(move.x, -2f, 2f);
        move.y = Mathf.Clamp(move.y, -2f, 2f);
        transform.Translate(move, Space.World);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, cols), Mathf.Clamp(transform.position.y, 0, rows), transform.position.z);
        lastMouseCoordinate = Input.mousePosition;
    }
    private Vector3 startLerping()
    {
        float timeSinceStarted = Time.time - lerpStartTime;
        float completePercentage = timeSinceStarted / lerpTime;
        Vector3 result = Vector3.Lerp(transform.position, lerpTargetPosition, completePercentage);
        return result;
    }
    public void lerpToPosition(Vector3 lerpTargetPosition, float lerpStartTime, float lerpTime)
    {
        this.lerpStartTime = lerpStartTime;
        this.lerpTime = lerpTime;
        this.lerpTargetPosition = new Vector3(lerpTargetPosition.x, lerpTargetPosition.y, transform.position.z);
        this.shouldLerp = true;
    }
}
