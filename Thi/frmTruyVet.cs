using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thi.Models;

namespace Thi
{
    public partial class frmTruyVet : Form
    {
        QLBN context = new QLBN();
        public frmTruyVet()
        {
            InitializeComponent();
            List<BenhNhan> listBN = context.BenhNhans.ToList();
            FillBNCombobox(listBN);
            //listTemp = listBN;
            BindGrid(listBN);
        }

        private void frmTruyVet_LocationChanged(object sender, EventArgs e)
        {

        }

        public void FillBNCombobox(List<BenhNhan> listBN)
        {
            this.cmb_BN.DataSource = listBN;
            this.cmb_BN.DisplayMember = "MaBNTenBN";
            this.cmb_BN.ValueMember = "MaBN";
        }



        

        private int ktF(BenhNhan bn)
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
        private void BindGrid(List<BenhNhan>  listTemp)
        {
            dtgc_TV.Rows.Clear();

            foreach (var item in listTemp)
            {
                int index = dtgc_TV.Rows.Add();
                dtgc_TV.Rows[index].Cells[0].Value = item.MaBN;
                dtgc_TV.Rows[index].Cells[1].Value = item.TenBN;
                dtgc_TV.Rows[index].Cells[2].Value = item.TinhTrang.TenTT;


                string F = string.Format("F{0}", ktF(item));
                dtgc_TV.Rows[index].Cells[3].Value = F;
            }
        }


        List<BenhNhan> listTemp = new List<BenhNhan>();
        private void cmb_BN_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = cmb_BN.Text;
            string firstSixCharacters = selectedValue.Length >= 6 ? selectedValue.Substring(0, 6) : selectedValue;
            BenhNhan temp = context.BenhNhans.FirstOrDefault(p => p.MaBN.ToString() == firstSixCharacters);

            listTemp.Clear(); // Xóa tất cả các phần tử trong danh sách

            if (cmb_BN.ValueMember.ToString() != "" ){
                // Lấy ra ValueMember của ComboBox cmb_BN
               

                while (temp.BNTXG != null)
                {
                    
                    temp = context.BenhNhans.FirstOrDefault(p => p.MaBN.ToString() == temp.BNTXG);
                    listTemp.Add(temp);
                }
                dtgc_TV.Rows.Clear();



                foreach (var item in listTemp)
                {
                    int index = dtgc_TV.Rows.Add();
                    dtgc_TV.Rows[index].Cells[0].Value = item.MaBN;
                    dtgc_TV.Rows[index].Cells[1].Value = item.TenBN;
                    dtgc_TV.Rows[index].Cells[2].Value = item.TinhTrang.TenTT;


                    string F = string.Format("F{0}", ktF(item));
                    dtgc_TV.Rows[index].Cells[3].Value = F;
                }   

            }
            
        }
    }
}
