using ExitGames.Client.Photon;
using GorillaNetworking;
using Photon.Pun;

namespace GorillaXS
{
    internal class RoomNotifications : MonoBehaviourPunCallbacks
    {
        private NetworkSystem networkSystem;

        private bool isSafety;

        private void Awake()
        {
            networkSystem = NetworkSystem.Instance;

            networkSystem.OnMultiplayerStarted += RoomJoined;
            networkSystem.OnReturnedToSinglePlayer += RoomLeft;
            networkSystem.OnPlayerJoined += PlayerJoined;
            networkSystem.OnPlayerLeft += PlayerLeft;

            isSafety = PlayFabAuthenticator.instance.GetSafety();
        }

        private void RoomJoined()
        {
            GorillaXS.PushNotification("Room Joined", string.Format("You have joined room {0} with {1}/{2} players", networkSystem.RoomName, networkSystem.RoomPlayerCount, PhotonNetworkController.Instance.GetRoomSize(networkSystem.GameModeString)));
        }

        private void RoomLeft()
        {
            GorillaXS.PushNotification("Room Left", "You have left the room");
        }

        private void PlayerJoined(int actorNumber)
        {
            NetPlayer player = networkSystem.GetPlayer(actorNumber);
            GorillaXS.PushNotification("Player Joined", string.Format("{0} has joined the room", isSafety ? player.DefaultName : player.NickName));
        }

        private void PlayerLeft(int actorNumber)
        {
            NetPlayer player = networkSystem.GetPlayer(actorNumber);
            GorillaXS.PushNotification("Player Left", string.Format("{0} has left the room", isSafety ? player.DefaultName : player.NickName));
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            base.OnRoomPropertiesUpdate(propertiesThatChanged);

            GorillaXS.PushNotification("Room Update", "The room properties have been updated", icon: "warning");
        }
    }
}
