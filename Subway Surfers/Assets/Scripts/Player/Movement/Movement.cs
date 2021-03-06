using UnityEngine;
using DG.Tweening;

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

        _velocity.y += gravity * Time.deltaTime;

        IsGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        IsWallLeft = Physics.Raycast(transform.position, -transform.right, 5f);
        IsWallRight = Physics.Raycast(transform.position, transform.right, 5f);

        if (IsGrounded && _velocity.y < 0f)
            // -2 je tu, proto??e furt chci, aby to hr????e tla??ilo k zemi p??esto??e na zemi je
            _velocity.y = -2f;

        if (IsGrounded && CurrentInstruction == Instruction.Jump)
            Jump();
        
        // Podm??nka nad - Pokud chce hr???? slidovat doprava, nen?? ve st??edu, ze?? nen?? vpravo nebo kdy?? chce slidovat doleva - sam?? podm??nka, tak slidne na st??ed
        if ((CurrentInstruction == Instruction.SlideRight && _currentPos != centerLandPos && !IsWallRight) || CurrentInstruction == Instruction.SlideLeft && _currentPos != centerLandPos && !IsWallLeft)
            SlideToSide(centerLandPos);
        else if (CurrentInstruction == Instruction.SlideRight && _currentPos == centerLandPos && !IsWallRight)
            SlideToSide(rightLandPos);
        else if (CurrentInstruction == Instruction.SlideLeft && _currentPos == centerLandPos && !IsWallLeft)
            SlideToSide(leftLandPos);

        if (IsGrounded && CurrentInstruction == Instruction.SlideDown)
            Slide();
        // Pokud nen?? na zemi, p??esto chce slidovat dol??, tak to hr????e spust?? dolu - pushne
        else if (!IsGrounded && CurrentInstruction == Instruction.SlideDown)
            _velocity.y -= pushDownForce;

        controller.Move(Vector3.forward * speed * Time.deltaTime);
        controller.Move(Vector3.up * _velocity.y * Time.deltaTime);

        // Po dokon??en?? slidu se collider znormalizuje - nastav?? se norm??ln?? velikost
        if (_justSlided && Time.time > _savedTimeToNormalizeColliders || _justSlided && CurrentInstruction == Instruction.Jump)
            NormalizeCollider();
    }

    private void SlideToSide(Transform side)
    {
        _desiredPos = side.position.x;
        transform.DOMoveX(_desiredPos, smoothTime);
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
        // ??as na to, kdy se m?? collider znormalizovat - norm??ln?? v????ka
        _savedTimeToNormalizeColliders = Time.time + 1f;
        // Zm??n?? se velikost collideru
        controller.height = slideHeight;
        // nastav?? se center collideru tro??ku n????
        controller.center = slideOffset;
        audioManager.PlaySFXSound(audioManager.SFX_Swoosh);
        _justSlided = true;
    }

    // Nastav?? v????ku collideru na norm??ln?? (startovn??), to sam?? s centrem
    public void NormalizeCollider()
    {
        controller.height = _normalColliderHeight;
        controller.center = _normalColliderCenter;
    }

    // Vykreslov??n?? nap??. r??dius?? - je vid??t pouze ve sc??n??, nikoliv ve h??e
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }
}
