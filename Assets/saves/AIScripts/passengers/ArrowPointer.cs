using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    public Transform target;
    public Transform player;

    void Update()
    {
        if (target != null && player != null)
        {
            Vector3 direction = target.position - player.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}