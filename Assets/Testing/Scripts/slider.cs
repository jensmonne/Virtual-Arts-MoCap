using AscentProtocol.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class SliderFOVController : MonoBehaviour
{
    [Header("References")]
    private Camera targetCamera;
    public Slider fovSlider;

    [Header("FOV Settings")]
    public float minFOV = 10f; 
    public float maxFOV = 170f; 

    private void Start()
    {
        targetCamera = SceneObjectRegistry.Instance.Get("Camera").gameObject.GetComponent<Camera>();

        if (targetCamera == null)
        {
            Debug.LogWarning("No Camera found in SceneObjectRegistry.");
        }

        // Assign slider range
        fovSlider.minValue = minFOV;
        fovSlider.maxValue = maxFOV;

        // Set initial value
        fovSlider.value = targetCamera.fieldOfView;

        fovSlider.onValueChanged.AddListener(UpdateFOV);
    }

    public void UpdateFOV(float value)
    {
        targetCamera.fieldOfView = value;
    }
}