using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EntryInputDefault : MonoBehaviour
{

    [SerializeField] private TMP_InputField _InputField;

    // Start is called before the first frame update
    void OnEnable()
    {
        _InputField.text = PlayerPreferenceEditor.GetPlayerName();
    }
}
