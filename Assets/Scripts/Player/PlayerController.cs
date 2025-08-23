using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed=5f;
    [SerializeField] private float fallSpeed = 2f;
    [SerializeField] private float cameraRotateSpeed = 500f;

    private Quaternion targetRotation;

    private bool isGrounded;

    [Header("GroundChecker Settings")]
    [SerializeField] private float groundCheckerRadius;
    [SerializeField] private Vector3 groundCheckerOffset;
    [SerializeField] private LayerMask WhatIsGround;
    
    private CameraController cameraController;
    private CharacterController characterController;
    private MeeleFighter meeleFighter;
    private Animator anim;


    private void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        meeleFighter = GetComponent<MeeleFighter>();
    }

    private void Update()
    {
        if(meeleFighter.InAction)
        {
            anim.SetFloat("moveAmount", 0);
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float moveAmount =Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

        Vector3 moveInput = (new Vector3(h, 0, v)).normalized;

        Vector3 moveDir = cameraController.PlanarRotation*moveInput;//旋转作用在向量上
        GroundCheck();
        if(isGrounded)
        {
            fallSpeed = -0.5f;
        }
        else
        {
            fallSpeed += Physics.gravity.y * Time.deltaTime;
        }
        var velocity = moveDir * moveSpeed;
        velocity.y = fallSpeed;

        characterController.Move(velocity * Time.deltaTime);
        if (moveAmount>0)
        {
            targetRotation = Quaternion.LookRotation(moveDir);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, cameraRotateSpeed*Time.deltaTime);

        anim.SetFloat("moveAmount", moveAmount, 0.2f, Time.deltaTime);
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckerOffset), groundCheckerRadius, WhatIsGround);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckerOffset), groundCheckerRadius);
    }

}
