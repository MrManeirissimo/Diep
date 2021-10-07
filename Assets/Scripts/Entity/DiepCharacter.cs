using UnityEngine;

[RequireComponent(typeof(HealthComponent), typeof(MovementComponent), typeof(CharacterDisplayComponent))]
public class DiepCharacter : PlayerCharacter {

    HealthComponent health;
    MovementComponent movement;
    CharacterDisplayComponent display;

    private void Awake() {
        health = GetComponent<HealthComponent>();
        movement = GetComponent<MovementComponent>();
        display = GetComponent<CharacterDisplayComponent>();
    }

    public void SetPosition(Vector3 newPosition) {
        transform.position = newPosition;
    }

    public void Move(float x, float y) => movement.Move(x, y);

    public void SetRadius(float radius) => display.SetRadius(radius);
    public void SetColor(Color32 color) => display.SetColor(color);
}