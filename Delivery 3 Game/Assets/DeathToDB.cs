using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class DeathToDB : MonoBehaviour
{
    public string filePath = Application.dataPath + "/Death positions.txt";
    public string serverUrl = "https://citmalumnes.upc.es/~maksymp/DeathPositions.php";

    private string lastReadContent = "";
    private DateTime lastFileUpdateTime;
    private int Aux = 0;

    void Start()
    {
        string filePath = Application.dataPath + "/Death positions.txt";
        Debug.Log("File Path: " + filePath);

        if (File.Exists(filePath))
        {
            string fileContent = File.ReadAllText(filePath);
            lastReadContent = fileContent;
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

                if (currentFileUpdateTime > lastFileUpdateTime)
                {
                    lastFileUpdateTime = currentFileUpdateTime;

                    string fileContent = File.ReadAllText(Application.dataPath + filePath);

                    // Check if the content has changed
                    if (fileContent != lastReadContent && Aux >= 2)
                    {
                        lastReadContent = fileContent;
                        Debug.LogWarning("File content has been updated. New content: " + fileContent);

                        // Get the last line of the file
                        string lastLine = GetLastLine(fileContent);

                        // Send the new last line to the server
                        if (!string.IsNullOrEmpty(lastLine))
                        {
                            StartCoroutine(SendDeathPositionToServer(lastLine));
                        }
                    }
                    else
                    {
                        Aux++;
                    }
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private string GetLastLine(string fileContent)
    {
        // Split the content into lines
        string[] lines = fileContent.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        // Iterate from the last line to find the most recent death position line
        for (int i = lines.Length - 1; i >= 0; i--)
        {
            if (lines[i].Contains("Death Player Num"))
            {
                return lines[i].Trim();
            }
        }

        return null;
    }

    private IEnumerator SendDeathPositionToServer(string deathPositionData)
    {
        // First, extract the position part from the deathPositionData
        string[] parts = deathPositionData.Split(new string[] { "Position: " }, StringSplitOptions.None);

        // If there's any position data found
        if (parts.Length > 1)
        {
            // Extract the position values (assuming they are in the format: "X Y Z")
            string positionData = parts[1].Trim();  // Remove any leading/trailing spaces
            string[] coordinates = positionData.Split('.');  // Split by periods

            if (coordinates.Length == 3)
            {
                // Convert the coordinates to float
                float x = float.Parse(coordinates[0].Trim());
                float y = float.Parse(coordinates[1].Trim());
                float z = float.Parse(coordinates[2].Trim());

                // Create a form to send the data
                WWWForm form = new WWWForm();
                form.AddField("X", x.ToString());
                form.AddField("Y", y.ToString());
                form.AddField("Z", z.ToString());

                // Debug: Log the values
                Debug.Log($"Sending Death Position - X: {x}, Y: {y}, Z: {z}");

                // URL for the server
                string serverUrl = "https://citmalumnes.upc.es/~maksymp/DeathPositions.php";

                using (UnityWebRequest webRequest = UnityWebRequest.Post(serverUrl, form))
                {
                    yield return webRequest.SendWebRequest();

                    if (webRequest.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError("Error sending data: " + webRequest.error);
                    }
                    else
                    {
                        // Print the response from the PHP script
                        Debug.Log("Server Response: " + webRequest.downloadHandler.text);
                    }
                }
            }
            else
            {
                Debug.LogError("Invalid Death Position format, expected 3 coordinates.");
            }
        }
        else
        {
            Debug.LogError("No position data found in the provided string.");
        }
    }
}
