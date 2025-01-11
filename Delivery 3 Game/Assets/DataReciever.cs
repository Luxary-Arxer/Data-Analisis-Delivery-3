using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class DataReciever : MonoBehaviour
{
    public GameObject cubePrefab;


    [Serializable]
    public class onDeathData
    {
        public float X, Y, Z;
        public int Id;
    }
    [Serializable]
    public class DataObject
    {
        public onDeathData[] data; 
    }

    public void Start()
    {
        StartCoroutine(GetServerData("https://citmalumnes.upc.es/~maksymp/GetDeathPositions.php", Color.cyan));
        StartCoroutine(GetServerData("https://citmalumnes.upc.es/~maksymp/GetEnemiesDeathPositions.php", Color.red));
    }

    IEnumerator GetServerData(string url, Color color)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            DataObject dataObject = JsonUtility.FromJson<DataObject>("{\"data\":" + json + "}");

            foreach (onDeathData data in dataObject.data)
            {   
                Vector3 positionSpawn = new Vector3(Mathf.Round(data.X), Mathf.Round(data.Y), Mathf.Round(data.Z));

                GameObject cube = Instantiate(cubePrefab, positionSpawn, Quaternion.identity);
                ChangeColor(positionSpawn, color);
            }
        }
    }

    public void ChangeColor(Vector3 position, Color color)
    {
        GameObject gameObject = FindObjectAtPos(position);

        if(gameObject != null)
        {
            ColorChanger colorChanger = gameObject.GetComponent<ColorChanger>();

            if (colorChanger != null)
            {
                colorChanger.ChangeColor(color);
            }
        }
    }

    public GameObject FindObjectAtPos(Vector3 position)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Cube"))
        {
            if (Vector3.Distance(obj.transform.position, position) < 0.1f)
            {
                return obj;
            }
        }
        return null;
    }
}
