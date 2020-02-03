using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newGlobals", menuName = "Config files/Globals")]
public class Globals : ScriptableObject
{
    public Feature systems;

    public GameManager gameManager;
}
