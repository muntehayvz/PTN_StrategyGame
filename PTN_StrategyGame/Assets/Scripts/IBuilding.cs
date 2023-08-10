using System.Collections.Generic;
using UnityEngine;

public class IBuilding : MonoBehaviour
{
    [SerializeField] protected string buildingName;
    [SerializeField] protected GameObject buildImage;
    [SerializeField] protected UnityEngine.Vector2 dimensions;
    protected int healthPoints; 
    public int MaxHealthPoints { get; protected set; } 

    [SerializeField] protected HealthBar healthBar; 

    protected virtual void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>(); // Get the health bar component from child objects
        Debug.Log(healthPoints);
    }

    // Display basic information about the building
    public virtual void DisplayInfo()
    {
        Debug.Log("Building: HP - " + healthPoints);
    }

    // Calculate and return the percentage of health remaining
    public virtual float GetHealthPercent()
    {
        return (float)healthPoints / MaxHealthPoints;
    }

    // Return the current health points of the building
    public virtual int GetHealth()
    {
        return healthPoints;
    }

    // Apply damage to the building and update the health bar
    public virtual void GetDamage(int damage)
    {
        healthPoints -= damage;
        healthBar.UpdateHealthBar(healthPoints, MaxHealthPoints); // Update the UI health bar
        if (healthPoints <= 0)
        {
            healthPoints = 0;
            Destroy(gameObject); // Destroy the building if health reaches zero
        }
    }
}
