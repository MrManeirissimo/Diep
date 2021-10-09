using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CharacterDisplayComponent : MonoBehaviour {
    public Color32 Color { get => color; }

    [SerializeField] float scale = 1;
    [SerializeField] Color32 color;
    TextMesh textMesh;
    SpriteRenderer sRenderer;

    private void Awake() {
        sRenderer = GetComponent<SpriteRenderer>();
        textMesh = GetComponentInChildren<TextMesh>();
    }

    public void SetRadius(float radius) {
        transform.localScale = new Vector3(radius * scale, radius * scale, 1);
    }

    public void SetColor(Color32 color) {
        this.color = color;
        sRenderer.material.color = color;

        foreach (var renderer in GetComponentsInChildren<SpriteRenderer>()) {
            renderer.material.color = color;
        }
    }

    public void SetHealthText(float currentHealth) {
        textMesh.text = Mathf.RoundToInt(currentHealth).ToString();
    }
}