using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DisplayTime : MonoBehaviour
{

    private TextMeshProUGUI DisplayText;
    public FloatSO LevelTime;

    // Start is called before the first frame update
    void Awake()
    {
        DisplayText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        var minutes = Mathf.Min(99, (int)(LevelTime.value / 60));
        var seconds = (int)(LevelTime.value % 60);
        var miliseconds = (int)((LevelTime.value % 1) * 100);

        DisplayText.text = string.Format("{0, 0:D2}:{1, 0:D2}.{2, 0:D2}", minutes, seconds, miliseconds);
    }
}
