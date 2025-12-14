using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Map
{
    public int id;
    public bool isLocked;
    public string mapName;
    public int unlockPrice;
    public int levelCompleted;
    public string mapDescription;

    public List<Level> routes;
}
