using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    Queue<T> objects;

    public ObjectPool(GameObject prefab, int size)
    {
        objects = new Queue<T>();
        for (int i = 0; i < size; i++)
        {
            GameObject newObject = Object.Instantiate(prefab);
            objects.Enqueue(newObject.GetComponent<T>());
            newObject.SetActive(false);
        }
    }

    public T GetObject()
    {
        T firstObject = objects.Dequeue();
        objects.Enqueue(firstObject);
        firstObject.gameObject.SetActive(true);
        return firstObject;
    }
}
