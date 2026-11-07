using UnityEngine;

public class CameraDeathZone : MonoBehaviour
{
    public Camera mainCamera;

    void Update()
    {
        if (mainCamera == null)
            return;
        
        if (!GameManager.Instance.isPaused)
        {
            Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

            if (viewportPosition.y < -0.1f)
            {
                Die();
            }
        }
    }

    void Die()
    {
        Debug.Log("Player died!");

        GameManager.Instance.EndGame();
    }
}
