using System.Collections;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    public InterfaceController interfaceController;
    public RecyclableSpawnerManager recyclableSpawnerManager;
    public GameObject stretchingObject;
    public GameObject playerObject;
    public Transform playerSpawnPoint;
    public string CurrentObjectName;
    public string CurrentObjectTag;


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
            playerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint")?.transform;
            // GameObject spawnObj = GameObject.Find("PlayerSpawnPoint");
            // if (spawnObj != null)
            //     playerSpawnPoint = spawnObj.transform;
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
        CurrentObjectName = collision.gameObject.name;
        CurrentObjectTag = gameObject.tag;
        string cleanObjectName = CurrentObjectName.Replace("(Clone)", "").Trim();
        Debug.Log("CurrentObjectName: " + cleanObjectName + ", CurrentObjectTag: " + CurrentObjectTag);

        if (collision.CompareTag("Recyclable") && cleanObjectName == gameObject.tag)
        {
            Debug.Log("Recyclable object destroyed");
            interfaceController.score += 1;
            interfaceController.UpdateScoreText();
            // StartCoroutine(GoodBinFeedback(cleanObjectName, CurrentObjectTag));
            interfaceController.gameStateText.text = $"Good! \n you trowed {cleanObjectName} in {CurrentObjectTag} bin";
            interfaceController.ShowText();
            if (interfaceController.score >= interfaceController.targetScore)
            {
                interfaceController.gameStateText.text = "You finished! \n Go to the exit!";
                interfaceController.ShowText();
                stretchingObject.GetComponent<StrechingObject>().ChangeStretch();
                Debug.Log("You finished! Go to the exit!");
                Destroy(collision.gameObject);
            }
            else recyclableSpawnerManager.SpawnNextRecyclable();
            // Destroy(collision.gameObject);
        }
        // if (collision.CompareTag("Player"))
        // {
        //     interfaceController.SetGameOver();
        //     // interfaceController.ResetGame();
        //     // collision.gameObject.SetActive(false);
        //     // GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        //     if (playerObject != null)
        //         playerObject.SetActive(false);
        // }

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player hit the bin - Game Over");
            interfaceController.SetGameOver();
            // interfaceController.ResetGame();
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
                playerObject.SetActive(false);
        }
        else if (collision.CompareTag("Recyclable") && cleanObjectName != gameObject.tag && !collision.CompareTag("Player"))
        {
            // Handle recyclable object destruction
            interfaceController.gameStateText.text = $"Wrong one! \n you cant trow {cleanObjectName} in {CurrentObjectTag} bin";
            interfaceController.ShowText();
            interfaceController.score -= 1;
            if (interfaceController.score < 0) interfaceController.score = 0;
            interfaceController.UpdateScoreText();
            Destroy(collision.gameObject);
            recyclableSpawnerManager.SpawnNextRecyclable();
            Debug.Log("Wrong recyclable object destroyed");
        }

        // else if (collision.CompareTag("Recyclable") && cleanObjectName != gameObject.tag && !collision.CompareTag("Player"))
        // {
        //     StartCoroutine(WrongBinFeedback(cleanObjectName, CurrentObjectTag, collision.gameObject));
        //     Destroy(collision.gameObject);
        // }

        // else
        // {
        //     Debug.Log("Non-recyclable object destroyed");
        //     Destroy(collision.gameObject);
        // }
        // if (collision.CompareTag("Recyclable") && CurrentObjectName == CurrentObjectTag)
        // {
        //     Debug.Log("Recyclable object destroyed");
        //     interfaceController.score += 1;
        //     interfaceController.UpdateScoreText();
        //     if (interfaceController.score >= interfaceController.targetScore)
        //     {
        //         interfaceController.gameStateText.text = "You finished! \n Go to the exit!";
        //         stretchingObject.GetComponent<StrechingObject>().ChangeStretch();
        //         Debug.Log("You finished! Go to the exit!");
        //     }
        //     else recyclableSpawnerManager.SpawnNextRecyclable();
        //     // Destroy(collision.gameObject);
        // }
        // if (collision.CompareTag("Player"))
        // {
        //     interfaceController.SetGameOver();
        //     interfaceController.ResetGame();
        //     // collision.gameObject.SetActive(false);
        //     // GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        //     if (playerObject != null)
        //         playerObject.SetActive(false);
        // }
        // if (collision.CompareTag("Recyclable") && collision.gameObject.name != gameObject.tag && !collision.CompareTag("Player"))
        // {
        //     // Handle recyclable object destruction
        //     Debug.Log("Wrong recyclable object destroyed");
        //     interfaceController.score -= 1;
        //     if (interfaceController.score < 0) interfaceController.score = 0;
        //     interfaceController.UpdateScoreText();
        // }
        // else
        // {
        //     Debug.Log("Non-recyclable object destroyed");
        //     Destroy(collision.gameObject);
        // }
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

    // private IEnumerator WrongBinFeedback(string objectName, string binTag, GameObject obj)
    // {
    //     interfaceController.gameStateText.text = $"Wrong one! \n you cant trow {objectName} in {binTag} bin";
    //     interfaceController.gameStateText.gameObject.SetActive(true);
    //     yield return new WaitForSeconds(2f); // or your preferred delay
    //     interfaceController.gameStateText.gameObject.SetActive(false);
    //     interfaceController.score -= 1;
    //     if (interfaceController.score < 0) interfaceController.score = 0;
    //     interfaceController.UpdateScoreText();
    //     Destroy(obj);
    //     recyclableSpawnerManager.SpawnNextRecyclable();
    //     Debug.Log("Wrong recyclable object destroyed");
    // }

    // private IEnumerator GoodBinFeedback(string objectName, string binTag)
    // {
    //     interfaceController.gameStateText.text = $"Good! \n you trowed {objectName} in {binTag} bin";
    //     interfaceController.gameStateText.gameObject.SetActive(true);
    //     yield return new WaitForSeconds(2f); // or your preferred delay
    //     interfaceController.gameStateText.gameObject.SetActive(false);
    // }
}
