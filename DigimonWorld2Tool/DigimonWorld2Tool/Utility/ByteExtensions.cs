using System;
using System.Collections.Generic;
using System.Text;

namespace DigimonWorld2Tool.Utility
{
    public static class ByteExtensions
    {
        /// <summary>
        /// Get the left value of a hexadecimal notated byte in decimals
        /// </summary>
        /// <param name="b">The byte to split</param>
        /// <returns>The left value of the hex value of the byte</returns>
        /// <remarks>Example: If the inputted byte is 0x18, returns 1</remarks>
        public static byte GetLefNiblet(this byte b) => (byte)((b & 0xF0) >> 4); //AND byte with 0xF0 (1111 0000) to keep only the active 4 higher bits, shift it right 4 times to return the half-byte value

        /// <summary>
        /// Get the right value of a hexadecimal notated byte in decimals
        /// </summary>
        /// <param name="b">The byte to split</param>
        /// <returns>The right value of the hex value of the bye</returns>
        /// <remarks>Example: If the inputted byte is 0x18, returns 8</remarks>
        public static byte GetRightNiblet(this byte b) => (byte)(b & 0x0F); //AND byte with 0x0F (0000 1111) to keep only the active 4 lower bits

        public static void SetLeftNiblet(this ref byte b, byte value)
        {
            byte rightNiblet = b.GetRightNiblet();
            b = (byte)(value << 4);
            b |= rightNiblet;
        }

        public static void SetRightNiblet (this ref byte b, byte value)
        {
            byte leftNiblet = b.GetLefNiblet();
            b = (byte)(leftNiblet << 4);
            b |= value;
        }
    }
}
