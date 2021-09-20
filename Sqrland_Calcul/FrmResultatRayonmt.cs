using DGVPrinterHelper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sqrland_Calcul
{
    public partial class FrmResultatRayonmt : Form
    {
        public FrmResultatRayonmt()
        {
            InitializeComponent();
        }

        private void FrmResultatRayonmt_Load(object sender, EventArgs e)
        {
            dgr.DataSource = Calc_Angle.rayonnements;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dgr.Rows.Count > 0)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "PDF (*.pdf) | *.pdf";
                save.FileName = "Result.pdf";
                bool ErrorMessage = false;
                if (save.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(save.FileName))
                    {
                        try
                        {
                            File.Delete(save.FileName);
                        }
                        catch (Exception ex)
                        {
                            ErrorMessage = true;
                            MessageBox.Show("Unable to write data in disk" + ex.Message);
                        }
                    }
                    if (!ErrorMessage)
                    {
                        try
                        {
                            PdfPTable pTable = new PdfPTable(dgr.Columns.Count - 1);
                            pTable.DefaultCell.Padding = 2;
                            pTable.WidthPercentage = 100;
                            pTable.HorizontalAlignment = Element.ALIGN_LEFT;


                            foreach (DataGridViewColumn col in dgr.Columns)
                            {
                                if (col.Index != 1)
                                {
                                    PdfPCell pcell = new PdfPCell(new Phrase(col.HeaderText));
                                    pTable.AddCell(pcell);
                                }
                            }

                            foreach (DataGridViewRow Row in dgr.Rows)
                            {
                                foreach (DataGridViewCell cell in Row.Cells)
                                {
                                    if (cell.ColumnIndex != 1)
                                    {
                                        pTable.AddCell(cell.Value.ToString());
                                    }
                                }
                            }

                            using (FileStream fileStream = new FileStream(save.FileName, FileMode.Create))
                            {
                                Document document = new Document(PageSize.A4, 8f, 16f, 16f, 8f);
                                PdfWriter.GetInstance(document, fileStream);
                                document.Open();
                                //title
                                BaseFont bfn = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                                Font fntHead = new Font(bfn, 20, 1, BaseColor.BLACK);
                                Paragraph prg = new Paragraph();
                                prg.Alignment = Element.ALIGN_CENTER;
                                prg.Add(new Chunk("Résultat Cheminement", fntHead));
                                document.Add(prg);
                                //Author
                                Paragraph prgAuthor = new Paragraph();
                                BaseFont btnAuthor = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                                Font fntAuthor = new Font(btnAuthor, 8, 2, BaseColor.BLACK);
                                prgAuthor.Alignment = Element.ALIGN_RIGHT;
                                prgAuthor.Add(new Chunk("Produit : GEOBAT Tétouan", fntAuthor));
                                prgAuthor.Add(new Chunk("\n Date : " + DateTime.Now.ToShortDateString(), fntAuthor));
                                document.Add(prgAuthor);

                                // line seperation
                                Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                                document.Add(p);
                                //line break
                                document.Add(new Chunk("\n"));
                                document.Add(pTable);
                                document.Close();
                                fileStream.Close();
                            }
                            MessageBox.Show("Data export successfuly", "info");
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
                MessageBox.Show("No record Found", "Info");

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "Resultat Rayonnement";
            printer.SubTitle = string.Format("Date: {0}", DateTime.Now.Date);
            printer.SubTitleFormatFlags = System.Drawing.StringFormatFlags.LineLimit | System.Drawing.StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = System.Drawing.StringAlignment.Near;
            printer.Footer = "SQRLand";
            printer.FooterSpacing = 15;
            printer.PrintDataGridView(dgr);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Application.Exit();
        }
    }
}
