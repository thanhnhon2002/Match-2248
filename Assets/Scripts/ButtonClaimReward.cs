using UnityEngine;
using UnityEngine.UI;

public class ButtonClaimReward : MonoBehaviour
{
    Button button;
    public MoveLeftRight player;
    public DiamondGroup diamondGroup;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickClaim);
    }
    void OnClickClaim()
    {
        diamondGroup.AddDiamond(player.multiple*5);
    }
}