using UnityEngine;
using TMPro;
using UnityEngine.XR;

public class XRDeleteAndDisplay : MonoBehaviour
{
    public Camera playerCamera; // Reference to the player's camera
    public float displayDuration = 2f; // Duration to show the TextMeshPro object
    public int objectsToDelete = 5; // Number of objects to progressively delete
    public float timeBetweenDeletes = 5f; // Time interval between object deletions

    private bool isDeleting = false;

    void Start()
    {
        // Optionally, start the deletion process after a delay or on a certain trigger.
        StartCoroutine(DeleteObjectsGradually());
    }

    // Coroutine to simulate gradual breakdown of objects
    private System.Collections.IEnumerator DeleteObjectsGradually()
    {
        while (objectsToDelete > 0)
        {
            if (!isDeleting)
            {
                isDeleting = true;

                // Gradually delete random objects and show TextMeshPro
                DeleteRandomGameObject();

                // Decrease the number of objects left to delete
                objectsToDelete--;

                // Wait before continuing the deletion process
                yield return new WaitForSeconds(timeBetweenDeletes);
                isDeleting = false;
            }
            else
            {
                yield return null; // Wait until the current deletion is complete
            }
        }

        // Optional: After all deletions, you might want to simulate a critical failure or crash
        SimulateCriticalFailure();
    }

    void DeleteRandomGameObject()
    {
        // Get all the GameObjects in the scene (excluding some base important ones)
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // Filter out the player camera, UI elements, and any objects you don't want to delete
        GameObject[] filteredObjects = System.Array.FindAll(allObjects, obj => obj != gameObject && obj != playerCamera.gameObject);

        if (filteredObjects.Length > 0)
        {
            // Select a random GameObject to delete
            GameObject randomObject = filteredObjects[Random.Range(0, filteredObjects.Length)];

            // Get the name of the deleted object
            string objectName = randomObject.name;

            // Destroy the GameObject
            Destroy(randomObject);

            // Create a TextMeshPro object in front of the player to display the name
            CreateTextMeshPro(objectName);
        }
    }

    void CreateTextMeshPro(string objectName)
    {
        // Create a new GameObject to hold the TextMeshPro component
        GameObject textObject = new GameObject("DeletedObjectName");

        // Add TextMeshPro component
        TextMeshPro text = textObject.AddComponent<TextMeshPro>();

        // Set the text to the deleted object's name
        text.text = objectName;

        // Position the TextMeshPro object in front of the player's camera
        textObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward * 2f; // 2 units in front of the player

        // Rotate the TextMeshPro object to always face the camera
        textObject.transform.rotation = Quaternion.LookRotation(textObject.transform.position - playerCamera.transform.position);

        // Optionally, make the text disappear after some time
        Destroy(textObject, displayDuration);
    }

    void SimulateCriticalFailure()
    {
        // Display a critical failure message or simulate application crash
        Debug.LogError("Critical Failure: Too many components removed, application is unstable!");

        // Here, you can trigger an error message or use the XR system to indicate a crash.
        // For example, showing a UI pop-up or flashing red screen.

        // Forcefully stop or restart the application (simulate crash)
        Application.Quit();
    }
}
