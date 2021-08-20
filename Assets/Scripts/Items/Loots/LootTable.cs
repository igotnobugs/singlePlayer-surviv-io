using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Spawn an item containing a weapon
 * 
 */


[CreateAssetMenu(menuName ="Loot Table")]
public class LootTable : ScriptableObject 
{
    public LootWeapon[] lootableWeapons;

    [System.Serializable]
    public struct LootWeapon {
        public Weapon weapon;
        public float chance;
        public float tablePosition;
    }

    public Weapon GetWeaponFromTable() {

        //Assign positions
        float totalWeight = 0;        
        for (int i = 0; i < lootableWeapons.Length; i++) {
            lootableWeapons[i].tablePosition = totalWeight;
            totalWeight += lootableWeapons[i].chance;
        }

        //Find weapon
        Weapon weaponToSpawn = lootableWeapons[0].weapon;
        float randomPosition = Random.Range(0, totalWeight);

        for (int i = lootableWeapons.Length - 1; i >= 0; i--) {
            if (randomPosition >= lootableWeapons[i].tablePosition) {
                weaponToSpawn = lootableWeapons[i].weapon;
                break;
            }
        }

        return weaponToSpawn;
    }
}


