/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID PLAY_ENTITY_DAMAGED = 65612959U;
        static const AkUniqueID PLAY_ENTITY_DEATH = 3288250068U;
        static const AkUniqueID PLAY_GUN_RELOAD_SWITCH = 1921754267U;
        static const AkUniqueID PLAY_GUN_SOUND_SWITCH = 14281227U;
        static const AkUniqueID PLAY_PLAYER_DASH = 2175711460U;
        static const AkUniqueID PLAY_PLAYER_DOUBLEJUMP = 594945751U;
        static const AkUniqueID PLAY_PLAYER_INITIALJUMP = 421515808U;
        static const AkUniqueID PLAY_SFX_TEST_PICKUP = 3823027101U;
        static const AkUniqueID PLAY_SFX_TESTSOUND = 3056398787U;
        static const AkUniqueID STOP_GUN_SOUND_SWITCH = 2746280817U;
    } // namespace EVENTS

    namespace SWITCHES
    {
        namespace DAMAGESOUND
        {
            static const AkUniqueID GROUP = 3254952287U;

            namespace SWITCH
            {
                static const AkUniqueID ENEMYDAMAGE = 1879435608U;
                static const AkUniqueID PLAYERDAMAGE = 337406793U;
            } // namespace SWITCH
        } // namespace DAMAGESOUND

        namespace GUNSETTING
        {
            static const AkUniqueID GROUP = 2794931643U;

            namespace SWITCH
            {
                static const AkUniqueID ARCSTREAM = 1655133539U;
                static const AkUniqueID BURSTFIRE = 485519469U;
                static const AkUniqueID CONSTANT = 3913713129U;
                static const AkUniqueID SHOTGUN = 51683977U;
            } // namespace SWITCH
        } // namespace GUNSETTING

    } // namespace SWITCHES

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID TESTBANK = 3291379323U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
