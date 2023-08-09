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

    public void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.instance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        Placed = true;
        GridBuildingSystem.instance.TakeArea(areaTemp);
        AstarPath.active.Scan();
        if (audioSource != null)
        {
            audioSource.Play();
        }
        TriggerPlacementParticleEffect();
    }

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
    private IEnumerator DestroyParticleEffectAfterPlay(ParticleSystem particleSystem)
    {
        yield return new WaitForSeconds(2f);

        particleSystem.Stop();
        Destroy(particleSystem.gameObject);
    }

    private void OnDestroy()
    {
        if (Placed && GridBuildingSystem.instance != null)
        {
            GridBuildingSystem.instance.ClearTilemapArea(area);
        }
    }

    #endregion

}
