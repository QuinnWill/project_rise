using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SimpleRope : MonoBehaviour
{

    private LineRenderer _lineRenderer;

    public Transform point1;
    public Transform point2;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        point1 = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!point1 || !point2)
        {
            return;

        }
        else if (point1.gameObject.activeInHierarchy && point2.gameObject.activeInHierarchy)
        {
            _lineRenderer.enabled = true;
            _lineRenderer.SetPosition(0, point1.position);
            _lineRenderer.SetPosition(1, point2.position);
        }
        else
        {
            _lineRenderer.enabled = false;
        }
    }

    public void SetPointTwo(GameObject trackObject)
    {
        point2 = trackObject.transform;
    }
}
