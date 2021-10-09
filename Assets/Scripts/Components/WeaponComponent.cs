using static NetworkFacade;
using System.Collections;

using UnityEngine;

public class WeaponComponent : MonoBehaviour {
    public GameObject ProjectilePrefab => projectilePrefab;
    public float Cooldown => cooldown;

    [SerializeField] float cooldown = .5f;
    [SerializeField] Transform weaponSocket;
    [SerializeField] GameObject projectilePrefab;
    bool canFire = true;

    public GameObject FireRemote(ushort id, Color32 color, Vector3 position, Vector3 velocity) {
        BulletBehavior bullet = PoolBullet();
        SetupBullet(bullet, GetClientID(), color, position, velocity);

        return bullet.gameObject;
    }
    public GameObject Fire() {
        if (!canFire)
            return null;

        StartCoroutine(CooldownRoutine());

        BulletBehavior bullet = PoolBullet();

        SetupBullet(bullet, GetClientID(), GetComponent<CharacterDisplayComponent>().Color, weaponSocket.position, weaponSocket.up);

        bullet.Replicate();

        return bullet.gameObject;
    }

    private void SetupBullet(BulletBehavior bullet, ushort id, Color32 color, Vector3 position, Vector3 velocity) {
        bullet.GetComponent<CharacterDisplayComponent>().SetColor(color);
        bullet.ownerID = id;
        bullet.transform.rotation = CalculateRotation(velocity);
        bullet.transform.position = position;
        bullet.gameObject.SetActive(true);
    }
    private Quaternion CalculateRotation(Vector3 velocity) {
        Quaternion rotation = Quaternion.LookRotation(velocity, Vector3.forward);
        rotation *= Quaternion.Euler(90, 0, 0);
        return rotation;
    }


    private void OnBulletDied(GameObject bullet) {
        var bulletComponent = bullet.GetComponent<BulletBehavior>();
        bulletComponent.OnLifeSpanEnded -= OnBulletDied;

        PoolingSystem.Instance.Return("Bullet", bullet);
    }
    private IEnumerator CooldownRoutine() {
        canFire = false;
        yield return new WaitForSeconds(cooldown);
        canFire = true;
    }
    private BulletBehavior PoolBullet() {
        var bulletObject = PoolingSystem.Instance.Request("Bullet");
        var bulletComponent = bulletObject.GetComponent<BulletBehavior>();
        bulletComponent.OnLifeSpanEnded += OnBulletDied;

        return bulletComponent;
    }
}