using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public float gravityForce = 10f;
    public float speed = 5f;
    private Rigidbody2D rb;
    [SerializeField] private GameObject character; 
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
        
        float moveX = Input.GetAxis("Horizontal");
        movement = new Vector2(moveX, 0);

        if (moveX > 0 && characterDirection.Value != 1)
        {
            UpdateCharacterDirectionServerRpc(1);
        }
        else if (moveX < 0 && characterDirection.Value != -1)
        {
            UpdateCharacterDirectionServerRpc(-1); 
        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        SubmitMovementServerRpc(movement * speed * Time.fixedDeltaTime);
    }

    [ServerRpc]
    private void SubmitMovementServerRpc(Vector2 movement)
    {
    
        Vector2 gravity = new Vector2(0, -gravityForce * Time.fixedDeltaTime);

        Vector2 newPosition = rb.position + movement + gravity;

        rb.MovePosition(newPosition);

        Vector3 pos = transform.position;

        Vector3 bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));

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
            character.transform.localScale = new Vector3(1, 1, 1); 
        }
        else if (newDirection < 0)
        {
            character.transform.localScale = new Vector3(-1, 1, 1); 
        }
    }
}
