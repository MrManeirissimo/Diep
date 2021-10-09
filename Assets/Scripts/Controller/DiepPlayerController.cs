using UnityEngine;

public class DiepPlayerController : PlayerController {
    public ushort PlayerID { get; set; }

    public virtual bool FireInputDown() => Input.GetMouseButton(0);
    public virtual float VerticalAxis() => Input.GetAxis("Vertical");
    public virtual float HorizontalAxis() => Input.GetAxis("Horizontal");

    private void Update() {
        DiepCharacter diepCharacter = (DiepCharacter)character;
        if(diepCharacter) {
            diepCharacter.Move(HorizontalAxis(), VerticalAxis());
            diepCharacter.Rotate(GetRotationAngle(diepCharacter.transform.position, diepCharacter.transform.up));

            if (FireInputDown()) {
                diepCharacter.Fire();
            }
        }
    }

    private static float GetRotationAngle(Vector3 position, Vector3 upVector) {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 lookDirection = (mousePosition - position).normalized;
        return Vector3.SignedAngle(upVector, lookDirection, Vector3.forward);
    }
}