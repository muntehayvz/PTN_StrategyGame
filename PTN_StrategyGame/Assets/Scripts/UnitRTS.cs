using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRTS : MonoBehaviour
{
    private GameObject selectedGameObject;
    private AIDestinationSetter movePosition;
    private CharacterFlip characterFlip;
    IAstarAI ai;

    private void Awake()
    {
        ai = GetComponent<IAstarAI>(); 
        selectedGameObject = transform.Find("Selected").gameObject;
        movePosition = GetComponent<AIDestinationSetter>();
        SetSelectedVisible(false);

        characterFlip = this.gameObject.GetComponentInChildren<CharacterFlip>();
        FlipCharacter(false);
    }

    public void FlipCharacter(bool visible)
    {
        characterFlip.enabled = visible;
    }

    public void SetSelectedVisible(bool visible)
    {
        selectedGameObject.SetActive(visible);
    }

    public void MoveTo(Transform targetPosition) // Hedef konumu Vector3 olarak alalım
    {
        movePosition.SetMovePosition(targetPosition);
    }

    public bool IsArrived()
    {
        if (ai != null)
        {
            if (ai.reachedEndOfPath && !ai.reachedDestination)
            {
                return true;
            }
        }
        return false;
    }
}
