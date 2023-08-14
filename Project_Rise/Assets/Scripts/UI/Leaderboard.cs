using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour
{
    public StringArraySO LeaderboardIDs;

    public List<LeaderBoardEntry> UIEntries;
    public List<BoardEntry> Entries;

    public LeaderboardEntrySO CurrentEntry;

    private void OnEnable()
    {
        StartCoroutine(StartSession());
        
    }
    private void OnDisable()
    {
        EndSessionImmediate();
    }

    private void UpdateUI()
    {
        if (Entries.Count == 0)
        {
            UIEntries[0].OverrideEntry = CurrentEntry;
            return;
        }

        
        for (int i = 0; i < Entries.Count; i++)
        {
            UIEntries[i].UpdateFields(Entries[i].name, Entries[i].time);
        }

        string tempName = "";
        float tempTime = 0;
        for (int i = 0; i < UIEntries.Count; i++)
        {
            if (tempName == "" && UIEntries[i].RecordedTime > CurrentEntry.time)
            {
                tempTime = UIEntries[i].RecordedTime;
                tempName = UIEntries[i].PlayerName;
                UIEntries[i].OverrideEntry = CurrentEntry;
            }
            else if (tempName != "")
            {
                var time = UIEntries[i].RecordedTime;
                var name = UIEntries[i].PlayerName;
                UIEntries[i].UpdateFields(tempName, tempTime);
                tempTime = time;
                tempName = name;
            }
            else if (i == Entries.Count)
            {
                UIEntries[i].OverrideEntry = CurrentEntry;
            }
        }

    }

    private IEnumerator LoginRoutine()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Player Was Logged in");
                done = true;
            }
            else
            {
                Debug.LogError("Error Logging in player" + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public void UpdateScores()
    {
        StartCoroutine(GetScoresRoutine(SceneManager.GetActiveScene().name));
    }

    private IEnumerator GetScoresRoutine(string levelName)
    {
        bool done = false;
        LootLockerSDKManager.GetScoreList(LeaderboardIDs.values[SceneManager.GetActiveScene().buildIndex - 2], 10, (response) =>
        {
            if (response.success)
            {
                Entries = new List<BoardEntry>();
                LootLockerLeaderboardMember[] members = response.items;
                for (int i = 0; i < members.Length; i++)
                {
                    Debug.Log(i);
                    Entries.Add(new BoardEntry(members[i].member_id, ((float)members[i].score) / 100));
                }
                done = true;
            }
            else
            {
                Debug.LogError("Couldn't retrieve scores" + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }


    private IEnumerator StartSession()
    {
        yield return LoginRoutine();

        yield return GetScoresRoutine(SceneManager.GetActiveScene().name);

        UpdateUI();
    }

    private void EndSessionImmediate()
    {
        LootLockerSDKManager.EndSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Player Was Logged in");
            }
            else
            {
                Debug.LogError("Error Logging in player" + response.Error);
            }
        });
    }

    private IEnumerator EndSession()
    {
        bool done = false;
        LootLockerSDKManager.EndSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Player Was Logged in");
                done = true;
            }
            else
            {
                Debug.LogError("Error Logging in player" + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public void SubmitCurrentScore()
    {
        if (CurrentEntry.playerName != "")
        {
            StartCoroutine(SubmitScoreRoutine());
            return;
        }
    }

    private IEnumerator SubmitScoreRoutine()
    {
        bool done = false;
        LootLockerSDKManager.SubmitScore(CurrentEntry.playerName, (int)(CurrentEntry.time * 100), LeaderboardIDs.values[SceneManager.GetActiveScene().buildIndex - 2], SceneManager.GetActiveScene().name, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Uploaded Score");
                done = true;
            }
            else
            {
                Debug.LogError("Failed" + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);

    }
}
[System.Serializable]
public class BoardEntry
{
    public string name;
    public float time;

    public BoardEntry(string _name, float _time)
    {
        name = _name;
        time = _time;
    }
}
