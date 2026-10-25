using System.Collections.Generic;
using TYU.Script.Virtual_OS.Application;
using UnityEngine;

public class PointMaker : MonoBehaviour
{
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private AppPosDataSO pointPosData;

    public List<AppPoint> MakePositions(Canvas cvs)
    {
        var result = new List<AppPoint>(pointPosData.positions.Count);
        foreach (var pos in pointPosData.positions)
        {
            GameObject ob = Instantiate(pointPrefab, cvs.transform);
            ob.GetComponent<RectTransform>().anchoredPosition = pos;
            var point = ob.GetComponent<AppPoint>();
            point.Initialize(cvs);
            result.Add(point);
        }

        return result;
    }
        
}
