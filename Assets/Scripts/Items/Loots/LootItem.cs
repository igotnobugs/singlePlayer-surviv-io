using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Has an item that can be picked by a unit
 */


public class LootItem : MonoBehaviour 
{
    [SerializeField] private Item itemStored = null;
    [SerializeField] private int itemAmount = 1;

    private Unit dropper = null;

    [Header("Item Loot Settings")]
    [SerializeField] private SpriteRenderer lootItemIcon = null;

    public LootItem(Item item, int amount) {
        itemStored = item;
        itemAmount = amount;
        lootItemIcon.sprite = item.itemIcon;
    }

    public void Init(Item item, int amount) {
        itemStored = item;
        itemAmount = amount;
        if (lootItemIcon != null) {
            lootItemIcon.sprite = item.itemIcon;
        }
    }

    public void SetIconColor(Color color) {
        lootItemIcon.color = color;
    }

    public Item GetStoredItem() {
        return itemStored;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Unit unit = collision.gameObject.GetComponent<Unit>();

        if (unit != null && unit != dropper) {
            if (itemAmount <= 0) { Debug.Log("Adding nothing or negative?"); }

            int leftOver = itemStored.AddTo(unit, itemAmount);

            if (leftOver <= 0) {              
                Destroy(gameObject);
            } else {
                itemAmount = leftOver;
                // Just add ammo then delete
                if (itemStored is Weapon) {
                    (itemStored as Weapon).AmmoRequired.AddTo(unit, (itemStored as Weapon).AmmoLoaded);
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Unit unit = collision.gameObject.GetComponent<Unit>();
        if (unit == dropper) {
            dropper = null;
        }
    }
}
