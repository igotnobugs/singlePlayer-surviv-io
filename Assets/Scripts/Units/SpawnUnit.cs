using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnit : MonoBehaviour 
{
    public Unit unitToSpawn;
    
    public void Spawn() {
        Unit newUnit = Instantiate(unitToSpawn, transform.position, transform.rotation);
        GameManager.allUnits.Add(newUnit);
        Destroy(gameObject);
    }
}
