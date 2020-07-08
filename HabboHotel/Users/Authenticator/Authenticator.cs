using System;
using System.Data;

namespace Neon.HabboHotel.Users.Authenticator
{
    public static class HabboFactory
    {
        public static Habbo GenerateHabbo(DataRow Row, DataRow UserInfo)
        {
            return new Habbo(Convert.ToInt32(Row["id"]), Convert.ToString(Row["username"]), Convert.ToInt32(Row["rank"]), Convert.ToString(Row["motto"]), Convert.ToString(Row["look"]),
                Convert.ToString(Row["gender"]), Convert.ToInt32(Row["credits"]), Convert.ToInt32(Row["activity_points"]),
                Convert.ToInt32(Row["home_room"]), NeonEnvironment.EnumToBool(Row["block_newfriends"].ToString()), Convert.ToInt32(Row["last_online"]),
                NeonEnvironment.EnumToBool(Row["hide_online"].ToString()), NeonEnvironment.EnumToBool(Row["hide_inroom"].ToString()),
                Convert.ToDouble(Row["account_created"]), Convert.ToInt32(Row["vip_points"]), Convert.ToString(Row["machine_id"]), Convert.ToString(Row["volume"]),
                NeonEnvironment.EnumToBool(Row["chat_preference"].ToString()), NeonEnvironment.EnumToBool(Row["focus_preference"].ToString()), NeonEnvironment.EnumToBool(Row["pets_muted"].ToString()), NeonEnvironment.EnumToBool(Row["bots_muted"].ToString()),
                NeonEnvironment.EnumToBool(Row["advertising_report_blocked"].ToString()), Convert.ToDouble(Row["last_change"].ToString()), Convert.ToInt32(Row["gotw_points"]), Convert.ToInt32(Row["user_points"]),
                NeonEnvironment.EnumToBool(Convert.ToString(Row["ignore_invites"])), Convert.ToDouble(Row["time_muted"]), Convert.ToDouble(UserInfo["trading_locked"]),
                NeonEnvironment.EnumToBool(Row["allow_gifts"].ToString()), Convert.ToInt32(Row["friend_bar_state"]), NeonEnvironment.EnumToBool(Row["disable_forced_effects"].ToString()),
                NeonEnvironment.EnumToBool(Row["allow_mimic"].ToString()), Convert.ToInt32(Row["rank_vip"]), Convert.ToByte(Row["guia"].ToString()), Convert.ToByte(Row["publi"].ToString()), Convert.ToByte(Row["builder"].ToString()), Convert.ToByte(Row["croupier"].ToString()), (Row["nux_user"].ToString() == "true"), Convert.ToByte(Row["targeted_buy"]), Convert.ToString(Row["namecolor"]), Convert.ToString(Row["tag"]), Convert.ToString(Row["tagcolor"]), Convert.ToByte(Row["changename"]), Convert.ToString(Row["pin_client"]), Convert.ToInt32(Row["cata_rank"]));
        }
    }
}