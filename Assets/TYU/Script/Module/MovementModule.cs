using System;
using PrimeTween;
using UnityEngine;

public class MovementModule : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float dashPow;
    [SerializeField] private TweenSettings<Vector2> dashTween;

    private bool _canMove;
    private Rigidbody2D _rb;
    private Vector2 _curDir;

    public void SetMovement()
    {
        if (!_canMove) return;
        _rb.linearVelocity = _curDir * speed;
    }

    public void SetDir(Vector2 dir)
    {
        _curDir = dir;
    }
    
    public void Dash()
    {
        if (!_canMove) return;
        Vector2 destination = (Vector2)_rb.transform.position + (_curDir * dashPow);
        dashTween.endValue = destination;
        Sequence.Create().ChainDelay(dashTween.settings.duration)
            .Group(Tween.RigidbodyMovePosition(_rb, dashTween))
            .ChainCallback(() => _canMove = true);
        
    }

    private void FixedUpdate()
    {
        SetMovement();
    }
}
