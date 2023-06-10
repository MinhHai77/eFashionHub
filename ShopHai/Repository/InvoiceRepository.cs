using Microsoft.EntityFrameworkCore;
using ShopHai.Models;
namespace ShopHai.Repository
{
    public interface IInvoiceRepository
    {
        bool Create(Invoice invoice);
        bool Update(Invoice invoice);
        bool Delete(Int16 InvoiceId);
        List<Invoice> GetAll();
        public Invoice FindById(Int16 id);
        List<Invoice> GetInvoicesByUserId(int userId);

    }
    public class InvoiceRepository : IInvoiceRepository
    {
        private ShopClothesContext _ctx;

        public InvoiceRepository(ShopClothesContext ctx)
        {
            _ctx = ctx;
        }

        public bool Create(Invoice invoice)
        {
            _ctx.Invoices.Add(invoice);
            _ctx.SaveChanges();
            return true;
        }

        public bool Update(Invoice invoice)
        {
            Invoice existingInvoice = _ctx.Invoices.FirstOrDefault(t => t.InvoiceId == invoice.InvoiceId);
            if (existingInvoice != null)
            {
                existingInvoice.UserId = invoice.UserId;
                existingInvoice.InvoiceDate = invoice.InvoiceDate;
                existingInvoice.TotalAmount = invoice.TotalAmount;
                existingInvoice.Status = invoice.Status;

                _ctx.SaveChanges();
                return true;
            }

            return false;
        }

        public bool Delete(Int16 InvoiceId)
        {
            Invoice invoice = _ctx.Invoices.FirstOrDefault(t => t.InvoiceId == InvoiceId);

            _ctx.Invoices.Remove(invoice);
            _ctx.SaveChanges();
            return true;

        }

        public List<Invoice> GetAll()
        {
            return _ctx.Invoices.ToList();
        }

        public Invoice FindById(short id)
        {
            Invoice c = _ctx.Invoices.FirstOrDefault(x => x.InvoiceId == id);
            return c;
        }

        public List<Invoice> GetInvoicesByUserId(int userId)
        {
            return _ctx.Invoices.Where(i => i.UserId == userId).ToList();
        }

    }
}

