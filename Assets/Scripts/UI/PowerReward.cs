using TMPro;
using UnityEngine;

public class PowerReward : MonoBehaviour
{
    const float TIME_SHOW =60f;
    float timePowerReward;
    [SerializeField] TextMeshProUGUI textReward;
    float timeShowPower;
    BounceLoop bounceLoop;
    [SerializeField] GameObject partical;
    private void Awake()
    {
        bounceLoop= GetComponent<BounceLoop>(); 
    }
    void Start()
    {
        SetTimePowerReward();
        partical.SetActive(false);
        bounceLoop.enabled = false;
    }
    public bool IsShowing()
    {
        return timeShowPower > 0;
    }
    void Update()
    {
        if(Time.time > timePowerReward&&timeShowPower==0)
        {
            timeShowPower = TIME_SHOW + Time.time;
            textReward.text = "+80";
            bounceLoop.enabled = true;
            partical.SetActive(true);
        }
        if(Time.time>timeShowPower&&timeShowPower>0)
        {
            textReward.text = "+20";
            bounceLoop.enabled = false;
            partical.SetActive(false);
            SetTimePowerReward();
        }
        
    }
    void SetTimePowerReward()
    {
        timeShowPower = 0;
        timePowerReward = Time.time + FirebaseManager.remoteConfig.TIME_POWER_REWARD;
    }
}
