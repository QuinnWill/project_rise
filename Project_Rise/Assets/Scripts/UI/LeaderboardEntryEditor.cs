using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardEntryEditor : MonoBehaviour
{
    public LeaderboardEntrySO CurrentEntry;
    public FloatSO CurrentTime;

    private void OnEnable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CurrentEntry.time = CurrentTime.value;
    }

    public void SetName(string name)
    {
        CurrentEntry.playerName = name;
        PlayerPreferenceEditor.SetPlayerName(name);
    }
}
