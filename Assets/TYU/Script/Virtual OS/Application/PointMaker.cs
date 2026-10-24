using System.Collections.Generic;
using TYU.Script.Virtual_OS.Application;
using UnityEngine;

public class PointMaker : MonoBehaviour
{
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private AppPosDataSO pointPosData;

    public List<AppPoint> MakePositions(Transform parentTrm)
    {
        var result = new List<AppPoint>(pointPosData.positions.Count);
        foreach (var pos in pointPosData.positions)
        {
            GameObject ob = Instantiate(pointPrefab, parentTrm);
            ob.GetComponent<RectTransform>().anchoredPosition = pos;
            result.Add(ob.GetComponent<AppPoint>());
        }

        return result;
    }
        
}
