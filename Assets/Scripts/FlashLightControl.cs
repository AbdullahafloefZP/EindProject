using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightControl : MonoBehaviour
{
    [SerializeField] private Transform flashLight;

    private void Awake()
    {
        flashLight.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (flashLight.gameObject.activeSelf)
            {
                flashLight.gameObject.SetActive(false);
            }
            else
            {
                flashLight.gameObject.SetActive(true);
            }
        }

        if (transform.localScale.x < 0)
                {
                    flashLight.localEulerAngles = new Vector3(0, 0, -90); 
                }
                else
                {
                    flashLight.localEulerAngles = new Vector3(0, 0, 90);
                }
    }
}
