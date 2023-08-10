using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Soldier
{
    public int HealthPoints { get; private set; }
    public int AttackDamage { get; private set; }

    // Constructor to initialize the soldier with specific health points and attack damage
    public Soldier(int healthPoints, int attackDamage)
    {
        HealthPoints = healthPoints;
        AttackDamage = attackDamage;    
    }
}
