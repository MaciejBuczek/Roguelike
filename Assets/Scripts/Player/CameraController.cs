﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera playerCamera;

    public float zoomSpeed = 0.5f;
    private float cameraSize;

    private int rows, cols;
    public float dragSpeed = 1.5f;      
    private Vector3 dragOrigin;
    private Vector3 lastMouseCoordinate;
    private bool shouldLerp = false;
    private float lerpStartTime;
    private float lerpTime;
    Vector3 lerpTargetPosition;

    // Use this for initialization
    void Start()
    {
        initializeValues();
    }
    // Update is called once per frame
    void Update()
    {
        zoomCamera();
        dragCamera();
        moveCameraToPlayer();
    }
    void initializeValues()
    {
        playerCamera = GetComponent<Camera>();
        rows = GameObject.Find("GameManager").GetComponent<DungeonGenerator>().rows;
        cols = GameObject.Find("GameManager").GetComponent<DungeonGenerator>().cols;
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
        if (Input.GetMouseButton(2))
        {
            transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed, -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * dragSpeed, 0);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, cols), Mathf.Clamp(transform.position.y, 0, rows), transform.position.z);
        }
    }
    void moveCameraToPlayer()
    {
        if (shouldLerp)
        {
            transform.position = startLerping();
            if (transform.position == lerpTargetPosition)
                shouldLerp = false;
        }
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
