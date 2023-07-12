using Unity.Services.Lobbies.Models;

namespace MainMenu
{
    public static class LobbyEvents
    {
        public delegate void LobbyUpdated(Lobby lobby);
        public static LobbyUpdated OnLobbyUpdated;

        public delegate void LobbyReady();
        public static LobbyReady OnLobbyReady;
        
        public delegate void LobbyUnReady();
        public static LobbyUnReady OnLobbyUnReady;
    }
}