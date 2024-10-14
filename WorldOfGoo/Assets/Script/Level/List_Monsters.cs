using System.Collections.Generic;
using UnityEngine;

public class List_Monsters : MonoBehaviour
{
    public List<MoveMonsters> _listMonsters = new();

    public static List_Monsters Instance;

    private void Awake()
    {
        Instance = this;
    }
}
