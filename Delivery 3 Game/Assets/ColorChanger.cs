using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public void ChangeColor(Color newColor)
    {
        Renderer renderer = GetComponent<Renderer>();
        if(renderer != null)
        {
            renderer.material.color = newColor;
        }
        else
        {
            renderer.material.color = Color.blue;
        }
    }
}
