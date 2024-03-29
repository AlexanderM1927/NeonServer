﻿#region

using Neon.Communication.Packets.Incoming;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Users;
using System.Collections.Concurrent;
using System.Linq;

#endregion

namespace Neon.HabboHotel.Items.Wired.Boxes.Triggers
{
    internal class ScoreAchievedBox : IWiredItem
    {
        public ScoreAchievedBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            StringData = "";
            SetItems = new ConcurrentDictionary<int, Item>();
        }

        public Room Instance { get; set; }
        public Item Item { get; set; }

        public WiredBoxType Type => WiredBoxType.TriggerScoreAchieved;

        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public void HandleSave(ClientPacket Packet)
        {
            _ = Packet.PopInt();
            int OwnerOnly = Packet.PopInt();
            string Message = Packet.PopString();

            BoolData = OwnerOnly == 1;
            StringData = Message;
        }

        public bool Execute(params object[] Params)
        {
            Habbo Player = (Habbo)Params[0];
            if (Player == null)
            {
                return false;
            }

            int.TryParse(StringData, out int scoreToGet);
            RoomUser User = Player.CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Player.Username);

            if (Instance.GetGameManager().Points[(int)User.Team] < scoreToGet)
            {
                return false;
            }

            Player.WiredInteraction = true;
            System.Collections.Generic.ICollection<IWiredItem> Effects = Instance.GetWired().GetEffects(this);
            System.Collections.Generic.ICollection<IWiredItem> Conditions = Instance.GetWired().GetConditions(this);

            foreach (IWiredItem Condition in Conditions.ToList())
            {
                if (!Condition.Execute(Player))
                {
                    return false;
                }

                Instance.GetWired().OnEvent(Condition.Item);
            }

            bool HasRandomEffectAddon =
                Effects.Where(x => x.Type == WiredBoxType.AddonRandomEffect).ToList().Any();
            if (HasRandomEffectAddon)
            {
                //Okay, so we have a random addon effect, now lets get the IWiredItem and attempt to execute it.
                IWiredItem RandomBox = Effects.FirstOrDefault(x => x.Type == WiredBoxType.AddonRandomEffect);
                if (!RandomBox.Execute())
                {
                    return false;
                }

                //Success! Let's get our selected box and continue.
                IWiredItem SelectedBox = Instance.GetWired().GetRandomEffect(Effects.ToList());
                if (!SelectedBox.Execute())
                {
                    return false;
                }

                //Woo! Almost there captain, now lets broadcast the update to the room instance.
                Instance?.GetWired().OnEvent(RandomBox.Item);
                Instance?.GetWired().OnEvent(SelectedBox.Item);
            }
            else
            {
                foreach (IWiredItem Effect in Effects.ToList())
                {
                    if (!Effect.Execute(Player))
                    {
                        return false;
                    }

                    Instance.GetWired().OnEvent(Effect.Item);
                }
            }

            return true;
        }
    }
}