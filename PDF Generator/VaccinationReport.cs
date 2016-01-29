using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PdfSharp.Pdf;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using System.Xml.XPath;
using VaccinationManager.Models;
using VaccinationManager.Controllers;
using PdfSharp.Drawing;
using System.Drawing;

namespace VaccinationManager.PDF_Generator
{
    public class VaccinationReport
    {
        public Document CreateDocument(List<ChildVaccination> vaccinations, Child child, string branch)
        {
            // Create a new MigraDoc document
            this.document = new Document();
            document.DefaultPageSetup.Orientation = Orientation.Landscape;
            this.document.Info.Title = "A sample invoice";
            this.document.Info.Subject = "Demonstrates how to create an invoice.";
            this.document.Info.Author = "Stefan Lange";

            DefineStyles();

            CreatePage();

            FillContent(vaccinations, child, branch);

            return this.document;
        }

        public PdfDocument CreateDocumentWithChart(string imageString)
        {
            byte[] imageData = Convert.FromBase64String(imageString);

            return AppendImageToPdf(imageData);
        }
        void DefineStyles()
        {
            // Get the predefined style Normal.
            Style style = this.document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Verdana";

            style = this.document.Styles[StyleNames.Header];
            style.Font.Name = "Verdana";
            style.Font.Name = "Times New Roman";
            style.Font.Size = 15;
            style.Font.Bold = true;
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            style = this.document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called Table based on style Normal
            style = this.document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Verdana";
            style.Font.Name = "Times New Roman";
            style.Font.Size = 9;

            // Create a new style called Reference based on style Normal
            style = this.document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "5mm";
            style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
        }

        public PdfDocument AppendImageToPdf(byte[] img)
        {
            double scale = 1;
            PdfDocument pdf = new PdfDocument();
            using (System.IO.MemoryStream msImg = new System.IO.MemoryStream(img))
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(msImg);
                PdfSharp.Pdf.PdfPage page = pdf.AddPage(new PdfPage());
                PdfSharp.Drawing.XGraphics gfx = PdfSharp.Drawing.XGraphics.FromPdfPage(page);
                PdfSharp.Drawing.XImage ximg = PdfSharp.Drawing.XImage.FromGdiPlusImage(image);

                scale = Scale(ximg.PixelWidth, page.Width.Value);
                gfx.DrawImage(
                    ximg,
                    10,
                    20,
                    ximg.PointWidth * scale,
                    ximg.PointHeight * scale
                );
               
                return pdf;
            }

        }

        double Scale(double imageWidth, double pageWidth)
        {
            double scale = pageWidth / imageWidth;
            return scale;
        }

