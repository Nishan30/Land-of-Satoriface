using DG.Tweening;
using UnityEngine;

public class ItemShop : MonoBehaviour
{
    [SerializeField] S_Collections s_Collections;
    [SerializeField] KeyCode _keyCode = KeyCode.B;
    [SerializeField] float _range = 5, _indicatorZOffset = 2;
    GameObject indicator;
    private bool inRange = false, showedPanel = false;
    string shopCode => this.GetType().ToString() + gameObject.GetInstanceID();
    private void Awake()
    {
        loadCollection();
    }
    private void Start()
    {
        GameObject go = new();
        go.transform.SetParent(transform);
        go.transform.position = transform.position + new Vector3(0, _indicatorZOffset, 0);
        indicator = IndicatorManager.AddIndicator(go.transform, _keyCode.ToString());
    }

    private void Update()
    {
        bool lastInRange = inRange;
        inRange = Vector3.Distance(transform.position, Player.CurrentPlayer.position) < _range;
        indicator.SetActive(inRange);
        if (inRange && Input.GetKeyDown(_keyCode))
        {
            if (showedPanel)
            {
                Tradingmanager.Instance.enableTradingScreen(false);
                Tradingmanager.Instance.SetSellItemPanel(false);
                showedPanel = false;
            }
            else
            {
                Tradingmanager.Instance.enableTradingScreen(true);
                Tradingmanager.Instance.SetSellItemPanel(true);
                Tradingmanager.removeAllItemFromSale();
                Tradingmanager.listItemsOnSale(s_Collections.ItemsCanCollect, saveCollection);
                showedPanel = true;
            }
        }
        if (lastInRange == true && inRange == false && showedPanel == true)
        {
            showedPanel = false;
            Tradingmanager.Instance.enableTradingScreen(false);
            Tradingmanager.Instance.SetSellItemPanel(false);

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 constPos = new Vector3(_range, 8, _range);
        Gizmos.DrawWireCube(transform.position + new Vector3(0, constPos.y, 0) / 2, constPos);
        Gizmos.DrawWireCube(transform.position + new Vector3(0, constPos.y * 0.98f, 0) / 2, constPos * 0.98f);
        Gizmos.DrawWireCube(transform.position + new Vector3(0, constPos.y * 0.96f, 0) / 2, constPos * 0.96f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _range);
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, _indicatorZOffset, 0), 0.2f);
    }

    private void saveCollection()
    {
        PlayerPrefs.SetString(shopCode, JsonUtility.ToJson(s_Collections));
    }

    private void loadCollection()
    {
        if (PlayerPrefs.HasKey(shopCode))
        {
            s_Collections = Instantiate(s_Collections);
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(shopCode), s_Collections);
        }
        else
        {
            s_Collections = Instantiate(s_Collections);
        }
    }



}