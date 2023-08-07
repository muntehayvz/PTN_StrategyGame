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
            Debug.Log("Can attack: " + canAttack);
            Debug.Log("attack: " + AttackDamage);
            Attack();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (canAttack && collision.gameObject.CompareTag("Building") && Input.GetMouseButtonDown(1))
        {
            Debug.Log("Attacking again ");
            Attack();
        }

    }

    private void Attack()
    {
        // Implement attack behavior
        // For example:
        Debug.Log("Attacking with damage: " + soldierData.AttackDamage);
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
