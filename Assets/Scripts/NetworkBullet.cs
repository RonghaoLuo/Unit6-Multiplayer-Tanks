using Unity.Netcode;
using UnityEngine;

public class NetworkBullet : NetworkBehaviour
{
    [SerializeField] private Rigidbody bulletRigidbody;

    public void ApplyBulletForce(float force)
    {
        bulletRigidbody.AddForce(transform.forward * force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsServer && IsSpawned)
        {
            NetworkObject.Despawn();

            if (collision.collider.CompareTag("Player"))
            {
                Debug.Log(collision.gameObject.name + " was hit");
                // Decrease Health
                NetworkPlayerInfo playerInfo = collision.collider.GetComponent<NetworkPlayerInfo>();
                playerInfo.DecreaseHealth();
            }
        }
        else
        {

        }
    }
}
