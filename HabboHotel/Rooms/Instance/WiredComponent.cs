﻿using Neon.Database.Interfaces;
using Neon.HabboHotel.Items;
using Neon.HabboHotel.Items.Wired;
using Neon.HabboHotel.Items.Wired.Boxes.Add_ons;
using Neon.HabboHotel.Items.Wired.Boxes.Conditions;
using Neon.HabboHotel.Items.Wired.Boxes.Effects;
using Neon.HabboHotel.Items.Wired.Boxes.Triggers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace Neon.HabboHotel.Rooms.Instance
{
    public class WiredComponent
    {
        private readonly Room _room;
        private readonly ConcurrentDictionary<int, IWiredItem> _wiredItems;

        public WiredComponent(Room Instance)//, RoomItem Items)
        {
            _room = Instance;
            _wiredItems = new ConcurrentDictionary<int, IWiredItem>();
        }

        public void OnCycle()
        {
            DateTime Start = DateTime.Now;
            foreach (KeyValuePair<int, IWiredItem> Item in _wiredItems.ToList())
            {
                Item SelectedItem = _room.GetRoomItemHandler().GetItem(Item.Value.Item.Id);

                if (SelectedItem == null)
                {
                    TryRemove(Item.Key);
                }

                if (Item.Value is IWiredCycle)
                {
                    IWiredCycle Cycle = (IWiredCycle)Item.Value;

                    if (Cycle.TickCount <= 0)
                    {
                        Cycle.OnCycle();
                    }
                    else
                    {
                        Cycle.TickCount--;
                    }
                }
            }
            TimeSpan Span = (DateTime.Now - Start);
            if (Span.Milliseconds > 400)
            {
                //log.Warn("<Room " + _room.Id + "> Wired took " + Span.TotalMilliseconds + "ms to execute - Rooms lagging behind");
            }
        }

        public IWiredItem LoadWiredBox(Item Item)
        {
            IWiredItem NewBox = GenerateNewBox(Item);

            DataRow Row = null;
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM wired_items WHERE id=@id LIMIT 1");
                dbClient.AddParameter("id", Item.Id);
                Row = dbClient.getRow();

                if (Row != null)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(Row["string"])))
                    {
                        if (NewBox.Type == WiredBoxType.ConditionMatchStateAndPosition || NewBox.Type == WiredBoxType.ConditionDontMatchStateAndPosition)
                        {
                            NewBox.StringData = "0;0;0";
                        }
                        else if (NewBox.Type == WiredBoxType.ConditionUserCountInRoom || NewBox.Type == WiredBoxType.ConditionUserCountDoesntInRoom)
                        {
                            NewBox.StringData = "0;0";
                        }
                        else if (NewBox.Type == WiredBoxType.ConditionFurniHasNoFurni)
                        {
                            NewBox.StringData = "0";
                        }
                        else if (NewBox.Type == WiredBoxType.EffectMatchPosition)
                        {
                            NewBox.StringData = "0;0;0";
                        }
                        else if (NewBox.Type == WiredBoxType.EffectMoveToDir)
                        {
                            NewBox.StringData = "0;0";
                        }
                        else if (NewBox.Type == WiredBoxType.EffectMoveAndRotate)
                        {
                            NewBox.StringData = "0;0";
                        }
                    }

                    NewBox.StringData = Convert.ToString(Row["string"]);
                    NewBox.BoolData = Convert.ToInt32(Row["bool"]) == 1;
                    NewBox.ItemsData = Convert.ToString(Row["items"]);

                    if (NewBox is IWiredCycle)
                    {
                        IWiredCycle Box = (IWiredCycle)NewBox;
                        Box.Delay = Convert.ToInt32(Row["delay"]);
                    }

                    foreach (string str in Convert.ToString(Row["items"]).Split(';'))
                    {
                        string sId = "0";

                        if (str.Contains(':'))
                        {
                            sId = str.Split(':')[0];
                        }

                        if (int.TryParse(str, out int Id) || int.TryParse(sId, out Id))
                        {
                            Item SelectedItem = _room.GetRoomItemHandler().GetItem(Convert.ToInt32(Id));
                            if (SelectedItem == null)
                            {
                                continue;
                            }

                            NewBox.SetItems.TryAdd(SelectedItem.Id, SelectedItem);
                        }
                    }
                }
                else
                {
                    NewBox.ItemsData = "";
                    NewBox.StringData = "";
                    NewBox.BoolData = false;
                    SaveBox(NewBox);
                }
            }

            if (!AddBox(NewBox))
            {
                // ummm
            }
            return NewBox;
        }

        public IWiredItem GenerateNewBox(Item Item)
        {
            switch (Item.GetBaseItem().WiredType)
            {
                case WiredBoxType.TriggerRoomEnter:
                    return new RoomEnterBox(_room, Item);
                case WiredBoxType.TriggerRepeat:
                    return new RepeaterBox(_room, Item);
                case WiredBoxType.TriggerLongRepeat:
                    return new LongTriggerBox(_room, Item);
                case WiredBoxType.TriggerStateChanges:
                    return new StateChangesBox(_room, Item);
                case WiredBoxType.TriggerUserSays:
                    return new UserSaysBox(_room, Item);
                case WiredBoxType.TriggerBotReachedAvatar:
                    return new BoxReachedAvatarBox(_room, Item);
                case WiredBoxType.TriggerWalkOffFurni:
                    return new UserWalksOffBox(_room, Item);
                case WiredBoxType.TriggerWalkOnFurni:
                    return new UserWalksOnBox(_room, Item);
                case WiredBoxType.TriggerGameStarts:
                    return new GameStartsBox(_room, Item);
                case WiredBoxType.TriggerGameEnds:
                    return new GameEndsBox(_room, Item);
                case WiredBoxType.TriggerUserFurniCollision:
                    return new UserFurniCollision(_room, Item);
                case WiredBoxType.TriggerUserSaysCommand:
                    return new UserSaysCommandBox(_room, Item);
                case WiredBoxType.TriggerScoreAchieved:
                    return new ScoreAchievedBox(_room, Item);

                case WiredBoxType.EffectShowMessage:
                    return new ShowMessageBox(_room, Item);
                case WiredBoxType.SendCustomMessageBox:
                    return new SendCustomMessageBox(_room, Item);
                case WiredBoxType.EffectProgressUserAchievement:
                    return new ProgressUserAchievementBox(_room, Item);
                case WiredBoxType.EffectTeleportToFurni:
                    return new TeleportUserBox(_room, Item);
                case WiredBoxType.EffectToggleNegativeFurniState:
                    return new ToggleNegativeFurniBox(_room, Item);
                case WiredBoxType.EffectToggleFurniState:
                    return new ToggleFurniBox(_room, Item);
                case WiredBoxType.EffectMoveAndRotate:
                    return new MoveAndRotateBox(_room, Item);
                case WiredBoxType.EffectKickUser:
                    return new KickUserBox(_room, Item);
                case WiredBoxType.EffectMuteTriggerer:
                    return new MuteTriggererBox(_room, Item);

                case WiredBoxType.EffectGiveReward:
                    return new GiveRewardBox(_room, Item);
                case WiredBoxType.EffectTimerReset:
                    return new EffectTimerResetBox(_room, Item);

                case WiredBoxType.TriggerAtGivenTime:
                    return new AtGivenTimeBox(_room, Item);

                case WiredBoxType.EffectMatchPosition:
                    return new MatchPositionBox(_room, Item);
                case WiredBoxType.EffectAddActorToTeam:
                    return new AddActorToTeamBox(_room, Item);
                case WiredBoxType.EffectRemoveActorFromTeam:
                    return new RemoveActorFromTeamBox(_room, Item);

                case WiredBoxType.EffectMoveToDir:
                    return new MoveToDirBox(_room, Item);
                /*
                
                case WiredBoxType.EffectMoveFurniToNearestUser:
                    return new MoveFurniToNearestUserBox(_room, Item);
                case WiredBoxType.EffectMoveFurniFromNearestUser:
                    return new MoveFurniFromNearestUserBox(_room, Item);

                   */

                case WiredBoxType.EffectAddScore:
                    return new AddScoreBox(_room, Item);
                case WiredBoxType.EffectAddRewardPoints:
                    return new AddRewardPoints(_room, Item);

                case WiredBoxType.ConditionFurniHasUsers:
                    return new FurniHasUsersBox(_room, Item);
                case WiredBoxType.ConditionTriggererOnFurni:
                    return new TriggererOnFurniBox(_room, Item);
                case WiredBoxType.ConditionTriggererNotOnFurni:
                    return new TriggererNotOnFurniBox(_room, Item);
                case WiredBoxType.ConditionFurniHasNoUsers:
                    return new FurniHasNoUsersBox(_room, Item);
                case WiredBoxType.ConditionFurniHasFurni:
                    return new FurniHasFurniBox(_room, Item);
                case WiredBoxType.ConditionIsGroupMember:
                    return new IsGroupMemberBox(_room, Item);
                case WiredBoxType.ConditionIsNotGroupMember:
                    return new IsNotGroupMemberBox(_room, Item);
                case WiredBoxType.ConditionUserCountInRoom:
                    return new UserCountInRoomBox(_room, Item);
                case WiredBoxType.ConditionUserCountDoesntInRoom:
                    return new UserCountDoesntInRoomBox(_room, Item);
                case WiredBoxType.ConditionIsWearingFX:
                    return new IsWearingFXBox(_room, Item);
                case WiredBoxType.ConditionIsNotWearingFX:
                    return new IsNotWearingFXBox(_room, Item);
                case WiredBoxType.ConditionIsWearingBadge:
                    return new IsWearingBadgeBox(_room, Item);
                case WiredBoxType.ConditionIsNotWearingBadge:
                    return new IsNotWearingBadgeBox(_room, Item);
                case WiredBoxType.ConditionMatchStateAndPosition:
                    return new FurniMatchStateAndPositionBox(_room, Item);
                case WiredBoxType.ConditionDontMatchStateAndPosition:
                    return new FurniDoesntMatchStateAndPositionBox(_room, Item);
                case WiredBoxType.ConditionFurniHasNoFurni:
                    return new FurniHasNoFurniBox(_room, Item);
                case WiredBoxType.ConditionActorHasHandItemBox:
                    return new ActorHasHandItemBox(_room, Item);
                case WiredBoxType.ConditionActorIsInTeamBox:
                    return new ActorIsInTeamBox(_room, Item);
                case WiredBoxType.ConditionActorIsNotInTeamBox:
                    return new ActorIsNotInTeamBox(_room, Item);
                case WiredBoxType.ConditionActorHasNotHandItemBox:
                    return new ActorHasNotHandItemBox(_room, Item);
                case WiredBoxType.ConditionDateRangeActive:
                    return new DateRangeIsActiveBox(_room, Item);
                /*
                case WiredBoxType.ConditionMatchStateAndPosition:
                    return new FurniMatchStateAndPositionBox(_room, Item);

                case WiredBoxType.ConditionFurniTypeMatches:
                    return new FurniTypeMatchesBox(_room, Item);
                case WiredBoxType.ConditionFurniTypeDoesntMatch:
                    return new FurniTypeDoesntMatchBox(_room, Item);
                case WiredBoxType.ConditionFurniHasNoFurni:
                    return new FurniHasNoFurniBox(_room, Item);*/

                case WiredBoxType.AddonRandomEffect:
                    return new AddonRandomEffectBox(_room, Item);
                case WiredBoxType.EffectMoveFurniToNearestUser:
                    return new MoveFurniToUserBox(_room, Item);
                case WiredBoxType.EffectExecuteWiredStacks:
                    return new ExecuteWiredStacksBox(_room, Item);

                case WiredBoxType.EffectTeleportBotToFurniBox:
                    return new TeleportBotToFurniBox(_room, Item);

                case WiredBoxType.TotalUsersCoincidence:
                    return new TotalUsersCoincidenceBox(_room, Item);

                case WiredBoxType.EffectBotChangesClothesBox:
                    return new BotChangesClothesBox(_room, Item);
                case WiredBoxType.EffectBotMovesToFurniBox:
                    return new BotMovesToFurniBox(_room, Item);
                case WiredBoxType.EffectBotCommunicatesToAllBox:
                    return new BotCommunicatesToAllBox(_room, Item);
                case WiredBoxType.EffectBotCommunicatesToUserBox:
                    return new BotCommunicateToUserBox(_room, Item);

                // Wireds Ropa
                case WiredBoxType.EffectApplyClothes:
                    return new ApplyClothesBox(_room, Item);
                case WiredBoxType.ConditionWearingClothes:
                    return new WearingClothesBox(_room, Item);
                case WiredBoxType.ConditionNotWearingClothes:
                    return new NotWearingClothesBox(_room, Item);
                case WiredBoxType.EffectBotGivesHanditemBox:
                    return new BotGivesHandItemBox(_room, Item);
                case WiredBoxType.EffectBotFollowsUserBox:
                    return new BotFollowsUserBox(_room, Item);
                case WiredBoxType.EffectSetRollerSpeed:
                    return new SetRollerSpeedBox(_room, Item);
                case WiredBoxType.EffectRegenerateMaps:
                    return new RegenerateMapsBox(_room, Item);
                case WiredBoxType.EffectGiveUserBadge:
                    return new GiveUserBadgeBox(_room, Item);
                case WiredBoxType.EffectGiveUserHanditem:
                    return new GiveUserHanditemBox(_room, Item);
                case WiredBoxType.EffectGiveUserEnable:
                    return new GiveUserEnableBox(_room, Item);
                case WiredBoxType.EffectGiveUserDance:
                    return new GiveUserDanceBox(_room, Item);
                case WiredBoxType.EffectGiveUserFreeze:
                    return new GiveUserFreezeBox(_room, Item);
                case WiredBoxType.EffectGiveUserFastwalk:
                    return new GiveUserFastwalkBox(_room, Item);
                case WiredBoxType.EffectMoveUser:
                    return new MoveUserBox(_room, Item);

                case WiredBoxType.EffectRaiseFurni:
                    return new RaiseFurniBox(_room, Item);
                case WiredBoxType.EffectLowerFurni:
                    return new LowerFurniBox(_room, Item);
                case WiredBoxType.EffectRoomForward:
                    return new RoomForwardBox(_room, Item);
                case WiredBoxType.ConditionActorHasDiamonds:
                    return new ActorHasDiamondsBox(_room, Item);
                case WiredBoxType.ConditionActorHasNotDiamonds:
                    return new ActorHasNotDiamondsBox(_room, Item);
                case WiredBoxType.ConditionActorHasDuckets:
                    return new ActorHasDucketsBox(_room, Item);
                case WiredBoxType.ConditionActorHasNotDuckets:
                    return new ActorHasNotDucketsBox(_room, Item);
                case WiredBoxType.ConditionActorHasNotCredits:
                    return new ActorHasNotCreditsBox(_room, Item);
                case WiredBoxType.ConditionActorHasRank:
                    return new ActorHasRankBox(_room, Item);
                case WiredBoxType.ConditionActorHasNotRank:
                    return new ActorHasNotRankBox(_room, Item);
                case WiredBoxType.EffectGiveUserDiamonds:
                    return new GiveUserDiamondsBox(_room, Item);
                case WiredBoxType.EffectGiveUserDuckets:
                    return new GiveUserDucketsBox(_room, Item);
                case WiredBoxType.EffectGiveUserCredits:
                    return new GiveUserCreditsBox(_room, Item);
                case WiredBoxType.EffectSendYouTubeVideo:
                    return new SendYouTubeVideoBox(_room, Item);
                case WiredBoxType.EffectShowAlertPHBox:
                    return new ShowAlertPHBox(_room, Item);
            }
            return null;
        }

        public bool IsTrigger(Item Item)
        {
            return Item.GetBaseItem().InteractionType == InteractionType.WIRED_TRIGGER;
        }

        public bool IsEffect(Item Item)
        {
            return Item.GetBaseItem().InteractionType == InteractionType.WIRED_EFFECT;
        }

        public bool IsCondition(Item Item)
        {
            return Item.GetBaseItem().InteractionType == InteractionType.WIRED_CONDITION;
        }

        public bool OtherBoxHasItem(IWiredItem Box, int ItemId)
        {
            if (Box == null)
            {
                return false;
            }

            ICollection<IWiredItem> Items = GetEffects(Box).Where(x => x.Item.Id != Box.Item.Id).ToList();

            if (Items != null && Items.Count > 0)
            {
                foreach (IWiredItem Item in Items)
                {
                    if (Item.Type != WiredBoxType.EffectMoveToDir && Item.Type != WiredBoxType.EffectMoveAndRotate && Item.Type != WiredBoxType.EffectMoveFurniFromNearestUser && Item.Type != WiredBoxType.EffectMoveFurniToNearestUser)
                    {
                        continue;
                    }

                    if (Item.SetItems == null || Item.SetItems.Count == 0)
                    {
                        continue;
                    }

                    if (Item.SetItems.ContainsKey(ItemId))
                    {
                        return true;
                    }
                    else
                    {
                        continue;
                    }
                }
            }


            return false;
        }

        public bool TriggerEvent(WiredBoxType Type, params object[] Params)
        {
            bool Finished = false;
            try
            {
                if (Type == WiredBoxType.TriggerUserSays)
                {
                    List<IWiredItem> RanBoxes = new List<IWiredItem>();
                    foreach (IWiredItem Box in _wiredItems.Values.ToList())
                    {
                        if (Box == null)
                        {
                            continue;
                        }

                        if (Box.Type == WiredBoxType.TriggerUserSays)
                        {
                            if (!RanBoxes.Contains(Box))
                            {
                                RanBoxes.Add(Box);
                            }
                        }
                    }

                    string Message = Convert.ToString(Params[1]);
                    foreach (IWiredItem Box in RanBoxes.ToList())
                    {
                        if (Box == null)
                        {
                            continue;
                        }

                        if (Message.Contains(" " + Box.StringData) || Message.Contains(Box.StringData + " ") || Message == Box.StringData)
                        {
                            Finished = Box.Execute(Params);
                        }
                    }
                    return Finished;
                }
                else
                {
                    foreach (IWiredItem Box in _wiredItems.Values.ToList())
                    {
                        if (Box == null)
                        {
                            continue;
                        }

                        if (Box.Type == Type && IsTrigger(Box.Item))
                        {
                            Finished = Box.Execute(Params);
                        }
                    }
                }
            }
            catch
            {
                //log.Error("Error when triggering Wired Event: " + e);
                return false;
            }

            return Finished;
        }

        public ICollection<IWiredItem> GetTriggers(IWiredItem Item)
        {
            List<IWiredItem> Items = new List<IWiredItem>();
            foreach (IWiredItem I in _wiredItems.Values)
            {
                if (IsTrigger(I.Item) && I.Item.GetX == Item.Item.GetX && I.Item.GetY == Item.Item.GetY)
                {
                    Items.Add(I);
                }
            }

            return Items;
        }

        public ICollection<IWiredItem> GetEffects(IWiredItem Item)
        {
            List<IWiredItem> Items = new List<IWiredItem>();

            foreach (IWiredItem I in _wiredItems.Values)
            {
                if (IsEffect(I.Item) && I.Item.GetX == Item.Item.GetX && I.Item.GetY == Item.Item.GetY)
                {
                    Items.Add(I);
                }
            }

            return Items.OrderBy(x => x.Item.GetZ).ToList();
        }

        public IWiredItem GetRandomEffect(ICollection<IWiredItem> Effects)
        {
            return Effects.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
        }

        public bool OnUserFurniCollision(Room Instance, Item Item)
        {
            if (Instance == null || Item == null)
            {
                return false;
            }

            foreach (Point Point in Item.GetSides())
            {
                if (Instance.GetGameMap().SquareHasUsers(Point.X, Point.Y))
                {
                    List<RoomUser> Users = Instance.GetGameMap().GetRoomUsers(Point);
                    if (Users != null && Users.Count > 0)
                    {
                        foreach (RoomUser User in Users.ToList())
                        {
                            if (User == null)
                            {
                                continue;
                            }

                            Item.UserFurniCollision(User);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }

            return true;
        }

        public ICollection<IWiredItem> GetConditions(IWiredItem Item)
        {
            List<IWiredItem> Items = new List<IWiredItem>();

            foreach (IWiredItem I in _wiredItems.Values)
            {
                if (IsCondition(I.Item) && I.Item.GetX == Item.Item.GetX && I.Item.GetY == Item.Item.GetY)
                {
                    Items.Add(I);
                }
            }

            return Items;
        }

        public void OnEvent(Item Item)
        {
            if (Item.ExtraData == "1")
            {
                return;
            }

            Item.ExtraData = "1";
            Item.UpdateState(false, true);
            Item.RequestUpdate(2, true);
        }

        public void SaveBox(IWiredItem Item)
        {
            string Items = "";
            IWiredCycle Cycle = null;
            if (Item is IWiredCycle)
            {
                Cycle = (IWiredCycle)Item;
            }

            foreach (Item I in Item.SetItems.Values)
            {
                Item SelectedItem = _room.GetRoomItemHandler().GetItem(Convert.ToInt32(I.Id));
                if (SelectedItem == null)
                {
                    continue;
                }

                if (Item.Type == WiredBoxType.EffectMatchPosition || Item.Type == WiredBoxType.ConditionMatchStateAndPosition || Item.Type == WiredBoxType.ConditionDontMatchStateAndPosition)
                {
                    Items += I.Id + ":" + I.GetX + "," + I.GetY + "," + I.GetZ + "," + I.Rotation + "," + I.ExtraData + ";";
                }
                else
                {
                    Items += I.Id + ";";
                }
            }

            if (Item.Type == WiredBoxType.EffectMatchPosition || Item.Type == WiredBoxType.ConditionMatchStateAndPosition || Item.Type == WiredBoxType.ConditionDontMatchStateAndPosition)
            {
                Item.ItemsData = Items;
            }

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("REPLACE INTO `wired_items` VALUES (@id, @items, @delay, @string, @bool)");
                dbClient.AddParameter("id", Item.Item.Id);
                dbClient.AddParameter("items", Items);
                dbClient.AddParameter("delay", (Item is IWiredCycle) ? Cycle.Delay : 0);
                dbClient.AddParameter("string", Item.StringData);
                dbClient.AddParameter("bool", Item.BoolData ? "1" : "0");
                dbClient.RunQuery();
            }
        }

        public bool AddBox(IWiredItem Item)
        {
            return _wiredItems.TryAdd(Item.Item.Id, Item);
        }

        public bool TryRemove(int ItemId)
        {
            return _wiredItems.TryRemove(ItemId, out _);
        }

        public bool TryGet(int id, out IWiredItem Item)
        {
            return _wiredItems.TryGetValue(id, out Item);
        }

        public void Cleanup()
        {
            _wiredItems.Clear();
        }
    }
}