using CodeMonkey.Utils;
using Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GameRTSController : MonoBehaviour
{
    [SerializeField] private Transform selectionAreaTranform;

    private Vector3 startPosition;
    private List<UnitRTS> selectedUnitRTSList;

    private void Awake()
    {
        selectedUnitRTSList= new List<UnitRTS>();
        selectionAreaTranform.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { // Left Mouse Button Pressed
            selectionAreaTranform.gameObject.SetActive(true);

            startPosition = UtilsClass.GetMouseWorldPosition();
        }

        if (Input.GetMouseButton(0))
        {
            //Left Mouse Button Held
            Vector3 currentMousePosition= UtilsClass.GetMouseWorldPosition();
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
            // Left Mouse Button Released
            selectionAreaTranform.gameObject.SetActive(false);

            Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(startPosition, UtilsClass.GetMouseWorldPosition());

            //Deselect all Units
            foreach(UnitRTS unitRTS in selectedUnitRTSList)
            {
                unitRTS.SetSelectedVisible(false);
            }

            selectedUnitRTSList.Clear();

            //Select Units within Selection Area
            foreach (Collider2D collider2D in collider2DArray) {
                UnitRTS unitRTS = collider2D.GetComponent<UnitRTS>();
                if (unitRTS != null)
                {
                    unitRTS.SetSelectedVisible(true);
                    selectedUnitRTSList.Add(unitRTS);
                }
            }
            Debug.Log(selectedUnitRTSList.Count);
        }

        if (Input.GetMouseButtonDown(1)) // Sağ tıklamayı kontrol edelim
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            Transform newTarget = hit.transform; // Tıklanan noktanın Transform'u

            foreach (UnitRTS unitRTS in selectedUnitRTSList)
            {
                unitRTS.MoveTo(newTarget);
            }
        }
    }
}
