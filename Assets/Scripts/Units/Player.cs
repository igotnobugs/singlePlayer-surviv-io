using UnityEngine;

public class Player : Unit 
{
    [SerializeField] private float shootingSensitivity = 0.05f;
    [SerializeField] private Joystick moveStick = null;
    [SerializeField] private Joystick shootStick = null;

    public override void Awake() {
        base.Awake();
        moveStick = GameObject.FindGameObjectWithTag("Moving Joystick").GetComponent<Joystick>();
        shootStick = GameObject.FindGameObjectWithTag("Shooting Joystick").GetComponent<Joystick>();
    }

    public override void Start() {
        base.Start();
        shootingSensitivity = Mathf.Clamp(shootingSensitivity, 0.01f, 0.99f);
    }

    private void Update() {
        Movement.MoveTowards(moveStick.Direction, moveSpeed);
        Movement.LookAt(shootStick.Direction, rotationSpeed);

        if (TriggerPulled(shootingSensitivity)) {
            ShootWeapon();
        }
    }

    private bool TriggerPulled(float sensitivity) {
        return shootStick.Direction.magnitude > 1.0f - sensitivity;
    }
}
