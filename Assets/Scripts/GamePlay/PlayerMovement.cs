using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public float gravityForce = 10f;
    public float speed = 5f;
    private Rigidbody2D rb;
    [SerializeField] private GameObject character; // Character GameObject'i
    private Vector2 movement;
    private Camera mainCamera;

    private NetworkVariable<int> characterDirection = new NetworkVariable<int>(1);

       private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        
        characterDirection.OnValueChanged += OnCharacterDirectionChanged;
    }

    private void Update()
    {
        if (!IsOwner) return;

        // Hareket girişlerini al
        float moveX = Input.GetAxis("Horizontal");
        movement = new Vector2(moveX, 0);

        // Karakterin yönünü değiştir
        if (moveX > 0 && characterDirection.Value != 1)
        {
            UpdateCharacterDirectionServerRpc(1); // Sağ tarafa bak
        }
        else if (moveX < 0 && characterDirection.Value != -1)
        {
            UpdateCharacterDirectionServerRpc(-1); // Sol tarafa bak
        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        // Hareket verilerini sunucuya gönder
        SubmitMovementServerRpc(movement * speed * Time.fixedDeltaTime);
    }

    [ServerRpc]
    private void SubmitMovementServerRpc(Vector2 movement)
    {
        // Yereçekimi kuvvetini ekliyoruz
        Vector2 gravity = new Vector2(0, -gravityForce * Time.fixedDeltaTime);

        // Karakterin yeni pozisyonunu hesapla
        Vector2 newPosition = rb.position + movement + gravity;

        // Yeni pozisyonu ayarla
        rb.MovePosition(newPosition);

        // Ekran sınırlarını kontrol et ve karakteri sınırlandır
        Vector3 pos = transform.position;

        // Kameranın sol alt ve sağ üst köşelerinin dünya koordinatları
        Vector3 bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));

        // Karakteri ekran sınırları içinde tut
        pos.x = Mathf.Clamp(pos.x, bottomLeft.x, topRight.x);
        transform.position = pos;
    }

    [ServerRpc]
    private void UpdateCharacterDirectionServerRpc(int direction)
    {
        characterDirection.Value = direction;
    }
  
    public void UpdateCharacterDirection(int direction)
    {
        characterDirection.Value = direction;
    }
    private void OnCharacterDirectionChanged(int oldDirection, int newDirection)
    {
        if (newDirection > 0)
        {
            character.transform.localScale = new Vector3(1, 1, 1); // Sağ tarafa bak
        }
        else if (newDirection < 0)
        {
            character.transform.localScale = new Vector3(-1, 1, 1); // Sol tarafa bak
        }
    }
}
