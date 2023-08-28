// Decompiled with JetBrains decompiler
// Type: GoogleAuthPcClient.InputDialog
// Assembly: TyGAPC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 18564C5F-6804-4D63-8798-EC57EE83E3AA
// Assembly location: D:\Work\_jimTools\谷歌秘钥\TyGAPC\TyGAPC.exe

using OtpNet;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GoogleAuthPcClient
{
  public class InputDialog : Form
  {
    private IContainer components;
    private Label label1;
    private TextBox textRemark;
    private Label label2;
    private TextBox textKey;
    private Button buttonOK;
    private Button buttonCancel;

    public InputDialog() => this.InitializeComponent();

    private void ButtonOK_Click(object sender, EventArgs e)
    {
      string text1 = this.textRemark.Text;
      string text2 = this.textKey.Text;
      if (string.IsNullOrWhiteSpace(text1) || string.IsNullOrWhiteSpace(text2))
      {
        int num1 = (int) MessageBox.Show("Notes or key cannot be empty (备注或密钥不能为空)", "error 错误提示");
      }
      else if (text1.Length > 20)
      {
        int num2 = (int) MessageBox.Show("Notes cannot exceed 20 chars (备注不能超过20字符)", "error 错误提示");
      }
      else
      {
        try
        {
          Base32Encoding.ToBytes(text2);
        }
        catch (Exception ex)
        {
          int num3 = (int) MessageBox.Show("Error: " + ex.Message);
          return;
        }
        foreach (GoogleKey googleKey in DataService.googleKeys)
        {
          if (googleKey.Remark.Equals(text1))
          {
            int num4 = (int) MessageBox.Show("Notes already exists (备注已存在)", "error 错误提示");
            return;
          }
          if (googleKey.Key.Equals(text2))
          {
            int num5 = (int) MessageBox.Show("Key already exists (密钥已存在)", "error 错误提示");
            return;
          }
        }
        DataService.inputKey = text2;
        DataService.inputRemark = text1;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void ButtonCancel_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.Cancel;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.textRemark = new TextBox();
      this.label2 = new Label();
      this.textKey = new TextBox();
      this.buttonOK = new Button();
      this.buttonCancel = new Button();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Font = new Font("宋体", 10.8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.label1.Location = new Point(23, 30);
      this.label1.Name = "label1";
      this.label1.Size = new Size(167, 19);
      this.label1.TabIndex = 0;
      this.label1.Text = "key notes (备注)";
      this.textRemark.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.textRemark.Location = new Point(27, 65);
      this.textRemark.Name = "textRemark";
      this.textRemark.Size = new Size(325, 30);
      this.textRemark.TabIndex = 1;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("宋体", 10.8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.label2.Location = new Point(23, 115);
      this.label2.Name = "label2";
      this.label2.Size = new Size(177, 19);
      this.label2.TabIndex = 2;
      this.label2.Text = "google key (密钥)";
      this.textKey.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.textKey.Location = new Point(27, 149);
      this.textKey.Name = "textKey";
      this.textKey.Size = new Size(325, 30);
      this.textKey.TabIndex = 3;
      this.buttonOK.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.buttonOK.Font = new Font("宋体", 10.8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.buttonOK.Location = new Point(27, 227);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new Size(153, 36);
      this.buttonOK.TabIndex = 4;
      this.buttonOK.Text = "add 添加";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new EventHandler(this.ButtonOK_Click);
      this.buttonCancel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.buttonCancel.Font = new Font("宋体", 10.8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.buttonCancel.Location = new Point(204, 227);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new Size(148, 36);
      this.buttonCancel.TabIndex = 5;
      this.buttonCancel.Text = "cancel 取消";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new EventHandler(this.ButtonCancel_Click);
      this.AutoScaleDimensions = new SizeF(8f, 15f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(380, 286);
      this.Controls.Add((Control) this.buttonCancel);
      this.Controls.Add((Control) this.buttonOK);
      this.Controls.Add((Control) this.textKey);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.textRemark);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.Name = nameof (InputDialog);
      this.Text = "add key (添加密钥)";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
