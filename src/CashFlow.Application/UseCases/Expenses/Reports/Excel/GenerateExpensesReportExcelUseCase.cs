using CashFlow.Domain.Extensions;
using CashFlow.Domain.Repositories.Expenses;
using ClosedXML.Excel;

namespace CashFlow.Application.UseCases.Expenses.Reports.Excel;

public class GenerateExpensesReportExcelUseCase : IGenerateExpensesReportExcelUseCase
{
    private const string CURRENCY = "R$";
    private readonly IExpensesReadOnlyRepository _repository;

    public GenerateExpensesReportExcelUseCase(IExpensesReadOnlyRepository repository)
    {
        _repository = repository;
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        var expenses = await _repository.FilterByMonth(month);
        if (expenses.Count == 0)
        {
            return Array.Empty<byte>();
        }

        using var workbook = new XLWorkbook();

        workbook.Author = "Vitor Guilherme";
        workbook.Style.Font.FontSize = 12;
        workbook.Style.Font.FontName = "Arial";

        var worksheet = workbook.Worksheets.Add(month.ToString("Y"));

        InsertHeader(worksheet);

        var raw = 2;
        foreach (var expense in expenses)
        {
            worksheet.Cell($"A{raw}").Value = expense.Title;
            worksheet.Cell($"B{raw}").Value = expense.Date;
            worksheet.Cell($"C{raw}").Value = expense.PaymentType.PaymentTypeToString();

            worksheet.Cell($"D{raw}").Value = expense.Amount;
            worksheet.Cell($"D{raw}").Style.NumberFormat.Format = $"-{CURRENCY} #,##0.00";

            worksheet.Cell($"E{raw}").Value = expense.Description;
            raw++;
        }

        worksheet.Columns().AdjustToContents();

        var file = new MemoryStream();
        workbook.SaveAs(file);

        return file.ToArray();
    }

    private void InsertHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = "Título";
        worksheet.Cell("B1").Value = "Data";
        worksheet.Cell("C1").Value = "Tipo de pagamento";
        worksheet.Cell("D1").Value = "Valor";
        worksheet.Cell("E1").Value = "Descrição";

        worksheet.Cells("A1:E1").Style.Font.Bold = true;
        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#82E0AA");
        worksheet.Cells("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cells("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cells("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cells("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        worksheet.Cells("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    }
}
