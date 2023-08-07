using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider healthSlider;
/*    private void Start()
    {
        healthSlider = this.gameObject.GetComponentInParent<Slider>();
    }*/

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        healthSlider.value = currentValue / maxValue;
    }
}
