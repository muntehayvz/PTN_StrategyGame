using UnityEngine;


public class Barracks : MonoBehaviour, IBuilding
{
    private int healthPoints = 100;
    public int MaxHealthPoints => 100;
    public int damageAmount => 100;

    [SerializeField] HealthBar healthBar;

    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
    }

    public void DisplayInfo()
    {
        Debug.Log("Barracks: HP - " + healthPoints);
    }

    public float GetHealthPercent()
    {
        return (float)healthPoints / MaxHealthPoints;
    }

    public int GetHealth()
    {
        return healthPoints;
    }

    public void GetDamage(int damage)
    {
        healthPoints -= damage;
        healthBar.UpdateHealthBar(healthPoints, MaxHealthPoints);
        if(healthPoints < 0) healthPoints = 0;
    }
}