        void CreatePage()
        {
            // Each MigraDoc document needs at least one section.
            Section section = this.document.AddSection();

            //// Put a logo in the header
            //Image image = section.Headers.Primary.AddImage("../../PowerBooks.png");
            //image.Height = "2.5cm";
            //image.LockAspectRatio = true;
            //image.RelativeVertical = RelativeVertical.Line;
            //image.RelativeHorizontal = RelativeHorizontal.Margin;
            //image.Top = ShapePosition.Top;
            //image.Left = ShapePosition.Right;
            //image.WrapFormat.Style = WrapStyle.Through;

            //TextFrame header = section.Headers.Primary.AddTextFrame();
            //Paragraph headerParagraph = header.AddParagraph();
            //headerParagraph.AddText("Child Vaccinations");

            // Create footer
            Paragraph paragraph = section.Footers.Primary.AddParagraph();
            paragraph.AddText("Hileya Trading PTY LTD");
            paragraph.Format.Font.Size = 9;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            //// Create the text frame for the address
            //this.addressFrame = section.AddTextFrame();
            //this.addressFrame.Height = "3.0cm";
            //this.addressFrame.Width = "7.0cm";
            //this.addressFrame.Left = ShapePosition.Left;
            //this.addressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            //this.addressFrame.Top = "5.0cm";
            //this.addressFrame.RelativeVertical = RelativeVertical.Page;

            //// Put sender in address frame
            //paragraph = this.addressFrame.AddParagraph("Hileya Trading PTY LTD");
            //paragraph.Format.Font.Name = "Times New Roman";
            //paragraph.Format.Font.Size = 7;
            //paragraph.Format.SpaceAfter = 3;

            // Add the print date field
            Details = section.AddParagraph();
            //paragraph.AddDateField("dd.MM.yyyy");

            var par = section.AddParagraph();
            par.AddFormattedText("Vaccinations");
            // Create the item table
            this.table = section.AddTable();
            this.table.Style = "Table";
            this.table.Borders.Color = Colors.Black;
            this.table.Borders.Width = 0.25;
            this.table.Borders.Left.Width = 0.5;
            this.table.Borders.Right.Width = 0.5;
            this.table.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns
            Column column = this.table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = this.table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = this.table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = this.table.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = this.table.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = this.table.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = this.table.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            // Create the header of the table
            Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = Colors.LightBlue;
            row.Cells[0].AddParagraph("Date Due");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[0].VerticalAlignment = VerticalAlignment.Bottom;

            row.Cells[1].AddParagraph("Name");
            row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[1].VerticalAlignment = VerticalAlignment.Bottom;

            row.Cells[2].AddParagraph("Abbreviation");
            row.Cells[2].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[2].VerticalAlignment = VerticalAlignment.Bottom;

            row.Cells[3].AddParagraph("Date Vaccinated");
            row.Cells[3].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[3].VerticalAlignment = VerticalAlignment.Bottom;

            row.Cells[4].AddParagraph("Serial Number");
            row.Cells[4].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[4].VerticalAlignment = VerticalAlignment.Bottom;

            row.Cells[5].AddParagraph("Nurse Signature");
            row.Cells[5].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[5].VerticalAlignment = VerticalAlignment.Bottom;

            row.Cells[6].AddParagraph("Place of Administration");
            row.Cells[6].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[6].VerticalAlignment = VerticalAlignment.Bottom;

            //row = table.AddRow();
            //row.HeadingFormat = true;
            //row.Format.Alignment = ParagraphAlignment.Center;
            //row.Format.Font.Bold = true;
            //row.Shading.Color = Colors.Blue;
            //row.Cells[1].AddParagraph("Quantity");
            //row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
            //row.Cells[2].AddParagraph("Unit Price");
            //row.Cells[2].Format.Alignment = ParagraphAlignment.Left;
            //row.Cells[3].AddParagraph("Discount (%)");
            //row.Cells[3].Format.Alignment = ParagraphAlignment.Left;
            //row.Cells[4].AddParagraph("Taxable");
            //row.Cells[4].Format.Alignment = ParagraphAlignment.Left;

            //this.table.SetEdge(0, 0, 6, 2, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);
        }
        void FillContent(List<ChildVaccination> vaccinations, Child child, string branch)
        {
            Details.AddFormattedText("Name", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            Details.AddText(child.Name);
            Details.AddLineBreak();

            Details.AddFormattedText("Surname", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            Details.AddText(child.Surname);
            Details.AddLineBreak();

            Details.AddFormattedText("ID Number", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            Details.AddText(child.IdNumber);
            Details.AddLineBreak();

            Details.AddFormattedText("Blood Type", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            Details.AddText(child.BloodType.ToString());
            Details.AddLineBreak();

            Details.AddFormattedText("Weight", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            Details.AddText(child.Weight.ToString());
            Details.AddLineBreak();

            Details.AddFormattedText("Height", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            Details.AddText(child.Height.ToString());
            Details.AddLineBreak();

            Details.AddFormattedText("Head Circumference", TextFormat.Bold);
            Details.AddTab();
            Details.AddText(child.HeadCircumference.ToString());
            Details.AddLineBreak();

            Details.AddLineBreak();
            if (child.Mother != null)
            {
                Details.AddFormattedText("Mother: Name", TextFormat.Bold);
                Details.AddTab();
                Details.AddTab();
                Details.AddText(child.Mother.Name.ToString());
                Details.AddLineBreak();

                Details.AddFormattedText("Mother: Surname", TextFormat.Bold);
                Details.AddTab();
                Details.AddTab();
                Details.AddText(child.Mother.Surname.ToString());
                Details.AddLineBreak();

                Details.AddFormattedText("Mother: ID Number", TextFormat.Bold);
                Details.AddTab();
                Details.AddText(child.Mother.IdNumber.ToString());
                Details.AddLineBreak();
            }
            Details.AddLineBreak();
            if (child.Father != null)
            {
                Details.AddFormattedText("Father: Name", TextFormat.Bold);
                Details.AddTab();
                Details.AddTab();
                Details.AddText(child.Father.Name.ToString());
                Details.AddLineBreak();

                Details.AddFormattedText("Father: Surname", TextFormat.Bold);
                Details.AddTab();
                Details.AddTab();
                Details.AddText(child.Father.Surname.ToString());
                Details.AddLineBreak();

                Details.AddFormattedText("Father: ID Number", TextFormat.Bold);
                Details.AddTab();
                Details.AddTab();
                Details.AddText(child.Father.IdNumber.ToString());
                Details.AddLineBreak();
            }
            Details.AddLineBreak();
            Details.AddLineBreak();

            // Iterate the invoice items
            foreach (var vacc in vaccinations)
            {
                //if (vacc.Existing)
                //{
                // Each item fills two rows
                Row row1 = this.table.AddRow();
                //Row row2 = this.table.AddRow();
                //row1.TopPadding = 1.5;
                //row1.Cells[0].Shading.Color = Colors.Gray;
                //row1.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                //row1.Cells[0].MergeDown = 1;
                //row1.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                //row1.Cells[1].MergeRight = 3;
                //row1.Cells[5].Shading.Color = Colors.Gray;
                //row1.Cells[5].MergeDown = 1;
                string dueDate = vacc.DueDate.HasValue ? vacc.DueDate.Value.ToShortDateString() : "";
                row1.Cells[0].AddParagraph(dueDate);
                row1.Cells[1].AddParagraph(vacc.Name);
                row1.Cells[2].AddParagraph(vacc.Code);
                row1.Cells[3].AddParagraph(vacc.DateVaccinated.HasValue ? vacc.DateVaccinated.Value.ToShortDateString() : "");
                row1.Cells[4].AddParagraph(vacc.SerialNumber ?? "");
                row1.Cells[5].AddParagraph(vacc.Signature ?? "");
                row1.Cells[6].AddParagraph(branch);


                this.table.SetEdge(0, this.table.Rows.Count - 2, 6, 2, Edge.Box, BorderStyle.Single, 0.75);
                //}
            }

            // Add an invisible row as a space line to the table
            Row row = this.table.AddRow();
            row.Borders.Visible = false;
        }
        public Paragraph Details { get; set; }

        public Document document { get; set; }

        public TextFrame addressFrame { get; set; }

        public MigraDoc.DocumentObjectModel.Tables.Table table { get; set; }
    }
}