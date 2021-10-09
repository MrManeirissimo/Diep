using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    public float speed = 5f;

    public Transform Target { get; set; }
    private void Start() {
        DiepEventManager.Instance.ListenToEvent("OnPlayerReceived",(sender, args) => {
            var player = args.Get<DiepCharacter>("player");
            if(player.GetController() != null) {
                Target = player.transform;
            }
        });
    }

    void Update() {
        if (Target != null) {
            Vector3 targetPos = Target.GetComponent<Renderer>().bounds.center;
            transform.position = Vector3.Lerp(
                transform.position,
                new Vector3(targetPos.x, targetPos.y, transform.position.z),
                speed * Time.deltaTime
            );
        }
    }
}
