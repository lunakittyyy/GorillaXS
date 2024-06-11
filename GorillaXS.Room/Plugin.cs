using BepInEx;
using ExitGames.Client.Photon;
using GorillaNetworking;
using Photon.Realtime;

namespace GorillaXS.Room
{
    [BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
    internal class Plugin : BaseUnityPlugin, IInRoomCallbacks
    {
        private NetworkSystem networkSystem;

        private bool isSafety;

        private void Awake()
        {
            GorillaTagger.OnPlayerSpawned(Initialize);
        }

        private void Initialize()
        {
            networkSystem = NetworkSystem.Instance;

            networkSystem.OnMultiplayerStarted += RoomJoined;
            networkSystem.OnReturnedToSinglePlayer += RoomLeft;

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

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            GorillaXS.PushNotification("Player Joined", string.Format("{0} has joined the room", isSafety ? newPlayer.DefaultName : newPlayer.NickName));
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            GorillaXS.PushNotification("Player Left", string.Format("{0} has left the room", isSafety ? otherPlayer.DefaultName : otherPlayer.NickName));
        }

        public void OnMasterClientSwitched(Player newMasterClient)
        {
            GorillaXS.PushNotification("Room Update", string.Format("The master client has been switched to {0}", isSafety ? newMasterClient.DefaultName : newMasterClient.NickName), icon: "warning");
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            GorillaXS.PushNotification("Room Update", "The room properties have been updated", icon: "warning");
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            // nothing
        }
    }
}
