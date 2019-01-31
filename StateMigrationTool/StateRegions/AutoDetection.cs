using System;
using System.Runtime.InteropServices;
using StateMigrationBackend.Models;
using System.Management;
using System.Threading.Tasks;
using System.Security;

namespace StateMigrationBackend.StateRegions
{
    class AutoDetection
    {
      public async Task<SystemDetails> DetectAsync()
        {
          return await Task.Factory.StartNew(() => {

              RamSize rm = new RamSize();
               return new SystemDetails()
               {
                   LogicalCPUCount = Environment.ProcessorCount,
                   Is64Bit = Environment.Is64BitOperatingSystem,
                   RAMSize = rm.Get(),
                   MachineName = Environment.MachineName,
                   OperatingSystem = OperatingSystem.GetOSFriendlyName()

               };
           });            
        }

      public async Task<int> GetOptimizedValue()
        {
            
            SystemDetails sysdt = new SystemDetails();
            sysdt = await DetectAsync();
            int result;

            switch (sysdt.LogicalCPUCount)
            {
                case 1:
                      switch(sysdt.RAMSize)
                    {
                        case 1:
                            result = 1;
                            break;

                        case 2:
                            result = 2;
                            break;

                        case 4:
                            result = 3;
                            break;

                        case 8:
                            result = 4;
                            break;

                        case 12:
                            result = 5;
                            break;

                        case 16:
                            result = 6;
                            break;

                        case 24:
                            result = 6;
                            break;

                        case 32:
                            result = 6;
                            break;

                        default:
                            result = 1;
                            break;
                    }
                    break;

                case 2:
                    switch (sysdt.RAMSize)
                    {
                        case 1:
                            result = 1;
                            break;

                        case 2:
                            result = 2;
                            break;

                        case 4:
                            result = 3;
                            break;

                        case 8:
                            result = 4;
                            break;

                        case 12:
                            result = 5;
                            break;

                        case 16:
                            result = 6;
                            break;

                        case 24:
                            result = 6;
                            break;

                        case 32:
                            result = 6;
                            break;

                        default:
                            result = 2;
                            break;
                    }
                    break;

                case 4:
                    switch (sysdt.RAMSize)
                    {
                        case 1:
                            result = 1;
                            break;

                        case 2:
                            result = 2;
                            break;

                        case 4:
                            result = 3;
                            break;

                        case 8:
                            result = 4;
                            break;

                        case 12:
                            result = 5;
                            break;

                        case 16:
                            result = 6;
                            break;

                        case 24:
                            result = 6;
                            break;

                        case 32:
                            result = 6;
                            break;

                        default:
                            result = 4;
                            break;
                    }
                    break;

                case 8:
                    switch (sysdt.RAMSize)
                    {
                        case 1:
                            result = 1;
                            break;

                        case 2:
                            result = 2;
                            break;

                        case 4:
                            result = 3;
                            break;

                        case 8:
                            result = 4;
                            break;

                        case 12:
                            result = 5;
                            break;

                        case 16:
                            result = 6;
                            break;

                        case 24:
                            result = 6;
                            break;

                        case 32:
                            result = 6;
                            break;

                        default:
                            result = 5;
                            break;
                    }
                    break;

                default:
                    result = 1;
                    break;

            }

            return result;            
            
        }

    }

    class RamSize
    {
        private readonly object _syncRootObj = new object();

        internal long Get()
        {
            lock(_syncRootObj)
            {
                long memoryKb;
                SafeNativeMethods.GetPhysicallyInstalledSystemMemory(out memoryKb);
                return (memoryKb / 1024 / 1024);
            }
            
        }
    }

    class OperatingSystem
    {
        public static string GetOSFriendlyName()
        {
            string result = string.Empty;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
            foreach (ManagementObject os in searcher.Get())
            {
                result = os["Caption"].ToString();
                break;
            }
            return result;
        }
    }

    [SuppressUnmanagedCodeSecurity]
    internal class SafeNativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetPhysicallyInstalledSystemMemory(out long TotalMemoryInKilobytes);
    }
}
