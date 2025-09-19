using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public event System.Action OnRestarted;

    [Header("Mouvement")]
    private Rigidbody2D rb;
    private PlayerActions inputActions;
    private Vector2 moveInput;

    [SerializeField] private float speed = 50;
    [SerializeField] private float acceleration = 20;

    [Header("Animation")]
    public Animator playerAnimator;

    [Header("Interaction")]
    // Points d'attachement pour les différentes directions
    public Transform upPoint; // Le point d'attache "up"
    public Transform leftPoint; // Le point d'attache "left"
    public Transform downPoint; // Le point d'attache "down"
    public Transform rightPoint; // Le point d'attache "right"
    private Transform currentPoint; // Le point d'attache actuel
    public float grabRadius = 1f;
    public LayerMask grabbableLayer;

    [Header("Lancer")]
    public float throwForce = 10f;
    private Vector2 lastMoveDirection = Vector2.right;
    private GameObject heldObject = null;

    public InterfaceController interfaceController;

    // Appelé en premier avant Start()
    void Awake()
    {
        // Récupère la référence au Rigidbody2D et désactive la gravité
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        // Crée une instance des PlayerActions et l'active
        inputActions = new PlayerActions();
        inputActions.Enable();
        LinkActions();

        // Initialise le point de départ du 'currentPoint'
        currentPoint = downPoint;
        // Removed DontDestroyOnLoad to allow player to be destroyed and respawned

        // Assign interfaceController automatically if not set
        if (interfaceController == null)
            interfaceController = FindFirstObjectByType<InterfaceController>();
    }

    void LinkActions()
    {
        inputActions.PlayerInput.Move.started += OnMove;
        inputActions.PlayerInput.Move.performed += OnMove;
        inputActions.PlayerInput.Move.canceled += OnMove;

        inputActions.PlayerInput.Grab.performed += OnGrab;
        inputActions.PlayerInput.Throw.performed += OnThrow;
        inputActions.PlayerInput.Restart.performed += OnRestart;
        // inputActions.PlayerInput.Attack.performed += OnAttatck;
    }

    void FixedUpdate()
    {
        Vector2 newVelocity = Vector2.Lerp(rb.linearVelocity, moveInput * speed, Time.fixedDeltaTime * acceleration);
        rb.linearVelocity = newVelocity;
    }

    void Update()
    {
        // On met à jour la dernière direction et on choisit le bon point d'attache
        if (moveInput.magnitude > 0.1f)
        {
            lastMoveDirection = moveInput.normalized;
            UpdateCurrentPoint();
        }
        // On envoie les informations de mouvement à l'Animator
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("IsMoving", moveInput.magnitude > 0.1f);
            playerAnimator.SetFloat("MoveX", moveInput.x);
            playerAnimator.SetFloat("MoveY", moveInput.y);
        }
    }

    void UpdateCurrentPoint()
    {
        float angle = Vector2.SignedAngle(Vector2.right, lastMoveDirection);

        if (angle >= -45 && angle < 45) // Direction droite
        {
            currentPoint = rightPoint;
        }
        else if (angle >= 45 && angle < 135) // Direction haut
        {
            currentPoint = upPoint;
        }
        else if (angle >= -135 && angle < -45) // Direction bas
        {
            currentPoint = downPoint;
        }
        else // Direction gauche
        {
            currentPoint = leftPoint;
        }

        // Si on tient un objet, on le déplace vers le nouveau point
        if (heldObject != null)
        {
            heldObject.transform.position = currentPoint.position;
        }
    }

    // Reçoit l'input de mouvement du New Input System
    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        // Debug.Log("Move Input: " + moveInput);
    }

    // Gère l'action de Grab/Drop
    void OnGrab(InputAction.CallbackContext context)
    {
        if (heldObject == null)
        {
            GrabObject();
        }
        else
        {
            DropObject();
        }
    }

    // Gère l'action de Throw
    void OnThrow(InputAction.CallbackContext context)
    {
        if (heldObject != null)
        {
            ThrowObject();
        }
    }

    // Gère l'action de Restart
    void OnRestart(InputAction.CallbackContext context)
    {
        // Reload the current scene
        interfaceController.ResetGame();
        if (OnRestarted != null) OnRestarted();
        // Respawn player and reset position
        transform.position = GameObject.Find("PlayerSpawnPoint").transform.position;
        gameObject.SetActive(true);
        FindFirstObjectByType<RecyclableSpawnerManager>()?.SpawnNextRecyclable();
    }

    // void OnAttatck(InputAction.CallbackContext context)
    // {
    //     // Attack logic here
    //     Debug.Log("Attack!");
    //     //Damage monster
    //     RaycastHit2D hit = Physics2D.Raycast(transform.position, lastMoveDirection, attackRange, monsterLayer);
    // }
    
    // Logique pour attraper un objet
    void GrabObject()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, grabRadius, grabbableLayer);

        if (hitColliders.Length > 0)
        {
            heldObject = hitColliders[0].gameObject;

            // Désactive les colliders de l'objet pour éviter les collisions
            foreach (Collider2D col in heldObject.GetComponents<Collider2D>())
            {
                col.enabled = false;
            }

            // Rend l'objet cinématique pour désactiver la physique
            Rigidbody2D heldRb = heldObject.GetComponent<Rigidbody2D>();
            if (heldRb != null)
            {
                heldRb.bodyType = RigidbodyType2D.Kinematic;
                heldRb.linearVelocity = Vector2.zero;
            }

            // On attache l'objet au point actuel
            heldObject.transform.SetParent(currentPoint);
            heldObject.transform.localPosition = Vector3.zero;
        }
    }

    // Logique pour lâcher un objet
    void DropObject()
    {
        if (heldObject == null) return;

        // Détache l'objet du point d'attache
        heldObject.transform.SetParent(null);

        // Réactive ses colliders et sa physique
        foreach (Collider2D col in heldObject.GetComponents<Collider2D>())
        {
            col.enabled = true;
        }

        Rigidbody2D heldRb = heldObject.GetComponent<Rigidbody2D>();
        if (heldRb != null)
        {
            heldRb.bodyType = RigidbodyType2D.Dynamic;
        }

        heldObject = null;
    }

    // Logique pour lancer un objet
    void ThrowObject()
    {
        if (heldObject == null) return;

        Rigidbody2D heldRb = heldObject.GetComponent<Rigidbody2D>();

        // Détache l'objet avant de réactiver la physique
        heldObject.transform.SetParent(null);
        foreach (Collider2D col in heldObject.GetComponents<Collider2D>())
        {
            col.enabled = true;
        }

        if (heldRb != null)
        {
            heldRb.bodyType = RigidbodyType2D.Dynamic;
            heldRb.linearVelocity = Vector2.zero;
            heldRb.AddForce(lastMoveDirection * throwForce, ForceMode2D.Impulse);
        }

        heldObject = null;
    }

}