﻿namespace Neon.HabboHotel.Items.Wired
{
    public enum WiredBoxType
    {
        None,
        TriggerRoomEnter,
        TriggerUserSays,
        TriggerRepeat,
        TriggerLongRepeat,
        TriggerStateChanges,
        TriggerWalkOnFurni,
        TriggerWalkOffFurni,
        TriggerGameStarts,
        TriggerGameEnds,
        TriggerUserFurniCollision,
        TriggerUserSaysCommand,
        TriggerAtGivenTime,
        TriggerScoreAchieved,
        TriggerBotReachedAvatar,

        EffectShowMessage,
        EffectTeleportToFurni,
        EffectToggleNegativeFurniState,
        EffectToggleFurniState,
        EffectKickUser,
        EffectMatchPosition,
        EffectMoveAndRotate,
        EffectMoveFurniToNearestUser,
        EffectMoveFurniFromNearestUser,
        EffectMuteTriggerer,
        EffectGiveReward,
        EffectExecuteWiredStacks,
        EffectAddScore,
        EffectAddRewardPoints,
        EffectApplyClothes,
        EffectMoveUser,
        EffectTimerReset,
        EffectMoveToDir,
        EffectProgressUserAchievement,
        EffectSendYouTubeVideo,

        EffectTeleportBotToFurniBox,
        EffectBotChangesClothesBox,
        EffectBotMovesToFurniBox,
        EffectBotCommunicatesToAllBox,
        EffectBotCommunicatesToUserBox,
        EffectBotFollowsUserBox,
        EffectBotGivesHanditemBox,

        EffectAddActorToTeam,
        EffectRemoveActorFromTeam,
        EffectSetRollerSpeed,
        EffectRegenerateMaps,
        EffectGiveUserBadge,
        EffectGiveUserHanditem,
        EffectGiveUserEnable,
        EffectGiveUserDance,
        EffectGiveUserFastwalk,
        EffectGiveUserFreeze,
        EffectGiveUserDiamonds,
        EffectGiveUserDuckets,
        EffectGiveUserCredits,

        ConditionFurniHasUsers,
        ConditionFurniHasFurni,
        ConditionTriggererOnFurni,
        ConditionIsGroupMember,
        ConditionIsNotGroupMember,
        ConditionTriggererNotOnFurni,
        ConditionFurniHasNoUsers,
        ConditionIsWearingBadge,
        ConditionIsWearingFX,
        ConditionIsNotWearingBadge,
        ConditionIsNotWearingFX,
        ConditionMatchStateAndPosition,
        ConditionDontMatchStateAndPosition,
        ConditionUserCountInRoom,
        ConditionUserCountDoesntInRoom,
        ConditionFurniTypeMatches,
        ConditionFurniTypeDoesntMatch,
        ConditionFurniHasNoFurni,
        ConditionActorHasHandItemBox,
        ConditionActorHasNotHandItemBox,
        ConditionActorIsInTeamBox,
        ConditionActorIsNotInTeamBox,
        ConditionWearingClothes,
        ConditionNotWearingClothes,
        ConditionDateRangeActive,
        ConditionLessSecs,
        ConditionMoreSecs,
        SendCustomMessageBox,
        TotalUsersCoincidence,

        AddonRandomEffect,

        // CUSTOM WIREDS - BY JAVAS
        EffectLowerFurni,
        EffectRaiseFurni,
        EffectRoomForward,
        EffectShowAlertPHBox,
        //EffectGiveDuckets,
        //EffectGiveCredits,
        //EffectGiveDiamonds,
        ConditionActorHasDiamonds,
        ConditionActorHasNotDiamonds,
        ConditionActorHasDuckets,
        ConditionActorHasNotDuckets,
        ConditionActorHasRank,
        ConditionActorHasNotRank,
        ConditionActorHasNotCredits
    }
}