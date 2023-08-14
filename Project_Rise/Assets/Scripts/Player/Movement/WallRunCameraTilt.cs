using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CinemachineRecomposer))]
public class WallRunCameraTilt : MonoBehaviour
{

    public PlayerController3D PlayerController;
    public CinemachineRecomposer recomposer;
    [SerializeField] PlayerStateSO _playerState;
    [Header("Lean Settings")]
    [Range(0,180)]
    public float maxLean;
    public float leanSpeed;

    private float _currentLean;


    private void Awake()
    {
        if (!recomposer)
        {
            recomposer = GetComponent<CinemachineRecomposer>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerState.WallRunning)
        {
            var wallForward = Vector3.Cross(Vector3.up, Vector3.ProjectOnPlane(PlayerController.WallNormal, Vector3.up).normalized);
            var horizontalCamLook = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
            var wallDot = Vector3.Dot(wallForward, horizontalCamLook);
            _currentLean = Mathf.Lerp(_currentLean, wallDot * maxLean, leanSpeed * Time.deltaTime);

        }
        else
        {
            _currentLean = Mathf.Lerp(_currentLean, 0, leanSpeed * Time.deltaTime);
        }
        recomposer.m_Dutch = _currentLean;
    }
}
