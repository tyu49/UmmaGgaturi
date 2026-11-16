using System;
using SUNFISHH.Scripts.Player;
using UnityEngine;

namespace SUNFISHH.Scripts
{
    public class BattleWindow : AbstractWindow
    {
        [Header("Battle Settings")] 
        [SerializeField] private float moveSpeed;
        [SerializeField] private RectTransform enemyParent;
        [SerializeField] private PlayerAgent player;

        public event Action<Vector2> Moved;
        private RectTransform _parentRect;
        private Vector2 _lastPos;
        private readonly Vector2[] _corners = new Vector2[4];

        protected override void Setting()
        {
            
        }
    }
}