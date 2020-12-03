using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<UnitSpawner> Spawners;
    
    private WaypointManager _waypointManager;

    public void Initialize(WaypointManager wpMngr)
    {
        _waypointManager = wpMngr;
        foreach (UnitSpawner spawner in Spawners)
        {
            spawner.Init(_waypointManager.GetPath(spawner.pathId));
        }
    }

    public void StartSpawners()
    {
        foreach (UnitSpawner spawner in Spawners)
        {
            spawner.StartSpawner();
        }
    }
}
