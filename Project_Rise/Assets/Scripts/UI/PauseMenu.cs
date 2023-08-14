using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private InputHandlerSO _InputHandler;

    public CanvasGroup pauseCanvas;

    public float TimeToPause;

    public bool IsPaused;

    public UnityEvent PauseEvent;
    public UnityEvent UnpauseEvent;

    private void OnEnable()
    {
        _InputHandler.PauseEvent += HandlePauseInput;
    }

    private void OnDisable()
    {
        _InputHandler.PauseEvent -= HandlePauseInput;
    }

    public void HandlePauseInput(InputAction.CallbackContext context)
    {
        Debug.Log("Pausing");
        if (context.started && !IsPaused)
        {
            IsPaused = true;
            if (TimeToPause == 0)
            {
                pauseCanvas.alpha = 1;
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                PauseEvent?.Invoke();
                return;
            }
            StartCoroutine(DoPause(TimeToPause));
        }
        else if (context.started && IsPaused)
        {
            IsPaused = false;
            if (TimeToPause == 0)
            {
                pauseCanvas.alpha = 0;
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                UnpauseEvent?.Invoke();
                return;
            }
            StartCoroutine(StopPause(TimeToPause));
        }
    }

    public void UnPause()
    {
        if (IsPaused)
        {
            IsPaused = false;
            if (TimeToPause == 0)
            {
                pauseCanvas.alpha = 0;
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                UnpauseEvent?.Invoke();
                return;
            }
            StartCoroutine(StopPause(TimeToPause));
        }
    }

    public IEnumerator DoPause(float fadeTime)
    {
        while (pauseCanvas.alpha < 1)
        {
            pauseCanvas.alpha = Mathf.MoveTowards(pauseCanvas.alpha, 1, Time.fixedDeltaTime / fadeTime);
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, 0, Time.fixedDeltaTime / fadeTime);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            
        }
    }

    public IEnumerator StopPause(float fadeTime)
    {
        while (pauseCanvas.alpha > 0)
        {
            pauseCanvas.alpha = Mathf.MoveTowards(pauseCanvas.alpha, 0, Time.fixedDeltaTime / fadeTime);
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, 1, Time.fixedDeltaTime / fadeTime);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            
        }
    }

}
