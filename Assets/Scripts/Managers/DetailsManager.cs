using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DetailsManager : MonoBehaviour
{
    public static DetailsManager Instance;
    [SerializeField] internal GameObject detailsPanel;
    [SerializeField] TextMeshProUGUI detailsText;
    [SerializeField] Button ButtonPrefab;
    [SerializeField] Transform buttonContainer;

    private void Awake()
    {
        Instance = this;
        detailsPanel.SetActive(false);
    }

    public static void ShowDetails(string str)
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        foreach (Transform item in Instance.buttonContainer)
        {
            Destroy(item.gameObject);
        }
        Instance.detailsPanel.SetActive(true);
        Instance.detailsText.text = str;
    }


    public static void hideDetails()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        foreach (Transform item in Instance.buttonContainer)
        {
            Destroy(item.gameObject);
        }
        Instance.detailsPanel.SetActive(false);
    }


    public static void addButtonToDetails(string text, Action a, bool Enabled)
    {
        var NewButton = Instantiate(Instance.ButtonPrefab, Instance.buttonContainer);
        NewButton.interactable = Enabled;
        if (Enabled)
            NewButton.onClick.AddListener(() => a());
        NewButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (Enabled) ? text : "<color=grey>" + text;
    }
}
