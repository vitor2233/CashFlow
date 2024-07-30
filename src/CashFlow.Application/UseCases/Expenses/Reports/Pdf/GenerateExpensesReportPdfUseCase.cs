using System.Reflection;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Colors;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using ClosedXML.Excel;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf;

public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
{
    private const string CURRENCY = "R$";
    private const int HEIGHT_ROW_EXPENSE_TABLE = 25;
    private readonly IExpensesReadOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;

    public GenerateExpensesReportPdfUseCase(IExpensesReadOnlyRepository repository, ILoggedUser loggedUser)
    {
        _repository = repository;
        _loggedUser = loggedUser;

        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        var loggedUser = await _loggedUser.Get();

        var expenses = await _repository.FilterByMonth(loggedUser, month);
        if (expenses.Count == 0)
        {
            return Array.Empty<byte>();
        }

        var document = CreateDocument(loggedUser.Name, month);
        var page = CreatePage(document);
        CreateHeaderWithProfilePhotoAndName(loggedUser.Name, page);

        var totalExpenses = expenses.Sum(e => e.Amount);
        CreateTotalSpentSection(page, month, totalExpenses);

        foreach (var expense in expenses)
        {
            var table = CreateExpensesTable(page);

            var row = table.AddRow();
            row.Borders.Top.Color = ColorsHelper.BLACK;
            row.Borders.Left.Color = ColorsHelper.BLACK;
            row.Borders.Right.Color = ColorsHelper.BLACK;
            row.Height = HEIGHT_ROW_EXPENSE_TABLE;

            AddExpenseTitle(row.Cells[0], expense.Title);

            AddHeaderForAmount(row.Cells[3]);

            row = table.AddRow();
            row.Cells[0].Borders.Left.Color = ColorsHelper.BLACK;
            row.Cells[2].Borders.Right.Color = ColorsHelper.BLACK;
            row.Cells[3].Borders.Right.Color = ColorsHelper.BLACK;
            row.Height = HEIGHT_ROW_EXPENSE_TABLE;

            row.Cells[0].AddParagraph(expense.Date.ToString("D"));
            SetStyleBaseForExpenseInformation(row.Cells[0]);
            row.Cells[0].Format.LeftIndent = 20;

            row.Cells[1].AddParagraph(expense.Date.ToString("t"));
            SetStyleBaseForExpenseInformation(row.Cells[1]);

            row.Cells[2].AddParagraph(expense.PaymentType.PaymentTypeToString());
            SetStyleBaseForExpenseInformation(row.Cells[2]);

            AddAmountForExpense(row.Cells[3], expense.Amount);

            if (!string.IsNullOrWhiteSpace(expense.Description))
            {
                var descriptionRow = table.AddRow();
                descriptionRow.Borders.Left.Color = ColorsHelper.BLACK;
                descriptionRow.Borders.Right.Color = ColorsHelper.BLACK;
                descriptionRow.Borders.Bottom.Color = ColorsHelper.BLACK;
                row.Height = HEIGHT_ROW_EXPENSE_TABLE;

                AddDescriptionRow(descriptionRow, expense.Description);

                row.Cells[3].MergeDown = 1;
            }

            AddWhiteSpace(table);
        }

        return RenderDocument(document);
    }

    private Document CreateDocument(string authorName, DateOnly month)
    {
        var document = new Document();
        document.Info.Title = $"Despesas de {month:Y}";
        document.Info.Title = authorName;

        var style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.RALEWAY_REGULAR;

        return document;
    }

    private void CreateTotalSpentSection(Section page, DateOnly month, decimal totalExpenses)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = "40";
        paragraph.Format.SpaceAfter = "40";
        paragraph.AddFormattedText($"Total gasto em {month:Y}",
            new Font { Name = FontHelper.RALEWAY_REGULAR, Size = 15 }
        );

        paragraph.AddLineBreak();

        paragraph.AddFormattedText($"{CURRENCY} {totalExpenses}",
            new Font { Name = FontHelper.WORKSANS_BLACK, Size = 50 }
        );
    }

    private void CreateHeaderWithProfilePhotoAndName(string name, Section page)
    {
        var table = page.AddTable();
        table.AddColumn("300");

        var row = table.AddRow();

        row.Cells[0].AddParagraph($"Olá, {name}");
        row.Cells[0].Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 16 };
        row.Cells[0].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
    }

    private void AddDescriptionRow(Row row, string description)
    {
        row.Cells[0].AddParagraph(description);
        row.Cells[0].Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 10, Color = ColorsHelper.BLACK };
        row.Cells[0].Shading.Color = ColorsHelper.GREEN_LIGHT;
        row.Cells[0].VerticalAlignment = VerticalAlignment.Center;
        row.Cells[0].MergeRight = 2;
        row.Cells[0].Format.LeftIndent = 20;
    }

    private Section CreatePage(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();

        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.BottomMargin = 80;
        section.PageSetup.TopMargin = 80;

        return section;
    }

    private Table CreateExpensesTable(Section page)
    {
        var table = page.AddTable();
        table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

        return table;
    }

    private byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer
        {
            Document = document,
        };

        renderer.RenderDocument();

        using var file = new MemoryStream();
        renderer.PdfDocument.Save(file);

        return file.ToArray();
    }

    private void AddExpenseTitle(Cell cell, string expenseTitle)
    {
        cell.AddParagraph(expenseTitle);
        cell.Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.RED_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = 20;
    }

    private void AddHeaderForAmount(Cell cell)
    {
        cell.AddParagraph("Valor");
        cell.Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14, Color = ColorsHelper.WHITE };
        cell.Shading.Color = ColorsHelper.RED_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void SetStyleBaseForExpenseInformation(Cell cell)
    {
        cell.Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 12, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.GREEN_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddAmountForExpense(Cell cell, decimal amount)
    {
        cell.AddParagraph($"- {CURRENCY} {amount}");
        cell.Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 14, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddWhiteSpace(Table table)
    {
        var row = table.AddRow();
        row.Height = 30;
        row.Borders.Visible = false;
    }
}
