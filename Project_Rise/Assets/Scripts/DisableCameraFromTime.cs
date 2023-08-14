using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DisableCameraFromTime : MonoBehaviour
{

    [SerializeField] private MonoBehaviour _InputProvider;

    private void Awake()
    {
        if (!_InputProvider)
        {
            _InputProvider = GetComponent<CinemachineInputProvider>();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
        {
            _InputProvider.enabled = false;
        }
        else
        {
            _InputProvider.enabled = true;
        }
    }
}
