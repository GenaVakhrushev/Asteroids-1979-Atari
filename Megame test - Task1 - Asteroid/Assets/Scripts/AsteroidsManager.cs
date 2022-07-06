using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsManager : MonoBehaviour
{
    static ObjectPool<Asteroid> asteroidsPool;

    static int asteroidsCount = 0;
    static int maxAsteroids = 2;

    static AsteroidsManager instance;

    public GameObject AsteroidPrefab;
    [Range(1, 5), SerializeField] float minStartSpeed = 2;
    [Range(5, 10), SerializeField] float maxStartSpeed = 5;

    public static float MinStartSpeed => instance.minStartSpeed;
    public static float MaxStartSpeed => instance.maxStartSpeed;

    public static int AsteroidsCount
    {
        get
        {
            return asteroidsCount;
        }
        set
        {
            asteroidsCount = value;
            if(asteroidsCount == 0)
            {
                instance.StartCoroutine(StartNextLevel());
            }
        }
    }

    public static ObjectPool<Asteroid> AsteroidsPool => asteroidsPool;

    private void Start()
    {
        instance = this;
        asteroidsPool = new ObjectPool<Asteroid>(AsteroidPrefab, 50);
        GameStateManager.OnGameStart.AddListener(OnGameStart);
    }

    void OnGameStart()
    {
        foreach (Asteroid asteroid in FindObjectsOfType<Asteroid>())
        {
            asteroid.gameObject.SetActive(false);
        }
        maxAsteroids = 2;
        LaunchAsteroids();
    }

    static IEnumerator StartNextLevel()
    {
        yield return new WaitForSeconds(2);
        maxAsteroids++;
        instance.LaunchAsteroids();
    }

    void LaunchAsteroids()
    {
        for (int i = 0; i < maxAsteroids; i++)
        {
            float launchPower = Random.Range(minStartSpeed, maxStartSpeed);
            asteroidsPool.GetObject().Launch(RandomVector2() * launchPower, RandomPositionOnBorder());
        }
        asteroidsCount = maxAsteroids;
    }

    Vector2 RandomVector2()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    Vector2 RandomPositionOnBorder()
    {
        Vector2 screenPos = Vector2.zero;
        int borderNum = Random.Range(0, 4);
        switch (borderNum)
        {
            case 0://left
                screenPos = new Vector2(0, Random.Range(0, Screen.height));
                break;
            case 1://top
                screenPos = new Vector2(Random.Range(0, Screen.width), Screen.height);
                break;
            case 2://right
                screenPos = new Vector2(Screen.width, Random.Range(0, Screen.height));
                break;
            case 3://down
                screenPos = new Vector2(Random.Range(0, Screen.width), 0);
                break;
        }
        return Camera.main.ScreenToWorldPoint(screenPos);
    }
}
