// Decompiled with JetBrains decompiler
// Type: OtpNet.Otp
// Assembly: TyGAPC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 18564C5F-6804-4D63-8798-EC57EE83E3AA
// Assembly location: D:\Work\_jimTools\谷歌秘钥\TyGAPC\TyGAPC.exe

using System;

namespace OtpNet
{
  public abstract class Otp
  {
    protected readonly IKeyProvider secretKey;
    protected readonly OtpHashMode hashMode;

    public Otp(byte[] secretKey, OtpHashMode mode)
    {
      if (secretKey == null)
        throw new ArgumentNullException(nameof (secretKey));
      this.secretKey = secretKey.Length != 0 ? (IKeyProvider) new InMemoryKey(secretKey) : throw new ArgumentException("secretKey empty");
      this.hashMode = mode;
    }

    protected abstract string Compute(long counter, OtpHashMode mode);

    protected internal long CalculateOtp(byte[] data, OtpHashMode mode)
    {
      byte[] hmac = this.secretKey.ComputeHmac(mode, data);
      int index = (int) hmac[hmac.Length - 1] & 15;
      return (long) (((int) hmac[index] & (int) sbyte.MaxValue) << 24 | ((int) hmac[index + 1] & (int) byte.MaxValue) << 16 | ((int) hmac[index + 2] & (int) byte.MaxValue) << 8 | ((int) hmac[index + 3] & (int) byte.MaxValue) % 1000000);
    }

    protected internal static string Digits(long input, int digitCount) => ((int) input % (int) Math.Pow(10.0, (double) digitCount)).ToString().PadLeft(digitCount, '0');

    protected bool Verify(
      long initialStep,
      string valueToVerify,
      out long matchedStep,
      VerificationWindow window)
    {
      if (window == null)
        window = new VerificationWindow();
      foreach (long validationCandidate in window.ValidationCandidates(initialStep))
      {
        if (this.ValuesEqual(this.Compute(validationCandidate, this.hashMode), valueToVerify))
        {
          matchedStep = validationCandidate;
          return true;
        }
      }
      matchedStep = 0L;
      return false;
    }

    private bool ValuesEqual(string a, string b)
    {
      if (a.Length != b.Length)
        return false;
      int num = 0;
      for (int index = 0; index < a.Length; ++index)
        num |= (int) a[index] ^ (int) b[index];
      return num == 0;
    }
  }
}
