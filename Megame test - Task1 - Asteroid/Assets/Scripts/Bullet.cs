using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Moveable
{
    float fliedDist = 0;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Vector2 moveVec = velocity * Time.deltaTime;

        fliedDist += moveVec.magnitude;
        if(fliedDist >= ScreenWidth)
        {
            gameObject.SetActive(false);
        }
    }

    public void Fire(Vector2 newVelocity)
    {
        fliedDist = 0;
        velocity = newVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //не реагировать с пулями
        if (collision.gameObject.GetComponent<Bullet>() != null)
            return;

        //пули корабля не реагируют с кораблём
        if(collision.gameObject.GetComponent<Ship>() != null && tag == "ShipBullet")
            return;

        //пули НЛО не реагируют с НЛО
        UFO ufo = collision.gameObject.GetComponentInParent<UFO>();
        if (ufo != null && tag == "UFOBullet")
            return;

        gameObject.SetActive(false);

        //пули НЛО не учавствуют в подсчёте очков
        if (tag == "UFOBullet")
            return;

        Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
        if (asteroid)
        {
            switch (asteroid.Size)
            {
                case AsteroidSize.Big:
                    Statistics.Points += PointsSettings.Instance.BigAsteroidPoints;
                    break;
                case AsteroidSize.Medium:
                    Statistics.Points += PointsSettings.Instance.MediumAsteroidPoints;
                    break;
                case AsteroidSize.Small:
                    Statistics.Points += PointsSettings.Instance.SmallAsteroidPoints;
                    break;
            }
        }

        if (ufo)
        {
            Statistics.Points += PointsSettings.Instance.UFOPoints;
        }
    }
}
