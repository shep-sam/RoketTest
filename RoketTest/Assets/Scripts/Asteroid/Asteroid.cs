using UnityEngine;
using TMPro;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro ratingText;

    public void SetData(Slot slot)
    {
        transform.position = new Vector3(slot.Position.x, slot.Position.y, 0);
        ratingText.text = slot.Rating.ToString();
    }
}
