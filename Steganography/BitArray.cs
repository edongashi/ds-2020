namespace Steganography
{
    class BitArray
    {
        private byte[] bytes;

        public BitArray(byte[] bytes)
        {
            this.bytes = bytes;
        }

        public int Length => bytes.Length * 8;

        public bool GetBit(int index)
        {
            int byteIndex = index / 8;
            int bitIndex = index % 8;
            return Utils.GetBit(bytes[byteIndex], bitIndex);
        }

        public void SetBit(int index, bool bit)
        {
            int byteIndex = index / 8;
            int bitIndex = index % 8;
            bytes[byteIndex] = (byte)Utils.SetBit(
                bytes[byteIndex],
                bitIndex,
                bit
            );
        }
    }
}
