using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thi.Models;

namespace Thi
{
    public partial class frm_QLBN : Form
    {
        QLBN context = new QLBN();
        public frm_QLBN()
        {
            InitializeComponent();
            
            List<TinhTrang> listTT = context.TinhTrangs.ToList();
            FillTTCombobox(listTT);

            List<BenhNhan> listBN = context.BenhNhans.ToList();
            FillNhiemTuCombobox(listBN);

            BindGrid(listBN);
        }

        private void frm_QLBN_Load(object sender, EventArgs e)
        {

        }


        public void FillTTCombobox(List<TinhTrang> listTT)
        {
            this.cmb_TinhTrang.DataSource = listTT;
            this.cmb_TinhTrang.DisplayMember = "TenTT";
            this.cmb_TinhTrang.ValueMember = "MaTT";
        }

        public void FillNhiemTuCombobox(List<BenhNhan> listBN)
        {
            this.cmb_LayNhiem.DataSource = listBN;
            this.cmb_LayNhiem.DisplayMember = "MaBN";
            this.cmb_LayNhiem.ValueMember = "MaBN";
        }


        private int ktF (BenhNhan bn)
        {
            int f = 1;
            if (bn.BNTXG == null)
            {
                return 0;
            } 
            else
            {
                BenhNhan temp = context.BenhNhans.FirstOrDefault(p => p.MaBN.ToString() == bn.BNTXG);
                
                
                while (temp.BNTXG != null)
                {
                    //t = string.Format("F{0}", f);
                    temp = context.BenhNhans.FirstOrDefault(p => p.MaBN.ToString() == temp.BNTXG);
                    f++;
                }
                
            }
            return f;


        }

        //HÀM binding gridview từ list sinh viên
        private void BindGrid(List<BenhNhan> listBN)
        {
            dtgv_BN.Rows.Clear();
            foreach (var item in listBN)
            {
                int index = dtgv_BN.Rows.Add();
                dtgv_BN.Rows[index].Cells[0].Value = item.MaBN;
                dtgv_BN.Rows[index].Cells[1].Value = item.TenBN;
                dtgv_BN.Rows[index].Cells[2].Value = item.TinhTrang.TenTT;


                string F = string.Format("F{0}", ktF(item));
                dtgv_BN.Rows[index].Cells[3].Value = F;
            }
        }

        int rowIndex = -1;
        String tempMaBN;
        private void dtgv_BN_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //lấy chỉ số dòng được chọn
            rowIndex = e.RowIndex;

            //kiểm tra nếu chỉ số dòng hợp lệ
            if (rowIndex >= 0 && rowIndex < dtgv_BN.Rows.Count)
            {
                DataGridViewRow selectedRow = dtgv_BN.Rows[rowIndex];
                txb_MaBN.Text = selectedRow.Cells[0].Value.ToString();
                tempMaBN = txb_MaBN.Text;
                txb_TenBN.Text = selectedRow.Cells[1].Value.ToString();
                cmb_TinhTrang.Text = selectedRow.Cells[2].Value.ToString();

                BenhNhan temp = context.BenhNhans.FirstOrDefault(p => p.MaBN.ToString() == txb_MaBN.Text);
               if(temp.BNTXG == null)
                {
                    cmb_LayNhiem.Text = "";
                }
                else
                {
                    cmb_LayNhiem.Text = temp.BNTXG.ToString();
                }
                
            }
        }



        private void btn_CapNhat_Click(object sender, EventArgs e)
        {

            if (txb_MaBN.Text == "" || txb_TenBN.Text == "")
            {
                MessageBox.Show("Vui Long Nhap thong tin BN");

            }

            if (rowIndex == -1)
            {
                string input = txb_MaBN.Text;

                // Sử dụng regex để kiểm tra chuỗi
                if (Regex.IsMatch(input, @"^(?=.*[a-zA-Z])(?=.*\d).{6}$"))
                {
                    // Chuỗi hợp lệ (bao gồm chính xác 10 chữ số)
                    // Thực hiện các hành động cần thiết tại đây

                    try
                    {
                        if (cmb_LayNhiem.SelectedValue.ToString() == "")
                        {
                            BenhNhan b = new BenhNhan()
                            {
                                MaBN = txb_MaBN.Text.ToString(),
                                TenBN = txb_TenBN.Text.ToString(),
                                MaTT = int.Parse(cmb_TinhTrang.SelectedValue.ToString()),
                                GhiChu = txt_GhiChu.Text.ToString(),

                                BNTXG = null,
                            };
                            context.BenhNhans.Add(b);
                            context.SaveChanges();
                            List<BenhNhan> ListB = context.BenhNhans.ToList();
                            BindGrid(ListB);

                        }
                        else
                        {

                            BenhNhan bn = new BenhNhan()
                            {
                                MaBN = txb_MaBN.Text.ToString(),
                                TenBN = txb_TenBN.Text.ToString(),
                                MaTT = int.Parse(cmb_TinhTrang.SelectedValue.ToString()),
                                GhiChu = txt_GhiChu.Text.ToString(),

                                BNTXG = cmb_LayNhiem.SelectedValue.ToString(),
                            };
                            context.BenhNhans.Add(bn);
                            context.SaveChanges();
                            List<BenhNhan> ListBN = context.BenhNhans.ToList();
                            BindGrid(ListBN);
                        }

                        MessageBox.Show("cap nhat thanh cong ");
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
                else
                {

                    // Chuỗi không hợp lệ
                    MessageBox.Show("LỖI KHÔNG ĐỊNH DẠNG , VUI LÒNG NHẬP 6 KY TU  !!!");
                    txb_MaBN.Clear();
                    txb_TenBN.Focus();
                }
            }

            else
            {
                BenhNhan dbUpdate = context.BenhNhans.FirstOrDefault(p => p.MaBN.ToString() == tempMaBN);
                if (dbUpdate != null)
                {
                    dbUpdate.MaBN = txb_MaBN.Text.ToString();
                    dbUpdate.TenBN = txb_TenBN.Text.ToString();
                    dbUpdate.MaTT = int.Parse(cmb_TinhTrang.SelectedValue.ToString());
                    dbUpdate.GhiChu = txt_GhiChu.Text.ToString();

                    if (cmb_LayNhiem.SelectedValue.ToString() == "")
                    {
                        dbUpdate.BNTXG = null;
                    }
                    else
                    {
                        dbUpdate.BNTXG = cmb_LayNhiem.SelectedValue.ToString();
                    }
                        

                    context.SaveChanges();
                    List<BenhNhan> ListStudents = context.BenhNhans.ToList();

                    BindGrid(ListStudents);
                    MessageBox.Show("sửa thành công !!!");
                }

            }




        }

        private void truyVếtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTruyVet f = new frmTruyVet();
            f.Show();
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Thoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
