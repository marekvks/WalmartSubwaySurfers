using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Movement movement;

    private void Update()
    {
        if (movement.CurrentInstruction == Movement.Instruction.Jump && movement.IsGrounded) animator.SetTrigger("Jump");
        if (movement.CurrentInstruction == Movement.Instruction.SlideDown && movement.IsGrounded) animator.SetTrigger("Slide");
    }
}
