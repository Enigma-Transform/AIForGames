using UnityEngine;

public class ColorChange : MonoBehaviour
{
    private Color[] colors = { Color.red, Color.green, Color.blue };
    private int currentColorIndex = 0;
    private float timeSinceLastColorChange = 0f;
    private float colorChangeInterval = 60f; // Change color every 60 seconds
    private float colorLerpTime = 2f; // Lerp to new color over 2 seconds
    private float colorLerpTimer = 0f;
    private Color startColor;
    private Color targetColor;

    void Start()
    {
        startColor = GetComponent<Renderer>().material.color;
        targetColor = colors[currentColorIndex];
    }

    void Update()
    {
        timeSinceLastColorChange += Time.deltaTime;
        if (timeSinceLastColorChange >= colorChangeInterval)
        {
            currentColorIndex = (currentColorIndex + 1) % colors.Length;
            startColor = GetComponent<Renderer>().material.color;
            targetColor = colors[currentColorIndex];
            colorLerpTimer = 0f;
            timeSinceLastColorChange = 0f;
        }

        if (colorLerpTimer < colorLerpTime)
        {
            float t = colorLerpTimer / colorLerpTime;
            GetComponent<Renderer>().material.color = Color.Lerp(startColor, targetColor, t);
            colorLerpTimer += Time.deltaTime;
        }
    }

}
