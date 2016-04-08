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
using VaccinationManager.DAL;

namespace VaccinationManager.PDF_Generator
{
    public class InvoiceReport
    {
        VaccinationContext db = new VaccinationContext();
        public Document CreateDocument(Invoice inputInvoice, string loggedInUser)
        {
            this.document = new Document();
            document.DefaultPageSetup.Orientation = Orientation.Portrait;
            this.document.Info.Title = "Vaccination Invoice";
            this.document.Info.Subject = "Contains Child's Vaccinations Invoice";
            this.document.Info.Author = loggedInUser;



            DefineStyles();
            CreatePage();
            FillContents(inputInvoice);

            return this.document;
        }

        public void DefineStyles()
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

        public void CreatePage()
        {
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
            paragraph.AddText("Vaccincation Manager");
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
            par.AddLineBreak();
            // Create the item table
            this.VaccincationsTable = section.AddTable();
            this.VaccincationsTable.Style = "Table";
            this.VaccincationsTable.Borders.Color = Colors.Black;
            this.VaccincationsTable.Borders.Width = 0.25;
            this.VaccincationsTable.Borders.Left.Width = 0.5;
            this.VaccincationsTable.Borders.Right.Width = 0.5;
            this.VaccincationsTable.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns
            Column column = this.VaccincationsTable.AddColumn("5cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = this.VaccincationsTable.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = this.VaccincationsTable.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = this.VaccincationsTable.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = this.VaccincationsTable.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = this.VaccincationsTable.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            // Create the header of the table
            Row row = VaccincationsTable.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = Colors.LightBlue;
            

            row.Cells[0].AddParagraph("Name");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[0].VerticalAlignment = VerticalAlignment.Bottom;

            row.Cells[1].AddParagraph("Abbreviation");
            row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[1].VerticalAlignment = VerticalAlignment.Bottom;

            row.Cells[2].AddParagraph("Date Vaccinated");
            row.Cells[2].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[2].VerticalAlignment = VerticalAlignment.Bottom;

            row.Cells[3].AddParagraph("Serial Number");
            row.Cells[3].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[3].VerticalAlignment = VerticalAlignment.Bottom;

            row.Cells[4].AddParagraph("Nurse Signature");
            row.Cells[4].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[4].VerticalAlignment = VerticalAlignment.Bottom;

            row.Cells[5].AddParagraph("Amount");
            row.Cells[5].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[5].VerticalAlignment = VerticalAlignment.Bottom;

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

        public void FillContents(Invoice contents)
        {


            Details.AddFormattedText("Invoice for Patient:", TextFormat.Bold);
            Details.AddTab();
            Details.AddText(contents.PatientChild.Name + " " + contents.PatientChild.Surname + " (ID: " + contents.PatientChild.IdNumber + ")");
            Details.AddLineBreak();

            Details.AddLineBreak(); Details.AddLineBreak();
            //---------------CENTRE DETAILS-----------------------------------------------------------------------------
            Details.AddTab(); Details.AddTab(); Details.AddTab(); Details.AddTab();
            Details.AddFormattedText("Medical Centre Details", TextFormat.Bold);
            Details.AddLineBreak(); Details.AddLineBreak();

            Details.AddFormattedText("Branch/Centre Name:", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            Details.AddText(contents.BranchInformation.Name);
            Details.AddLineBreak();

            Details.AddFormattedText("Manager:", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            Details.AddText(contents.BranchInformation.Overseer_Name + " " + contents.BranchInformation.Overseer_Surname);
            Details.AddLineBreak();

            Details.AddFormattedText("Practice Number:", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            Details.AddText(contents.BranchInformation.Practice_No);
            Details.AddLineBreak();

            Details.AddFormattedText("Physical Address:", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            Details.AddText(contents.BranchInformation.Address);
            Details.AddLineBreak();

            Details.AddFormattedText("Tel:", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            Details.AddText(contents.BranchInformation.Tel_Number);
            Details.AddLineBreak();

            Details.AddFormattedText("Fax:", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            Details.AddText(contents.BranchInformation.Fax_Number);
            Details.AddLineBreak(); Details.AddLineBreak();

            //------------------------END CENTRE DETAILS----------------------------------------------------------------

            //------------------------BANKING DETAILS-------------------------------------------------------------------

            Details.AddTab(); Details.AddTab(); Details.AddTab(); Details.AddTab();
            Details.AddFormattedText("Banking Details", TextFormat.Underline);
            Details.AddLineBreak(); Details.AddLineBreak();

            Details.AddFormattedText("Bank Name:", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            Details.AddText(contents.BranchInformation.Bank_Name);
            Details.AddLineBreak();

            Details.AddFormattedText("Account No:", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            Details.AddText(contents.BranchInformation.Account_Number);
            Details.AddLineBreak();

            Details.AddFormattedText("Branch No:", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            Details.AddText(contents.BranchInformation.Branch_Number);
            Details.AddLineBreak(); Details.AddLineBreak();

            Details.AddTab(); Details.AddTab(); Details.AddTab(); Details.AddTab();
            Details.AddFormattedText("Invoice Date: " + contents.InvoiceFromDate.Date.ToString() , TextFormat.Underline);
            Details.AddLineBreak(); Details.AddLineBreak();

            
            //------------------------END BANKING DETAILS---------------------------------------------------------------


            //-----------------------VACCINATIONS-----------------------------------------------------------------------

            var vaccinations = contents.VaccinationList.Where(x => x.Date == DateTime.Today.Date).ToList();

            foreach (var vacc in vaccinations)
            {
                //if (vacc.Existing)
                //{
                // Each item fills two rows
                Row row1 = this.VaccincationsTable.AddRow();
                //Row row2 = this.table.AddRow();
                //row1.TopPadding = 1.5;
                //row1.Cells[0].Shading.Color = Colors.Gray;
                //row1.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                //row1.Cells[0].MergeDown = 1;
                //row1.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                //row1.Cells[1].MergeRight = 3;
                //row1.Cells[5].Shading.Color = Colors.Gray;
                //row1.Cells[5].MergeDown = 1;
                
                VaccinationDefinition vaccDef = db.VaccinationDefinitions.FirstOrDefault(x=>x.Id == vacc.VaccinationDefinitionId);
                VaccinationPrice vaccPrice = db.VaccinationPrices.FirstOrDefault(x=>x.VaccinationDefId == vacc.VaccinationDefinitionId.ToString());

                row1.Cells[0].AddParagraph(vaccDef.Name);
                row1.Cells[1].AddParagraph(vaccDef.Code);
                row1.Cells[2].AddParagraph(vacc.Date.ToString());
                row1.Cells[3].AddParagraph(vacc.SerialNumber ?? "");
                row1.Cells[4].AddParagraph(vacc.Signature ?? "");
                row1.Cells[5].AddParagraph(vaccPrice.PriceAmount.ToString());


                this.VaccincationsTable.SetEdge(0, this.VaccincationsTable.Rows.Count - 2, 6, 2, Edge.Box, BorderStyle.Single, 0.75);
                //}
            }

            //-----------------------END VACCINATIONS-------------------------------------------------------------------
        }


        public Paragraph Details { get; set; }

        public Document document { get; set; }

        public TextFrame addressFrame { get; set; }

        public MigraDoc.DocumentObjectModel.Tables.Table VaccincationsTable { get; set; }

        public MigraDoc.DocumentObjectModel.Tables.Table ExtendedFeesTable { get; set; }
    }
}
