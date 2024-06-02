using UnityEngine;

public class DamageTester : MonoBehaviour
{
    private CharacterHealth playerHealth;

    private void Start()
    {
        playerHealth = GetComponent<CharacterHealth>();
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space) && playerHealth.IsOwner)
        {
            playerHealth.TakeDamageServerRpc(10f);
        }*/
    }
}