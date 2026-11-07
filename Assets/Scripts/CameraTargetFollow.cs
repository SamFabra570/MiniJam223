using UnityEngine;

public class CameraTargetFollow : MonoBehaviour
{
    public Transform character;

    private float fixedY;

    void Start()
    {
        fixedY = transform.position.y;
    }

    void LateUpdate()
    {
        if (character == null)
            return;

        transform.position = new Vector3(character.position.x, fixedY, transform.position.z);
    }
}
