using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace ShareBytes
{
    
    public partial class FileAccess : Form
    {

        public FileAccess()
        {
            InitializeComponent();
            //AutoFitForm.SetGroupBoxLoction(groupBox1);
            this.ControlBox = false;
            
        }
        void stoploading()
        {
            circularProgressBar1.Visible = false;
        }
        private void FileAccess_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            circularProgressBar1.Visible = true;
            if (backgroundWorker1.IsBusy != true)
            {
                backgroundWorker1.RunWorkerAsync();
            }
            
        }

        private void DGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            string path ="C:\\Users\\"+ Environment.UserName +"\\Downloads\\Sharebytes\\"+clsValues.UserID;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                try
                {
                    clsGooglStorage obj = new clsGooglStorage();
                    if (Directory.Exists(path))
                    {
                        obj.DownloadObject(clsValues.UserID,cmbFolder.Text+"/"+DGV["File Name", e.RowIndex].Value.ToString(), DGV["File Name", e.RowIndex].Value.ToString());
                        MessageBox.Show("Done ! Check \"Downloads\\Sharebytes\\"+clsValues.UserID+"\" Folder");
                    }
                    else
                    {
                        Directory.CreateDirectory(path);
                        obj.DownloadObject(clsValues.UserID, cmbFolder.Text + "/" + DGV["File Name",e.RowIndex].Value.ToString(), DGV["File Name", e.RowIndex].Value.ToString());
                        MessageBox.Show("Done ! Check \"Downloads\\Sharebytes\\" + clsValues.UserID + "\" Folder");
                    }
                    
                }
                catch(Exception ex) {

                }
            }
            
        }
        string globalcmbFolder;
        private void cmbFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            circularProgressBar1.Visible = true;
            globalcmbFolder = cmbFolder.Text;
            if (backgroundWorker2.IsBusy != true)
            {
                backgroundWorker2.RunWorkerAsync();
            }
        }
        DataTable cmbFolderDT=new DataTable();
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //cmbFolderDT= BindcmbFolder();
            clsDatastore obj = new clsDatastore();
            DataTable dt = new DataTable();
            try
            {
                string Folders = obj.GetUserFolders();
                string[] arrFname = null;
                dt.Columns.Add("Folders");
                arrFname = Folders.Split(',');

                for (int i = 0; i < arrFname.Length; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["Folders"] = arrFname[i];
                    dt.Rows.Add(dr);
                }

                //cmbFolder.DisplayMember = "Folders";
                //cmbFolder.ValueMember = "Folders";
                //cmbFolder.DataSource = dt;
            }
            catch (Exception ex)
            {

            }
            cmbFolderDT = dt;
        }

        DataTable dgvDT = new DataTable();
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            clsGooglStorage obj = new clsGooglStorage();
            DataTable dt = obj.ListObjects(globalcmbFolder + "/");
            dgvDT = dt;
            
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cmbFolder.DisplayMember = "Folders";
            cmbFolder.ValueMember = "Folders";
            cmbFolder.DataSource = cmbFolderDT;
            stoploading();
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DGV.DataSource = dgvDT;
            DGV.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DGV.Columns[0].DefaultCellStyle.ForeColor = Color.Black;
            DGV.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DGV.Columns[1].DefaultCellStyle.ForeColor = Color.Black;
            stoploading();
        }
    }

}
