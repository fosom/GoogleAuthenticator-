// Decompiled with JetBrains decompiler
// Type: OtpNet.KeyUtilities
// Assembly: TyGAPC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 18564C5F-6804-4D63-8798-EC57EE83E3AA
// Assembly location: D:\Work\_jimTools\谷歌秘钥\TyGAPC\TyGAPC.exe

using System;

namespace OtpNet
{
  internal class KeyUtilities
  {
    internal static void Destroy(byte[] sensitiveData)
    {
      if (sensitiveData == null)
        throw new ArgumentNullException(nameof (sensitiveData));
      new Random().NextBytes(sensitiveData);
    }

    internal static byte[] GetBigEndianBytes(long input)
    {
      byte[] bytes = BitConverter.GetBytes(input);
      Array.Reverse((Array) bytes);
      return bytes;
    }

    internal static byte[] GetBigEndianBytes(int input)
    {
      byte[] bytes = BitConverter.GetBytes(input);
      Array.Reverse((Array) bytes);
      return bytes;
    }
  }
}
