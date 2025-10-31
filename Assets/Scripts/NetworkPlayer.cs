using Unity.Netcode;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Rigidbody rigidBodyReference;

    [Header("Shooting")]
    [SerializeField] private NetworkBullet tankProjectilePrefab;
    [SerializeField] private Transform weaponTip;
    [SerializeField] private float bulletSpeed;

    private float forwardValue;
    private float rotateValue;

    //private void OnNetworkInstantiate(NetworkMessageInfo info)
    //{
        
    //} Delete NetworkMessageInfo Internal Class

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsLocalPlayer)
        {
            Debug.Log("Initializing Inputs and Variables");
        }

        Debug.Log("Player " + OwnerClientId + " Spawned");
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer)
        {
            forwardValue = Input.GetAxisRaw("Vertical");
            rotateValue = Input.GetAxisRaw("Horizontal");

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ShootProjectileRpc();
            }
        }
    }

    private void FixedUpdate()
    {
        if (IsLocalPlayer)
        {
            rigidBodyReference.AddForce(transform.forward * moveSpeed * forwardValue);
            rigidBodyReference.AddTorque(Vector3.up * rotateSpeed * rotateValue);
        }
    }

    [Rpc(SendTo.Server)]
    private void ShootProjectileRpc()
    {
        NetworkBullet clonedBullet = Instantiate(tankProjectilePrefab, position: weaponTip.position, rotation: weaponTip.rotation);
        clonedBullet.NetworkObject.Spawn();
        clonedBullet.ApplyBulletForce(bulletSpeed);

    }
}
