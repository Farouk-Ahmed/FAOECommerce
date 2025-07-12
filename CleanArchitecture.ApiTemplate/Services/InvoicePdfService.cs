using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using CleanArchitecture.DataAccess.Models;
using System.Linq;
using System.IO;

namespace CleanArchitecture.ApiTemplate.Services
{
    public class InvoicePdfService
    {
        public byte[] GenerateInvoicePdf(Invoice invoice)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);
                    page.Content()
                        .Column(col =>
                        {
                            col.Item().Text($"Invoice #{invoice.InvoiceNumber}").FontSize(20).Bold();
                            col.Item().Text($"Date: {invoice.InvoiceDate:yyyy-MM-dd}");
                            col.Item().Text($"Customer: {invoice.User?.FirstName} {invoice.User?.LastName}");
                            col.Item().Text($"Order ID: {invoice.OrderId}");
                            col.Item().Text($"Total Amount: {invoice.TotalAmount:C}").Bold();
                            col.Item().Text("");
                            col.Item().Text("Purchase Details:").FontSize(16).Bold();
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                });
                                table.Header(header =>
                                {
                                    header.Cell().Text("Product").Bold();
                                    header.Cell().Text("Qty").Bold();
                                    header.Cell().Text("Unit Price").Bold();
                                    header.Cell().Text("Total").Bold();
                                });
                                foreach (var item in invoice.OrderItems)
                                {
                                    table.Cell().Text(item.Product?.Name ?? "");
                                    table.Cell().Text(item.Quantity.ToString());
                                    table.Cell().Text(item.UnitPrice.ToString("C"));
                                    table.Cell().Text((item.UnitPrice * item.Quantity).ToString("C"));
                                }
                            });
                        });
                });
            });
            using var ms = new MemoryStream();
            document.GeneratePdf(ms);
            return ms.ToArray();
        }
    }
}
