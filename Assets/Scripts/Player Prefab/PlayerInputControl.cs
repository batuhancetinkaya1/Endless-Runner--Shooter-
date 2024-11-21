using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputControl : MonoBehaviour
{
    [SerializeField] PlayerAnimControl playerAnimControl;
    [SerializeField] PlayerShooter playerShooter;
    [SerializeField] PlayerChangeLine playerChangeLine;

    public void KeyA()
    {
        if (!playerAnimControl.isJumping && !playerChangeLine.isChangingLine && playerChangeLine.currentLine != (int)PlayerChangeLine.Lines.Left)
        {
            playerAnimControl.Jump();
            playerAnimControl.StopRunning();
            playerAnimControl.StopShooting();
            playerChangeLine.TurnLeft(OnLineChangeComplete);
        }
    }

    public void KeyD()
    {
        if (!playerAnimControl.isJumping && !playerChangeLine.isChangingLine && playerChangeLine.currentLine != (int)PlayerChangeLine.Lines.Right)
        {
            playerAnimControl.Jump();
            playerAnimControl.StopRunning();
            playerAnimControl.StopShooting();
            playerChangeLine.TurnRight(OnLineChangeComplete);
        }
    }

    private void OnLineChangeComplete()
    {
        playerAnimControl.ResetJump();
        playerAnimControl.StartRunning();
    }

    public void KeyShootDown()
    {
        if (!playerAnimControl.isJumping && !playerChangeLine.isChangingLine)
        {
            playerAnimControl.StartShooting();
            playerShooter.TryShoot();
        }
    }

    public void KeyShootUp()
    {
        playerAnimControl.StopShooting();
    }

    public void KeyB()
    {
        playerAnimControl.StartRunning();
        playerAnimControl.GameOn();
    }
}