using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableTracker : MonoBehaviour
{
    public enum collectableType { UNASSIGNED, SONG_1, SONG_2, SONG_3, SONG_4, NUM_TYPES}
    Dictionary<collectableType, int> collectIndex;

    void Start()
    {
        initDictionary();
    }

    void initDictionary()
    {
        collectIndex = new Dictionary<collectableType, int>();

        collectIndex.Add(collectableType.UNASSIGNED, 0);
        collectIndex.Add(collectableType.SONG_1 , 0);
        collectIndex.Add(collectableType.SONG_2, 0);
        collectIndex.Add(collectableType.SONG_3, 0);
        collectIndex.Add(collectableType.SONG_4, 0);
    }

    public void addCollectable(collectableType itemType)
    {
        collectIndex[itemType] += 1;
    }

    public int getNumCollectable(collectableType itemType)
    {
        return collectIndex[itemType];
    }
}
