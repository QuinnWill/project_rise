using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerState", menuName = "Player/PlayerState", order = 0)]
public class PlayerStateSO : ScriptableObject
{
    [SerializeField]
    public PlayerMoveState CurrentState { get {
            if (Swinging)
            {
                return PlayerMoveState.Swinging;
            }
            else if (WallRunning)
            {
                return PlayerMoveState.WallRunning;
            }
            else if (!Grounded)
            {
                return PlayerMoveState.Airborne;
            }
            else if (Sliding)
            {
                return PlayerMoveState.Sliding;
            }
            else if (Crouching)
            {
                return PlayerMoveState.Crouching;
            }
            else
            {
                return PlayerMoveState.Grounded;
            }
            
        } }

    public bool Grounded;
    public bool Sliding;
    public bool Crouching;
    public bool Swinging;
    public bool WallRunning;
}

[System.Serializable]
public enum PlayerMoveState
{
    Airborne,
    Grounded,
    Crouching,
    Sliding,
    Swinging,
    WallRunning,
}
