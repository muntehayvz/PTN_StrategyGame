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
        // Get the SoldierController instance if not assigned
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoldierController>();
            }
            return instance;
        }
    }

    // Initialize the soldier with its type's attributes
    public void Initialize(Soldier soldierType)
    {
        soldierData = soldierType;

        HealthPoints = soldierData.HealthPoints;
        AttackDamage = soldierData.AttackDamage;
    }

    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>(); // Get the health bar component from child objects
        canAttack = false; // Disable attack by default
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Perform attack only on right mouse click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Building") && GameRTSController.Instance.selectedUnitRTSList.Contains(this.GetComponent<UnitRTS>()))
                {
                    float distance = Vector2.Distance(this.transform.position, hit.collider.transform.position);

                    if (distance <= 0.6f)
                    {
                        Barracks targetBarracks = hit.collider.GetComponent<Barracks>();
                        PowerPlant targetPowerPlant = hit.collider.GetComponent<PowerPlant>();

                        SetTargetBarracks(targetBarracks);
                        SetTargetPowerPlant(targetPowerPlant);
                        Attack(); // Initiate the attack
                    }
                }
                if (canAttack)
                {
                    SoldierController targetSoldier = hit.collider.GetComponent<SoldierController>();
                    if (targetSoldier != null && targetSoldier != this)
                    {
                        // Calculate distance between soldiers
                        float distance = Vector2.Distance(this.transform.position, targetSoldier.transform.position);

                        // Check if the distance is within attack range
                        if (distance <= 0.4f)
                        {
                            anim.SetBool("isShooting", true); // Play shooting animation
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
    }

    // Set the target barracks for attack
    public void SetTargetBarracks(Barracks barracks)
    {
        targetBarracks = barracks;
    }

    // Set the target power plant for attack
    public void SetTargetPowerPlant(PowerPlant powerPlant)
    {
        targetPowerPlant = powerPlant;
    }

    // Enable the soldier's ability to attack
    public void EnableAttack()
    {
        canAttack = true;
    }

    // Disable the soldier's ability to attack
    public void DisableAttack()
    {
        canAttack = false;
    }

    // Perform the attack on the targeted buildings
    private void Attack()
    {
        Debug.Log("Attacking with damage: " + soldierData.AttackDamage);
        if (targetBarracks != null)
        {
            targetBarracks.GetDamage(AttackDamage); // Inflict damage on the targeted barracks
            Debug.Log("Barrack health: " + targetBarracks.GetHealth());
        }
        if (targetPowerPlant != null)
        {
            targetPowerPlant.GetDamage(AttackDamage); // Inflict damage on the targeted power plant
            Debug.Log("Power plant health: " + targetPowerPlant.GetHealth());
        }
    }

    // Inflict damage on the soldier and update health bar
    public void TakeDamage(int damage)
    {
        Debug.Log("Taking damage: " + soldierData.HealthPoints);
        HealthPoints -= damage;
        healthBar.UpdateHealthBar(HealthPoints, soldierData.HealthPoints);

        if (HealthPoints <= 0)
        {
            Die(); // Initiate the death process
        }
    }

    // Perform death actions and destroy the soldier
    private void Die()
    {
        Debug.Log("Soldier died.");
        anim.SetBool("isDead", true);
        StartCoroutine(DeathAnimation());
    }

    // Calculate and return the percentage of health remaining
    public float GetHealthPercent()
    {
        return (float)HealthPoints / this.soldierData.HealthPoints;
    }

    // Reset the shooting animation after a delay
    private IEnumerator ResetShootingAnimation()
    {
        // Wait for a short delay
        yield return new WaitForSeconds(0.3f); // Change the delay as needed

        // Reset the shooting animation to false
        anim.SetBool("isShooting", false);
    }

    // Initiate the death animation and destroy the soldier
    private IEnumerator DeathAnimation()
    {
        yield return new WaitForSeconds(deathDelay);

        Destroy(gameObject); // Destroy the soldier GameObject
    }
}