using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Deathpositontofile : MonoBehaviour
{

    //Al morir guardar la posicion del player
    [System.Serializable]
    public class PosicionPlayer
    {
        public int numero_Muerte;
        public float x, y, z;

        public PosicionPlayer(int newNumero_Muerte, float new_x, float new_y, float new_z)
        {
            numero_Muerte = newNumero_Muerte;
            x = new_x;
            y = new_y;
            z = new_z;
        }
    }

    int muertes = 0;
    public GameObject PlayerObject;
    private Vector3 PlayerPosition;

    public
    List<PosicionPlayer> MyListDeath = new List<PosicionPlayer>();

    public void Start()
    {
        File.AppendAllText(Application.dataPath + "/Death positions.txt", "\nNew Session \n\n");

    }
    public void Update()
    {
        PlayerPosition = PlayerObject.transform.position;
        if (Input.GetKeyDown(KeyCode.R))
        {
            File.WriteAllText(Application.dataPath + "/Death positions.txt", "Death positions\n\n");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Muertes_Json();
        }

    }
    public void Muertes_Json()
    {
        //Al morir guardar la posicion del player
        muertes = muertes + 1;

        Debug.Log("Muertes: " + muertes
            + "| Position X: " + PlayerPosition.x
            + "| Position Y: " + PlayerPosition.y
            + "| Position Z: " + PlayerPosition.z);

        string content = "Death Player Num "
            + muertes.ToString() + "\n " +
            PlayerPosition.x.ToString() + ", " +
            PlayerPosition.y.ToString() + ", " +
            PlayerPosition.z.ToString() + "\n";

        File.AppendAllText(Application.dataPath + "/Death positions.txt", content);
    }

}
