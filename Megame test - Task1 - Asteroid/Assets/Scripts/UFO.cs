using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : Moveable
{
    float speed => ScreenWidth / 10f;

    ObjectPool<Bullet> bulletsPool;

    Ship ship;

    public GameObject BulletPrefab;

    public AudioClip FireClip;
    public AudioClip DeathSound;

    void Start()
    {
        GameStateManager.OnGameStart.AddListener(OnGameStart);
        ship = GameObject.FindGameObjectWithTag("Player").GetComponent<Ship>();

        bulletsPool = new ObjectPool<Bullet>(BulletPrefab, 10);
    }

    void OnGameStart()
    {
        Hide();
    }

    IEnumerator WaitRespawn()
    {
        float waitTime = Random.Range(20, 40f);
        yield return new WaitForSeconds(waitTime);
        Spawn();
    }

    void Spawn()
    {
        bool onLeftSide = Random.Range(0, 2) == 0;
        float xScreenPos = onLeftSide ? 0 : Screen.width;
        float yScreenPos = Random.Range(Screen.height * 0.2f, Screen.height * 0.8f);
        Vector2 newPos = Camera.main.ScreenToWorldPoint(new Vector3(xScreenPos, yScreenPos, 0));

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        transform.position = newPos;
        velocity = (onLeftSide ? Vector2.right : Vector2.left) * speed;
        StartCoroutine(Fire());
    }

    void Hide()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        velocity = Vector2.zero;
        StopAllCoroutines();
        StartCoroutine(WaitRespawn());
    }

    IEnumerator Fire()
    {
        AudioEffects.Play(FireClip);
        Bullet bullet = bulletsPool.GetObject();
        bullet.transform.position = transform.position;
        bullet.Fire((ship.transform.position - transform.position).normalized * ship.BulletSpeed);

        float cooldown = Random.Range(2, 5f);
        yield return new WaitForSeconds(cooldown);
        StartCoroutine(Fire());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //не реагировать со своими пулями
        if (collision.gameObject.GetComponent<Bullet>() != null && collision.gameObject.tag == "UFOBullet")
            return;

        AudioEffects.Play(DeathSound);
        Hide();
    }
}
