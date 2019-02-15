using TMPro;
using UnityEngine;

public class ScaleText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scaleText;

    private IInput input;

    public void Init(IInput input)
    {
        this.input = input;
        input.OnScaleChanged += OnScaleChanged;
    }

    public void OnDestroy()
    {
        input.OnScaleChanged -= OnScaleChanged;
    }

    public void OnScaleChanged(int scale)
    {
        scaleText.text = $"Scale (N): {scale}";
    }
}
