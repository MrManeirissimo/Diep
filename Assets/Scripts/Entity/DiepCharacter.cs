using System;
using UnityEngine;

[RequireComponent(typeof(HealthComponent), typeof(MovementComponent), typeof(CharacterDisplayComponent))]
public class DiepCharacter : PlayerCharacter {
    public ushort PlayerID { get; set; }

    WeaponComponent weapon;
    HealthComponent health;
    MovementComponent movement;
    CharacterDisplayComponent display;

    private void Awake() {
        weapon = GetComponent<WeaponComponent>();
        health = GetComponent<HealthComponent>();
        movement = GetComponent<MovementComponent>();
        display = GetComponent<CharacterDisplayComponent>();
    }

    public void SetPosition(Vector3 newPosition) {
        transform.position = newPosition;
    }

    public void Move(float x, float y) => movement.Move(x, y);
    public void Rotate(float z) => movement.Rotate(z);
    public void Fire() => weapon.Fire();

    public void SetRadius(float radius) => display.SetRadius(radius);
    public void SetColor(Color32 color) => display.SetColor(color);

    public void TakeDamage(float damage) {
        health.Add(-damage);
        display.SetHealthText(health.Current);
    }
}