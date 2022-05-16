using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class Movement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float jumpForce = 3f;

    [SerializeField] private float gravity = -800f;
    private Vector3 m_Velocity = Vector3.zero;

    [HideInInspector] public bool IsWallLeft, IsWallRight;
    [HideInInspector] public bool IsGrounded;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.1f;

    [SerializeField] private float slideHeight = 0.64f;
    [SerializeField] private Vector3 slideOffset;
    
    [SerializeField] private float smoothTime = 0.15f;
    [SerializeField] private float pushDownForce = 10f;


    [SerializeField] private Transform centerLandPos, rightLandPos, leftLandPos;

    [SerializeField] private AudioManager audioManager;
    
    private float currentXPos;

    private float m_NormalColliderHeight;
    private Vector3 m_NormalColliderCenter;

    private bool justSlided;
    private float m_SavedTimeToNormalizeColliders;


    public enum Instruction
    {
        Run,
        Jump,
        SlideDown,
        SlideLeft,
        SlideRight
    }

    public Instruction CurrentInstruction = Instruction.Run;

    private void Start()
    {
        m_NormalColliderHeight = controller.height;
        m_NormalColliderCenter = controller.center;
    }

    private void Update()
    {
        Debug.Log(controller.detectCollisions);
        if (CurrentInstruction != Instruction.Run) Debug.Log(CurrentInstruction.ToString());
        Move();
    }

    private void Move()
    {
        // tudum tudum

        IsGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        m_Velocity.y += gravity * Time.deltaTime;

        if (IsGrounded && m_Velocity.y < 0f)
        {
            m_Velocity.y = -2f;
        }

        if (IsGrounded && CurrentInstruction == Instruction.Jump)
        {
            Jump();
        }

        IsWallLeft = Physics.Raycast(transform.position, -transform.right, 5f);
        IsWallRight = Physics.Raycast(transform.position, transform.right, 5f);

        if ((CurrentInstruction == Instruction.SlideRight && IsWallLeft && !IsWallRight) || CurrentInstruction == Instruction.SlideLeft && IsWallRight && !IsWallLeft) SlideToSide(centerLandPos);
            else if (CurrentInstruction == Instruction.SlideLeft && !IsWallLeft) SlideToSide(leftLandPos);
        else if (CurrentInstruction == Instruction.SlideRight && !IsWallRight) SlideToSide(rightLandPos);

        if (IsGrounded && CurrentInstruction == Instruction.SlideDown)
        {
            Slide();
        }
        else if (!IsGrounded && CurrentInstruction == Instruction.SlideDown)
        {
            m_Velocity.y -= pushDownForce;
        }

        controller.Move(Vector3.forward * speed * Time.deltaTime);
        controller.Move(Vector3.up * m_Velocity.y * Time.deltaTime);

        //transform.position = Vector3.Lerp(transform.position, pos, smoothTime);

        if ((justSlided && Time.time > m_SavedTimeToNormalizeColliders) || justSlided && CurrentInstruction == Instruction.Jump)
        {
            NormalizeCollider();
            justSlided = false;
        }
    }

    private void SlideToSide(Transform side)
    {
        currentXPos = side.position.x;
        transform.DOMoveX(currentXPos, smoothTime);
        audioManager.PlaySFXSound(audioManager.SFX_Swoosh);
    }

    private void Jump()
    {
        m_Velocity.y = Mathf.Sqrt(gravity * -2f * jumpForce);
        audioManager.PlaySFXSound(audioManager.SFX_Swoosh);
    }

    private void Slide()
    {
        m_SavedTimeToNormalizeColliders = Time.time + 1f;
        controller.height = slideHeight;
        controller.center = slideOffset;
        audioManager.PlaySFXSound(audioManager.SFX_Swoosh);
        justSlided = true;
    }

    public void NormalizeCollider()
    {
        controller.height = m_NormalColliderHeight;
        controller.center = m_NormalColliderCenter;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }
}