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
        StartCoroutine(GetServerData());
    }

    IEnumerator GetServerData()
    {
        string url = "https://citmalumnes.upc.es/~maksymp/GetDeathPositions.php";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Debug.Log("Data received: " + json);
            DataObject dataObject = JsonUtility.FromJson<DataObject>("{\"data\":" + json + "}");

            foreach (onDeathData data in dataObject.data)
            {   
                Vector3 positionSpawn = new Vector3(Mathf.Round(data.X), Mathf.Round(data.Y), Mathf.Round(data.Z));

                GameObject cube = Instantiate(cubePrefab, positionSpawn, Quaternion.identity);
            }
        }
    }

    public void ChangeColor(Vector3 position)
    {
        GameObject gameObject = FindObjectAtPos(position);

        if(gameObject != null)
        {
            ColorChanger colorChanger = gameObject.GetComponentInChildren<ColorChanger>();

            if(colorChanger != null)
            {
                colorChanger.ChangeColor(Color.red);
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
