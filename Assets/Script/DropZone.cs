using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{

    public GameObject targetObject;

    public ScoreManager scoreManager; // Reference to the ScoreManager

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetObject)
        {
            scoreManager.AddScore(1);
            Debug.Log("Score + 1");
            // Optional: Destroy or deactivate the RAM object
            // Destroy(other.gameObject);
        }
    }
}