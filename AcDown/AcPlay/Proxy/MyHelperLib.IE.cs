using System;
using System.Runtime.InteropServices;

namespace Kaedei.AcPlay.Proxy.IEHelper
 {

 
     /// <summary>
     /// 设置WebBrowser控件的代理服务
     /// 设置后不会影响IE浏览器
     /// </summary>
     public class IEProxy
     {
         private const int INTERNET_OPTION_PROXY = 38;
         private const int INTERNET_OPEN_TYPE_PROXY = 3;
         private const int INTERNET_OPEN_TYPE_DIRECT = 1;
 
         private string ProxyStr;       
 
         [DllImport("wininet.dll", SetLastError = true)]
         private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);
 
         public struct Struct_INTERNET_PROXY_INFO
         {
             public int dwAccessType;
             public IntPtr proxy;
             public IntPtr proxyBypass;
         }
 
         /// <summary>
         /// 设置WebBrowser控件代理服务
         /// </summary>
         /// <param name="strProxy"></param>
         /// <returns></returns>
         private bool InternetSetOption(string strProxy)
         {
             int bufferLength;
             IntPtr intptrStruct;
             Struct_INTERNET_PROXY_INFO struct_IPI;
 
             if (string.IsNullOrEmpty(strProxy) || strProxy.Trim().Length == 0)
             {
                 strProxy = string.Empty;
                 struct_IPI.dwAccessType = INTERNET_OPEN_TYPE_DIRECT;
             }
             else
             {
                 struct_IPI.dwAccessType = INTERNET_OPEN_TYPE_PROXY;
             }
 
             struct_IPI.proxy = Marshal.StringToHGlobalAnsi(strProxy);
             struct_IPI.proxyBypass = Marshal.StringToHGlobalAnsi("local");
 
             bufferLength = Marshal.SizeOf(struct_IPI);
             intptrStruct = Marshal.AllocCoTaskMem(bufferLength);
             Marshal.StructureToPtr(struct_IPI, intptrStruct, true);
 
             return InternetSetOption(IntPtr.Zero, INTERNET_OPTION_PROXY, intptrStruct, bufferLength);
         }
 
         public IEProxy(string strProxy)
         {
             this.ProxyStr = strProxy;
         }
 
         /// <summary>
         /// 设置IE代理服务
         /// </summary>
         /// <returns></returns>
         public bool RefreshIESettings()
         {
             return InternetSetOption(this.ProxyStr);
         }
 
         /// <summary>
         /// 取消IE代理服务
         /// </summary>
         /// <returns></returns>
         public bool DisableIEProxy()
         {
             return InternetSetOption(string.Empty);
         }
     }
 }