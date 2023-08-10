using System.Collections;
using UnityEngine;

[System.Serializable]
public class Building : MonoBehaviour
{
    public bool Placed { get; private set; }
    public BoundsInt area;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] ParticleSystem placementParticleEffect;

    #region Build Methods

    void Start()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = true;
        }
    }

    // Check if the building can be placed in its current position on the grid
    public bool CanBePlaced()
    {
        Vector3Int positionInt = GridBuildingSystem.instance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        if (GridBuildingSystem.instance.CanTakeArea(areaTemp))
        {
            return true;
        }
        return false;
    }

    // Place the building on the grid
    public void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.instance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        Placed = true;
        GridBuildingSystem.instance.TakeArea(areaTemp); // Occupy grid cells in the area
        AstarPath.active.Scan(); // Update pathfinding graph
        if (audioSource != null)
        {
            audioSource.Play(); // Play placement sound
        }
        TriggerPlacementParticleEffect(); // Trigger placement particle effect
    }

    // Trigger the particle effect for building placement
    private void TriggerPlacementParticleEffect()
    {
        if (placementParticleEffect != null)
        {
            ParticleSystem particleSystem = placementParticleEffect.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Play();
                StartCoroutine(DestroyParticleEffectAfterPlay(particleSystem));
            }
        }
    }

    // Destroy the particle effect after it has finished playing
    private IEnumerator DestroyParticleEffectAfterPlay(ParticleSystem particleSystem)
    {
        yield return new WaitForSeconds(2f);

        particleSystem.Stop();
        Destroy(particleSystem.gameObject);
    }

    // Clear grid area when the building is destroyed
    private void OnDestroy()
    {
        if (Placed && GridBuildingSystem.instance != null)
        {
            GridBuildingSystem.instance.ClearTilemapArea(area);
        }
    }
    #endregion
}
