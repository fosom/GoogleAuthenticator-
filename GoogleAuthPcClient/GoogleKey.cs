// Decompiled with JetBrains decompiler
// Type: GoogleAuthPcClient.GoogleKey
// Assembly: TyGAPC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 18564C5F-6804-4D63-8798-EC57EE83E3AA
// Assembly location: D:\Work\_jimTools\谷歌秘钥\TyGAPC\TyGAPC.exe

namespace GoogleAuthPcClient
{
  internal class GoogleKey
  {
    private string remark;
    private string key;
    private string code;

    public string Remark
    {
      get => this.remark;
      set => this.remark = value;
    }

    public string Key
    {
      get => this.key;
      set => this.key = value;
    }

    public string Code
    {
      get => this.code;
      set => this.code = value;
    }
  }
}
