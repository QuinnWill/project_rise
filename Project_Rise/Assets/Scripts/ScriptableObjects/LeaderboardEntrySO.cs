using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LeaderboardEntry", menuName = "Player/LeaderboardEntry")]
public class LeaderboardEntrySO : ScriptableObject
{
    public string playerName;
    public float time;
}
