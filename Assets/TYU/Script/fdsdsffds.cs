using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class fdsdsffds : MonoBehaviour
{
    [SerializeField] private Canvas ca;
    [SerializeField] private List<RectTransform> ssddfs;
    [SerializeField] private AppPosDataSO data;

    [ContextMenu("dddd")]
    public void dddd()
    {
        foreach (var adf in ssddfs)
        {
            adf.SetParent(ca.transform);
            // data.add(adf.anchoredPosition);
        }
    }
}
