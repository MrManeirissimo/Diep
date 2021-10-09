using UnityEngine;

public class MovementComponent : MonoBehaviour {
    [SerializeField] float speed = 5;
    [SerializeField] float rotationSpeed = 5;
    Rigidbody2D body;

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
    }

    public void Move(float x, float y) {
        Vector3 worldDirection = transform.InverseTransformDirection(new Vector3(x, y, 0));
        transform.Translate(worldDirection * speed * Time.deltaTime);
    }

    public void Rotate(float z) {
        transform.rotation = transform.rotation * Quaternion.Euler(0, 0, z * rotationSpeed * Time.deltaTime);
    }
}