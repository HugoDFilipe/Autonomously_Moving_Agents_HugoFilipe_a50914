using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public sealed class World 
{

    private static readonly World instance = new World();
    private static GameObject[] hidingSpots;


    static World()
    {
        hidingSpots = GameObject.FindGameObjectsWithTag("hide");
    }

    private World()
    {

    }

    public static World Instace
    {
        get { return instance; }
    }

    public GameObject[] GetHiddingSpots()
    {
        return hidingSpots;
    }
}
