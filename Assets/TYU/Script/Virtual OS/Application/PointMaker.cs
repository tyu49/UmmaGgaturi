using UnityEngine;

public class PointMaker : MonoBehaviour
{
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private AppPosDataSO pointPosData;
    
    
    [ContextMenu("MakePoints")]
    public void MakePositions()
    {
        foreach (var pos in pointPosData.positions)
        {
            GameObject ob = Instantiate(pointPrefab, transform);
            ob.GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }
        
}
