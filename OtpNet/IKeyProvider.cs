// Decompiled with JetBrains decompiler
// Type: OtpNet.IKeyProvider
// Assembly: TyGAPC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 18564C5F-6804-4D63-8798-EC57EE83E3AA
// Assembly location: D:\Work\_jimTools\谷歌秘钥\TyGAPC\TyGAPC.exe

namespace OtpNet
{
  public interface IKeyProvider
  {
    byte[] ComputeHmac(OtpHashMode mode, byte[] data);
  }
}
