using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockAnimScript : MonoBehaviour
{
    public void DeactivateThis()
    {
        gameObject.SetActive(false);
    }
}
