using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberDisplayBehaviour : MonoBehaviour
{

    [Header("Animation")]
    public Animation numberAnimation;

    [Header("Display")]
    public TextMeshPro textDisplay;

    public void SetupDisplay(int newNumber, Vector3 newPosition, Color newColor)
    {
        UpdateNumber(newNumber);
        UpdateColor(newColor);
        UpdatePosition(newPosition);
        PlaySequence();
    }    

    void UpdateNumber(int numberValue)
    {
        textDisplay.SetText(numberValue.ToString());
    }

    void UpdateColor(Color color)
    {
        textDisplay.color = color;
    }

    void UpdatePosition(Vector3 position)
    {
        transform.SetPositionAndRotation(position, transform.rotation);
    }

    void PlaySequence()
    {
        numberAnimation.Play();
    }

    void RemoveNumber()
    {
        gameObject.SetActive(false);
    }

}
