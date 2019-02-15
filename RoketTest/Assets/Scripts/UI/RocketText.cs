using UnityEngine;
using TMPro;

public class RocketText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rocketRatingText;

    public void Init(int rocketRating)
    {
        rocketRatingText.text = $"Rocket rating: {rocketRating}";
    }
}
