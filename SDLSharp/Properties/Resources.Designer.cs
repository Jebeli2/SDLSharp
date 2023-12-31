﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SDLSharp.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SDLSharp.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] entypo {
            get {
                object obj = ResourceManager.GetObject("entypo", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to name=Female
        ///level=1
        ///speed=3
        ///humanoid=true
        ///power=melee,1,5
        ///# power=ranged,35,5
        ///
        ///portrait=images/portraits/female01.png
        ///
        ///sfx_attack=swing,soundfx/melee_attack.ogg
        ///sfx_attack=shoot,soundfx/melee_attack.ogg
        ///sfx_attack=cast,soundfx/melee_attack.ogg
        ///sfx_hit=soundfx/female_hit.ogg
        ///sfx_die=soundfx/female_die.ogg
        ///sfx_critdie=soundfx/female_die.ogg
        ///
        ///# animation info
        ///gfxpart=chest,animations/avatar/female/plate_cuirass.txt
        ///gfxpart=feet,animations/avatar/female/plate_boots.txt
        ///gfxpart=hands,animations/avatar/female/pla [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string female {
            get {
                return ResourceManager.GetString("female", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to name=Male
        ///level=1
        ///speed=4.5
        ///humanoid=true
        ///melee_range=1.1875
        ///#power=ranged,35,5
        ///power=melee,1,100
        ///power=ranged,231,90
        ///power=ranged,146,90
        ///power=ranged,44,100
        ///
        ///portrait=images/portraits/male01.png
        ///
        ///sfx_attack=swing,soundfx/melee_attack.ogg
        ///sfx_attack=shoot,soundfx/melee_attack.ogg
        ///sfx_attack=cast,soundfx/melee_attack.ogg
        ///sfx_hit=soundfx/male_hit.ogg
        ///sfx_die=soundfx/male_die.ogg
        ///sfx_critdie=soundfx/male_die.ogg
        ///
        ///# animation info
        ///gfxpart=chest,animations/avatar/male/cloth_shirt.txt
        ///gfxpart=feet,animations/avat [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string male {
            get {
                return ResourceManager.GetString("male", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] Roboto_Regular {
            get {
                object obj = ResourceManager.GetObject("Roboto-Regular", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] tiled_collision {
            get {
                object obj = ResourceManager.GetObject("tiled_collision", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///img=tiled_collision
        ///
        ///tile=16,0,0,64,32,32,16
        ///tile=17,64,0,64,32,32,16
        ///tile=18,128,0,64,32,32,16
        ///tile=19,192,0,64,32,32,16
        ///tile=20,256,0,64,32,32,16
        ///tile=21,320,0,64,32,32,16
        ///.
        /// </summary>
        internal static string tileset_collision {
            get {
                return ResourceManager.GetString("tileset_collision", resourceCulture);
            }
        }
    }
}
