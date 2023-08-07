using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SoldierController : MonoBehaviour
{
    private Soldier soldierData;
    private bool canAttack = false;
    private int HealthPoints;
    private int AttackDamage;
    private Barracks targetBarracks; // Add this line
    private PowerPlant targetPowerPlant; // Add this line


    private static SoldierController instance;
    public static SoldierController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoldierController>();
            }
            return instance;
        }
    }
    public void SetTargetBarracks(Barracks barracks)
    {
        targetBarracks = barracks;
    }
    public void SetTargetPowerPlant(PowerPlant powerPlant)
    {
        targetPowerPlant = powerPlant;
    }
    public void Initialize(Soldier soldierType)
    {
        soldierData = soldierType;

        HealthPoints = soldierData.HealthPoints;
        AttackDamage = soldierData.AttackDamage;
    }

    public void enableAttack()
    {
        canAttack = true;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canAttack && collision.gameObject.CompareTag("Building"))
        {
            Barracks targetBarracks = collision.gameObject.GetComponent<Barracks>();
            PowerPlant targetPowerPlant = collision.gameObject.GetComponent<PowerPlant>();

            Debug.Log("Attacking again ");
            SetTargetBarracks(targetBarracks);
            SetTargetPowerPlant(targetPowerPlant);
            Attack();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (canAttack && collision.gameObject.CompareTag("Building") && Input.GetMouseButtonDown(1))
        {
            Attack();
        }

    }

    private void Attack()
    {
        // Implement attack behavior
        // For example:
        Debug.Log("Attacking with damage: " + soldierData.AttackDamage);
        if (targetBarracks != null)
        {
            targetBarracks.GetDamage(AttackDamage);
            Debug.Log("barrack health: "+targetBarracks.GetHealth());
        }
        if (targetPowerPlant != null)
        {
            targetPowerPlant.GetDamage(AttackDamage);
            Debug.Log("powerplant health: " + targetPowerPlant.GetHealth());
        }
    }


    public void TakeDamage(int damage)
    {
        // Implement damage-taking logic
        // For example:
        // HealthPoints -= damage;
        // if (HealthPoints <= 0)
        // {
        //     Die();
        // }
    }

    private void Die()
    {
        // Implement death behavior
        // For example:
        Debug.Log("Soldier died.");
        Destroy(gameObject);
    }
}
