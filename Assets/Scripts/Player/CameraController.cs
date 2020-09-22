using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 0.5f;

    public float dragSpeed = 1.5f;      
    private bool shouldLerpToPlayer = false;
    private float lerpStartTime;
    private float lerpTime;
    Vector3 lerpTargetPosition;

    public PixelPerfectCameraHelper cameraHelper;

    // Update is called once per frame
    void Update()
    {
        //cameraHelper.MoveTo(transform.position);
        ZoomCamera();
        DragCamera();
        MoveCameraToPlayer();
    }
    void ZoomCamera()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            cameraHelper.ZoomIn();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            cameraHelper.ZoomOut();
        }
    }
    void DragCamera()
    {
        if (Input.GetMouseButton(2))
        {
            Vector3 moveDir = new Vector3(-Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y"), 0).normalized;
            cameraHelper.Move(moveDir * dragSpeed * Time.deltaTime);
        }
    }
    void MoveCameraToPlayer()
    {
        if (shouldLerpToPlayer)
        {
            lerpTargetPosition = transform.parent.transform.position + new Vector3(0, 0, transform.position.z);
            transform.position = StartLerping();
            if (transform.position == lerpTargetPosition)
                shouldLerpToPlayer = false;
        }
    }
    private Vector3 StartLerping()
    {
        float timeSinceStarted = Time.time - lerpStartTime;
        float completePercentage = timeSinceStarted / lerpTime;
        Vector3 result = Vector3.Lerp(transform.position, lerpTargetPosition, completePercentage);
        return result;
    }
    public void LerpToPosition(Vector3 lerpTargetPosition, float lerpStartTime, float lerpTime)
    {
        this.lerpStartTime = lerpStartTime;
        this.lerpTime = lerpTime;
        this.lerpTargetPosition = lerpTargetPosition - new Vector3(0, 0, transform.position.z);
        this.shouldLerpToPlayer = true;
    }
}
