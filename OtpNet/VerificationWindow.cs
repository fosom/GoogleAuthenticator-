// Decompiled with JetBrains decompiler
// Type: OtpNet.VerificationWindow
// Assembly: TyGAPC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 18564C5F-6804-4D63-8798-EC57EE83E3AA
// Assembly location: D:\Work\_jimTools\谷歌秘钥\TyGAPC\TyGAPC.exe

using System.Collections.Generic;

namespace OtpNet
{
  public class VerificationWindow
  {
    public static readonly VerificationWindow RfcSpecifiedNetworkDelay = new VerificationWindow(1, 1);
    private readonly int previous;
    private readonly int future;

    public VerificationWindow(int previous = 0, int future = 0)
    {
      this.previous = previous;
      this.future = future;
    }

    public IEnumerable<long> ValidationCandidates(long initialFrame)
    {
      yield return initialFrame;
      int i;
      for (i = 1; i <= this.previous; ++i)
      {
        long num = initialFrame - (long) i;
        if (num >= 0L)
          yield return num;
        else
          break;
      }
      for (i = 1; i <= this.future; ++i)
        yield return initialFrame + (long) i;
    }
  }
}
