using UnityEngine;

public class Hunger : MonoBehaviour
{
    public static Hunger Instance;
    [SerializeField] float hoursForHunger, hoursToReachDeath;
    [SerializeField] Transform hungerBar;
    float seconds;
    private void Awake()
    {
        Instance = this;
        seconds = hoursForHunger * 3600f;
    }
    private void Update()
    {
        seconds -= Time.deltaTime;
        float factor = seconds / (hoursForHunger * 3600f);
        factor = Mathf.Clamp01(factor);
        hungerBar.localScale = new Vector3(1, factor, 1);
        if (factor <= 0)
        {
            Player.instance.dealDamagerPerHour(hoursToReachDeath);
        }
    }

    public void getEnergy(float t)
    {
        float hoursToSeconds = hoursForHunger * 3600f;
        seconds = Mathf.Clamp(Mathf.Lerp(0, hoursToSeconds, t) + seconds, 0, hoursToSeconds);
    }
}
