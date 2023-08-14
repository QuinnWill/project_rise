using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "ButtonFunctions", menuName = "UI/ButtonFunctions")]
public class ButtonFunctions : ScriptableObject
{

    public void MoveObjectRight(Transform other)
    {
        other.DOMoveX(other.position.x + 300, 1).SetEase(Ease.OutBounce);
    }

    public void MoveObjectLeft(Transform other)
    {
        other.DOMoveX(other.position.x - 300, 1).SetEase(Ease.OutBounce);
    }

    public void LoadScene(Scene scene)
    {
        SceneManager.LoadScene(scene.name);
    }

    public void MoveScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToNextScene()
    {
        var sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (sceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            sceneIndex = 0;
        }
        SceneManager.LoadScene(sceneIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
