// Decompiled with JetBrains decompiler
// Type: GoogleAuthPcClient.Properties.Settings
// Assembly: TyGAPC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 18564C5F-6804-4D63-8798-EC57EE83E3AA
// Assembly location: D:\Work\_jimTools\谷歌秘钥\TyGAPC\TyGAPC.exe

using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace GoogleAuthPcClient.Properties
{
  [CompilerGenerated]
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.1.0.0")]
  internal sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    public static Settings Default
    {
      get
      {
        Settings defaultInstance = Settings.defaultInstance;
        Settings settings = defaultInstance;
        return settings;
      }
    }
  }
}
