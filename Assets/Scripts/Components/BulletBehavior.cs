using static DiepPlugin.GameNetworkTag;
using static NetworkFacade;

using System.Collections;
using System;

using UnityEngine;

public class BulletBehavior : MonoBehaviour {
    public event Action<GameObject> OnLifeSpanEnded;
    CharacterDisplayComponent display;

    public ushort ownerID;
    public float radius = 0.3f;
    public float speed = 10f;
    public float lifespan = 3.0f;
    public float damage = 10;

    public void Replicate() {
        SendClientMessage(
            (writer) => {
                writer.Write(ownerID);
                writer.Write(transform.position.x);
                writer.Write(transform.position.y);
                writer.Write(transform.up.x);
                writer.Write(transform.up.y);
                writer.Write(display.Color.r);
                writer.Write(display.Color.g);
                writer.Write(display.Color.b);
                writer.Write(radius);
            }, SPAWN_BULLET, DarkRift.SendMode.Reliable
        );
    }

    private void Awake() {
        display = GetComponent<CharacterDisplayComponent>();
    }
    private void OnEnable() {
        StartCoroutine(StartLifeSpawnCounting());
        GetComponent<Rigidbody2D>().velocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (ownerID == GetClientID()) {
            DiepCharacter player = collision.GetComponent<DiepCharacter>();
            if (player) {
                if (player.PlayerID != ownerID) {
                    player.TakeDamage(damage);
                }
            }
        }
    }

    private IEnumerator StartLifeSpawnCounting() {
        yield return new WaitForSeconds(lifespan);
        OnLifeSpanEnded?.Invoke(gameObject);
    }
}