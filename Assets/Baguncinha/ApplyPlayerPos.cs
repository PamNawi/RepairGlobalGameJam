﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyPlayerPos : MonoBehaviour
{
    Material Mat;
    GameObject Player;
    public  GameObject[] Points;
    public float Radius = 10;

    void Start()
    {
        // Get the material
        Mat = GetComponent<Renderer>().material;
        // Get the player object
        Points = GameObject.FindGameObjectsWithTag("ColorPaint");
    }

    void Update()
    {
        Points = GameObject.FindGameObjectsWithTag("ColorPaint");
        Texture2D texture = new Texture2D(Points.Length, 1);
        Color c = new Color();
        for (int x = 0; x < texture.width; x++)
        {
            c.r = Points[x].transform.position.x/255f;
            c.g = Points[x].transform.position.y/255f;
            c.b = Points[x].transform.position.z/255f;
            c.a = 1.0f;
            texture.SetPixel(x, 1, c);
        }
        texture.Apply();
        Mat.SetTexture("_PosTex", texture);
        Mat.SetInt("_NPoints", Points.Length);
        Mat.SetFloat("_Dist", Radius);
        
    }
}