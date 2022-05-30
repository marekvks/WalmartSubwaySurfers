using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using UnityEditor.Callbacks;

public class Movement : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private AudioManager audioManager;
    
    [Header("Components")]
    [SerializeField] private CharacterController controller;

    [Header("Movement Speed, Jump & Slide Values")]
    // Movement Speed
    [SerializeField] private float speed = 6f;
    // Jump & Gravity
    [SerializeField] private float jumpForce = 3f;
    private Vector3 _velocity = Vector3.zero;
    [SerializeField] private float gravity = -80f;
    // Slide
    [SerializeField] private float smoothTime = 0.15f;
    [SerializeField] private float pushDownForce = 10f;
    
    private bool _justSlided;
    
    [SerializeField] private float slideHeight = 0.64f;
    [SerializeField] private Vector3 slideOffset;

    private float _savedTimeToNormalizeColliders;
    
    private float _normalColliderHeight;
    private Vector3 _normalColliderCenter;

    
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [HideInInspector] public bool IsGrounded;
    
    // Wall Checks
    [HideInInspector] public bool IsWallLeft, IsWallRight;

    [Header("Land Positions")]
    [SerializeField] private Transform centerLandPos;
    [SerializeField] private Transform rightLandPos;
    [SerializeField] private Transform leftLandPos;
    private float _desiredPos;

    private Transform _currentPos;


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
        _currentPos = centerLandPos;
        _normalColliderHeight = controller.height;
        _normalColliderCenter = controller.center;
    }

    private void Update()
    {
        Move();
    }
    
    private void Move()
    {
        // tudum tudum

        _velocity.y += gravity * Time.deltaTime; // Gravitace - 1/2gt^2

        IsGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        IsWallLeft = Physics.Raycast(transform.position, -transform.right, 5f);
        IsWallRight = Physics.Raycast(transform.position, transform.right, 5f);

        if (IsGrounded && _velocity.y < 0f) // Pokud je na zemi a velocity.y je menší než 0, tak bude nastavena na -2, protože kdybych pak chtěl vyskočit, velocity.y by byla tak malá, že bych nevyskočil
            _velocity.y = -2f; // -2 je tu, protože furt chci, aby to hráče tlačilo k zemi

        if (IsGrounded && CurrentInstruction == Instruction.Jump)
            Jump();
        
        if ((CurrentInstruction == Instruction.SlideRight && _currentPos != centerLandPos && !IsWallRight) || CurrentInstruction == Instruction.SlideLeft && _currentPos != centerLandPos && !IsWallLeft)
            // Podmínka nad - Pokud chce hráč slidovat doprava, není ve středu, zeď není vpravo nebo když chce slidovat doleva - samá podmínka, tak slidne na střed
            SlideToSide(centerLandPos);
        else if (CurrentInstruction == Instruction.SlideRight && _currentPos == centerLandPos && !IsWallRight) // Pokud chce slideovat doprava, hráč je ve prostředku a zeď není vpravo, tak sliduje
            SlideToSide(rightLandPos);
        else if (CurrentInstruction == Instruction.SlideLeft && _currentPos == centerLandPos && !IsWallLeft) // Pokud chce slideovat doleva, hráč je ve prostředku a zeď není vlevo, tak sliduje
            SlideToSide(leftLandPos);

        if (IsGrounded && CurrentInstruction == Instruction.SlideDown)
            Slide();
        else if (!IsGrounded && CurrentInstruction == Instruction.SlideDown) // Pokud není na zemi, přesto chce slidovat dolů, tak to hráče spustí dolu - pushne
            _velocity.y -= pushDownForce;

        controller.Move(Vector3.forward * speed * Time.deltaTime);
        controller.Move(Vector3.up * _velocity.y * Time.deltaTime);

        if (_justSlided && Time.time > _savedTimeToNormalizeColliders || _justSlided && CurrentInstruction == Instruction.Jump) // Po dokončení slidu se collider znormalizuje - nastaví se normální velikost
            NormalizeCollider();
    }

    private void SlideToSide(Transform side)
    {
        _desiredPos = side.position.x; // Desired pozice
        transform.DOMoveX(_desiredPos, smoothTime); // Move do strany přes DOTween
        audioManager.PlaySFXSound(audioManager.SFX_Swoosh);
        _currentPos = side;
    }

    private void Jump()
    {
        _velocity.y = Mathf.Sqrt(gravity * -2f * jumpForce);
        audioManager.PlaySFXSound(audioManager.SFX_Swoosh);
    }

    private void Slide()
    {
        _savedTimeToNormalizeColliders = Time.time + 1f; // Čas na to, kdy se má collider znormalizovat - normální výška
        controller.height = slideHeight; // Změní se velikost collideru
        controller.center = slideOffset; // nastaví se center collideru trošku níž
        audioManager.PlaySFXSound(audioManager.SFX_Swoosh);
        _justSlided = true;
    }

    public void NormalizeCollider() // Nastaví výšku collideru na normální (startovní), to samé s centrem
    {
        controller.height = _normalColliderHeight;
        controller.center = _normalColliderCenter;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius); // Vykresluje radius ground checku - jen pro debug
    }
}
