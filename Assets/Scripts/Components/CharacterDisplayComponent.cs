using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CharacterDisplayComponent : MonoBehaviour {
    [SerializeField] float scale = 1;
    SpriteRenderer sRenderer;

    private void Awake() {
        sRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetRadius(float radius) {
        transform.localScale = new Vector3(radius * scale, radius * scale, 1);
    }

    public void SetColor(Color32 color) {
        sRenderer.material.color = color;
    }
}