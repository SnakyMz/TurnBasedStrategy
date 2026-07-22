using UnityEngine;

public class GridVisual : MonoBehaviour
{
    [SerializeField] MeshRenderer visual;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show(Material material)
    {
        visual.enabled = true;
        visual.material = material;
    }

    public void Hide()
    {
        visual.enabled = false;
    }
}
