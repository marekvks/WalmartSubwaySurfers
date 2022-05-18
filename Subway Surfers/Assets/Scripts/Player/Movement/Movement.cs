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
    
    private float desiredPos;

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
    private Transform currentPos;

    private void Start()
    {
        currentPos = centerLandPos;
        m_NormalColliderHeight = controller.height;
        m_NormalColliderCenter = controller.center;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        // tudum tudum

        m_Velocity.y += gravity * Time.deltaTime; // Gravitace - 1/2gt^2

        IsGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer); // Checkuje jestli je hráč na zemi
        IsWallLeft = Physics.Raycast(transform.position, -transform.right, 5f); // Checkuje jestli je zeď vlevo
        IsWallRight = Physics.Raycast(transform.position, transform.right, 5f); // Checkuje jestli je zeď vpravo

        if (IsGrounded && m_Velocity.y < 0f) // Pokud je na zemi a velocity.y je menší než 0, tak bude nastavena na -2, protože kdybych pak chtěl vyskočit, velocity.y by byla tak malá, že bych nevyskočil
            m_Velocity.y = -2f; // -2 je tu, protože furt chci, aby to hráče tlačilo k zemi

        if (IsGrounded && CurrentInstruction == Instruction.Jump) // Když je na zemi, chce vyskočit vyskočí
            Jump();
        
        if ((CurrentInstruction == Instruction.SlideRight && currentPos != centerLandPos && !IsWallRight) || CurrentInstruction == Instruction.SlideLeft && currentPos != centerLandPos && !IsWallLeft)
            // Podmínka nad - Pokud chce hráč slidovat doprava, není ve středu, zeď není vpravo nebo když chce slidovat doleva - samá podmínka, tak slidne na střed
            SlideToSide(centerLandPos);
        else if (CurrentInstruction == Instruction.SlideRight && currentPos == centerLandPos && !IsWallRight) // Pokud chce slideovat doprava, hráč je ve prostředku a zeď není vpravo, tak sliduje
            SlideToSide(rightLandPos);
        else if (CurrentInstruction == Instruction.SlideLeft && currentPos == centerLandPos && !IsWallLeft) // Pokud chce slideovat doleva, hráč je ve prostředku a zeď není vlevo, tak sliduje
            SlideToSide(leftLandPos);

        if (IsGrounded && CurrentInstruction == Instruction.SlideDown) // Pokud je na zemi a chce slidovat dolů, tak udělá animaci slidu a collider se nastaví na menší
            Slide();
        else if (!IsGrounded && CurrentInstruction == Instruction.SlideDown) // Pokud není na zemi, přesto chce slidovat dolů, tak to hráče spustí dolu - pushne
            m_Velocity.y -= pushDownForce;

        controller.Move(Vector3.forward * speed * Time.deltaTime); // Move přes Character Controller
        controller.Move(Vector3.up * m_Velocity.y * Time.deltaTime);

        if ((justSlided && Time.time > m_SavedTimeToNormalizeColliders) || justSlided && CurrentInstruction == Instruction.Jump) // Po dokončení slidu se collider znormalizuje - nastaví se normální velikost
            NormalizeCollider();
        }

    private void SlideToSide(Transform side)
    {
        desiredPos = side.position.x; // Desired pozice
        transform.DOMoveX(desiredPos, smoothTime); // Move do strany přes DoTween
        audioManager.PlaySFXSound(audioManager.SFX_Swoosh); // AudioManager přehraje SFX
        currentPos = side;
    }

    private void Jump()
    {
        m_Velocity.y = Mathf.Sqrt(gravity * -2f * jumpForce); // Kalkulce jumpu
        audioManager.PlaySFXSound(audioManager.SFX_Swoosh); // AudioManager přehraje SFX
    }

    private void Slide()
    {
        m_SavedTimeToNormalizeColliders = Time.time + 1f; // Čas na to, kdy se má collider znormalizovat - normální výška
        controller.height = slideHeight; // Změní se velikost collideru
        controller.center = slideOffset; // nastaví se center collideru trošku níž
        audioManager.PlaySFXSound(audioManager.SFX_Swoosh); // AudioManager přehraje SFX
        justSlided = true;
    }

    public void NormalizeCollider() // Nastaví výšku collideru na normální (startovní), to samé s centerem
    {
        controller.height = m_NormalColliderHeight;
        controller.center = m_NormalColliderCenter;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius); // Vykresluje radius groundchecku - jen pro debug
    }
}
