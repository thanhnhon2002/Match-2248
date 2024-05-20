using UnityEngine;
using UnityEngine.UI;

public class ButtonClaimReward : MonoBehaviour
{
    public MoveLeftRight player;
    public DiamondGroup diamondGroup;
    public void OnClickClaim()
    {
        UIManager.Instance.SpawnEffectReward(transform);
        diamondGroup.AddDiamond(player.multiple * player.multiple, false);
    }
}