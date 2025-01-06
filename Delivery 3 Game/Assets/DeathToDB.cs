using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class DeathToDB : MonoBehaviour
{
    public string filePath = Application.dataPath + "/Death positions.txt"; // Path to your .txt file
    public string serverUrl = "https://yourserver.com/ReceiveDeathPosition.php"; // Replace with your PHP endpoint

    private string lastReadContent = "";  // Store the last read content of the file
    private DateTime lastFileUpdateTime;

    void Start()
    {
        string filePath = Application.dataPath + "/Death positions.txt";
        Debug.Log("File Path: " + filePath);

        if (File.Exists(filePath))
        {
            string fileContent = File.ReadAllText(filePath);
            lastReadContent = fileContent;  // Store the initial content
            Debug.Log("File Content at Start: " + fileContent);
            StartCoroutine(MonitorFile());
        }
        else
        {
            Debug.LogError("File not found in the Assets folder.");
        }
    }

    private IEnumerator MonitorFile()
    {
        while (true)
        {
            if (File.Exists(Application.dataPath + filePath))
            {
                DateTime currentFileUpdateTime = File.GetLastWriteTime(Application.dataPath + filePath);

                // Check if the file has been updated
                if (currentFileUpdateTime > lastFileUpdateTime)
                {
                    lastFileUpdateTime = currentFileUpdateTime;

                    // Read the entire content of the file
                    string fileContent = File.ReadAllText(Application.dataPath + filePath);

                    // Check if the content has changed
                    if (fileContent != lastReadContent)
                    {
                        lastReadContent = fileContent;
                        Debug.LogWarning("File content has been updated. New content: " + fileContent);

                        // Send the new content to the server
                        StartCoroutine(SendDeathPositionToServer(fileContent));
                    }
                }
            }

            // Check the file every second
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator SendDeathPositionToServer(string deathPositionData)
    {
        WWWForm form = new WWWForm();
        form.AddField("DeathPosition", deathPositionData);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(serverUrl, form))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error sending data: " + webRequest.error);
            }
            else
            {
                Debug.Log("Successfully sent data: " + webRequest.downloadHandler.text);
            }
        }
    }
}
