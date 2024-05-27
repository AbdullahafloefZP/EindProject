using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EjectedBullet : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("DeactivateBullet", 0.5f);
    }

    private void DeactivateBullet()
    {
        gameObject.SetActive(false);
    }
}