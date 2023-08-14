using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public InputHandlerSO InputHandler;
    public bool LevelInProgress;

    [Min(0)]
    public float EndLevelTime;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        InputHandler.EnableAll();
    }

    private void OnDisable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
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

        if (EndLevelTime == 0)
        {
            StartCoroutine(StopGame());
        }

    }

    public IEnumerator StopGame()
    {
        yield return new WaitForSeconds(Time.fixedDeltaTime);
        Time.timeScale = Mathf.MoveTowards(Time.timeScale, 0, EndLevelTime / Time.fixedDeltaTime);
    }
}
