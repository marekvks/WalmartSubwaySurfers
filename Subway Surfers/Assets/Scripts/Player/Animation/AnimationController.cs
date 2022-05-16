using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Movement movement;

    private void Update()
    {
        if (movement.CurrentInstruction == Movement.Instruction.Jump && movement.IsGrounded) animator.SetTrigger("Jump");
        if (movement.CurrentInstruction == Movement.Instruction.SlideDown && movement.IsGrounded) animator.SetTrigger("Slide");

        if (movement.CurrentInstruction == Movement.Instruction.SlideLeft && !movement.IsWallLeft) animator.SetInteger("SlideToSide", -1); // -1 - left, 1- right
            else if (movement.CurrentInstruction == Movement.Instruction.SlideRight && !movement.IsWallRight) animator.SetInteger("SlideToSide", 1); // 1 - left, 2 - right
            else animator.SetInteger("SlideToSide", 0);
    }
}
