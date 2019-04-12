using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace MoneyManager
{
    class hardwareid
    {
        public static string MachineID()
        {
            string location = @"SOFTWARE\Microsoft\Cryptography";
            string name = "MachineGuid";

            using (RegistryKey localMachineX64View = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (RegistryKey rk = localMachineX64View.OpenSubKey(location))
                {
                    if (rk == null)
                        throw new KeyNotFoundException(string.Format("Key Not Found: {0}", location));

                    object gmachineid = rk.GetValue(name);
                    if (gmachineid == null)
                        throw new IndexOutOfRangeException(string.Format("Index Not Found: {0}", name));

                    return gmachineid.ToString();
                }
            }

        }
    }
}

