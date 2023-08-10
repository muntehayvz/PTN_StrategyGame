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

    public AudioSource soldierClickSound;

    private void Awake()
    {
        ai = GetComponent<IAstarAI>(); 
        selectedGameObject = transform.Find("Selected").gameObject;
        movePosition = GetComponent<AIDestinationSetter>();
        SetSelectedVisible(false);

        characterFlip = this.gameObject.GetComponentInChildren<CharacterFlip>();
        FlipCharacter(false);
    }

    // Enable or disable character flipping based on selected units
    public void FlipCharacter(bool selected)
    {
        if (selectedGameObject != null)
        {
            characterFlip.enabled = selected;
        }
    }

    // Set the visibility of the selected game object indicator
    public void SetSelectedVisible(bool visible)
    {
        if (selectedGameObject != null)
        {
            selectedGameObject.SetActive(visible);
            if(visible)
            {
                PlaySoldierClickSound();
            }
        }
    }
    public void PlaySoldierClickSound()
    {
        soldierClickSound.Play();
    }
    // Move the unit to the specified target position
    public void MoveTo(Transform targetPosition)
    {
        if (movePosition != null)
        {
            movePosition.SetMovePosition(targetPosition); // Set the move position for pathfinding
        }
    }

    // Check if the unit has arrived at its destination
    public bool IsArrived()
    {
        if (ai != null)
        {
            if (ai.reachedEndOfPath && !ai.reachedDestination)
            {
                return true; // Return true if the unit has reached its path end but not its final destination
            }
        }
        return false; // Return false if the unit has not yet arrived
    }
}
