using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    public InterfaceController interfaceController;
    public RecyclableSpawnerManager recyclableSpawnerManager;
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
            }
            recyclableSpawnerManager.SpawnNextRecyclable();
        }
        if (collision.CompareTag("Player"))
        {
            interfaceController.SetGameOver();
        }
        Destroy(collision.gameObject);
    }
    private void Start() {
        if (interfaceController == null) {
            interfaceController = GameObject.FindObjectOfType<InterfaceController>();
        }
        if (recyclableSpawnerManager == null) {
            recyclableSpawnerManager = GameObject.FindObjectOfType<RecyclableSpawnerManager>();
        }
    }
}
