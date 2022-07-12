using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType { Keyboard, KeyboardMouse }

public class Ship : Moveable
{
    ObjectPool<Bullet> bulletsPool;
    float lastShotTime = 0;

    Collider2D collider;
    SpriteRenderer spriteRenderer;
    Color shipColor
    {
        get
        {
            return spriteRenderer.color;
        }
        set
        {
            spriteRenderer.color = value;
        }
    }

    //движение и поворот
    public float MaxSpeed = 4;
    public float RotationSpeed = 270;
    public float Acceleration = 0.05f;

    public int IgnoreDamageTime = 3;

    //пули
    public float BulletSpeed = 10;
    public GameObject BulletPrefab;

    public InputType CurrentInputType = InputType.KeyboardMouse;

    public AudioClip DeathClip;
    public AudioClip FireClip;
    public AudioSource TrushSound;

    void Start()
    {
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bulletsPool = new ObjectPool<Bullet>(BulletPrefab, 25);
        GameStateManager.OnGameStart.AddListener(OnGameStart);
    }

    void Update()
    {
        if (NeedFire() && Time.time - lastShotTime > 1 / 3.0)
        {
            Fire();
        }
    }

    protected override void FixedUpdate()
    {
        Move();
        Rotate();
        base.FixedUpdate();
    }

    void OnGameStart()
    {
        StopAllCoroutines();
        Respawn();
    }

    void Move()
    {
        if (GoForward())
        {
            TrushSound.mute = false;
            Vector2 newVelocity = velocity + (Vector2)transform.up * Acceleration;
            if(newVelocity.magnitude < MaxSpeed)
            {
                velocity = newVelocity;
            }
            else
            {
                velocity = newVelocity.normalized * MaxSpeed;
            }
        }
        else
        {
            TrushSound.mute = true;
        }
    }

    void Rotate()
    {
        switch (CurrentInputType)
        {
            case InputType.Keyboard:
                RotateKeyboard();
                break;
            case InputType.KeyboardMouse:
                RotateMouse();
                break;
        }
    }

    //ввод поворота клавиатурой
    void RotateKeyboard()
    {
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, 0, RotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, 0, -RotationSpeed * Time.deltaTime);
        }
    }

    //поворот мышью
    void RotateMouse()
    {
        Vector2 toMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        bool rotateСlockwise = Vector2.Angle(transform.right, toMouse) < 90;
        float angleToMouse = Vector2.Angle(transform.up, toMouse);
        float rotationAngle = RotationSpeed * Time.deltaTime;

        if (angleToMouse < rotationAngle)
        {
            rotationAngle = angleToMouse;
        }
        transform.Rotate(0, 0, rotationAngle * (rotateСlockwise ? -1 : 1));
    }

    //ввод ускорения
    bool GoForward()
    {
        switch (CurrentInputType)
        {
            case InputType.Keyboard:
                return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
            case InputType.KeyboardMouse:
                return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetMouseButton(1);
        }
        return false;
    }

    //ввод выстрела
    bool NeedFire()
    {
        switch (CurrentInputType)
        {
            case InputType.Keyboard:
                return Input.GetKeyDown(KeyCode.Space);
            case InputType.KeyboardMouse:
                return Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
        }
        return false;
    }

    //выстрел
    void Fire()
    {
        AudioEffects.Play(FireClip);
        Bullet bullet = bulletsPool.GetObject();
        bullet.transform.position = transform.position;
        bullet.Fire(transform.up * BulletSpeed);
        lastShotTime = Time.time;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //не реагировать со своими пулями
        if (collision.gameObject.GetComponent<Bullet>() != null && collision.gameObject.tag == "ShipBullet")
            return;

        AudioEffects.Play(DeathClip);

        Statistics.HP--;
        if(Statistics.HP == 0)
        {
            GameStateManager.Lose();
            return;
        }

        Respawn();
    }

    public void Respawn()
    {
        transform.position = Vector3.zero;
        velocity = Vector2.zero;
        StartCoroutine(IgnoreDamage());
    }

    IEnumerator IgnoreDamage()
    {
        Color defColor = shipColor;
        Color transpColor = new Color(0, 0, 0, 0);

        collider.enabled = false;

        for (float i = 0; i < IgnoreDamageTime; i += 0.25f)
        {
            if (shipColor == transpColor)
                shipColor = defColor;
            else
                shipColor = transpColor;
            yield return new WaitForSeconds(0.25f);
        }
        shipColor = defColor;

        collider.enabled = true;
    }
}
