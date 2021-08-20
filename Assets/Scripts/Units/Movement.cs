using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour 
{
    private Vector2 moveSpeed;
    private Vector3 lookDirection;
    private Rigidbody2D rb;

    public virtual void Awake() {
        rb = GetComponent<Rigidbody2D>();       
        lookDirection = Vector2.up;
    }

    private void FixedUpdate() {
        rb.velocity = moveSpeed;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, lookDirection);      
    }

    public void MoveTowards(Vector3 direction, float speed) {
        moveSpeed = direction * speed;
    }

    public void LookAt(Vector3 direction, float rotationSpeed) {
        if (direction != Vector3.zero) {
            lookDirection = Vector3.RotateTowards(lookDirection, direction,
                                    Time.deltaTime * rotationSpeed, 0.1f);
        }
    }


    public void Stop() {
        moveSpeed = Vector2.zero;
    }

    private bool isMovingToPosition = false;

    public void MoveToPosition(Vector3 location, float moveSpeed, float lookSpeed, Action onComplete = null) {
        Vector3 dir = location - transform.position;
        LookAt(dir, lookSpeed);

        if (Vector2.Distance(transform.position, location) < 0.2) {          
            if (isMovingToPosition) {
                Stop();
                onComplete?.Invoke();
                isMovingToPosition = false;
            }
        } else {
            isMovingToPosition = true;               
            MoveTowards(dir.normalized, moveSpeed);         
        }

        
    }
}
