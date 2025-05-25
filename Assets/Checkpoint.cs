using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int CheckpointNumber;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("Checkpoint", CheckpointNumber);
            Destroy(gameObject);
        }
    }
}
