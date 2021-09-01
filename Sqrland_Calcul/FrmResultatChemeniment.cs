using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Sqrland_Calcul
{
    public partial class FrmResultatChemeniment : Form
    {
        public FrmResultatChemeniment()
        {
            InitializeComponent();
        }

        private void FrmResultatChemeniment_Load(object sender, EventArgs e)
        {
            dataGridView4.DataSource = Calc_Angle.cheminements;
            dataGridView4.Columns[1].Visible = false;
            dataGridView4.Columns[2].HeaderText = "Angle";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            if (dataGridView4.Rows.Count > 0)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "PDF (*.pdf) | *.pdf";
                save.FileName = "Result.pdf";
                bool ErrorMessage = false;
                if(save.ShowDialog()== DialogResult.OK)
                {
                    if (File.Exists(save.FileName))
                    {
                        try
                        {
                            File.Delete(save.FileName);
                        }catch(Exception ex)
                        {
                            ErrorMessage = true;
                            MessageBox.Show("Unable to write data in disk"+ex.Message);
                        }
                    }
                    if (!ErrorMessage)
                    {
                        try
                        {
                            PdfPTable pTable = new PdfPTable(dataGridView4.Columns.Count);
                            pTable.DefaultCell.Padding = 2;
                            pTable.WidthPercentage = 100;
                            pTable.HorizontalAlignment = Element.ALIGN_LEFT;

                            foreach(DataGridViewColumn col in dataGridView4.Columns)
                            {
                                PdfPCell pcell = new PdfPCell(new Phrase(col.HeaderText));
                                pTable.AddCell(pcell);
                            }
                            foreach (DataGridViewRow Row in dataGridView4.Rows)
                            {
                                foreach (DataGridViewCell cell in Row.Cells)
                                {
                                    pTable.AddCell(cell.Value.ToString());
                                }
                            }

                            using(FileStream fileStream = new FileStream(save.FileName, FileMode.Create))
                            {
                                Document document = new Document(PageSize.A4,8f,16f,16f,8f);
                                PdfWriter.GetInstance(document, fileStream);
                                document.Open();
                                document.Add(pTable);
                                document.Close();
                                fileStream.Close();
                            }
                            MessageBox.Show("Data export successfuly","info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error exporting" + ex.Message);
                        }

                    }
                }
            }
            else
            {
                MessageBox.Show("No record Found","Info");

            }
        }
    }
}
