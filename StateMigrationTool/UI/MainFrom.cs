using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using StateMigrationBackend.Models;
using StateMigrationBackend.StateRegions;

namespace StateMigration.UI
{
    public partial class mainFrom : MetroFramework.Forms.MetroForm
    {
        #region GlobalVariables        

        CancellationTokenSource cts = new CancellationTokenSource();
        
        Regions regions = new Regions();        

        SystemDetails sysdetails = new SystemDetails();

        SettingsModel settings = new SettingsModel();     

        List<string> currentStats = new List<string>();

        List<string> custompaths = new List<string>();

        public OperationModel CurrentOperationModel { get; set; }
        
        private static string user = string.Empty;
        
        private static string TargetPath = string.Empty;        

        System.Timers.Timer btnInitializetimer = new System.Timers.Timer();        

        #endregion

        #region Main

        public mainFrom()
        {            
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            InitializeUIDefaults();
            
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;                               
                return cp;
            }
        }
                

        //protected override void OnPaintBackground(PaintEventArgs e)
        //{

        //}

        private void MainFrom_Load(object sender, EventArgs e)
        {
            //SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);            
            btnInitializetimer.Interval = 200;
            btnInitializetimer.Elapsed += btnInitializetimer_Elapsed;
            btnInitializetimer.Start();

        }

        #endregion

        #region GlobalHelpers

        private void btnInitializetimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (IsPersonvalid(objPerson))
            {
                if(mtbtnBackup.IsHandleCreated)
                    mtbtnBackup.Invoke((MethodInvoker)(() => mtbtnBackup.Enabled = true));
                if(mtbtnRestore.IsHandleCreated)
                    mtbtnRestore.Invoke((MethodInvoker)(() => mtbtnRestore.Enabled = true));
                btnInitializetimer.Stop();
                
            }

        }

        private bool IsPersonvalid(Person _perobj)
        {
            if (_perobj != null)
            {
                return (string.IsNullOrWhiteSpace(_perobj.Name) && string.IsNullOrWhiteSpace(_perobj.Company)) ? false : true;
            }
            else
                return false;
            
        }

        #endregion

        #region ButtonEvents             

        private void mtbtnBackup_Click(object sender, EventArgs e)
        {
            InitializeBackupUI();

            Destination destination = new Destination();        

            cts = new CancellationTokenSource();
            
            user = ((mttxtboxUserID.Text).Equals(Environment.UserName) ? Environment.UserName : mttxtboxUserID.Text).ToLower();

            TargetPath = string.IsNullOrWhiteSpace(destination.Resolve(objPerson)) ? Path.Combine(@"C:\Temp", user) : Path.Combine(destination.Resolve(objPerson), user);
            
            StartBackup(regions, user,objPerson, TargetPath, cts.Token);

        }

        private void mtbtnRestore_Click(object sender, EventArgs e)
        {
            InitializeRestoreUI();

            Destination destination = new Destination();

            cts = new CancellationTokenSource();

            user = ((mttxtboxUserID.Text).Equals(Environment.UserName) ? Environment.UserName : mttxtboxUserID.Text).ToLower();
            TargetPath = string.IsNullOrWhiteSpace(destination.Resolve(objPerson)) ? Path.Combine(@"C:\Temp", user) : Path.Combine(destination.Resolve(objPerson), user);
            
            StartRestore(user, TargetPath, cts.Token);

        }

        private void mtbtnCancel_Click(object sender, EventArgs e)
        {
            cts.Cancel();           

        }

        private void mtbtnExit_Click(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate {
                cts.Dispose();                
            });
            btnInitializetimer.Dispose();
            Application.Exit();
        }

        #endregion

        #region ContextMenu

        private void addItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            fbdDefaults.Description = "Select Item";            
            fbdDefaults.RootFolder = Environment.SpecialFolder.MyComputer;
            if (fbdDefaults.ShowDialog() == DialogResult.OK)
            {
                lstViewDefaultPaths.Items.Add(fbdDefaults.SelectedPath);
                lstViewDefaultPaths.Update();                
                regions.SetPath(fbdDefaults.SelectedPath);
                custompaths.Add(fbdDefaults.SelectedPath);
                //SettingResponseLabelInfo($"Selected Source : {objPerson.CustomHomeDirectory}");

            }
        }

        private void removeItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (lstViewDefaultPaths.SelectedIndex != -1)
            {
                for (int i = lstViewDefaultPaths.SelectedItems.Count - 1; i >= 0; i--)
                lstViewDefaultPaths.Items.Remove(lstViewDefaultPaths.SelectedItems[i]);
            }            
            
        }





        #endregion
               

        private void mtabtlinklblEmail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:UK.IT.WintelServerSupport@fly.virgin.com");
        }

        private void mtabtlinklblSkype_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("sip:<Harpal.Singh@fly.virgin.com>");
        }
      
    
    }
}
