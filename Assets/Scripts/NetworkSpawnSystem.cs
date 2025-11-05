using Unity.Netcode;
using UnityEngine;

public class NetworkSpawnSystem : NetworkBehaviour
{
    [SerializeField] private Transform[] allSpawnPoints;

    public Transform GetRandomSpawnPoint()
    {
        return allSpawnPoints[Random.Range(0, allSpawnPoints.Length - 1)];
    }
}
