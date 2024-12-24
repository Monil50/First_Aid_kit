using UnityEngine;
using UnityEngine.UI;

public class ObjectScaler : MonoBehaviour
{
    public GameObject pipeModel;
    public Slider scaleSlider;

    private Vector3 initialScale;

    void Start()
    {
        initialScale = pipeModel.transform.localScale;
    }

    void Update()
    {
        // Scale the pipe uniformly based on the slider value
        pipeModel.transform.localScale = initialScale * scaleSlider.value;
    }
}