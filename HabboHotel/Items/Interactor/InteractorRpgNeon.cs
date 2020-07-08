using Neon.Communication.Packets.Outgoing.Rooms.Chat;
using Neon.HabboHotel.Rooms;
using System;

namespace Neon.HabboHotel.Items.Interactor
{
    internal class InteractorRpgNeon : IFurniInteractor
    {
        public void OnPlace(GameClients.GameClient Session, Item Item)
        {
            Item.ExtraData = "0";
            Item.UpdateState(true, true);
        }

        public void OnRemove(GameClients.GameClient Session, Item Item)
        {
        }

        public void OnTrigger(GameClients.GameClient Session, Item Item, int Request, bool HasRights)
        {
            if (Session == null || Session.GetHabbo() == null || Item == null)
            {
                return;
            }

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
            {
                return;
            }

            RoomUser Actor = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (Actor == null)
            {
                return;
            }

            int tick = int.Parse(Item.ExtraData);

            if (tick < 23)
            {
                if (Actor.CurrentEffect == 27)
                {
                    if (Gamemap.TileDistance(Actor.X, Actor.Y, Item.GetX, Item.GetY) < 2)
                    {
                        RoomUser ThisUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Username);
                        Random random = new Random();
                        tick++;
                        Item.ExtraData = tick.ToString();
                        Item.UpdateState(true, true);
                        int X = Item.GetX, Y = Item.GetY, Rot = Item.Rotation;
                        double Z = Item.GetZ;
                        int randomNumber = random.Next(1, 9);
                        if (randomNumber == 1)
                        {
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "[Boss] " + "Contraataque: Estilo Tierra, suelo de agua púrpura - en [" + ThisUser.GetUsername().ToString() + "]", 0, 34));
                            Session.GetHabbo().Effects().ApplyEffect(185);
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Estoy atrapado en un suelo de agua púrpura.*", 0, 6));
                            System.Threading.Thread.Sleep(3000);
                            Session.GetHabbo().Effects().ApplyEffect(27);
                        }
                        if (randomNumber == 2)
                        {
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "[Boss] " + "Contraataque: Estilo Vacío, explosión de bombas - en [" + ThisUser.GetUsername().ToString() + "]", 0, 34));
                            Session.GetHabbo().Effects().ApplyEffect(108);
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*AIIIIIII Fui golpeado por una bomba*", 0, 6));
                            System.Threading.Thread.Sleep(3000);
                            Session.GetHabbo().Effects().ApplyEffect(27);
                        }
                        if (randomNumber == 3)
                        {
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "[Boss] " + "Contraataque: Estilo Cura, lechón de vida - en [" + ThisUser.GetUsername().ToString() + "]", 0, 34));
                            Session.GetHabbo().Effects().ApplyEffect(23);
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Mi vida está siendo succionada!", 0, 6));
                            System.Threading.Thread.Sleep(3000);
                            Session.GetHabbo().Effects().ApplyEffect(27);
                            tick--;
                            tick--;
                            Item.ExtraData = tick.ToString();
                            Item.UpdateState(true, true);
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "[Boss] " + "Tomé 50% de la vida de [" + ThisUser.GetUsername().ToString() + "] para curarme 2%", 0, 34));
                        }
                        if (randomNumber == 4)
                        {
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "[Boss] " + "Eso duele [" + ThisUser.GetUsername().ToString() + "]", 0, 34));
                        }
                        if (randomNumber == 5)
                        {
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "[Boss] " + "Hasta que no és malo [" + ThisUser.GetUsername().ToString() + "]", 0, 34));
                        }
                        if (randomNumber == 6)
                        {
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "[Boss] " + "Eres fuerte, incluso me recuerdas a una persona mayor. [" + ThisUser.GetUsername().ToString() + "]", 0, 34));
                        }
                        if (randomNumber == 7)
                        {
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "[Boss] " + "¿Cómo te atreves a enfrentarme [" + ThisUser.GetUsername().ToString() + "]?", 0, 34));
                        }
                        if (randomNumber == 8)
                        {
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "[Boss] " + "Hasta que no és malo [" + ThisUser.GetUsername().ToString() + "]", 0, 34));
                        }
                        if (randomNumber == 9)
                        {
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "[Boss] " + "Contraataque: Estilo Cura, lechón de vida - en [" + ThisUser.GetUsername().ToString() + "]", 0, 34));
                            Session.GetHabbo().Effects().ApplyEffect(23);
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Mi vida está siendo succionada!", 0, 6));
                            System.Threading.Thread.Sleep(3000);
                            Session.GetHabbo().Effects().ApplyEffect(27);
                            tick--;
                            tick--;
                            tick--;
                            tick--;
                            tick--;
                            tick--;
                            tick--;
                            Item.ExtraData = tick.ToString();
                            Item.UpdateState(true, true);
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "[Boss] " + "Tomé 80% de la vida de [" + ThisUser.GetUsername().ToString() + "] para curarme 10%", 0, 34));
                        }

                        if (tick == 19)
                        {
                            NeonEnvironment.GetGame().GetPinataManager().ReceiveCrackableReward(Actor, Room, Item);
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "[Boss] " + "Fui derrotado por un simple humano! WINS: [" + ThisUser.GetUsername().ToString() + "]", 0, 34));
                        }
                    }
                }
                else
                {
                    Session.SendWhisper("¡Vaya, no tienes el equipo de lucha! escriba el comando [:enable 27]");
                    return;
                }
            }
        }

        public void OnWiredTrigger(Item Item)
        {

        }
    }
}
