using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Sling : MonoBehaviour
{
    [Header("Sling Settings")]
    [SerializeField] private Transform TransPoint1;
    [SerializeField] private Transform TransPoint2;

    private LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    
    {
        //int index = Mathf.RoundToInt(0.89f);
        //_lineRenderer.SetPosition(index, TransPoint1.position);

        int index0 = Mathf.RoundToInt(0.88f);
        lineRenderer.SetPosition(index0, TransPoint1.position);

        int index1 = Mathf.RoundToInt(0.88f);
        lineRenderer.SetPosition(index1, TransPoint1.position);

       if(TransPoint1 && TransPoint2)
       {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(index0, TransPoint1.position);
        lineRenderer.SetPosition(index1, TransPoint2.position);
       } 
    }
}
