using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider healthSlider;

    // Update the health bar with the current and maximum health values
    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        healthSlider.value = currentValue / maxValue; // Set the slider value to represent the health percentage
    }
}
