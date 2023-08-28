// Decompiled with JetBrains decompiler
// Type: GoogleAuthPcClient.DataService
// Assembly: TyGAPC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 18564C5F-6804-4D63-8798-EC57EE83E3AA
// Assembly location: D:\Work\_jimTools\谷歌秘钥\TyGAPC\TyGAPC.exe

using OtpNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace GoogleAuthPcClient
{
  internal class DataService
  {
    public static List<GoogleKey> googleKeys = new List<GoogleKey>();
    public static string inputRemark = string.Empty;
    public static string inputKey = string.Empty;
    private static byte[] AES_IV = Encoding.UTF8.GetBytes("5243740823620719");
    private static string AES_PWD = "https:www.coinba.pro,http:coinba.co,";
    private static string programRoot1 = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
    private static string programRoot2 = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
    private static string programRoot3 = Environment.CurrentDirectory;
    private static string fileName = "googleKeyClient.cfg";

    public static void removeAndSave(int row)
    {
      --row;
      DataService.googleKeys.RemoveAt(row);
      DataService.updateLocalFileData();
    }

    public static void updateLocalFileData()
    {
      string str = Path.Combine(DataService.programRoot1, DataService.fileName);
      FileStream fileStream = DataService.getFileStream(str);
      if (fileStream == null)
      {
        str = Path.Combine(DataService.programRoot2, DataService.fileName);
        fileStream = DataService.getFileStream(str);
        if (fileStream == null)
        {
          str = Path.Combine(DataService.programRoot3, DataService.fileName);
          fileStream = DataService.getFileStream(str);
          if (fileStream == null)
          {
            int num = (int) MessageBox.Show("请以管理员身份运行本程序，否则不能读取、保存数据文件。");
            return;
          }
        }
      }
      fileStream.Close();
      using (StreamWriter streamWriter = new StreamWriter(str, false, Encoding.UTF8))
      {
        foreach (GoogleKey googleKey in DataService.googleKeys)
        {
          streamWriter.WriteLine(DataService.Encrypt(DataService.AES_PWD, googleKey.Remark));
          streamWriter.WriteLine(DataService.Encrypt(DataService.AES_PWD, googleKey.Key));
        }
      }
    }

    public static void initLocalFileData()
    {
      List<string> stringList = new List<string>();
      FileStream fileStream = DataService.getFileStream(Path.Combine(DataService.programRoot1, DataService.fileName));
      if (fileStream == null)
      {
        fileStream = DataService.getFileStream(Path.Combine(DataService.programRoot2, DataService.fileName));
        if (fileStream == null)
        {
          fileStream = DataService.getFileStream(Path.Combine(DataService.programRoot3, DataService.fileName));
          if (fileStream == null)
          {
            int num = (int) MessageBox.Show("请以管理员身份运行本程序，否则不能读取、保存数据文件。");
            return;
          }
        }
      }
      using (StreamReader streamReader = new StreamReader((Stream) fileStream, Encoding.UTF8))
      {
        for (string str = streamReader.ReadLine(); str != null; str = streamReader.ReadLine())
        {
          if (!string.IsNullOrWhiteSpace(str))
            stringList.Add(str);
        }
      }
      if (stringList == null || stringList.Count < 1 || stringList.Count % 2 != 0)
        return;
      for (int index = 0; index < stringList.Count; index += 2)
      {
        GoogleKey googleKey = new GoogleKey()
        {
          Remark = DataService.Decrypt(DataService.AES_PWD, stringList[index]),
          Key = DataService.Decrypt(DataService.AES_PWD, stringList[index + 1])
        };
        googleKey.Code = DataService.getCode(googleKey.Key);
        DataService.googleKeys.Add(googleKey);
      }
    }

    private static FileStream getFileStream(string filePath)
    {
      FileStream fileStream;
      try
      {
        fileStream = File.Exists(filePath) ? new FileStream(filePath, FileMode.OpenOrCreate) : File.Create(filePath);
      }
      catch (Exception ex)
      {
        return (FileStream) null;
      }
      return fileStream;
    }

    public static string getCode(string key) => new Totp(Base32Encoding.ToBytes(key)).ComputeTotp();

    public static byte[] GetKey(string pwd)
    {
      while (pwd.Length < 32)
        pwd += "0";
      pwd = pwd.Substring(0, 32);
      return Encoding.UTF8.GetBytes(pwd);
    }

    public static string Encrypt(string pwd, string data)
    {
      using (AesCryptoServiceProvider cryptoServiceProvider = new AesCryptoServiceProvider())
      {
        cryptoServiceProvider.Key = DataService.GetKey(pwd);
        cryptoServiceProvider.IV = DataService.AES_IV;
        ICryptoTransform encryptor = cryptoServiceProvider.CreateEncryptor(cryptoServiceProvider.Key, cryptoServiceProvider.IV);
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write))
          {
            using (StreamWriter streamWriter = new StreamWriter((Stream) cryptoStream))
              streamWriter.Write(data);
            return Convert.ToBase64String(memoryStream.ToArray());
          }
        }
      }
    }

    public static string Decrypt(string pwd, string data)
    {
      byte[] buffer = Convert.FromBase64String(data);
      using (AesCryptoServiceProvider cryptoServiceProvider = new AesCryptoServiceProvider())
      {
        cryptoServiceProvider.Key = DataService.GetKey(pwd);
        cryptoServiceProvider.IV = DataService.AES_IV;
        ICryptoTransform decryptor = cryptoServiceProvider.CreateDecryptor(cryptoServiceProvider.Key, cryptoServiceProvider.IV);
        using (MemoryStream memoryStream = new MemoryStream(buffer))
        {
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Read))
          {
            using (StreamReader streamReader = new StreamReader((Stream) cryptoStream))
              return streamReader.ReadToEnd();
          }
        }
      }
    }
  }
}
