using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CrouchCamEffector : MonoBehaviour
{

    public PlayerStateSO PlayerState;

    public float range;

    public float duration;

    private bool wasCrouching;

    // Update is called once per frame
    void Update()
    {
        if (PlayerState.Crouching && !wasCrouching)
        {
            Debug.Log("Crouching!");
            transform.DOLocalMoveY(-range, duration).SetEase(Ease.OutFlash);
        }
        else if(wasCrouching && !PlayerState.Crouching)
        {
            Debug.Log("UnCrouching!");
            transform.DOLocalMoveY(0, duration).SetEase(Ease.OutFlash);
        }

        wasCrouching = PlayerState.Crouching;
    }
}
