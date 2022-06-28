using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapLine : MonoBehaviour
{
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = this.GetComponent<LineRenderer>();

        int nCount = this.transform.childCount;
        lineRenderer.positionCount = nCount;

        for (int i = 0; i < nCount; i++)
        {
            Transform currDot = this.transform.GetChild(i).transform;
            lineRenderer.SetPosition(i, new Vector3(currDot.position.x, 3f, currDot.position.z));
        }
    }
}
