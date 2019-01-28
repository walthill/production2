using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    private float Timer;

    public Transform itemSpawn;

    public GameObject item;

    [Header("Spawner Settings")]
    public int ArrayPoint = 0;
    


    //array of booleans

    public bool[] lane0 = new bool[3];


    // Start is called before the first frame update
    void Start()
    {
        itemSpawn = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if(Timer>1)
        {
            if (lane0[ArrayPoint])
            {
                GameObject tar = Instantiate(item, itemSpawn.position, itemSpawn.rotation) as GameObject;
                Timer = 0;
            }
            else
            {
                Timer = 0;
            }
            //Debug.Log("yeet yeet");
            ArrayPoint++;


        }
        if (ArrayPoint > lane0.Length - 1)
        {
            ArrayPoint = 0;
        }
        
        

    }

    



}
