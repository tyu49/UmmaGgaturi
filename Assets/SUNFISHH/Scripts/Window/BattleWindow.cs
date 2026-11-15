using UnityEngine;

namespace SUNFISHH.Scripts
{
    public class BattleWindow : AbstractWindow
    {

        [Header("Battle Settings")] 
        [SerializeField] private float moveSpeed;
        [SerializeField] private RectTransform enemyParent;
        [SerializeField] private Player.Player player;
    }
}