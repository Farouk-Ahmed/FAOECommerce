using Microsoft.AspNetCore.Mvc;
using CleanArchitecture.DataAccess.Models;
using CleanArchitecture.ApiTemplate.Services;
using System.Linq;

namespace CleanArchitecture.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InvoiceController : ControllerBase
    {
        private readonly IRepository<Invoice> _invoiceRepository;
        private readonly InvoicePdfService _pdfService;

        public InvoiceController(IRepository<Invoice> invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
            _pdfService = new InvoicePdfService();
        }

        [HttpGet("{id}/pdf")]
        public IActionResult GetInvoicePdf(int id)
        {
            var invoice = _invoiceRepository.Get(i => i.Id == id, "Order,OrderItems,User", true);
            if (invoice == null)
                return NotFound();

            var pdfBytes = _pdfService.GenerateInvoicePdf(invoice);
            return File(pdfBytes, "application/pdf", $"Invoice_{invoice.InvoiceNumber}.pdf");
        }
    }
}
