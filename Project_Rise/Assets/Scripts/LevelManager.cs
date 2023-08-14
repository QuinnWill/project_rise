using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;
    public InputHandlerSO InputHandler;

    public float TimeStopTime;

    public bool LevelInProgress;

    public UnityEvent LevelStarted;
    public UnityEvent LevelEnded;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        LevelInProgress = true;
    }

    private void OnEnable()
    {
        InputHandler.EnableAll();
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        LevelStarted?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndLevel()
    {
        if (!LevelInProgress)
        {
            return;
        }

        LevelInProgress = false;

        InputHandler.DisablePersistent();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(LerpTimeScale(0, TimeStopTime));
        LevelEnded?.Invoke();
    }

    private IEnumerator LerpTimeScale(float goalTime, float lerpTime)
    {
        while (Time.timeScale != goalTime)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, goalTime, Time.fixedDeltaTime / lerpTime);
        }
        Time.timeScale = goalTime;
    }
}
