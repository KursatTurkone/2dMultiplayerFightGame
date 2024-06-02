using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : NetworkBehaviour
{
    public NetworkVariable<float> Health = new NetworkVariable<float>(100f);
    public Image healthBarFill;

    public delegate void DieEvent();

    public static event DieEvent OnDieEvent;

    private void Start()
    {
        if (IsOwner)
        {
            Health.OnValueChanged += OnHealthChanged;
        }

        UpdateHealthBar(Health.Value);
    }

    private void OnDestroy()
    {
        if (IsOwner)
        {
            Health.OnValueChanged -= OnHealthChanged;
        }
    }

    private void OnHealthChanged(float oldValue, float newValue)
    {
        UpdateHealthBar(newValue);
    }


    public void TakeDamageClient(float damage)
    {
        if (!IsServer)
            return;

        Health.Value -= damage;

        if (Health.Value <= 0)
        {
            Health.Value = 0;
            NotifyClientOfDeathClientRpc(new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { OwnerClientId }
                }
            });
            OnDie();
        }
    }

    [ClientRpc]
    private void NotifyClientOfDeathClientRpc(ClientRpcParams clientRpcParams = default)
    {
        Debug.Log("Character died on client");
        OnDieEvent.Invoke();
    }

    private void OnDie()
    {
        ulong clientId = OwnerClientId;
        NetworkManager.Singleton.DisconnectClient(clientId);
        gameObject.SetActive(false);
    }

    private void UpdateHealthBar(float currentHealth)
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / 100f;
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Health.OnValueChanged += OnHealthChanged;
        UpdateHealthBar(Health.Value);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        Health.OnValueChanged -= OnHealthChanged;
    }
}