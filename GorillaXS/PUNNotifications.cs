using Photon.Pun;
using Photon.Realtime;

namespace GorillaXS
{
    internal class PUNNotifications : MonoBehaviourPunCallbacks
    {
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);

            Plugin.Notify("Player Joined", $"{newPlayer.NickName} joined");
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);

            Plugin.Notify("Player Left", $"{otherPlayer.NickName} left");
        }
    }
}
