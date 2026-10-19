using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AppPosDataSO", menuName = "ScreenPositionData/AppPosDataSO")]
public class AppPosDataSO : ScriptableObject
{
    [field: SerializeField] public List<Vector2> positions { get; private set; }

}
