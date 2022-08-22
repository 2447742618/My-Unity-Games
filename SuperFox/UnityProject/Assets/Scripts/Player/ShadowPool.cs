using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPool : MonoBehaviour
{
    public static ShadowPool Instance;

    public GameObject shadowPrefabs;

    private int shadowCount = 10;

    Queue<GameObject> availableObjects = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        FillPool();
    }

    private void Start()
    {
        shadowCount = 10;
    }

    private void FillPool()
    {
        for (int i = 0; i < shadowCount; i++)
        {
            var newShadow = Instantiate(shadowPrefabs);
            newShadow.transform.SetParent(transform);

            ReturnPool(newShadow);
        }
    }

    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        availableObjects.Enqueue(gameObject);
    }

    public GameObject GetFromPool()
    {
        if (availableObjects.Count == 0) FillPool();//防止数量不够
        var gameObject = availableObjects.Dequeue();

        gameObject.SetActive(true);
        return gameObject;
    }
}
