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
        File.AppendAllText(Application.dataPath + "/Death positions.txt", "________New Session________\n");
    }

    public void Update()
    {
        PlayerPosition = PlayerObject.transform.position;
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Reseted File");
            File.WriteAllText(Application.dataPath + "/Death positions.txt", "<DEATH POSITIONS>\n\n");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Muertes_Json();
        }

    }
    //Funcion a llamar cuando se muere
    public void Muertes_Json()
    {

        muertes = muertes + 1;

        Debug.Log("Muertes: " + muertes
            + "| Position X: " + PlayerPosition.x
            + "| Position Y: " + PlayerPosition.y
            + "| Position Z: " + PlayerPosition.z);

        string content = "Death Player Num: "
            + muertes.ToString() + " | Position: " +
            PlayerPosition.x.ToString() + ". " +
            PlayerPosition.y.ToString() + ". " +
            PlayerPosition.z.ToString() + "\n";

        File.AppendAllText(Application.dataPath + "/Death positions.txt", content);
    }

}
