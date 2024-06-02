using Unity.Netcode;
using UnityEngine;

public class PlayerDamageCollider : NetworkBehaviour
{
    public float damageAmount = 10f; // Verilecek hasar miktarı

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return; 

        Debug.Log("Trigger entered by: " + other.name); 

        CharacterHealth otherPlayerHealth = other.GetComponent<CharacterHealth>();
        if (otherPlayerHealth != null && otherPlayerHealth != GetComponentInParent<CharacterHealth>())
        {
            Debug.Log("Dealing damage to: " + other.name);
            otherPlayerHealth.TakeDamageClient(damageAmount);
        }
    }
}