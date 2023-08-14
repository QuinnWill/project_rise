using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    public FloatSO TimeObject;

    public bool IsCounting;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsCounting)
        {
            TimeObject.value += Time.deltaTime;
        }
    }

    public void StartTimer()
    {
        IsCounting = true;
    }

    public void StopTimer()
    {
        IsCounting = false;
    }

    public void RestartTimer()
    {
        TimeObject.value = 0;
    }
}
