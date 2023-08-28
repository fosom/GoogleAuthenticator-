// Decompiled with JetBrains decompiler
// Type: OtpNet.InMemoryKey
// Assembly: TyGAPC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 18564C5F-6804-4D63-8798-EC57EE83E3AA
// Assembly location: D:\Work\_jimTools\谷歌秘钥\TyGAPC\TyGAPC.exe

using System;
using System.Security.Cryptography;

namespace OtpNet
{
  public class InMemoryKey : IKeyProvider
  {
    private static readonly object platformSupportSync = new object();
    private readonly object stateSync = new object();
    private readonly byte[] KeyData;
    private readonly int keyLength;

    public InMemoryKey(byte[] key)
    {
      if (key == null)
        throw new ArgumentNullException(nameof (key));
      this.keyLength = key.Length != 0 ? key.Length : throw new ArgumentException("The key must not be empty");
      this.KeyData = new byte[(int) Math.Ceiling((Decimal) key.Length / 16M) * 16];
      Array.Copy((Array) key, (Array) this.KeyData, key.Length);
    }

    internal byte[] GetCopyOfKey()
    {
      byte[] destinationArray = new byte[this.keyLength];
      lock (this.stateSync)
        Array.Copy((Array) this.KeyData, (Array) destinationArray, this.keyLength);
      return destinationArray;
    }

    public byte[] ComputeHmac(OtpHashMode mode, byte[] data)
    {
      using (HMAC hmacHash = InMemoryKey.CreateHmacHash(mode))
      {
        byte[] copyOfKey = this.GetCopyOfKey();
        try
        {
          hmacHash.Key = copyOfKey;
          return hmacHash.ComputeHash(data);
        }
        finally
        {
          KeyUtilities.Destroy(copyOfKey);
        }
      }
    }

    private static HMAC CreateHmacHash(OtpHashMode otpHashMode)
    {
      HMAC hmacHash;
      switch (otpHashMode)
      {
        case OtpHashMode.Sha256:
          hmacHash = (HMAC) new HMACSHA256();
          break;
        case OtpHashMode.Sha512:
          hmacHash = (HMAC) new HMACSHA512();
          break;
        default:
          hmacHash = (HMAC) new HMACSHA1();
          break;
      }
      return hmacHash;
    }
  }
}
