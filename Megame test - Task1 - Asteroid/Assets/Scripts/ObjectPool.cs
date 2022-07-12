using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    Queue<T> objects;

    GameObject prefab;

    public ObjectPool(GameObject prefab, int size)
    {
        objects = new Queue<T>();
        this.prefab = prefab;
        for (int i = 0; i < size; i++)
        {
            GameObject newObject = Object.Instantiate(prefab);
            objects.Enqueue(newObject.GetComponent<T>());
            newObject.SetActive(false);
        }
    }

    public T GetObject()
    {
        if (objects.Peek().gameObject.activeSelf)
        {
            ExpandPool();
        }

        T firstObject = objects.Dequeue();
        objects.Enqueue(firstObject);
        firstObject.gameObject.SetActive(true);
        return firstObject;
    }

    void ExpandPool()
    {
        T[] oldObjects = objects.ToArray();

        objects.Clear();
        for (int i = 0; i < oldObjects.Length; i++)
        {
            GameObject newObject = Object.Instantiate(prefab);
            objects.Enqueue(newObject.GetComponent<T>());
            newObject.SetActive(false);
        }

        for (int i = 0; i < oldObjects.Length; i++)
        {
            objects.Enqueue(oldObjects[i]);
        }
    }
}
