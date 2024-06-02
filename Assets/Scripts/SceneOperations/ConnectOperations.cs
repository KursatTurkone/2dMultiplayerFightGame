using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ConnectOperations : MonoBehaviour
{
    [SerializeField] private GameObject gameState; 
    void Start()
    {
        if (GameState.Instance == null)
            Instantiate(gameState); 
        if (!GameState.Instance.IsPlayer)
        {
            NetworkManager.Singleton.StartServer();
        }
        else
        {
            NetworkManager.Singleton.StartClient();
        }
    }
    
}
