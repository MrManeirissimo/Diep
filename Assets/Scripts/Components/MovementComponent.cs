using UnityEngine;

public class MovementComponent : MonoBehaviour {
    [SerializeField] float speed = 5;
    Vector3 movePosition;

    public void Move(float x, float y) {

        transform.Translate(new Vector3(x, y, 0) * speed * Time.deltaTime); 
    }
}