using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;


namespace ShareBytes
{
    public partial class Add_Folder : Form
    {
        
        public Add_Folder()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string Folder_Name = null;
            clsDatastore obj = new clsDatastore();
            try
            {
                if (txtFolderName.Text != String.Empty)
                {
                    foreach (DataGridViewRow row in dgv_folders.Rows)
                        Folder_Name = Folder_Name + row.Cells["Folders"].Value.ToString() + ",";

                    Folder_Name = Folder_Name + txtFolderName.Text;
                     obj.UpdateUserFolder(Folder_Name);
                    LoadDGVData();
                }
                else
                {
                    MessageBox.Show("Enter Folder Name");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void dgv_Product_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string Folder_Name = null;
            clsDatastore obj = new clsDatastore();
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {

                    dgv_folders.AutoGenerateColumns = false;
                    dgv_folders.Rows[e.RowIndex].Selected = true;
                    if (dgv_folders.SelectedCells.Count > 0)
                    {
                        DataGridViewRow selectrow = dgv_folders.Rows[e.RowIndex];
                        //Folder_Name = selectrow.Cells["Folders"].Value.ToString();

                        if (dgv_folders.Columns[e.ColumnIndex].HeaderText == "Delete")
                        {
                            //if (DialogResult.Yes == MessageBox.Show("Do You Want Delete ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                            if (MessageBox.Show("Do You Want to Delete ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                dgv_folders.Rows.RemoveAt(e.RowIndex);
                                
                                foreach (DataGridViewRow row in dgv_folders.Rows)
                                    Folder_Name=Folder_Name+ row.Cells["Folders"].Value.ToString()+",";

                                Folder_Name = Folder_Name.TrimEnd(',');
                                try
                                {
                                    obj.UpdateUserFolder(Folder_Name);
                                }
                                catch (Exception ex) { }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {  }
        }

        void LoadDGVData()
        {
            clsDatastore obj = new clsDatastore();
            try
            {
                string Folders = obj.GetUserFolders();
                string[] arrFname = null;
                DataTable dt = new DataTable();
                dt.Columns.Add("Folders");
                arrFname = Folders.Split(',');

                for (int i = 0; i < arrFname.Length; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["Folders"] = arrFname[i];
                    dt.Rows.Add(dr);
                }
                dgv_folders.DataSource = dt;
            }
            catch (Exception ex) { }
        }

        private void Add_Form_Load(object sender, EventArgs e)
        {
            LoadDGVData();
        }
    }
}
