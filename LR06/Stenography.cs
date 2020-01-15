using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stenography
{
    public class Stenography
    {
        public static Bitmap Encrypt(Bitmap inImage, string sourceMessage)
        {
            byte[] sourceMessageBytes = Encoding.Unicode.GetBytes(sourceMessage);
            Array.Resize(ref sourceMessageBytes, sourceMessageBytes.Length + 2);

            BitArray sourceMessageBitArray = new BitArray(sourceMessageBytes);

            int length = inImage.Height * inImage.Width * 3;
            if (length < sourceMessageBitArray.Length)
                throw new Exception("Повідомлення завелике для обраного зображення");

            int k = 0;
            byte r, g, b;
            var outImage = (Bitmap)inImage.Clone();
            for (int i = 0; i < inImage.Height; i++)
            {
                for (int j = 0; j < inImage.Width; j++)
                {
                    Color pixel = inImage.GetPixel(j, i);
                    r = pixel.R;
                    g = pixel.G;
                    b = pixel.B;
                    r = SetBit(sourceMessageBitArray[k++], pixel.R);
                    outImage.SetPixel(j, i, Color.FromArgb(r, g, b));
                    if (k == sourceMessageBitArray.Length) return outImage;
                    g = SetBit(sourceMessageBitArray[k++], pixel.G);
                    outImage.SetPixel(j, i, Color.FromArgb(r, g, b));
                    if (k == sourceMessageBitArray.Length) return outImage;
                    b = SetBit(sourceMessageBitArray[k++], pixel.B);
                    outImage.SetPixel(j, i, Color.FromArgb(r, g, b));
                    if (k == sourceMessageBitArray.Length) return outImage;
                }
            }
            return outImage;
        }

        public static string Decrypt(Bitmap image)
        {
            List<bool> sourceMessageBits = new List<bool>();
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    Color pixel = image.GetPixel(j, i);
                    sourceMessageBits.Add(GetBit(pixel.R));
                    if (IsEnd(sourceMessageBits)) return GetMessage(sourceMessageBits);
                    sourceMessageBits.Add(GetBit(pixel.G));
                    if (IsEnd(sourceMessageBits)) return GetMessage(sourceMessageBits);
                    sourceMessageBits.Add(GetBit(pixel.B));
                    if (IsEnd(sourceMessageBits)) return GetMessage(sourceMessageBits);
                }
            }
            return GetMessage(sourceMessageBits);
        }

        private static string GetMessage(List<bool> bits)
        {
            var bytes = new List<byte>();
            for (int i = 0; i < bits.Count - 16; i += 8)
            {
                bytes.Add(ConvertBoolArrayToByte(bits.GetRange(i, 8).ToArray()));
            }
            return Encoding.Unicode.GetString(bytes.ToArray());
        }

        private static byte ConvertBoolArrayToByte(bool[] source)
        {
            byte result = 0;
            int index = 8 - source.Length;
            foreach (bool b in source)
            {
                if (b) result |= (byte)(1 << index);
                index++;
            }
            return result;
        }

        private static bool IsEnd(List<bool> bits)
        {
            int n = 16;
            if (bits.Count % n != 0) return false;
            if (bits.Count < n) return false;
            for (; n > 0; n--)
            {
                if (bits[bits.Count - n] == true) return false;
            }
            return true;
        }

        public static bool GetBit(byte b)
        {
            return b % 2 == 1;
        }

        private static bool GetBit(byte b, int position)
        {
            return (1 == ((b >> position) & 1));
        }

        public static byte SetBit(bool bit, byte b)
        {
            return (byte)(bit ? (b | 1) : (b & ~1));
        }
    }
}
