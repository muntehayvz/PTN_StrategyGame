using UnityEngine;

public class PowerPlant : MonoBehaviour, IBuilding
{
    public int HealthPoints => 50;

    public void DisplayInfo()
    {
        Debug.Log("PowerPlant: HP - " + HealthPoints);
    }
}