using Unity.Netcode;
using UnityEngine;

public class PlayerDamageCollider : NetworkBehaviour
{
    public float damageAmount = 10f; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return; 
        CharacterHealth otherPlayerHealth = other.GetComponent<CharacterHealth>();
        if (otherPlayerHealth != null && otherPlayerHealth != GetComponentInParent<CharacterHealth>())
        {
            otherPlayerHealth.TakeDamageClient(damageAmount);
        }
    }
}