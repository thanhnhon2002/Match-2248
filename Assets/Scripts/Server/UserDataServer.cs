using UnityEngine;

[System.Serializable]
public class UserDataServer
{
    public string nickName;
    public string idGuest;
    public string idGoogle;
    public string idFacebook;
    public string idApple;
    public int indexPlayer;
    public int maxIndex;
    public int avatarIndex;

    public void CopyFromLocalData()
    {
        if (GameSystem.userdata == null)
        {
            Debug.LogError("user data currently is null");
            return;
        }
        if (GameSystem.userdata.gameData == null)
        {
            Debug.LogError("user game data null");
            return;
        }
        this.maxIndex = GameSystem.userdata.gameData.maxIndex;
        this.indexPlayer = GameSystem.userdata.gameData.indexPlayer;
        this.nickName = GameSystem.userdata.nickName;
        this.avatarIndex = GameSystem.userdata.avatarIndex;
    }
}
