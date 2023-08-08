using Pathfinding;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SoldierController : MonoBehaviour
{
    private Soldier soldierData;
    private bool canAttack = false;
    private int HealthPoints;
    private int AttackDamage;
    private Barracks targetBarracks;
    private PowerPlant targetPowerPlant;
    [SerializeField] HealthBar healthBar;
    private float deathDelay = 1.0f;
    [SerializeField] private Animator anim;

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


    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
    }

    private void Update()
    {
        if (canAttack && Input.GetMouseButtonDown(1)) // Sadece sağ tıklama ile saldırı
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                SoldierController targetSoldier = hit.collider.GetComponent<SoldierController>();
                if (targetSoldier != null && targetSoldier != this)
                {
                    // Calculate distance between soldiers
                    float distance = Vector2.Distance(transform.position, targetSoldier.transform.position);

                    // Check if the distance is within attack range
                    if (distance <= 0.4f)
                    {
                        anim.SetBool("isShooting", true);
                        StartCoroutine(ResetShootingAnimation());

                        // Inflict damage on the target soldier
                        targetSoldier.TakeDamage(AttackDamage);

                    }
                    else
                    {
                        Debug.Log("Target is too far for attack.");
                    }
                }
            }
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

/*    private void OnCollisionStay2D(Collision2D collision)
    {
        if (canAttack && collision.gameObject.CompareTag("Building") && Input.GetMouseButtonDown(1))
        {
            Attack();
        }
    }*/

    private void Attack()
    {
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

        Debug.Log("taking damage: "+ soldierData.HealthPoints);
        HealthPoints -= damage;
        healthBar.UpdateHealthBar(HealthPoints, soldierData.HealthPoints);

        if (HealthPoints <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Soldier died.");
        anim.SetBool("isDead", true);
        StartCoroutine(DeathAnimation());
    }

    public float GetHealthPercent()
    {
        return (float)HealthPoints / this.soldierData.HealthPoints;
    }

    private IEnumerator ResetShootingAnimation()
    {
        // Wait for a short delay
        yield return new WaitForSeconds(0.3f); // Change the delay as needed

        // Reset the shooting animation to false
        anim.SetBool("isShooting", false);
    }

    private IEnumerator DeathAnimation()
    {
        yield return new WaitForSeconds(deathDelay);

        Destroy(gameObject);
    }

}
