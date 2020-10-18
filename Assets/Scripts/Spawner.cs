using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> objects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetRandom()
    {
        int index = Random.Range(0, objects.Count);
        GameObject obj = objects[index];
        objects.RemoveAt(index);
        return obj;
    }

    public bool IsEmpty()
    {
        return objects.Count > 0 ? false : true;
    }
}
