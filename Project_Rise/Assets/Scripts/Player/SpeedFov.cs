using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CinemachineRecomposer))]
public class SpeedFov : MonoBehaviour
{
    [SerializeField] CinemachineRecomposer _recomposer;

    public Rigidbody Body;
    [Range(0.001f, 5)]
    [SerializeField] float _maxZoomChange = 0.005f;

    [SerializeField] float _baseSpeed;
    [Min(0.01f)]
    [SerializeField] float _maxSpeed;

    [SerializeField] AnimationCurve _viewChangeCurve;

    private void Awake()
    {
        if (!_recomposer)
        {
            _recomposer = GetComponent<CinemachineRecomposer>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentVelocity = Body.velocity;
        currentVelocity.y *= 0.2f;
        float targetZoom = _viewChangeCurve.Evaluate((currentVelocity.sqrMagnitude - (_baseSpeed * _baseSpeed)) / (_maxSpeed * _maxSpeed));
        _recomposer.m_ZoomScale = Mathf.Lerp(_recomposer.m_ZoomScale, targetZoom, _maxZoomChange * Time.deltaTime);
    }
}
