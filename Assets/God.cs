using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class God : MonoBehaviour
{
    public float currentTime = 0f;
    public float timeScale = 1000f;
    public bool systemRun = true;
    public float rotatedAngle = 0f;
    public InputField GPSTimeInputField;
    public Text GPSTimePlaceholder;
    public Button TimeScaleButton;
    public Text TimeScaleButtonText;
    public Light Solar;
    private int[] timeScaleSelection = new int[] {1, 10, 100, 1000, 10000, 100000};
    private int currentTimeScaleSelection = 3;
    public float GetCurrentTime()
    {
        return currentTime;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AddTime());
        GPSTimeInputField.onEndEdit.AddListener(delegate { ChangeGPSTime(GPSTimeInputField); });
        TimeScaleButton.onClick.AddListener(delegate { ChangeTimeScale(); });
    }

    IEnumerator AddTime()
    {
        while(systemRun)
        {
            currentTime += timeScale * Time.deltaTime;
            GPSTimePlaceholder.text = currentTime.ToString("e");
            //transform.Rotate(0, -Mathf.Rad2Deg * Coordinate.earthRotationAngularVelocity * Time.deltaTime * timeScale, 0);
            //Quaternion rotation = new Quaternion(0, Coordinate.AngleNormalize(currentTime / (2 * Mathf.PI / Coordinate.earthRotationAngularVelocity), 0, 2 * Mathf.PI) * Mathf.Rad2Deg, 0, 1);
            transform.rotation = Quaternion.Euler(0, -Coordinate.AngleNormalize(currentTime * Coordinate.earthRotationAngularVelocity, 0, 2 * Mathf.PI) * Mathf.Rad2Deg, 0);
            rotatedAngle = Coordinate.AngleNormalize(currentTime * Coordinate.earthRotationAngularVelocity, 0, 2 * Mathf.PI) * Mathf.Rad2Deg;
            yield return null;
        }
    }
    public void ChangeGPSTime(InputField inputField)
    {
        currentTime = float.Parse(inputField.text);
        inputField.text = "";
    }
    public void ChangeTimeScale()
    {
        ++currentTimeScaleSelection;
        if (currentTimeScaleSelection >= timeScaleSelection.Length)
        {
            currentTimeScaleSelection = 0;
        }
        timeScale = timeScaleSelection[currentTimeScaleSelection];
        TimeScaleButtonText.text = "x" + timeScale.ToString();
    }
}
