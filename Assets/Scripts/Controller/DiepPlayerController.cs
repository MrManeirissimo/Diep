using UnityEngine;

public class DiepPlayerController : PlayerController {

    public virtual bool FireInputDown() => Input.GetMouseButtonDown(0);
    public virtual float VerticalAxis() => Input.GetAxis("Vertical");
    public virtual float HorizontalAxis() => Input.GetAxis("Horizontal");

    private void Update() {
        if (FireInputDown()) {
            print("Hi");
        }

        DiepCharacter diepCharacter = (DiepCharacter)character;
        if(diepCharacter) {
            diepCharacter.Move(HorizontalAxis(), VerticalAxis());
        }
    }
}