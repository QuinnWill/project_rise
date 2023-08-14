using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRenderRope : MonoBehaviour
{

    private LineRenderer _LineRenderer;
    [SerializeField] private List<Vector3> _CurrRopeSegments = new List<Vector3>();
    private List<Vector3> _PrevRopeSegments = new List<Vector3>();
    private float _SegmentLength = 0.25f;
    [Min(0.01f)]
    public float ropeLength = 1;
    public int segments = 20;
    public int iterations = 100;

    public Transform StartPos;
    public Transform EndPos;

    private void Awake()
    {
        
        if (!StartPos || !EndPos)
        {
            Destroy(gameObject);
        }

        _LineRenderer = GetComponent<LineRenderer>();
        _LineRenderer.positionCount = segments;

        _SegmentLength = ropeLength / segments;

        for (int i = 0; i < segments; i++)
        {
            var startingPosition = StartPos.position - Vector3.down * i;
            _CurrRopeSegments.Add(startingPosition);
            _PrevRopeSegments.Add(startingPosition);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Simulate();
        DrawRope();
    }

    private void FixedUpdate()
    {
        _SegmentLength = ropeLength / segments;
    }

    private void CheckStartEndPos()
    {
        if (!StartPos.gameObject.activeSelf || !EndPos.gameObject.activeSelf)
        {

        }
        else { }
    }

    private void DrawRope()
    {
        _LineRenderer.SetPositions(_CurrRopeSegments.ToArray());
    }

    private void Simulate()
    {
        //Simulate
        var gravity = Vector3.down * 25f;
        for (int i = 0; i < segments; i++)
        {
            Vector3 velocity = _CurrRopeSegments[i] - _PrevRopeSegments[i];
            _PrevRopeSegments[i] = _CurrRopeSegments[i];
            _CurrRopeSegments[i] += velocity * Time.deltaTime;
            _CurrRopeSegments[i] += gravity * Time.deltaTime;
        }
        //Constrain
        for (int i = 0; i < iterations; i++)
        {
            ApplyConstraints();
        }
    }

    private void ApplyConstraints()
    {
        _CurrRopeSegments[0] = StartPos.position;
        _CurrRopeSegments[segments - 1] = EndPos.position;

        for (int i = 0; i < segments - 1; i++)
        {
            Vector3 firstPos = _CurrRopeSegments[i];
            Vector3 nextPos = _CurrRopeSegments[i + 1];

            float dist = (firstPos - nextPos).magnitude;
            float error = Mathf.Abs(dist - _SegmentLength);
            Vector3 changeDir = Vector3.zero;

            if (dist > _SegmentLength)
            {
                changeDir = (firstPos - nextPos).normalized;
            }
            else if (dist < _SegmentLength)
            {
                changeDir = (nextPos - firstPos).normalized;
            }

            changeDir *= error;

            if (i == 0)
            {
                _CurrRopeSegments[i + 1] += changeDir;
            }
            /*else if (i == segments - 1)
            {
                _CurrRopeSegments[i] -= changeDir;
            }*/
            else
            {
                _CurrRopeSegments[i] -= changeDir;
                _CurrRopeSegments[i + 1] += changeDir;
            }
        }
    }

}


