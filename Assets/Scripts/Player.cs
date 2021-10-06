using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Player : MonoBehaviour {

    [SerializeField] float speed;
    [SerializeField] float scale;
    Vector3 movePosition;

    private void Awake() {
        movePosition = transform.position;
    }

    private void Update() {
        if (speed != 0)
            transform.position = Vector3.MoveTowards(transform.position, movePosition, speed * Time.deltaTime);
    }

    public void SetColor(Color32 color) {
        var renderer = GetComponent<Renderer>();
        if (renderer) renderer.material.color = color;
    }

    public void SetRadius(float radius) {
        transform.localScale = new Vector3(radius * scale, radius * scale, 1);
    }

    public void SetMovePosition(Vector3 newPosition) {
        movePosition = newPosition;
    }
}