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
    private float gridInterval = 0.3f; 
    
    private static GameRTSController instance;
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
        selectedUnitRTSList = new List<UnitRTS>();
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
            // Left Mouse Button Released
            selectionAreaTranform.gameObject.SetActive(false);

            Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(startPosition, UtilsClass.GetMouseWorldPosition());

            //Deselect all Units
            foreach (UnitRTS unitRTS in selectedUnitRTSList)
            {
                unitRTS.SetSelectedVisible(false);
                unitRTS.FlipCharacter(false);
            }

            selectedUnitRTSList.Clear();

            //Select Units within Selection Area
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
            Debug.Log(selectedUnitRTSList.Count);
        }

        //Her hangi bir noktaya sag click yagildiginda bu noktaya asker veya askerlerin hareketi saglanir
        if (Input.GetMouseButtonDown(1)) // Sağ tıklamayı kontrol edelim
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                Transform newTarget = hit.collider.transform; // Tıklanan noktanın Transform'u

                if (selectedUnitRTSList.Count > 1) // Birden fazla asker seçiliyse
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
                else // Sadece bir asker seçiliyse
                {
                    if (selectedUnitRTSList.Count == 1)
                    {
                        UnitRTS unitRTS = selectedUnitRTSList[0];
                        if (unitRTS != null)
                        {
                            unitRTS.MoveTo(newTarget);
                            SoldierController soldierController = unitRTS.GetComponent<SoldierController>();
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


    private List<Transform> GetPositionListAround(Transform startPosition, float distance, int positionCount)
    {
        List<Transform> positionList = new List<Transform>();
        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360f / positionCount);
            Vector3 dir = ApplyRotationToVector(new Vector3(1, 0), angle);
            Vector3 position = startPosition.position + dir * (distance * 0.6f);

            GameObject emptyObject = new GameObject(); // Boş bir GameObject oluştur
            emptyObject.transform.position = position; // Pozisyonunu ayarla
            positionList.Add(emptyObject.transform); // Listeye ekle
            Destroy(emptyObject);
        }
        return positionList;
    }


    private Vector3 ApplyRotationToVector(Vector3 vec, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * vec;
    }

}
