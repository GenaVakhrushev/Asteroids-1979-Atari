using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AsteroidSize { Small, Medium, Big }

public class Asteroid : Moveable
{
    AsteroidSize size = AsteroidSize.Big;

    public AsteroidSize Size => size;

    AudioClip currentClip;
    public AudioClip SmallExplosion;
    public AudioClip MediumExplosion;
    public AudioClip LargeExplosion;

    public void Launch(Vector2 newVelocity, Vector2 position)
    {
        transform.position = position;
        velocity = newVelocity;
    }

    public void SetSize(AsteroidSize asteroidSize)
    {
        size = asteroidSize;
        switch (asteroidSize)
        {
            case AsteroidSize.Big:
                currentClip = LargeExplosion;
                transform.localScale = new Vector3(1, 1, 1);
                break;
            case AsteroidSize.Medium:
                currentClip = MediumExplosion;
                transform.localScale = new Vector3(0.75f, 0.75f, 1);
                break;
            case AsteroidSize.Small:
                currentClip = SmallExplosion;
                transform.localScale = new Vector3(0.5f, 0.5f, 1);
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Asteroid>() != null)
            return;

        if (collision.gameObject.GetComponent<Bullet>() == null || size == AsteroidSize.Small)
        {
            AsteroidsManager.AsteroidsCount--;
        }
        else
        {
            AsteroidsManager.AsteroidsCount++;

            float newSpeed = Random.Range(AsteroidsManager.MinStartSpeed, AsteroidsManager.MaxStartSpeed);
            float velocityAngle = Vector2.Angle(velocity, Vector2.right) * (Vector2.Angle(velocity, Vector2.up) < 90 ? 1 : -1);

            Asteroid asteroid = AsteroidsManager.AsteroidsPool.GetObject();
            float launchRadAngle = (velocityAngle + AsteroidsSettings.Instance.DestroyAngle) * Mathf.Deg2Rad;
            asteroid.Launch(new Vector2(Mathf.Cos(launchRadAngle), Mathf.Sin(launchRadAngle)).normalized * newSpeed, transform.position);
            asteroid.SetSize(size - 1);

            asteroid = AsteroidsManager.AsteroidsPool.GetObject();
            launchRadAngle = (velocityAngle - AsteroidsSettings.Instance.DestroyAngle) * Mathf.Deg2Rad;
            asteroid.Launch(new Vector2(Mathf.Cos(launchRadAngle), Mathf.Sin(launchRadAngle)).normalized * newSpeed, transform.position);
            asteroid.SetSize(size - 1);
        }
        if (currentClip == null)
            currentClip = LargeExplosion;
        AudioEffects.Play(currentClip);
        gameObject.SetActive(false);
    }
}
