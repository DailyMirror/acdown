﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kaedei.AcPlay.Redirector {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class MukioRes {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal MukioRes() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Kaedei.AcPlay.Redirector.MukioRes", typeof(MukioRes).Assembly);
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
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///&lt;conf&gt;
        ///  &lt;performance&gt;
        ///    &lt;!-- 最长弹幕/像素 --&gt;
        ///    &lt;maxwidth&gt;2048&lt;/maxwidth&gt;
        ///    &lt;!-- 最高弹幕/像素 --&gt;
        ///    &lt;maxheight&gt;768&lt;/maxheight&gt;
        ///    &lt;!-- 表面弹幕容量,包括有特效和无特效 --&gt;
        ///    &lt;maxonstage&gt;120&lt;/maxonstage&gt;
        ///    &lt;!-- 特效弹幕容量,如果超出该容量,但是未达到表面弹幕容量,超出部分为无特效 --&gt;
        ///    &lt;maxwitheffect&gt;80&lt;/maxwitheffect&gt;
        ///  &lt;/performance&gt;
        ///  &lt;server&gt;
        ///    &lt;!-- 使用mukio播放器的方法处理参数,不用改变 --&gt;
        ///    &lt;onhost&gt;yes&lt;/onhost&gt;
        ///    &lt;!-- 弹幕加载地址,变量{$id}为弹幕id --&gt;
        ///    &lt;load&gt;http://mukioplayer/{$id}.xml&lt;/load&gt;
        ///    &lt;!-- POS [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string mukioconf {
            get {
                return ResourceManager.GetString("mukioconf", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;!DOCTYPE html PUBLIC &quot;-//W3C//DTD XHTML 1.0 Transitional//EN&quot; &quot;http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd&quot;&gt;
        ///&lt;html xmlns=&quot;http://www.w3.org/1999/xhtml&quot;&gt;
        ///&lt;head&gt;
        ///&lt;meta http-equiv=&quot;Content-Type&quot; content=&quot;text/html; charset=UTF-8&quot; /&gt;
        ///&lt;title&gt;AcPlay&lt;/title&gt;
        ///&lt;/head&gt;
        ///&lt;body&gt;
        ///&lt;embed width=&quot;100%&quot; height=&quot;100%&quot; id=&quot;MukioPlayer&quot; 
        ///	pluginspage=&quot;http://www.adobe.com/shockwave/download/download.cgi?P1_Prod_Version=ShockwaveFlash&quot; 
        ///	src=&quot;{PLAYER}?vid={VIDEOID}&amp;type=video&amp;sort=new&quot; 
        ///	type=&quot;application/ [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Mukiohtml {
            get {
                return ResourceManager.GetString("Mukiohtml", resourceCulture);
            }
        }
    }
}
