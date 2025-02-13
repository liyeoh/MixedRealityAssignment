using UnityEngine;
using TMPro;

public class DeleteAndDisplay : MonoBehaviour
{
    public Camera playerCamera; // Reference to the player's camera
    public float displayDuration = 2f; // Duration to show the TextMeshPro object

    void Start()
    {
        DeleteRandomGameObject();
    }

    void DeleteRandomGameObject()
    {
        // Get all the GameObjects in the scene
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // Filter out the player and UI elements to avoid deleting them
        GameObject[] filteredObjects = System.Array.FindAll(allObjects, obj => obj != gameObject && obj != playerCamera.gameObject);

        if (filteredObjects.Length > 0)
        {
            // Select a random GameObject to delete
            GameObject randomObject = filteredObjects[Random.Range(0, filteredObjects.Length)];

            // Get the name of the deleted object
            string objectName = randomObject.name;

            // Destroy the GameObject
            Destroy(randomObject);

            // Create the TextMeshPro in front of the player
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
}
