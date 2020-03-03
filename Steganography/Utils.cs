using System.Linq;

namespace Steganography
{
    static class Utils
    {
        public static bool GetBit(int val, int index)
        {
            return (val & (1 << index)) != 0;
        }

        public static int SetBit(int val, int index, bool bit)
        {
            if (bit)
            {
                return val | (1 << index);
            }
            else
            {
                return val & ~(1 << index);
            }
        }

        public static byte[] Join(params byte[][] arrays)
        {
            return arrays.SelectMany(x => x).ToArray();
        }
    }
}
