using UnityEngine;

public class CeilingManager : MonoBehaviour
{
    private float freezeSpeed = 0;
    private CameraMovement cam;

    private void Start()
    {
        cam = gameObject.GetComponentInParent<CameraMovement>();
    }

    private void OnTriggerEnter2D(Collider2D entity)
    {
        if (entity.tag == "Player")
        {
            cam.PlayNextWave();
        }
    }

    private void OnTriggerExit2D(Collider2D entity)
    {
        if (entity.tag == "Player")
        {
        }
    }
}