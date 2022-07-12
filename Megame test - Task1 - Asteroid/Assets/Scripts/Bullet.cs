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
        //�� ����������� � ������
        if (collision.gameObject.GetComponent<Bullet>() != null)
            return;

        //���� ������� �� ��������� � �������
        if(collision.gameObject.GetComponent<Ship>() != null && tag == "ShipBullet")
            return;

        //���� ��� �� ��������� � ���
        UFO ufo = collision.gameObject.GetComponentInParent<UFO>();
        if (ufo != null && tag == "UFOBullet")
            return;

        gameObject.SetActive(false);

        //���� ��� �� ���������� � �������� �����
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
