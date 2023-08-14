using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Slider))]
public class SensitivitySlider : MonoBehaviour
{

    private Slider _slider;

    [SerializeField] private TextMeshProUGUI _textMesh;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _slider.value = PlayerPreferenceEditor.GetSensitivity();
    }


    public void SetSensitivity()
    {
        PlayerPreferenceEditor.SetSensitivity(_slider.value);

        if (_textMesh)
        {
            _textMesh.text = string.Format("{0:0.00}", _slider.value);
        }
    }
}
