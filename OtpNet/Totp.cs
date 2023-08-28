// Decompiled with JetBrains decompiler
// Type: OtpNet.Totp
// Assembly: TyGAPC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 18564C5F-6804-4D63-8798-EC57EE83E3AA
// Assembly location: D:\Work\_jimTools\谷歌秘钥\TyGAPC\TyGAPC.exe

using System;

namespace OtpNet
{
  public class Totp : Otp
  {
    private const long unixEpochTicks = 621355968000000000;
    private const long ticksToSeconds = 10000000;
    private readonly int step;
    private readonly int totpSize;
    private readonly TimeCorrection correctedTime;

    public Totp(
      byte[] secretKey,
      int step = 30,
      OtpHashMode mode = OtpHashMode.Sha1,
      int totpSize = 6,
      TimeCorrection timeCorrection = null)
      : base(secretKey, mode)
    {
      Totp.VerifyParameters(step, totpSize);
      this.step = step;
      this.totpSize = totpSize;
      this.correctedTime = timeCorrection ?? TimeCorrection.UncorrectedInstance;
    }

    private static void VerifyParameters(int step, int totpSize)
    {
      if (step <= 0)
        throw new ArgumentOutOfRangeException(nameof (step));
      if (totpSize <= 0)
        throw new ArgumentOutOfRangeException(nameof (totpSize));
      if (totpSize > 10)
        throw new ArgumentOutOfRangeException(nameof (totpSize));
    }

    public string ComputeTotp(DateTime timestamp) => this.ComputeTotpFromSpecificTime(this.correctedTime.GetCorrectedTime(timestamp));

    public string ComputeTotp() => this.ComputeTotpFromSpecificTime(this.correctedTime.CorrectedUtcNow);

    private string ComputeTotpFromSpecificTime(DateTime timestamp) => this.Compute(this.CalculateTimeStepFromTimestamp(timestamp), this.hashMode);

    public bool VerifyTotp(string totp, out long timeStepMatched, VerificationWindow window = null) => this.VerifyTotpForSpecificTime(this.correctedTime.CorrectedUtcNow, totp, window, out timeStepMatched);

    public bool VerifyTotp(
      DateTime timestamp,
      string totp,
      out long timeStepMatched,
      VerificationWindow window = null)
    {
      return this.VerifyTotpForSpecificTime(this.correctedTime.GetCorrectedTime(timestamp), totp, window, out timeStepMatched);
    }

    private bool VerifyTotpForSpecificTime(
      DateTime timestamp,
      string totp,
      VerificationWindow window,
      out long timeStepMatched)
    {
      return this.Verify(this.CalculateTimeStepFromTimestamp(timestamp), totp, out timeStepMatched, window);
    }

    private long CalculateTimeStepFromTimestamp(DateTime timestamp) => (timestamp.Ticks - 621355968000000000L) / 10000000L / (long) this.step;

    public int RemainingSeconds() => this.RemainingSecondsForSpecificTime(this.correctedTime.CorrectedUtcNow);

    public int RemainingSeconds(DateTime timestamp) => this.RemainingSecondsForSpecificTime(this.correctedTime.GetCorrectedTime(timestamp));

    private int RemainingSecondsForSpecificTime(DateTime timestamp) => this.step - (int) ((timestamp.Ticks - 621355968000000000L) / 10000000L % (long) this.step);

    protected override string Compute(long counter, OtpHashMode mode) => Otp.Digits(this.CalculateOtp(KeyUtilities.GetBigEndianBytes(counter), mode), this.totpSize);
  }
}
