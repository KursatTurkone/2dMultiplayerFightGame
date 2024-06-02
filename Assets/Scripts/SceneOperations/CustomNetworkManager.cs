using Unity.Netcode;
using UnityEngine;

public class CustomNetworkManager : MonoBehaviour
{
    public Vector3 player1SpawnPosition;
    public Vector3 player2SpawnPosition;
    [SerializeField] private GameObject PlayerPrefab; 
    private int playerCount = 0;

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnServerStarted()
    {
        // Sunucu başlatıldığında yapılacak işlemler
        playerCount = 0;
    }

    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            Vector3 spawnPosition;

            if (playerCount == 0)
            {
                spawnPosition = player1SpawnPosition;
            }
            else if (playerCount == 1)
            {
                spawnPosition = player2SpawnPosition;
            }
            else
            {
                return;
            }

            GameObject playerInstance = Instantiate(PlayerPrefab, spawnPosition, Quaternion.identity);
            playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
            if(playerCount== 1)
                playerInstance.GetComponent<PlayerMovement>().UpdateCharacterDirection(-1); 
            playerCount++;
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            playerCount--;
        }
    }
}