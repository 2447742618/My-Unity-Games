using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballPool : MonoBehaviour
{
    public static FireballPool Instance;

    public GameObject fireballPrefabs;

    public int fireballCount = 8;

    public AudioSource burningAudio;
    Queue<GameObject> availableObjects = new Queue<GameObject>();

    bool burning;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        burningAudio = GetComponent<AudioSource>();
        FillPool();
    }

    private void Start()
    {
        fireballCount = 8;
        burning = false;
    }

    private void Update()
    {
        if (availableObjects.Count == fireballCount && burning) StartCoroutine("AudioDisappear");
    }

    private void FillPool()
    {
        for (int i = 0; i < fireballCount; i++)
        {
            var newFireball = Instantiate(fireballPrefabs);
            newFireball.transform.SetParent(transform);

            ReturnPool(newFireball);
        }
    }

    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        availableObjects.Enqueue(gameObject);
    }

    public GameObject GetFromPool()
    {
        if (burning == false) StopCoroutine("AudioDisappear");
        burning = true;

        if (availableObjects.Count == 0) FillPool();//防止数量不够
        var gameObject = availableObjects.Dequeue();

        gameObject.SetActive(true);
        return gameObject;
    }

    IEnumerator AudioDisappear()
    {
        burning = false;
        float percent = 1;

        while (percent > 0)
        {
            percent -= 0.01f;
            burningAudio.volume = percent;
            yield return new WaitForFixedUpdate();
        }

        burningAudio.Stop();
        yield break;
    }

}
