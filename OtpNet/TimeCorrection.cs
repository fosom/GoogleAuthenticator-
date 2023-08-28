// Decompiled with JetBrains decompiler
// Type: OtpNet.TimeCorrection
// Assembly: TyGAPC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 18564C5F-6804-4D63-8798-EC57EE83E3AA
// Assembly location: D:\Work\_jimTools\谷歌秘钥\TyGAPC\TyGAPC.exe

using System;

namespace OtpNet
{
  public class TimeCorrection
  {
    public static readonly TimeCorrection UncorrectedInstance = new TimeCorrection();
    private readonly TimeSpan timeCorrectionFactor;

    private TimeCorrection() => this.timeCorrectionFactor = TimeSpan.FromSeconds(0.0);

    public TimeCorrection(DateTime correctUtc) => this.timeCorrectionFactor = DateTime.UtcNow - correctUtc;

    public TimeCorrection(DateTime correctTime, DateTime referenceTime) => this.timeCorrectionFactor = referenceTime - correctTime;

    public DateTime GetCorrectedTime(DateTime referenceTime) => referenceTime - this.timeCorrectionFactor;

    public DateTime CorrectedUtcNow => this.GetCorrectedTime(DateTime.UtcNow);

    public TimeSpan CorrectionFactor => this.timeCorrectionFactor;
  }
}
