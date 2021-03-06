﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Diagnostics_Example
{
    public partial class MainForm : Form
    {
        private CodeArtEng.Diagnostics.ProcessExecutor procExecutor;

        public MainForm()
        {
            InitializeComponent();
            diagnosticsTextBox1.OutputFile = "Output.log";

            chkListenerEnabled.Checked = diagnosticsTextBox1.ListenerEnabled;
            chkAutoFlushEnabled.Checked = diagnosticsTextBox1.FlushEnabled;

            procExecutor = new CodeArtEng.Diagnostics.ProcessExecutor();
            propertyGrid1.SelectedObject = procExecutor;
        }

        private void chkListenerEnabled_CheckedChanged(object sender, EventArgs e)
        {
            diagnosticsTextBox1.ListenerEnabled = chkListenerEnabled.Checked;
        }

        private void chkAutoFlushEnabled_CheckedChanged(object sender, EventArgs e)
        {
            diagnosticsTextBox1.FlushEnabled = chkAutoFlushEnabled.Checked;
        }

        private void BtFlush_Click(object sender, EventArgs e)
        {
            Trace.Flush();
        }

        private void BtWrite_Click(object sender, EventArgs e)
        {
            for(int x = 0; x < 20; x++)
                Trace.Write(x.ToString() + " ");

            Trace.WriteLine(" ");
            Trace.WriteLine(" "); 
            
            for(int x = 0; x < 20; x++)
                Trace.WriteLine("Line " + x.ToString());
        }

        private void WorkerThread_DoWork(object sender, DoWorkEventArgs e)
        {
            Trace.WriteLine("Write message from worker thread...");
            for (int x = 0; x < 200; x++)
            {
                //Debug.WriteLine("Thread: Debug Message " + x.ToString());
                Trace.WriteLine("Thread: Trace Message " + x.ToString());
                System.Threading.Thread.Sleep(10);
            }
        }

        private void btThreadWrite_Click(object sender, EventArgs e)
        {
            WorkerThread.RunWorkerAsync();
            for (int x = 0; x < 100; x++)
            {
                //Debug.WriteLine("Main: Debug Message " + x.ToString());
                Trace.WriteLine("Main: Trace Message " + x.ToString());
                System.Threading.Thread.Sleep(20);
                Application.DoEvents();
            }
        }

        private void chkShowTimeStamp_CheckedChanged(object sender, EventArgs e)
        {
            diagnosticsTextBox1.ShowTimeStamp = chkShowTimeStamp.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CodeArtEng.Diagnostics.ProcessResult result = procExecutor.Execute(false);
            if(result != null)
            {
                if (result.ErrorDetected) Trace.WriteLine("ERROR Detected.");
                foreach (string line in result.Output)
                    Trace.WriteLine(line);
            }

        }
    }
}
