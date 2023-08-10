using UnityEngine;

public class CharacterFlip : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Check for a right mouse button click
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition); // Convert mouse position to world coordinates
            mousePosition.z = 0; // Ensure the z-coordinate is zero for 2D comparison

            Vector3 characterPosition = transform.position; // Get the character's position

            if (mousePosition.x < characterPosition.x)
            {
                // If the clicked point is to the left of the character's position, flip the character horizontally (flipX)
                spriteRenderer.flipX = true;
            }
            else
            {
                // If the clicked point is to the right of the character's position, do not flip the character
                spriteRenderer.flipX = false;
            }
        }
    }
}