using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class SpawnList : MonoBehaviour
{
    private List<GameObject> spawns;
    private Spawner spawner;

    private void Awake()
    {
        spawns = GameObject.FindGameObjectsWithTag("SpawnPoint").ToList<GameObject>();
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject obj, sp;
        int i = 0;

        while (spawns.Count > 0 && !spawner.IsEmpty())
        {
            sp = spawns[i];

            obj = spawner.GetRandom();

            Instantiate(obj, sp.transform.position, quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
