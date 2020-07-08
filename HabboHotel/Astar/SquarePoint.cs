using Neon.HabboHotel.Groups;
using Neon.HabboHotel.Items;
using Neon.HabboHotel.Pathfinding;
using Neon.HabboHotel.Rooms;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Neon.HabboHotel.Astar
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SquarePoint
    {
        private readonly RoomUser mUser;
        private readonly int mX;
        private readonly int mY;
        private readonly double mDistance;
        private readonly byte mSquareData;
        private readonly bool mInUse;
        private readonly bool mOverride;
        private readonly bool mLastStep;
        private readonly Gamemap mMap;

        public SquarePoint(RoomUser User, Vector2D From, int pTargetX, int pTargetY, byte SquareData, bool pOverride, Gamemap pGameMap)
        {
            mUser = User;
            mX = From.X;
            mY = From.Y;
            mSquareData = SquareData;
            mInUse = true;
            mOverride = pOverride;
            mDistance = 0.0;
            mLastStep = (From.X == pTargetX) && (From.Y == pTargetY);
            mDistance = DreamPathfinder.GetDistance(From.X, From.Y, pTargetX, pTargetY);
            mMap = pGameMap;
        }

        public int X => mX;

        public int Y => mY;

        public double GetDistance => mDistance;

        public bool CanWalk
        {
            get
            {
                if (!mLastStep)
                {
                    if (!mOverride)
                    {
                        return ((mSquareData == 1) || (mSquareData == 4));
                    }

                    return true;
                }

                if (mLastStep)
                {
                    if (mMap != null)
                    {
                        List<Item> Items = mMap.GetAllRoomItemForSquare(X, Y);
                        if (Items.Count > 0)
                        {
                            bool HasGroupGate = Items.ToList().Where(x => x.GetBaseItem().InteractionType == InteractionType.GUILD_GATE).ToList().Count() > 0;
                            if (HasGroupGate)
                            {
                                Item I = Items.FirstOrDefault(x => x.GetBaseItem().InteractionType == InteractionType.GUILD_GATE);
                                if (I != null)
                                {
                                    if (!NeonEnvironment.GetGame().GetGroupManager().TryGetGroup(I.GroupId, out Group Group))
                                    {
                                        return false;
                                    }

                                    if (mUser.GetClient() == null || mUser.GetClient().GetHabbo() == null)
                                    {
                                        return false;
                                    }

                                    if (Group.IsMember(mUser.GetClient().GetHabbo().Id))
                                    {
                                        I.InteractingUser = mUser.GetClient().GetHabbo().Id;
                                        I.ExtraData = "1";
                                        I.UpdateState(false, true);

                                        I.RequestUpdate(4, true);

                                        return true;
                                    }
                                    else
                                    {
                                        if (mUser.Path.Count > 0)
                                        {
                                            mUser.Path.Clear();
                                        }

                                        mUser.PathRecalcNeeded = false;
                                        return false;
                                    }
                                }
                            }

                            bool HasHcGate = Items.ToList().Where(x => x.GetBaseItem().InteractionType == InteractionType.HCGATE).ToList().Count() > 0;
                            if (HasHcGate)
                            {
                                Item I = Items.FirstOrDefault(x => x.GetBaseItem().InteractionType == InteractionType.HCGATE);
                                if (I != null)
                                {
                                    if (mUser.GetClient() == null || mUser.GetClient().GetHabbo() == null)
                                    {
                                        return false;
                                    }

                                    bool IsHc = mUser.GetClient().GetHabbo().GetClubManager().HasSubscription("habbo_vip");
                                    if (!IsHc)
                                    {
                                        return false;
                                    }

                                    if (mUser.GetClient().GetHabbo().GetClubManager().HasSubscription("habbo_vip"))
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                            }

                            bool HasVIPGate = Items.ToList().Where(x => x.GetBaseItem().InteractionType == InteractionType.VIPGATE).ToList().Count() > 0;
                            if (HasVIPGate)
                            {
                                Item I = Items.FirstOrDefault(x => x.GetBaseItem().InteractionType == InteractionType.VIPGATE);
                                if (I != null)
                                {
                                    bool IsVIP = mUser.GetClient().GetHabbo().GetClubManager().HasSubscription("club_vip");
                                    if (!IsVIP)
                                    {
                                        return false;
                                    }

                                    if (mUser.GetClient() == null || mUser.GetClient().GetHabbo() == null)
                                    {
                                        return false;
                                    }

                                    if (mUser.GetClient().GetHabbo().GetClubManager().HasSubscription("club_vip"))
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }

                if (!mOverride)
                {
                    if (mSquareData == 3)
                    {
                        return true;
                    }
                    if (mSquareData == 1)
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
                return false;
            }
        }
        public bool InUse => mInUse;
    }
}