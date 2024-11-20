// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("/eUn1ziK6OLfWjF+p17bCsjhSloGLpf19pwzu2Bob3IPfpPYl7Fh98CUIHJOxO/urX0D0Ig4NpMozBwjigkHCDiKCQIKigkJCL/vAc8HPBVu0n7n2xEnwFSFVUyN+ne7vrEB48peuSCh4EzoRpk+yCxKtfwwDZGB04xakKObhOAsrjKh54a3xWPczJTxfXXFtoNeh1AdzO01nvA2YURL+LzLWzpD3xl9dAgAXCbrrfXN5I8ThzYjtdhGDQUPykYgzrSPM1KwN5A4igkqOAUOASKOQI7/BQkJCQ0IC44wGTkGIt+Gig1nIywyMZr+YABCeOJ/zzVUcC7LVJa7CIhZtXQsxfuQousb8BacpQh0NAYdI0Oq2F335EGOuqncHtWLOQoLCQgJ");
        private static int[] order = new int[] { 7,5,6,5,6,9,13,13,10,10,13,11,13,13,14 };
        private static int key = 8;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
