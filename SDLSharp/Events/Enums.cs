namespace SDLSharp.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public enum EventActivation
    {
        None,
        Trigger,
        Load,
        Exit,
        Leave,
        Clear,
        Static
    }

    public enum EventComponentType
    {
        None,
        Tooltip,
        InterMap,
        IntraMap,
        MapMod,
        SoundFX,
        Msg,
        ShakyCam,
        Spawn,
        Music,
        Repeat,
        Stash,
        RequiresStatus,
        RequiresNotStatus,
        SetStatus,
        UnsetStatus,
        NPCHotspot,
        Power,
        VoxIntro
    }
}
