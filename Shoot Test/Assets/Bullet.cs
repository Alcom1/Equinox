using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{
    public float moveSpeed = 10.0f;
    public Vector3 velocity;

    private void FixedUpdate()
    {
        // we want the bullet to be updated only on the server
        if (!base.isServer)
            return;

        // transform bullet on the server
        transform.position += velocity * Time.deltaTime * moveSpeed;
    }
}