using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthUIController : MonoBehaviour
{
    public GameObject heartContainer;
    private float fillValue;
    [SerializeField] private Player plyrRef;

    // Update is called once per frame
    void Update()
    {
        fillValue = (float)plyrRef.Health;
        fillValue = fillValue / plyrRef.MaxHealth;
        heartContainer.GetComponent<Image>().fillAmount = fillValue;
    }
}
