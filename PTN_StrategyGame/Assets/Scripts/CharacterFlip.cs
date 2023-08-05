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
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            Vector3 characterPosition = transform.position;

            if (mousePosition.x < characterPosition.x)
            {
                // Tıklanan nokta karakterin sol tarafındaysa
                spriteRenderer.flipX = true;
            }
            else
            {
                // Tıklanan nokta karakterin sağ tarafındaysa
                spriteRenderer.flipX = false;
            }
        }
    }
}
