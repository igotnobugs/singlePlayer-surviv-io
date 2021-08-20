using UnityEngine;

public class Weapon : Item 
{
    [Header("Weapon")]
    [SerializeField] protected float damage = 10;
    [SerializeField] protected Ammo ammoRequired;
    [SerializeField] protected int ammoLoaded = 2;
    [SerializeField] protected int maxAmmoLoaded = 2;
    [SerializeField] protected float fireRate = 1.0f; 
    [SerializeField] protected float reloadTime = 2.0f;

    [Header("Position Nodes")]
    public TransformNodes placement;

    protected Unit user;
    public Ammo AmmoRequired { get { return ammoRequired; } }
    public int AmmoLoaded { get { return ammoLoaded; } set { ammoLoaded = value; } }
    
    protected float fireCoolDown = 0;

    public void Equip(TransformNodes ownerNodes, Unit equippedBy) {
        user = equippedBy;
        transform.position = ownerNodes.gun.position;
        transform.rotation = user.transform.rotation;
        ownerNodes.rightHand.position = placement.rightHand.position;
        ownerNodes.leftHand.position = placement.leftHand.position;
    }

    public virtual void FireEffect() { }
}
