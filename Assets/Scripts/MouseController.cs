using UnityEngine;

[RequireComponent(typeof(Player))]
public class MouseController : MonoBehaviour {
    Player player;

    private void Awake() {
        player = GetComponent<Player>();
        FindObjectOfType<Camera>().GetComponent<CameraFollow>().Target = transform;
    }

    private void Update() {
        Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePoint.z = 0;

        player.SetMovePosition(mousePoint);
    }
}