using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private ZaPosledniMesicJsemVystridalAsi15MovementScriptuAleTadyJeDalsi playerMovement;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerMovement.m_IsGrounded) playerAnimator.SetTrigger("Jump");
        if (Input.GetKeyDown(KeyCode.S) && playerMovement.m_IsGrounded) playerAnimator.SetTrigger("Slide");

        /*if (Input.GetKeyDown(KeyCode.A) && !playerMovement.m_IsWallLeft) playerAnimator.SetInteger("Slide", 0); // 1 - left, 2 - right
            else if (Input.GetKeyDown(KeyCode.A) && !playerMovement.m_IsWallRight) playerAnimator.SetInteger("Slide", 1); // 1 - left, 2 - right
            else playerAnimator.SetInteger("Slide", 0);*/ // Slide animace není potřeba, protože prostě slidne hrozně rychle a takhle se mi to líbí :D
    }
}
