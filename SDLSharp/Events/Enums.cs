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

    public enum PowerState
    {
        Invalid = -1,
        Instant = 0,
        Attack = 1
    }

    public enum PowerType
    {
        Invalid = -1,
        Fixed = 0,
        Missile = 1,
        Repeater = 2,
        Spawn = 3,
        Transform = 4,
        Effect = 5,
        Block = 6
    }

    public enum StartingPos
    {
        Source,
        Target,
        Melee
    }

    public enum SourceType
    {
        Invalid = -1,
        Hero = 0,
        Neutral = 1,
        Enemy = 2,
        Ally = 3
    }
}
