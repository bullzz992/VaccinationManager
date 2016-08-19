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
using Color = MigraDoc.DocumentObjectModel.Color;
using Image = MigraDoc.DocumentObjectModel.Shapes.Image;

namespace VaccinationManager.PDF_Generator
{
    public class InvoiceReport
    {
        VaccinationContext db = new VaccinationContext();
        private string globalCurrentUser;
        public Document CreateDocument(Invoice inputInvoice, string loggedInUser)
        {
            globalCurrentUser = loggedInUser;
            this.document = new Document();
            document.DefaultPageSetup.Orientation = Orientation.Portrait;
            this.document.Info.Title = "Vaccination Invoice";
            this.document.Info.Subject = "Contains Child's Vaccinations Invoice";
            this.document.Info.Author = loggedInUser;

            if (inputInvoice.VaccinationList.Count < 1)
            {
                Section s = this.document.AddSection();
                Paragraph p = s.AddParagraph("No vaccinations for the selected date");
                p.Format.Font.Size = 20;
                p.Format.Font.Color = Colors.Red;
                p.AddLineBreak(); p.AddLineBreak(); p.AddLineBreak(); p.AddLineBreak(); p.AddLineBreak(); p.AddLineBreak();
                p.Format.Alignment = ParagraphAlignment.Center;
                return this.document;

            }


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
            style.Font.Name = "Calibri";

            style = this.document.Styles[StyleNames.Heading1];
            style.Font.Name = "Calibri";
            style.Font.Name = "Calibri";
            style.Font.Size = 9;
            style.Font.Bold = true;
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            style = this.document.Styles[StyleNames.Header];
            style.Font.Name = "Calibri"; 
            style.Font.Name = "Calibri";
            style.Font.Size = 9;
            style.Font.Bold = true;
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            style = this.document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called Table based on style Normal
            style = this.document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Calibri";
            style.Font.Name = "Calibri";
            style.Font.Size = 11;

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

            
            // Create the item table
            this.VaccincationsTable = section.AddTable();
            this.VaccincationsTable.Style = "Table";
            this.VaccincationsTable.Borders.Color = Colors.Gray;
            this.VaccincationsTable.Borders.Width = 0.25;
            this.VaccincationsTable.Borders.Left.Width = 0.5;
            this.VaccincationsTable.Borders.Right.Width = 0.5;
            this.VaccincationsTable.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns
            Column column = this.VaccincationsTable.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = this.VaccincationsTable.AddColumn("5cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = this.VaccincationsTable.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = this.VaccincationsTable.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = this.VaccincationsTable.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = this.VaccincationsTable.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            // Create the header of the table
            Row row = VaccincationsTable.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            //row.Shading.Color = Colors.LightBlue;
            

            row.Cells[0].AddParagraph("Date");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[0].VerticalAlignment = VerticalAlignment.Bottom;

            row.Cells[1].AddParagraph("Description");
            row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[1].VerticalAlignment = VerticalAlignment.Bottom;

            row.Cells[2].AddParagraph("Article/Nappi Code");
            row.Cells[2].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[2].VerticalAlignment = VerticalAlignment.Bottom;

            row.Cells[3].AddParagraph("ICD 10");
            row.Cells[3].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[3].VerticalAlignment = VerticalAlignment.Bottom;

            row.Cells[4].AddParagraph("QTY");
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
            Totals = section.AddParagraph();
            BankingDetails = section.AddParagraph();
            BankingDetails.Format.Alignment = ParagraphAlignment.Right;
        }

        public void FillContents(Invoice contents)
        {
            #region Top paragraph data
            
            Details.Format.Font.Size = 18;
            Details.AddTab(); Details.AddTab(); Details.AddTab(); Details.AddTab(); Details.AddTab();
            Details.AddFormattedText("Invoice", TextFormat.Bold);
            Details.AddLineBreak();

            Details.AddLineBreak(); Details.AddLineBreak();
            //---------------CENTRE DETAILS-----------------------------------------------------------------------------
            Details.Format.Font.Size = 12;
            Details.AddTab(); Details.AddTab(); Details.AddTab(); Details.AddTab(); 
            Details.AddFormattedText("Medical Centre Details", TextFormat.Bold);
            Details.AddLineBreak(); Details.AddLineBreak();

            Details.Format.Font.Size = 11;
            Details.AddFormattedText("Branch Centre Name:", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            if (contents != null && (contents.BranchInformation != null && contents.BranchInformation.Name != null)) Details.AddText(contents.BranchInformation.Name);
            Details.AddLineBreak();

            Details.AddFormattedText("Manager:", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            if (contents != null && (contents.BranchInformation != null && contents.BranchInformation.Overseer_Name != null))
                Details.AddText(contents.BranchInformation.Overseer_Name + " " + contents.BranchInformation.Overseer_Surname);
            Details.AddLineBreak();

            Details.AddFormattedText("Practice Number:", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            if (contents != null && contents.BranchInformation != null && contents.BranchInformation.Practice_No != null)
            {
                Details.AddText(contents.BranchInformation.Practice_No);
            }

            Details.AddLineBreak();

            Details.AddFormattedText("Address:", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab(); Details.AddTab();
            if (contents != null && contents.BranchInformation != null && contents.BranchInformation.Address != null)
                Details.AddText(contents.BranchInformation.Address);
            Details.AddLineBreak(); Details.AddLineBreak();

            Details.AddFormattedText("Telephone:", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            if (contents != null && contents.BranchInformation != null && contents.BranchInformation.Tel_Number != null)
                Details.AddText(contents.BranchInformation.Tel_Number);
            Details.AddLineBreak();

            Details.AddFormattedText("Fax:", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            if (contents != null && contents.BranchInformation != null && contents.BranchInformation.Fax_Number != null)
                Details.AddText(contents.BranchInformation.Fax_Number);
            Details.AddLineBreak(); Details.AddLineBreak();

            //------------------------Parent Details---------------------------------------------------
            var parent = db.Parents.FirstOrDefault(x => x.IdNumber == contents.PatientChild.MotherId);

            if (parent == null)
            {
                parent = db.Parents.FirstOrDefault(x => x.IdNumber == contents.PatientChild.FatherId);
            }

            Details.Format.Font.Size = 11;
            Details.AddTab(); Details.AddTab(); Details.AddTab(); Details.AddTab(); 
            Details.AddFormattedText("Medical Aid Principal Member Details", TextFormat.Bold);
            Details.AddLineBreak(); Details.AddLineBreak();

            Details.AddFormattedText("Name:", TextFormat.Bold);
            Details.AddTab(); Details.AddTab();
            Details.AddTab(); Details.AddTab();
            if (parent != null && parent.Name != null) { Details.AddText(parent.Name); }
            Details.AddLineBreak();
            


            Details.AddFormattedText("Surname:", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            if (parent != null && parent.Surname != null) Details.AddText(parent.Surname);
            Details.AddLineBreak();
            


            Details.AddFormattedText("ID Number:", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            if (parent != null && parent.IdNumber != null) Details.AddText(parent.IdNumber);
            Details.AddLineBreak();
            

            Details.AddFormattedText("Address:", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            if (parent != null && parent.Address != null)
            {

                Address addressResult = parent.Address;
                Details.AddText(addressResult.AddressLine1 + ", " + addressResult.AddressLine2 + ", " + addressResult.Suburb + ", " + addressResult.Town + ", " + addressResult.PostalCode); //Fetch Address from ID and display
            }
                
            Details.AddLineBreak(); Details.AddLineBreak();
            

            Details.AddFormattedText("Telephone:", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            if (parent != null && parent.HomeTel != null) Details.AddText(parent.HomeTel);
            Details.AddLineBreak();
            

            Details.AddFormattedText("Cell:", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            if (parent != null && parent.Cellphone != null) Details.AddText(parent.Cellphone);
            Details.AddLineBreak(); Details.AddLineBreak();

            //----------------------END Parent Details-------------------------------------------------

            //----------------------Medical Aid Info---------------------------------------------------
            
            Details.AddFormattedText("Employer:", TextFormat.Bold);
            Details.AddTab(); Details.AddTab();
            Details.AddTab();
            if(parent != null && parent.Employer != null)
                Details.AddText(parent.Employer);
            Details.AddLineBreak();
            
            Details.AddFormattedText("Office Phone:", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            if (parent != null && parent.Telephone != null)
                if (parent != null) Details.AddText(parent.Telephone);
            Details.AddLineBreak();
            
            Details.AddFormattedText("Medical Aid:", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            Details.AddTab();
            if (parent != null && parent.MedicalAidName != null) Details.AddText(parent.MedicalAidName);
            Details.AddLineBreak();
            
            Details.AddFormattedText("Medical Aid No:", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            if (parent != null && parent.MedicalAidNumber != null)
                Details.AddText(parent.MedicalAidNumber);
            Details.AddLineBreak();
            
            Details.AddFormattedText("Medical Aid Plan:", TextFormat.Bold);
            Details.AddTab();
            Details.AddTab();
            if (parent != null && parent.MedicalAidPlan != null) Details.AddText(parent.MedicalAidPlan);
            Details.AddLineBreak();

            //----------------------END Medical Aid Info-----------------------------------------------

            //------------------------END CENTRE DETAILS----------------------------------------------------------------


            #endregion

            //-----------------------VACCINATIONS-----------------------------------------------------------------------

            var vaccinations = contents.VaccinationList;
            var temp = db.UserStatus.FirstOrDefault(x => x.Username == globalCurrentUser);
            string currentBranch = "";
            if (temp != null)
            {
                currentBranch = temp.Branch_Practice_No;
            }

            decimal sum = Convert.ToDecimal(0.0);
            var extendedFees = db.ExtendedFees.Where(x => x.IncludeInReport == "Include" && x.Branch == currentBranch).ToList();
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
                int quantity = 1;
                row1.Cells[0].AddParagraph(contents.InvoiceFromDate.ToString("yyyy-MM-dd"));
                if (vaccDef != null && vaccDef.Description != null) row1.Cells[1].AddParagraph(vaccDef.Description);
                if (vaccDef != null && vaccDef.Code != null) row1.Cells[2].AddParagraph(vaccDef.Code);
                if (vaccDef != null && vaccDef.ICDCode != null) row1.Cells[3].AddParagraph(vaccDef.ICDCode);
                row1.Cells[4].AddParagraph(quantity.ToString()); // Dummy value for QTY
                if (vaccDef.Price == null)
                {
                    row1.Cells[5].AddParagraph("0.00");
                }
                else
                {
                    row1.Cells[5].AddParagraph(vaccDef.Price.ToString());
                }


                this.VaccincationsTable.SetEdge(0, this.VaccincationsTable.Rows.Count - 2, 6, 2, Edge.Box, BorderStyle.Single, 0.75);
                decimal price;
                if (vaccDef.Price == null)
                {
                    price = Convert.ToDecimal(0.0);
                }
                else
                {
                    price = Convert.ToDecimal(vaccDef.Price);
                }
                sum += (price * quantity);
                //}
            }
            foreach (var item in extendedFees)
            {
                Row row1 = this.VaccincationsTable.AddRow();
                int quantity = 1;

                row1.Cells[0].AddParagraph(contents.InvoiceFromDate.ToString());
                row1.Cells[1].AddParagraph(item.FeeName);
                row1.Cells[2].AddParagraph(item.NappiCode);
                row1.Cells[3].AddParagraph(item.FeeCode);
                row1.Cells[4].AddParagraph(quantity.ToString()); // Dummy value for QTY
                row1.Cells[5].AddParagraph(item.Amount.ToString());
                this.VaccincationsTable.SetEdge(0, this.VaccincationsTable.Rows.Count - 2, 6, 2, Edge.Box, BorderStyle.Single, 0.75);

                sum += (item.Amount*quantity);
            }
            //-----------------------END VACCINATIONS-------------------------------------------------------------------

            //------------------------BANKING DETAILS-------------------------------------------------------------------
            Totals.Format.Alignment = ParagraphAlignment.Right;
            Totals.AddLineBreak();
            Totals.Format.Font.Size = 13;
            Totals.AddFormattedText("Total:", TextFormat.Bold);
            Totals.AddTab();
            Totals.AddTab();
            Totals.AddText("R " + sum);
            Totals.AddTab(); Details.AddTab();
            Totals.AddLineBreak(); Totals.AddLineBreak();

            BankingDetails.Format.Alignment = ParagraphAlignment.Left;
            BankingDetails.AddFormattedText("Banking Details", TextFormat.Underline);
            BankingDetails.AddLineBreak(); Details.AddLineBreak();

            BankingDetails.AddFormattedText("Bank Name:", TextFormat.Bold);
            if (!string.IsNullOrEmpty(contents.BranchInformation.Bank_Name))
            {
                BankingDetails.AddText(contents.BranchInformation.Bank_Name);
            }
            BankingDetails.AddTab();
            BankingDetails.AddLineBreak();

            BankingDetails.AddFormattedText("Account No:", TextFormat.Bold);
            if (!string.IsNullOrEmpty(contents.BranchInformation.Account_Number))
            {
                BankingDetails.AddText(contents.BranchInformation.Account_Number);
            }
            BankingDetails.AddTab();
            BankingDetails.AddLineBreak();
            
            BankingDetails.AddFormattedText("Branch No:", TextFormat.Bold);
            if (!string.IsNullOrEmpty(contents.BranchInformation.Branch_Number))
            {
                BankingDetails.AddText(contents.BranchInformation.Branch_Number);
            }
            BankingDetails.AddTab();
            BankingDetails.AddLineBreak(); Details.AddLineBreak();

            //Details.AddTab(); Details.AddTab(); Details.AddTab(); Details.AddTab();
            //Details.AddFormattedText("Invoice Date: " + contents.InvoiceFromDate.Date.ToString(), TextFormat.Underline);
            //Details.AddLineBreak(); Details.AddLineBreak();


            //------------------------END BANKING DETAILS---------------------------------------------------------------
        }

        public Paragraph Totals { get; set; }
        public Paragraph Details { get; set; }
        public Paragraph BankingDetails { get; set; }

        public Document document { get; set; }

        public TextFrame addressFrame { get; set; }

        public MigraDoc.DocumentObjectModel.Tables.Table VaccincationsTable { get; set; }

        public MigraDoc.DocumentObjectModel.Tables.Table ExtendedFeesTable { get; set; }
    }
}
