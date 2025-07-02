using System.Runtime.InteropServices;

namespace ConnectionProfiler.LocalHost.Credentials
{
    internal static class CredentialManager
    {
        public static Credential? GetCredential(string target)
        {
            var cred = new Credential();
            bool result = CredRead(target, CRED_TYPE.GENERIC, 0, out IntPtr credPtr);
            if (!result) return null;

            try
            {
                var native = Marshal.PtrToStructure<CREDENTIAL>(credPtr);
                cred.Target = Marshal.PtrToStringUni(native.TargetName);
                cred.Username = Marshal.PtrToStringUni(native.UserName);
                cred.Password = Marshal.PtrToStringUni(native.CredentialBlob, native.CredentialBlobSize / 2);
                return cred;
            }
            finally
            {
                CredFree(credPtr);
            }
        }

        [DllImport("advapi32", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool CredRead(string target, CRED_TYPE type, int reservedFlag, out IntPtr credentialPtr);

        [DllImport("advapi32", SetLastError = true)]
        private static extern void CredFree(IntPtr cred);

        private enum CRED_TYPE : int { GENERIC = 1 }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct CREDENTIAL
        {
            public int Flags;
            public int Type;
            public IntPtr TargetName;
            public IntPtr Comment;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
            public int CredentialBlobSize;
            public IntPtr CredentialBlob;
            public int Persist;
            public int AttributeCount;
            public IntPtr Attributes;
            public IntPtr TargetAlias;
            public IntPtr UserName;
        }

        public class Credential
        {
            public string? Target { get; set; }
            public string? Username { get; set; }
            public string? Password { get; set; }
        }
    }
}
