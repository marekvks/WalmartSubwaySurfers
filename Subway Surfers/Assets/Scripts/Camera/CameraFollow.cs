using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime = 10f;

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.position + offset, smoothTime * Time.deltaTime);
    }
}
