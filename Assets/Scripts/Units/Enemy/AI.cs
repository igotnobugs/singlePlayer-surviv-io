using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AI : Unit 
{
    [SerializeField] public Animator StateMachine { get; private set; }

    [Header("AI Settings")]
    public Unit target;
    public LootItem lootTarget;
    public float sightRange = 7.0f;
    public float wanderRadius = 2;
    public float shootRange = 6.0f;
    public float chaseRange = 9.0f;

    [SerializeField] private Collider2D[] lootArray;
    public override void Awake() {
        base.Awake();
        StateMachine = GetComponent<Animator>();
    }

    private void Update() {      
        StateMachine.SetFloat("gameTime", StateMachine.GetFloat("gameTime") + Time.deltaTime);
    }

    public LootItem FindWeapon(float range) {
        LayerMask mask = LayerMask.GetMask("Loot");
        LayerMask wallMask = LayerMask.GetMask("Wall");

        lootArray = Physics2D.OverlapCircleAll(transform.position, range, mask);
        
        for (int i = 0; i < lootArray.Length; i++) {
            if (lootArray[i] == null) continue;
            Vector2 direction = lootArray[i].transform.position - transform.position;

            if (Physics2D.Raycast(transform.position, direction, sightRange, wallMask)) {
                continue;
            }

            if (lootArray[i].TryGetComponent(out LootItem loot)) {
                if (loot.GetStoredItem().TryGetComponent(out Weapon weapon)) {
                    return loot;
                }
            }
        }
        return null;
    }

    public LootItem FindAnything(float range) {
        LayerMask mask = LayerMask.GetMask("Loot");
        LayerMask wallMask = LayerMask.GetMask("Wall");

        lootArray = Physics2D.OverlapCircleAll(transform.position, range, mask);

        for (int i = 0; i < lootArray.Length; i++) {
            if (lootArray[i] == null) continue;
            Vector2 direction = lootArray[i].transform.position - transform.position;

            if (Physics2D.Raycast(transform.position, direction, sightRange, wallMask)) {
                continue;
            }

            if (lootArray[i].TryGetComponent(out LootItem loot)) {
                return loot;
            }
        }
        return null;
    }

    public Unit FindUnit(float range) {
        LayerMask mask = LayerMask.GetMask("Unit");
        LayerMask wallMask = LayerMask.GetMask("Wall");
        Collider2D[] unitArray = Physics2D.OverlapCircleAll(transform.position, range, mask);
        //Find anyone except myself
        for (int i = 0; i < unitArray.Length; i++) {
            if (unitArray[i] == null) continue;
            Vector2 direction = unitArray[i].transform.position - transform.position;

            if (Physics2D.Raycast(transform.position, direction, sightRange, wallMask)) {
                continue;
            }

            if (unitArray[i].TryGetComponent(out Unit unitFound)) {
                if (unitFound != this) {
                    return unitFound;
                }
            }
        }
        return null;
    }

    public void StartWandering() {
        InvokeRepeating("Wander", 0.05f, 1.5f);
    }

    public void StopWandering() {
        CancelInvoke("Wander");
    }

    private void Wander() {
        Vector3 wanderTarget = new Vector3(Random.Range(-1.0f, 1.0f),
                   Random.Range(-1.0f, 1.0f),
                   0);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetPosition = transform.position + wanderTarget;
        Movement.MoveToPosition(targetPosition, moveSpeed, rotationSpeed);
    }

}
