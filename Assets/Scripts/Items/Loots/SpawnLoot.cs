using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLoot : MonoBehaviour 
{
    public LootItem basicWeaponLootItem;
    public LootItem basicAmmoLootItem;

    public LootTable lootTableToUse;

    public Transform gunSpawn;
    public Transform[] ammoSpawn;


    private void Start() {
        //Spawn;
    }

    public void Spawn() {
        Weapon weapon = lootTableToUse.GetWeaponFromTable();
        Ammo ammoToSpawn = weapon.AmmoRequired;

        LootItem weaponLoot = Instantiate(basicWeaponLootItem, 
            gunSpawn.transform.position,
            gunSpawn.transform.rotation);
        weaponLoot.Init(weapon, 1);
        Destroy(gunSpawn.gameObject);

        for (int i = 0; i < ammoSpawn.Length; i++) {
            LootItem ammoLoot = Instantiate(basicAmmoLootItem, 
                ammoSpawn[i].transform.position,
                ammoSpawn[i].transform.rotation);
            ammoLoot.name = ammoToSpawn.name;
            ammoLoot.Init(ammoToSpawn, 5);
            ammoLoot.SetIconColor(ammoToSpawn.color);
            Destroy(ammoSpawn[i].gameObject);
        }
    }

}
