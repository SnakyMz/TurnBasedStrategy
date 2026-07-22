using UnityEngine;

public class GridVisual : MonoBehaviour
{
    [SerializeField] GameObject visual;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show()
    {
        visual.GetComponent<MeshRenderer>().enabled = true;
    }

    public void Hide()
    {
        visual.GetComponent<MeshRenderer>().enabled = false;
    }
}
