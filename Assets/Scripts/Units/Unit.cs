using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/* Component-Based
 * Never inherit this but refer to it
 */

public class Unit : MonoBehaviour, IDamageable 
{
    [SerializeField] private string unitName = "Unit";
    [SerializeField] public int health = 100;
    [SerializeField] public float moveSpeed = 10.0f;
    [SerializeField] public float rotationSpeed = 5.0f;

    [SerializeField] private Inventory inventory = null;
    [SerializeField] public TransformNodes body;
    [SerializeField] public Movement Movement { private set; get; }

    [Header("Auto Settings")]
    [SerializeField] public bool autoReload = true;
    [SerializeField] public bool infiniteAmmo = false;

    [Header("Information")]
    [SerializeField] public Unit lastHitBy;
    public Action OnDeath;

    private int maxHealth = 100;
    public bool IsAlive { get; private set; } = true;

    public string UnitName { get { return unitName; } }
    public Inventory Inventory { get { return inventory; } }

    public virtual void Awake() {
        if (TryGetComponent(out Movement movement)) {
            Movement = movement;
        }
    }

    public virtual void Start() {
        if (inventory == null) {
            Debug.Log("Unit has no inventory!");
        }
        maxHealth = health;
    }

    public void ShootWeapon(Weapon weapon) {
        bool stillUseable = weapon.Use();

        if (!stillUseable) {
            if (autoReload && (weapon as Gun)) {
                (weapon as Gun).Reload();
            }
        }
    }

    public void ShootWeapon() {
        ShootWeapon(inventory.equippedWeapon);
    }

    public int GetHealth() {
        return Mathf.Clamp(health, 0, maxHealth);
    }

    public int GetMaxHealth() {
        return maxHealth;
    }

    public float GetHealthRatio() {
        return health / (float)maxHealth;
    }

    public void Damage(int amount, Unit by) {
        if (amount <= 0) return;
        lastHitBy = by;
        health -= amount;

        if (health <= 0 && IsAlive) {
            health = 0;
            IsAlive = false;
            GameManager.unitsLeft--;
            GameManager.allUnits.Remove(this);
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }

    public void Heal(int amount) {
        int healAmount = Mathf.Abs(amount);
        if (health + healAmount > maxHealth) {
            health = maxHealth;
        } else {
            health += healAmount;
        }       
    }
}
