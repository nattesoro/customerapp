using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CustomerApp.Data;
using CustomerApp.Models;

namespace CustomerApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly CustomerContext _context;

        public CustomersController(CustomerContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index(string filters, string searchString, int? pageNumber)
        {
            int pageSize = 3;
            ViewData["CurrentFilter"] = searchString;

            if (String.IsNullOrEmpty(filters))
                return View(await PaginatedList<Customer>.CreateAsync(_context.Customers.AsNoTracking(), pageNumber ?? 1, pageSize));
            else
            {
                var customerContext = _context.Customers.Where(c => c.FirstName == searchString);

                if (filters=="LastName")
                    customerContext = _context.Customers.Where(c => c.LastName == searchString);
                else if (filters == "Email")
                    customerContext = _context.Customers.Where(c => c.Email == searchString);
                else if (filters == "MobileNumber")
                    customerContext = _context.Customers.Where(c => c.MobileNumber == searchString);

                List<Customer> lst = await PaginatedList<Customer>.CreateAsync(customerContext.AsNoTracking(),pageNumber ?? 1, pageSize);

                return View(lst);
            }

        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Logs/5
        public async Task<IActionResult> Logs(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerLogs = _context.CustomerLogs.Where(c => c.CustomerID == id);

            if (customerLogs == null)
            {
                return NotFound();
            }
            
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerID == id);
            ViewData["FullName"] = customer.FullName;

            List<CustomerLog> lst = await customerLogs.ToListAsync();

            return View(lst);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerID,FirstName,LastName,Email,MobileNumber,CreateDate,UpdateDate")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.FullName = String.Format("{0} {1}", customer.FirstName, customer.LastName);
                _context.Add(customer);
                await _context.SaveChangesAsync();

                var customerLog = new CustomerLog[]
                {
                    new CustomerLog{ CustomerID=customer.CustomerID, LogDateTime=DateTime.Now, LogDescription="New Customer successfully added."}
                };
                _context.CustomerLogs.Add(customerLog[0]);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerID,FirstName,LastName,Email,MobileNumber,CreateDate,UpdateDate")] Customer customer)
        {
            if (id != customer.CustomerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    customer.FullName = String.Format("{0} {1}", customer.FirstName, customer.LastName);
                    customer.UpdateDate = DateTime.Now;
                    _context.Update(customer);
                    await _context.SaveChangesAsync();

                    var customerLog = new CustomerLog[]
                    {
                        new CustomerLog{ CustomerID=customer.CustomerID, LogDateTime=DateTime.Now, LogDescription="Customer has been modified."}
                    };
                    _context.CustomerLogs.Add(customerLog[0]);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                var customerLog = new CustomerLog[]
                {
                    new CustomerLog{ CustomerID=customer.CustomerID, LogDateTime=DateTime.Now, LogDescription="Customer has been deleted."}
                };
                _context.CustomerLogs.Add(customerLog[0]);
                await _context.SaveChangesAsync();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerID == id);
        }
        public ActionResult ModalPopUp()
        {
            return View();
        }
    }
}
