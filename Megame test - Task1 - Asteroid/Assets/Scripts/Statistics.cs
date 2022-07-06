using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    [SerializeField] Text PointsText;
    [SerializeField] Text HPText;

    static Statistics instance;

    void Start()
    {
        instance = this;
    }

    public static int Points
    {
        get
        {
            return int.Parse(instance.PointsText.text);
        }
        set
        {
            instance.PointsText.text = value.ToString();
        }
    }
    public static int HP
    {
        get
        {
            return int.Parse(instance.HPText.text);
        }
        set
        {
            instance.HPText.text = value.ToString();
        }
    }


    public static void Reset()
    {
        Points = 0;
        HP = 5;
    }
}
