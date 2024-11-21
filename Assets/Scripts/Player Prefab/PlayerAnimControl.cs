using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimControl : MonoBehaviour
{
    [SerializeField] public Animator animator;

    public bool isJumping = false;
    public bool isShooting = false;
    public bool isRunning = false;
    public bool isDeath = false;
    public bool isGameOn = false;

    public void Reset()
    {
        ResetJump();
        StopShooting();
        StopRunning();
        DeathFalse();
        isGameOn = false;
        // Make sure to reset the animator state
        animator.Play("Idle_Wave", 0, 0f);
        animator.Update(0f);
    }

    public void Jump()
    {
        isJumping = true;
        animator.SetBool("IsJumping", true);
    }

    public void ResetJump()
    {
        isJumping = false;
        animator.SetBool("IsJumping", false);
    }

    public void StartShooting()
    {
        isShooting = true;
        animator.SetBool("IsShooting", true);
    }

    public void StopShooting()
    {
        isShooting = false;
        animator.SetBool("IsShooting", false);
    }

    public void StartRunning()
    {
        isRunning = true;
        animator.SetBool("IsRunning", true);
    }

    public void StopRunning()
    {
        isRunning = false;
        animator.SetBool("IsRunning", false);
    }

    public void DeathTrue()
    {
        isDeath = true;
        animator.SetBool("IsDeath", true);
        StopRunning(); // Make sure to stop running animation
    }

    public void DeathFalse()
    {
        isDeath = false;
        animator.SetBool("IsDeath", false);
    }

    public void GameOn()
    {
        isGameOn = true;
    }

    public void GameOff()
    {
        isGameOn = false;
    }
}
