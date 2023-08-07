using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Barracks : MonoBehaviour, IBuilding
{
    public int HealthPoints => 100;

    public void DisplayInfo()
    {
        Debug.Log("Barracks: HP - " + HealthPoints);
    }
}