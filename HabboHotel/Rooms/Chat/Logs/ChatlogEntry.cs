using Neon.HabboHotel.Users;
using System;

namespace Neon.HabboHotel.Rooms.Chat.Logs
{
    public sealed class ChatlogEntry
    {
        private readonly int _playerId;
        private readonly int _roomId;
        private readonly string _message;
        private readonly double _timestamp;

        private readonly WeakReference _playerReference;
        private readonly WeakReference _roomReference;

        public ChatlogEntry(int PlayerId, int RoomId, string Message, double Timestamp, Habbo Player = null, RoomData Instance = null)
        {
            _playerId = PlayerId;
            _roomId = RoomId;
            _message = Message;
            _timestamp = Timestamp;

            if (Player != null)
            {
                _playerReference = new WeakReference(Player);
            }

            if (Instance != null)
            {
                _roomReference = new WeakReference(Instance);
            }
        }

        public int PlayerId => _playerId;

        public int RoomId => _roomId;

        public string Message => _message;

        public double Timestamp => _timestamp;

        public Habbo PlayerNullable()
        {
            if (_playerReference.IsAlive)
            {
                Habbo PlayerObj = (Habbo)_playerReference.Target;

                return PlayerObj;
            }

            return null;
        }

        public Room RoomNullable()
        {
            if (_roomReference.IsAlive)
            {
                Room RoomObj = (Room)_roomReference.Target;
                if (RoomObj.mDisposed)
                {
                    return null;
                }

                return RoomObj;
            }
            return null;
        }
    }
}
