using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    public InterfaceController interfaceController;
    public RecyclableSpawnerManager recyclableSpawnerManager;
    public GameObject stretchingObject;
    public GameObject playerObject;
    public Transform playerSpawnPoint;


    private void Start()
    {
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }
        if (interfaceController == null)
        {
            interfaceController = GameObject.FindFirstObjectByType<InterfaceController>();
        }
        if (recyclableSpawnerManager == null)
        {
            recyclableSpawnerManager = GameObject.FindFirstObjectByType<RecyclableSpawnerManager>();
        }
        if (stretchingObject == null)
        {
            stretchingObject = GameObject.FindFirstObjectByType<StrechingObject>().gameObject;
        }
        if (playerSpawnPoint == null)
        {
            GameObject spawnObj = GameObject.Find("PlayerSpawnPoint");
            if (spawnObj != null)
                playerSpawnPoint = spawnObj.transform;
        }
        // Subscribe to restart event (assuming InterfaceController or PlayerController exposes one)
        if (interfaceController != null)
        {   
            PlayerController pc = playerObject?.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.OnRestarted += RespawnPlayer;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Recyclable"))
        {
            Debug.Log("Recyclable object destroyed");
            interfaceController.score += 1;
            interfaceController.UpdateScoreText();
            if (interfaceController.score >= interfaceController.targetScore)
            {
                interfaceController.gameStateText.text = "You finished! \n Go to the exit!";
                stretchingObject.GetComponent<StrechingObject>().ChangeStretch();
                Debug.Log("You finished! Go to the exit!");
            }
            else recyclableSpawnerManager.SpawnNextRecyclable();
        }
        if (collision.CompareTag("Player"))
        {
            interfaceController.SetGameOver();
            // collision.gameObject.SetActive(false);
            // GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
                playerObject.SetActive(false);
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }

    public void RespawnPlayer()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null && playerSpawnPoint != null)
        {
            // Respawn player at the spawn point
            playerObject.transform.position = playerSpawnPoint.position;
            playerObject.SetActive(true);
        }
        recyclableSpawnerManager.SpawnNextRecyclable();
    }
}
