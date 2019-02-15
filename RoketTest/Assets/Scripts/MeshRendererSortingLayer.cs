using UnityEngine;
using System.Collections;

public class MeshRendererSortingLayer : MonoBehaviour
{
    [SerializeField]
    private Renderer renderer;

    [SerializeField]
    private string sortingLayerName;

    [SerializeField]
    private int sortingOrder;

    public void Start()
    {
        renderer.sortingLayerName = sortingLayerName;
        renderer.sortingOrder = sortingOrder;
    }
}
