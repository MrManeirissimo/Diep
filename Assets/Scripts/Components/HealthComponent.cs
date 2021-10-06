using UnityEngine;

public class HealthComponent : MonoBehaviour {
    public event System.Action OnZeroHealth;
    public event System.Action OnMaxHealth;

    public bool IsMaxHealth => current == max;
    public bool IsZeroHealth => current == 0;

    [SerializeField] float max = 100;
    [SerializeField] float current = 100;

    public void Reset() => Set(max);
    public void Start() => Reset();


    public virtual void Add(float value) {
        if((IsMaxHealth && value.IsPositive()) || 
            (IsZeroHealth && value.IsNegative())) {
            return;
        }

        Set(Mathf.Clamp(current + value, 0, max));
    }
    public virtual void Set(float value) {
        current = value;
        CheckHealthState();
    }

    void CheckHealthState() {
        if (IsMaxHealth)
            OnMaxHealth?.Invoke();

        else if (IsZeroHealth)
            OnZeroHealth?.Invoke();
    }
}