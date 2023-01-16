using System.Collections.Generic;
using UnityEngine;

public static class DataHolder
{
    public static int SkinIndex;
    public static bool MouseInput;
    public static int CrystallsEntered;
    public static int balance = 900;
    public static float RenderDistance = 20f;
    public static List<GameObject> AllFood = new List<GameObject>();
    public static string RoomName;


    public static void ClearList()
    {
        AllFood.Clear();
    }

    public static GameObject GetRandomFood()
    {
        if (AllFood.Count > 1)
        {
            return AllFood[Random.Range(0, AllFood.Count)];
        }
        else return null;
    }
}
