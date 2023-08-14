using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderBoardEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private TextMeshProUGUI _time;

    public string PlayerName;
    public float RecordedTime;

    public LeaderboardEntrySO OverrideEntry;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OverrideEntry)
        {
            if (OverrideEntry.playerName == "")
            {
                UpdateFields("----", OverrideEntry.time);
            }
            else
            {
                UpdateFields(OverrideEntry.playerName, OverrideEntry.time);
            }
        }
    }

    private string TimeToString(float time)
    {
        var minutes = Mathf.Min(99, (int)(time / 60));
        var seconds = (int)(time % 60);
        var miliseconds = (int)((time % 1) * 100);

        return string.Format("{0, 0:D2}:{1, 0:D2}.{2, 0:D2}", minutes, seconds, miliseconds);
    }

    public void UpdateFields(string newName, float newTime)
    {
        PlayerName = newName;
        RecordedTime = newTime;
        _playerName.text = newName;
        _time.text = TimeToString(newTime);
    }
}
