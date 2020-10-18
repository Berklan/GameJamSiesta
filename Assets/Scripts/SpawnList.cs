using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpawnList : MonoBehaviour
{
    private GameObject[] spawns;
    private Spawner spawner;

    private void Awake()
    {
        spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject obj, sp;
        int i = 0;

        while (i < spawns.Length && !spawner.IsEmpty())
        {
            sp = spawns[i];
            obj = spawner.GetRandom();

            Instantiate(obj, sp.transform.position, quaternion.identity);

            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
