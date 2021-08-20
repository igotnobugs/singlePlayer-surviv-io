using System.Collections.Generic;
using UnityEngine;

/* Has it's own separate gameObject in a Unit
 */

public class Inventory : MonoBehaviour 
{
    [SerializeField] private bool autoEquipWeapon = true;

    [SerializeField] public Weapon defaultWeapon = null;
    [SerializeField] public Weapon equippedWeapon = null;

    [SerializeField] private int weaponIndexEquipped;
    [SerializeField] private int weaponCount = 0;
    [SerializeField] private Weapon[] weaponsStored = null;
    [SerializeField] private List<ItemStack> stacks = new List<ItemStack>();

    public ItemStack activeAmmoStack;

    private Unit owner;

    public delegate void InventoryEvent();
    public event InventoryEvent OnInventoryUpdated;

    private void Awake() {
        owner = GetComponentInParent<Unit>();
        AddItem(defaultWeapon, 1, false);          
    }

    private void Start() {
        if (equippedWeapon == null) {
            EquipWeapon(weaponsStored[0]);
        }
    }

    public void EquipWeapon(Weapon weapon) {

        if (equippedWeapon != null)
            equippedWeapon.gameObject.SetActive(false);

        equippedWeapon = weapon;
        equippedWeapon.gameObject.SetActive(true);     
        equippedWeapon.Equip(owner.body, owner);

        activeAmmoStack = GetItemStack(equippedWeapon.AmmoRequired);

        OnInventoryUpdated?.Invoke();
    }

    //Return amount not added
    public int AddItem(Item item, int amount = 1, bool willClone = true) {
        if (amount <= 0) amount = 1;

        int leftOver = AddToStackOrMakeNewStack(item, amount);

        AddWeapon(item as Weapon, leftOver, willClone);

        OnInventoryUpdated?.Invoke();
        return leftOver;
    }

    private int AddWeapon(Weapon weapon, int amount, bool willClone = true) {
        if (weapon == null) return amount;
        if (CheckAlreadyHave(weapon)) return amount;

        int leftOver = 1; // Can only add 1 weapon
        weaponCount++;
        Weapon newWeapon;
        if (willClone) {
            newWeapon = Instantiate(weapon, transform);
        } else {
            newWeapon = weapon;
        }

        int index = StoreWeaponGetIndex(newWeapon);

        // Make the ammo stack available
        AddToStackOrMakeNewStack(newWeapon.AmmoRequired, 0);

        if (equippedWeapon == null && index >= 0) {
            if (equippedWeapon == defaultWeapon) {             
                EquipWeapon(weaponsStored[index]);
                weaponIndexEquipped = index;
            }
        } else {
            weaponsStored[index].gameObject.SetActive(false);
        }

        if (autoEquipWeapon) {
            EquipWeapon(weaponsStored[index]);
            weaponIndexEquipped = index;
        }

        return leftOver;
    }

    public void EquipWeaponCycle(bool ignoreFist = false) {
        int nextWeaponIndex = weaponIndexEquipped + 1;

        if (nextWeaponIndex >= weaponsStored.Length ||
            weaponsStored[nextWeaponIndex] == null) {
            if (ignoreFist) {
                nextWeaponIndex = 1;
            } else {
                nextWeaponIndex = 0;
            }        
        }
        weaponIndexEquipped = nextWeaponIndex;
        EquipWeapon(weaponsStored[weaponIndexEquipped]);
    }

    public bool CheckAlreadyHave(Weapon weapon) {
        for (int i = 0; i < weaponsStored.Length; i++) {           
            if (weaponsStored[i] == null) return false;
            if (weapon.ItemName == weaponsStored[i].ItemName) return true;         
        }
        // Full?
        return true;
    }

    public int StoreWeaponGetIndex(Weapon weapon) {
        for (int i = 0; i < weaponsStored.Length; i++) {
            if (weaponsStored[i] == null) {
                weaponsStored[i] = weapon;
                return i;
            }
        }
        // Full?
        return -1;
    }

    private int AddToStackOrMakeNewStack(Item item, int amount) {
        if (item == null) return 0;
        int leftOver = amount;

        for (int i = 0; i < stacks.Count; i++) {
            if (stacks[i].itemStored == item) {
                leftOver = stacks[i].AddAmount(amount);
                return leftOver;
            }
        }

        ItemStack newStack = new ItemStack(item, item.MaxItemInStack);
        leftOver = newStack.AddAmount(amount);
        stacks.Add(newStack);

        return leftOver;
    } 

    public int GetAmmo(int amount) {
        int amountRecieved = Mathf.Clamp(activeAmmoStack.itemAmount, 0, amount);
        activeAmmoStack.itemAmount -= amountRecieved;
        OnInventoryUpdated?.Invoke();
        return amountRecieved;
    }

    public bool IsEquippedWeaponDefault() {
        //Default weapon is usually fist
        return equippedWeapon == defaultWeapon;
    }

    public bool IsEquippedWeapon(Weapon weapon) {
        //Default weapon is usually fist
        return equippedWeapon == weapon;
    }

    // Reduce the amount of calls from this
    // Currently only used by EquipWeapon()
    public ItemStack GetItemStack(Item item) {
        for (int i = 0; i < stacks.Count; i++) {
            if (stacks[i].itemStored == item) {
                return stacks[i];
            }
        }
        return null;
    }

    public int GetWeaponCount() {
        return weaponCount;
    }

}