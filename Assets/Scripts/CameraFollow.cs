using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform character;

    [Header("Camera Settings")]
    public float smoothSpeed = 5f;
    public float horizontalOffset = 0f;

    private float fixedY;

    void Start()
    {
        fixedY = transform.position.y;
    }

    void LateUpdate()
    {
        if (character == null)
            return;

        Vector3 targetPosition = new Vector3(character.position.x + horizontalOffset, fixedY, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
