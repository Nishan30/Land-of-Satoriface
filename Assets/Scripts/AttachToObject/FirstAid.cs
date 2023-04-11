using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MoneyManager;
using static DetailsManager;

public class FirstAid : MonoBehaviour
{
    [SerializeField] string Item;
    [SerializeField] int price;
    [SerializeField] int healthGives;
    [SerializeField] KeyCode m_keyCode;
    [SerializeField] float hoursToRespawn;
    GameObject indicator;

    private void Start()
    {
        indicator = IndicatorManager.AddIndicator(transform, m_keyCode.ToString());
        indicator.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            indicator.SetActive(true);
        }
    }

    private void Update()
    {
        if (indicator.activeSelf && Input.GetKeyDown(m_keyCode))
        {
            if (DetailsManager.Instance.detailsPanel.activeSelf)
            {
                hideDetails();
            }
            else
            {
                ShowDetails(Item + " gives you " + healthGives + "% health" + "\n" + "needed " + price + " Coinconnect Cash");
                addButtonToDetails("Buy", () => buyFirstAid(), canBuy(true, price));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            indicator.SetActive(false);
            hideDetails();
        }
    }

    private void buyFirstAid()
    {
        if (Buy(true, price))
        {
            Player.instance.DealDamage(-healthGives);
            onGetPower();
        }
    }

    private void onGetPower()
    {
        Player.instance.delay(hoursToRespawn * 3600f, () => gameObject.SetActive(true));
        indicator.SetActive(false);
        hideDetails();
        gameObject.SetActive(false);
    }
}
