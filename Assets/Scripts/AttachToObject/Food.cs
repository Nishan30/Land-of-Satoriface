using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MoneyManager;
using static DetailsManager;

public class Food : MonoBehaviour
{
    [SerializeField] string foodItem;
    [SerializeField] int price;
    [SerializeField] int energyInPercentage;
    [SerializeField] bool Needed_Coinconnect_Cash = true;
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
                ShowDetails(foodItem + " gives you " + energyInPercentage + "% energy" + "\n" + "needed " + price + ((Needed_Coinconnect_Cash) ? " Coinconnect Cash" : "Goldenbit"));
                addButtonToDetails("Buy", () => buyFood(), canBuy(Needed_Coinconnect_Cash, price));
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


    private void buyFood()
    {
        if (Buy(Needed_Coinconnect_Cash, price))
        {
            Hunger.Instance.getEnergy((float)energyInPercentage / 100f);
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
