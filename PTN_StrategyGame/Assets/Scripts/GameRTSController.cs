using CodeMonkey.Utils;
using Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;
using static UnityEngine.GraphicsBuffer;

public class GameRTSController : MonoBehaviour
{
    [SerializeField] private Transform selectionAreaTranform; 
    private Vector3 startPosition; 
    public List<UnitRTS> selectedUnitRTSList;
    private float gridInterval = 0.3f; // Interval for grid positioning

    private static GameRTSController instance; // Singleton instance
    public static GameRTSController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameRTSController>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        selectedUnitRTSList = new List<UnitRTS>(); // Initialize the list of selected units
        selectionAreaTranform.gameObject.SetActive(false); // Hide the selection area
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectionAreaTranform.gameObject.SetActive(true); // Show the selection area

            startPosition = UtilsClass.GetMouseWorldPosition(); // Get the starting mouse position
        }

        if (Input.GetMouseButton(0))
        {
            // Update the selection area's position and scale based on mouse movement
            Vector3 currentMousePosition = UtilsClass.GetMouseWorldPosition();
            Vector3 lowerLeft = new Vector3(
                Mathf.Min(startPosition.x, currentMousePosition.x),
                Mathf.Min(startPosition.y, currentMousePosition.y)
            );
            Vector3 upperRight = new Vector3(
                Mathf.Max(startPosition.x, currentMousePosition.x),
                Mathf.Max(startPosition.y, currentMousePosition.y)
            );
            selectionAreaTranform.position = lowerLeft;
            selectionAreaTranform.localScale = upperRight - lowerLeft;
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectionAreaTranform.gameObject.SetActive(false); // Hide the selection area

            Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(startPosition, UtilsClass.GetMouseWorldPosition());

            // Deselect all Units
            foreach (UnitRTS unitRTS in selectedUnitRTSList)
            {
                unitRTS.SetSelectedVisible(false);
                unitRTS.FlipCharacter(false);
            }

            selectedUnitRTSList.Clear(); // Clear the list of selected units

            // Select Units within Selection Area
            foreach (Collider2D collider2D in collider2DArray)
            {
                UnitRTS unitRTS = collider2D.GetComponent<UnitRTS>();
                if (unitRTS != null)
                {
                    unitRTS.SetSelectedVisible(true);
                    selectedUnitRTSList.Add(unitRTS);
                    unitRTS.FlipCharacter(true);
                }
            }
        }

        // Handle movement and actions upon right-click
        if (Input.GetMouseButtonDown(1)) // Right Mouse Button Pressed
        {
            // Handle unit movement and action based on right-click
            HandleUnitMovementAndAction();
        }
    }

    // Calculate positions around a target position with a specified distance and count
    private List<Transform> GetPositionListAround(Transform startPosition, float distance, int positionCount)
    {
        List<Transform> positionList = new List<Transform>();
        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360f / positionCount);
            Vector3 dir = ApplyRotationToVector(new Vector3(1, 0), angle);
            Vector3 position = startPosition.position + dir * (distance * 0.6f);

            GameObject emptyObject = new GameObject();
            emptyObject.transform.position = position;
            positionList.Add(emptyObject.transform);
            Destroy(emptyObject);
        }
        return positionList;
    }

    // Apply rotation to a vector
    private Vector3 ApplyRotationToVector(Vector3 vec, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * vec;
    }

    // Handle unit movement and actions based on right-click
    private void HandleUnitMovementAndAction()
    {
        // Get the mouse position in world space
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            Transform newTarget = hit.collider.transform; // Get the clicked target's Transform

            // Handle movement and action for multiple selected units
            if (selectedUnitRTSList.Count > 1)
            {
                List<Transform> targetTransformList = GetPositionListAround(newTarget, gridInterval, selectedUnitRTSList.Count);
                int targetposindex = 0;

                foreach (UnitRTS unitRTS in selectedUnitRTSList)
                {
                    if (unitRTS != null)
                    {
                        unitRTS.MoveTo(targetTransformList[targetposindex]);
                        targetposindex = (targetposindex + 1) % targetTransformList.Count;
                        SoldierController soldierController = unitRTS.GetComponent<SoldierController>();

                        // Enable or disable attack based on the target
                        if (soldierController != null)
                        {
                            if (hit.collider.CompareTag("Soldier"))
                            {
                                soldierController.EnableAttack();
                            }
                            else
                            {
                                soldierController.DisableAttack();
                            }
                        }
                    }
                }
            }
            else // Handle movement and action for a single selected unit
            {
                if (selectedUnitRTSList.Count == 1)
                {
                    UnitRTS unitRTS = selectedUnitRTSList[0];
                    if (unitRTS != null)
                    {
                        unitRTS.MoveTo(newTarget);
                        SoldierController soldierController = unitRTS.GetComponent<SoldierController>();

                        // Enable or disable attack based on the target
                        if (soldierController != null)
                        {
                            if (hit.collider.CompareTag("Soldier"))
                            {
                                soldierController.EnableAttack();
                            }
                            else
                            {
                                soldierController.DisableAttack();
                            }
                        }
                    }
                }
            }
        }
    }
}
