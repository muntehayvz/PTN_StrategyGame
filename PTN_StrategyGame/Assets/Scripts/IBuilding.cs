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
        healthBar = GetComponentInChildren<HealthBar>();
        Debug.Log(healthPoints);
    }

    public virtual void DisplayInfo()
    {
        Debug.Log("Building: HP - " + healthPoints);
    }

    public virtual float GetHealthPercent()
    {
        return (float)healthPoints / MaxHealthPoints;
    }

    public virtual int GetHealth()
    {
        return healthPoints;
    }

    public virtual void GetDamage(int damage)
    {
        healthPoints -= damage;
        healthBar.UpdateHealthBar(healthPoints, MaxHealthPoints);
        if (healthPoints <= 0)
        {
            healthPoints = 0;
            Destroy(gameObject);
        }
    }
}