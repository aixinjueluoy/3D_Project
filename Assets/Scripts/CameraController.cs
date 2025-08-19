using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private float followDistance;

    [SerializeField] private float rotateSpeed=2f;

    [SerializeField] private Vector2 framingOffset;

    [SerializeField] private float rotationX;
    [SerializeField] private float rotationY;

    [SerializeField] private float minVerticalAngle=45f;
    [SerializeField] private float maxVerticalAngle=45f;

    [SerializeField] private bool invertX;//反转
    [SerializeField] private bool invertY;//反转

    private float invertXVal;
    private float invertYVal;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        invertXVal = invertX ? -1 : 1;
        invertYVal = invertY ? -1 : 1;

        rotationX += Input.GetAxis("Mouse Y") * rotateSpeed*invertXVal;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

        rotationY += Input.GetAxis("Mouse X") * rotateSpeed*invertYVal;

        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);
        var focusPosition = followTarget.position + new Vector3(framingOffset.x,framingOffset.y);
        transform.position = focusPosition - targetRotation * new Vector3(0, 0, followDistance);
        transform.rotation = targetRotation;

    }
    public Quaternion PlanarRotation => Quaternion.Euler(0,rotationY,0);
}
