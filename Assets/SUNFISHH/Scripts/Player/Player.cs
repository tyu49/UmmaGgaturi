using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SUNFISHH.Scripts.Player
{
    public class Player : AbstractAgent
    {
        [Header("Settings")] 
        [SerializeField] private float _followDur = 0.3f;
        private Vector2 _mousePos;

        private void Update()
        {
            _mousePos = Mouse.current.position.ReadValue();
            //Tween.Position(transform, _mousePos, _followDur);
            Vector2 next = Vector2.Lerp(transform.position, _mousePos, _followDur);
            transform.position = next;
        }
    }
}