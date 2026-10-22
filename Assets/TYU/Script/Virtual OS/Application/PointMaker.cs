using UnityEngine;

public class PointMaker : MonoBehaviour
{
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private AppPosDataSO pointPosData;
    
    
    [ContextMenu("MakePoints")]
    public void MakePositions(Transform parentTrm)
    {
        foreach (var pos in pointPosData.positions)
        {
            GameObject ob = Instantiate(pointPrefab, parentTrm);
            ob.GetComponent<RectTransform>().anchoredPosition = pos;
            
        }
    }
        
}
