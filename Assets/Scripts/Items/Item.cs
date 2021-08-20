using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Item
 *  |----> Weapon ---> Gun ---> (Guns)
 *  |----> Ammo
 *  |----> Armor
 *  \----> Consumable
 */

public class Item : MonoBehaviour
{
    [Header("Item")]
    [SerializeField] public Sprite itemIcon;
    [SerializeField] protected string itemName = "Item";
    [SerializeField] protected int maxItemInStack = 1;

    public string ItemName { get { return itemName; } }
    public int MaxItemInStack { get { return maxItemInStack; } }
    public virtual bool Use() { throw new System.NotImplementedException(); }

    public int AddTo(Unit unit, int amount) {
        return unit.Inventory.AddItem(this, amount);
    }
}