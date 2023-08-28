// Decompiled with JetBrains decompiler
// Type: OtpNet.Base32Encoding
// Assembly: TyGAPC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 18564C5F-6804-4D63-8798-EC57EE83E3AA
// Assembly location: D:\Work\_jimTools\谷歌秘钥\TyGAPC\TyGAPC.exe

using System;

namespace OtpNet
{
  public class Base32Encoding
  {
    public static byte[] ToBytes(string input)
    {
      input = !string.IsNullOrEmpty(input) ? input.TrimEnd('=') : throw new ArgumentNullException(nameof (input));
      int length = input.Length * 5 / 8;
      byte[] bytes = new byte[length];
      byte num1 = 0;
      byte num2 = 8;
      int index = 0;
      foreach (char c in input)
      {
        int num3 = Base32Encoding.CharToValue(c);
        if (num2 > (byte) 5)
        {
          int num4 = num3 << (int) num2 - 5;
          num1 |= (byte) num4;
          num2 -= (byte) 5;
        }
        else
        {
          int num5 = num3 >> 5 - (int) num2;
          byte num6 = (byte) ((uint) num1 | (uint) num5);
          bytes[index++] = num6;
          num1 = (byte) (num3 << 3 + (int) num2);
          num2 += (byte) 3;
        }
      }
      if (index != length)
        bytes[index] = num1;
      return bytes;
    }

    public static string ToString(byte[] input)
    {
      if (input == null || input.Length == 0)
        throw new ArgumentNullException(nameof (input));
      int length = (int) Math.Ceiling((double) input.Length / 5.0) * 8;
      char[] chArray1 = new char[length];
      byte b1 = 0;
      byte num1 = 5;
      int num2 = 0;
      foreach (byte num3 in input)
      {
        byte b2 = (byte) ((uint) b1 | (uint) num3 >> 8 - (int) num1);
        chArray1[num2++] = Base32Encoding.ValueToChar(b2);
        if (num1 < (byte) 4)
        {
          byte b3 = (byte) ((int) num3 >> 3 - (int) num1 & 31);
          chArray1[num2++] = Base32Encoding.ValueToChar(b3);
          num1 += (byte) 5;
        }
        num1 -= (byte) 3;
        b1 = (byte) ((int) num3 << (int) num1 & 31);
      }
      if (num2 != length)
      {
        char[] chArray2 = chArray1;
        int index = num2;
        int num4 = index + 1;
        int num5 = (int) Base32Encoding.ValueToChar(b1);
        chArray2[index] = (char) num5;
        while (num4 != length)
          chArray1[num4++] = '=';
      }
      return new string(chArray1);
    }

    private static int CharToValue(char c)
    {
      int num = (int) c;
      if (num < 91 && num > 64)
        return num - 65;
      if (num < 56 && num > 49)
        return num - 24;
      if (num < 123 && num > 96)
        return num - 97;
      throw new ArgumentException("Character is not a Base32 character.", nameof (c));
    }

    private static char ValueToChar(byte b)
    {
      if (b < (byte) 26)
        return (char) ((uint) b + 65U);
      if (b < (byte) 32)
        return (char) ((uint) b + 24U);
      throw new ArgumentException("Byte is not a Base32 value.", nameof (b));
    }
  }
}
