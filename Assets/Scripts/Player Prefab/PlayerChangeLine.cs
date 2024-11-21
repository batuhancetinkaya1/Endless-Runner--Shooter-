using DG.Tweening;
using System;
using UnityEngine;

public class PlayerChangeLine : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float shift;
    [SerializeField] float performTime;
    public int currentLine = 0;
    public bool isChangingLine { get; private set; }

    public enum Lines
    {
        Left = -1,
        Center = 0,
        Right = 1
    }

    public void TurnLeft(Action onComplete)
    {
        if (currentLine != (int)(Lines.Left) && !isChangingLine)
        {
            isChangingLine = true;
            currentLine -= 1;
            rb.transform.DOMoveX(rb.transform.position.x - shift, performTime)
                .SetEase(Ease.InOutQuart)
                .OnComplete(() =>
                {
                    isChangingLine = false;
                    onComplete?.Invoke();
                });
        }
    }

    public void TurnRight(Action onComplete)
    {
        if (currentLine != (int)(Lines.Right) && !isChangingLine)
        {
            isChangingLine = true;
            currentLine += 1;
            rb.transform.DOMoveX(rb.transform.position.x + shift, performTime)
                .SetEase(Ease.InOutQuart)
                .OnComplete(() =>
                {
                    isChangingLine = false;
                    onComplete?.Invoke();
                });
        }
    }

    public void Reset()
    {
        isChangingLine = false;
        currentLine = 0;
    }
}