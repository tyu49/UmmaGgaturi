using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SUNFISHH.Scripts.Player
{
    public class PlayerAgent : AbstractAgent
    {
        [Header("Settings")] 
        [SerializeField] private float _followDur = 0.3f;
        private Vector2 _mousePos;

        private void Update()
        {
            MouseValue();
            KeyBoardValue();
        }

        private void MouseValue()
        {
            _mousePos = Mouse.current.position.ReadValue();
            //Tween.Position(transform, _mousePos, _followDur);
            
            Vector2 next = Vector2.Lerp(transform.position, ClampRange(_mousePos), _followDur);
            transform.position = next;
        }

        private void KeyBoardValue()
        {
            if (Keyboard.current.wKey.wasPressedThisFrame)
            {
                TryChangeRange(Vector2.up);
            }

            if (Keyboard.current.aKey.wasPressedThisFrame)
            {
                TryChangeRange(Vector2.left);
            }

            if (Keyboard.current.sKey.wasPressedThisFrame)
            {
                TryChangeRange(Vector2.down);
            }

            if (Keyboard.current.dKey.wasPressedThisFrame)
            {
                TryChangeRange(Vector2.right);
            }
        }
    }
}