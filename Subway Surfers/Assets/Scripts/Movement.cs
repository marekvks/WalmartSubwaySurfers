using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float speedMultiplyer = 100f;
    [SerializeField] private float jumpForce = 6f;

    [HideInInspector] public bool m_IsWallLeft, m_IsWallRight;
    [HideInInspector] public bool m_IsGrounded;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.1f;
    
    
    [SerializeField] private float slideHeight = 0.64f;
    [SerializeField] private Vector3 slideOffset;


    [SerializeField] private Transform centerLandPos, rightLandPos, leftLandPos;

    [SerializeField] private AudioManager audioManager;


    private Vector3 distanceToTravel = Vector3.zero;
    
    
    private float currentXPos;

    private float m_NormalColliderHeight;
    private Vector3 m_NormalColliderCenter;

    private bool justSlided;
    private float m_SavedTimeToNormalizeColliders;

    private void Start()
    {
        m_NormalColliderHeight = playerCollider.height;
        m_NormalColliderCenter = playerCollider.center;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        // tudum tudum
        rb.AddForce(transform.forward * speed * speedMultiplyer * Time.deltaTime);

        m_IsGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (m_IsGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        m_IsWallLeft = Physics.Raycast(transform.position, -transform.right, 5f);
        m_IsWallRight = Physics.Raycast(transform.position, transform.right, 5f);

        if ((Input.GetKeyDown(KeyCode.D) && m_IsWallLeft) || Input.GetKeyDown(KeyCode.A) && m_IsWallRight) SlideToSide(centerLandPos);
            else if (Input.GetKeyDown(KeyCode.A) && !m_IsWallLeft) SlideToSide(leftLandPos);
        else if (Input.GetKeyDown(KeyCode.D) && !m_IsWallRight) SlideToSide(rightLandPos);

        if (m_IsGrounded && Input.GetKeyDown(KeyCode.S))
        {
            Slide();
        }
        else if (!m_IsGrounded && Input.GetKeyDown(KeyCode.S))
        {
            rb.velocity -= Vector3.up * 10f;
        }

        /*Vector3 pos = transform.position;
        pos.x = currentXPos;
        transform.position = Vector3.Lerp(transform.position, pos, 10f * Time.deltaTime);*/

        if ((justSlided && Time.time > m_SavedTimeToNormalizeColliders) || justSlided && Input.GetKeyDown(KeyCode.Space))
        {
            NormalizeCollider();
            justSlided = false;
        }
    }

    private void SlideToSide(Transform side)
    {
        //currentXPos = side.position.x;

        distanceToTravel.x = side.position.x;
        
        audioManager.PlaySFXSound(audioManager.SFX_Swoosh);
    }

    private void Jump()
    {
        rb.velocity += Vector3.up * jumpForce;
        audioManager.PlaySFXSound(audioManager.SFX_Swoosh);
    }

    private void Slide()
    {
        justSlided = true;
        m_SavedTimeToNormalizeColliders = Time.time + 1f;
        playerCollider.height = slideHeight;
        playerCollider.center = slideOffset;
        audioManager.PlaySFXSound(audioManager.SFX_Swoosh);
    }

    public void NormalizeCollider()
    {
        playerCollider.height = m_NormalColliderHeight;
        playerCollider.center = m_NormalColliderCenter;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }
}