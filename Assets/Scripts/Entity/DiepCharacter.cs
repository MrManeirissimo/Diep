using UnityEngine;

[RequireComponent(typeof(HealthComponent), typeof(MovementComponent))]
public class DiepCharacter : PlayerCharacter {

    HealthComponent health;
    MovementComponent movement;

    private void Awake() {
        health = GetComponent<HealthComponent>();
        movement = GetComponent<MovementComponent>();
    }

    public void Move(float x, float y) => movement.Move(x, y);
}