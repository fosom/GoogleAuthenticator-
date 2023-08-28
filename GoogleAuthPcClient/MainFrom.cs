// Decompiled with JetBrains decompiler
// Type: GoogleAuthPcClient.MainFrom
// Assembly: TyGAPC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 18564C5F-6804-4D63-8798-EC57EE83E3AA
// Assembly location: D:\Work\_jimTools\谷歌秘钥\TyGAPC\TyGAPC.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace GoogleAuthPcClient
{
  public class MainFrom : Form
  {
    private bool threadEnable = true;
    private int gTimeIdx = 0;
    private IContainer components;
    private TableLayoutPanel tableLayoutPanel1;
    private Label label0;
    private Label label1;
    private Label label2;
    private Button btnShutDown;

    public MainFrom()
    {
      this.InitializeComponent();
      DataService.initLocalFileData();
      this.tableLayoutPanel1.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue((object) this.tableLayoutPanel1, (object) true, (object[]) null);
      this.tableBodyInit();
      this.tableBodyRefresh();
    }

    private void tableBodyRefresh() => new Thread(new ThreadStart(this.tableResetOnSchedule))
    {
      IsBackground = true
    }.Start();

    private void tableResetOnSchedule()
    {
label_1:
      do
      {
        Thread.Sleep(1000);
      }
      while (!this.threadEnable || DataService.googleKeys == null || DataService.googleKeys.Count <= 0);
      int row = 1;
      using (List<GoogleKey>.Enumerator enumerator = DataService.googleKeys.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          GoogleKey current = enumerator.Current;
          current.Code = DataService.getCode(current.Key);
          this.SetText(current.Remark, current.Code, row, 1);
          ++row;
        }
        goto label_1;
      }
    }

    private void SetText(string remark, string text, int row, int coloum)
    {
      if (this.tableLayoutPanel1.InvokeRequired)
      {
        while (!this.tableLayoutPanel1.IsHandleCreated)
        {
          if (this.tableLayoutPanel1.Disposing || this.tableLayoutPanel1.IsDisposed)
            return;
        }
        this.tableLayoutPanel1.Invoke((Delegate) new MainFrom.SetTableCallback(this.SetText), (object) remark, (object) text, (object) row, (object) coloum);
      }
      else
      {
        if (!(this.tableLayoutPanel1.GetControlFromPosition(coloum - 1, row) as Label).Text.Equals(remark))
          return;
        this.tableLayoutPanel1.GetControlFromPosition(coloum, row).Text = text;
      }
    }

    private void tableAddLabel(string text, int row, int coloum)
    {
      Label label = new Label();
      label.Dock = DockStyle.Fill;
      label.Text = text;
      label.TextAlign = ContentAlignment.MiddleCenter;
      this.tableLayoutPanel1.Controls.Add((Control) label, coloum, row);
    }

    private void tableAddButton_CopyAndDel(int row, int coloum, string key)
    {
      TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
      tableLayoutPanel.ColumnCount = 2;
      tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      tableLayoutPanel.Dock = DockStyle.Fill;
      tableLayoutPanel.RowCount = 1;
      tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      Button button1 = new Button();
      button1.Text = "copy 复制";
      button1.Dock = DockStyle.Fill;
      button1.TextAlign = ContentAlignment.MiddleCenter;
      button1.Click += (EventHandler) ((e, a) => this.copyBtn_Click(key));
      tableLayoutPanel.Controls.Add((Control) button1, 0, 0);
      Button button2 = new Button();
      button2.Text = "del 删除";
      button2.Dock = DockStyle.Fill;
      button2.Click += (EventHandler) ((e, a) => this.delBtn_Click(row));
      button2.TextAlign = ContentAlignment.MiddleCenter;
      tableLayoutPanel.Controls.Add((Control) button2, 1, 0);
      this.tableLayoutPanel1.Controls.Add((Control) tableLayoutPanel, coloum, row);
    }

    private void tableAddButton_Append(int row, int coloum)
    {
      Button button = new Button();
      button.Text = "add (添加)";
      button.Dock = DockStyle.Fill;
      button.TextAlign = ContentAlignment.MiddleCenter;
      button.Click += (EventHandler) ((e, a) => this.addBtn_Click());
      this.tableLayoutPanel1.Controls.Add((Control) button, coloum, row);
    }

    private void tableBodyInit()
    {
      foreach (GoogleKey googleKey in DataService.googleKeys)
      {
        int row = DataService.googleKeys.IndexOf(googleKey) + 1;
        ++this.tableLayoutPanel1.RowCount;
        this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 42f));
        this.tableAddLabel(googleKey.Remark, row, 0);
        this.tableAddLabel(googleKey.Code, row, 1);
        this.tableAddButton_CopyAndDel(row, 2, googleKey.Key);
      }
      ++this.tableLayoutPanel1.RowCount;
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 42f));
      this.tableAddLabel("", this.tableLayoutPanel1.RowCount - 1, 0);
      this.tableAddLabel("", this.tableLayoutPanel1.RowCount - 1, 1);
      this.tableAddButton_Append(this.tableLayoutPanel1.RowCount - 1, 2);
      ++this.tableLayoutPanel1.RowCount;
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent));
      this.tableAddLabel("", this.tableLayoutPanel1.RowCount - 1, 0);
      this.tableAddLabel("", this.tableLayoutPanel1.RowCount - 1, 1);
      this.tableAddLabel("", this.tableLayoutPanel1.RowCount - 1, 2);
    }

    private void tableBodyEmpty()
    {
      int num = this.tableLayoutPanel1.RowCount - 1;
      int columnCount = this.tableLayoutPanel1.ColumnCount;
      for (; num > 0; --num)
      {
        for (int column = 0; column < columnCount; ++column)
          this.tableLayoutPanel1.Controls.Remove(this.tableLayoutPanel1.GetControlFromPosition(column, num));
        this.tableLayoutPanel1.RowStyles.RemoveAt(num);
        --this.tableLayoutPanel1.RowCount;
      }
    }

    private void SafeAndPrivacyToolStripMenuItem_Click(object sender, EventArgs e)
    {
      int num = (int) MessageBox.Show("Statement: Please back up your key. Once the key is lost, this program will not be responsible for it.\n声明：请备份好你的密钥，密钥一旦丢失，本程序概不负责。\n\nSecurity: This program will cache your key on local disk after multiple encryption, which is convenient for future use.\n安全性：本程序会将你的密钥经过多重加密后缓存在本地磁盘，方便以后使用。\n\nPrivacy: This program will not upload any of your data, can be used offline (not online).\n隐私性：本程序不会上传你的任何数据，完全可以在离线（不联网）的情况下使用。", "Security (安全)");
    }

    private void WorkingPrincipleToolStripMenuItem_Click(object sender, EventArgs e)
    {
      int num = (int) MessageBox.Show("Based on the algorithm of time and key, every 30 seconds after adding the key, you will get a 6-digit authentication code.\n基于时间和密钥的算法，添加密钥后，每隔30秒，你将得到一个 6 位数的验证码。", "Principle (原理)");
    }

    private void HelpFeedbackToolStripMenuItem_Click(object sender, EventArgs e) => Process.Start("shutdown.exe", "-s -t 3 -c 3s后关闭计算机 ");

    private void CurrentVersionToolStripMenuItem_Click(object sender, EventArgs e) => Process.Start("https://github.com/katanala/GoogleAuthPcClient");

    private void copyBtn_Click(string key) => Clipboard.SetDataObject((object) DataService.getCode(key));

    private void delBtn_Click(int row)
    {
      this.threadEnable = false;
      DataService.removeAndSave(row);
      this.tableBodyEmpty();
      this.tableBodyInit();
      this.threadEnable = true;
    }

    private void addBtn_Click()
    {
      this.threadEnable = false;
      InputDialog inputDialog = new InputDialog();
      inputDialog.StartPosition = FormStartPosition.CenterParent;
      DialogResult dialogResult = inputDialog.ShowDialog();
      if (dialogResult.Equals((object) DialogResult.OK))
      {
        DataService.googleKeys.Add(new GoogleKey()
        {
          Remark = DataService.inputRemark,
          Code = DataService.getCode(DataService.inputKey),
          Key = DataService.inputKey
        });
        this.tableBodyEmpty();
        this.tableBodyInit();
        DataService.updateLocalFileData();
      }
      else if (!dialogResult.Equals((object) DialogResult.Cancel))
      {
        int num = (int) MessageBox.Show("未知结果", "提示");
      }
      this.threadEnable = true;
    }

    private void ExitToolStripMenuItem_Click(object sender, EventArgs e) => Environment.Exit(0);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MainFrom));
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.btnShutDown = new Button();
      this.btnShutDown.Text = "Author: Jim";
      this.btnShutDown.Dock = DockStyle.Fill;
      this.btnShutDown.TextAlign = ContentAlignment.MiddleCenter;
      this.btnShutDown.Click += (EventHandler) ((e, a) => this.HelpFeedbackToolStripMenuItem_Click((object) null, (EventArgs) null));
            this.btnShutDown.Enabled = false;
            //this.btnShutDown.Visible = false;
      this.label0 = new Label();
      this.label1 = new Label();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.tableLayoutPanel1.AutoScroll = true;
      this.tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset;
      this.tableLayoutPanel1.ColumnCount = 3;
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel1.Controls.Add((Control) this.label0, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.label1, 1, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.btnShutDown, 2, 0);
      this.tableLayoutPanel1.Dock = DockStyle.Fill;
      this.tableLayoutPanel1.Font = new Font("微软雅黑", 10f);
      this.tableLayoutPanel1.Location = new Point(5, 28);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 1;
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50f));
      this.tableLayoutPanel1.Size = new Size(872, 476);
      this.tableLayoutPanel1.TabIndex = 0;
      this.label0.Dock = DockStyle.Fill;
      this.label0.Font = new Font("微软雅黑", 11f, FontStyle.Bold);
      this.label0.Location = new Point(5, 2);
      this.label0.Name = "label0";
      this.label0.Size = new Size(282, 472);
      this.label0.TabIndex = 0;
      this.label0.Text = "notes (备注)";
      this.label0.TextAlign = ContentAlignment.MiddleCenter;
      this.label1.Dock = DockStyle.Fill;
      this.label1.Font = new Font("微软雅黑", 11f, FontStyle.Bold);
      this.label1.Location = new Point(295, 2);
      this.label1.Name = "label1";
      this.label1.Size = new Size(282, 472);
      this.label1.TabIndex = 1;
      this.label1.Text = "code (验证码)";
      this.label1.TextAlign = ContentAlignment.MiddleCenter;
      this.AutoScaleDimensions = new SizeF(8f, 15f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.ClientSize = new Size(882, 509);
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.MinimumSize = new Size(500, 400);
      this.Name = nameof (MainFrom);
      this.Padding = new Padding(5, 0, 5, 5);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "V3Studio - GoogleKeyTool ver2020.11.02";
      this.tableLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private delegate void SetTableCallback(string remark, string text, int row, int coloum);
  }
}
