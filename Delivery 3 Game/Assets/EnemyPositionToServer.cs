using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyPositionToServer : MonoBehaviour
{
    public static EnemyPositionToServer Instance; // Singleton para facilitar el acceso

    private string serverUrl = "https://citmalumnes.upc.es/~maksymp/EnemiesDeathPositions.php";

    private void Awake()
    {
        // Configurar el Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SendEnemyDeathPosition(Vector3 position)
    {
        Debug.Log($"Enviando posición de enemigo muerto: X = {position.x}, Y = {position.y}, Z = {position.z}");
        StartCoroutine(SendPositionToServer(position));
    }

    private IEnumerator SendPositionToServer(Vector3 position)
    {
        WWWForm form = new WWWForm();
        form.AddField("X", position.x.ToString());
        form.AddField("Y", position.y.ToString());
        form.AddField("Z", position.z.ToString());

        using (UnityWebRequest webRequest = UnityWebRequest.Post(serverUrl, form))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al enviar los datos: " + webRequest.error);
            }
            else
            {
                Debug.Log("Respuesta del servidor: " + webRequest.downloadHandler.text);
            }
        }
    }
}
