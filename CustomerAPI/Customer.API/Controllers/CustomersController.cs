using CustomerAPI.Dto;
using CustomerAPI.Infrastructure;
using CustomerAPI.Mappers;
using CustomerAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CustomerAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerContext _context;

        public CustomersController(CustomerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a specific customer by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Customer retrieved</response>
        /// <response code="404">Customer not found</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CustomerDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<CustomerDTO>> GetCustomerByIdAsync(int id)
        {
            var campaign = await _context.Customers.SingleOrDefaultAsync(c => c.Id == id);

            if (campaign is null)
            {
                return NotFound();
            }

            return CustomerMapper.MapCustomerModelToDto(campaign);
        }

        /// <summary>
        /// Retrieves a list of customers matching the search criteria 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <remarks>
        /// 'name' is optional. If provided, searches for partial match on the customer's first or last name
        /// </remarks>
        /// <response code="200">Search completed successfully</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<CustomerDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<CustomerDTO>>> GetCustomersAsync(string name = null)
        {
            var search = $"%{name}%";

            IQueryable<Customer> searchQuery = _context.Customers;
            if (!string.IsNullOrWhiteSpace(name))
            {
                searchQuery = searchQuery.Where(c => EF.Functions.Like(c.FirstName, search) || EF.Functions.Like(c.LastName, search));
            }

            var customerList = await searchQuery.ToListAsync();
            if (customerList is null)
            {
                return Ok();
            }

            return CustomerMapper.MapCustomerModelListToDtoList(customerList);
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <param name="customerDto">customer details</param>
        /// <returns></returns>
        /// <response code="400">request has invalid parameters</response>
        /// <response code="201">Customer created successfully</response>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateCustomerAsync([FromBody] CustomerCommandDTO customerDto)
        {
            if (customerDto is null)
            {
                return BadRequest();
            }

            var customer = CustomerMapper.MapCustomerCommandDtoToModel(customerDto);

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomerByIdAsync), new { id = customer.Id }, null);
        }

        /// <summary>
        /// Updates the details of an existing customer
        /// </summary>
        /// <param name="id">unique customer id</param>
        /// <param name="customerDto">customer details</param>
        /// <returns></returns>
        /// <response code="400">request has invalid parameters</response>
        /// <response code="404">customer not found</response>
        /// <response code="201">customer updated successfully</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateCustomerAsync(int id, [FromBody] CustomerCommandDTO customerDto)
        {
            if (id < 1 || customerDto is null)
            {
                return BadRequest();
            }

            var customerToUpdate = await _context.Customers.FindAsync(id);
            if (customerToUpdate is null)
            {
                return NotFound();
            }

            customerToUpdate.FirstName = customerDto.FirstName;
            customerToUpdate.LastName = customerDto.LastName;
            customerToUpdate.DateOfBirth = customerDto.DateOfBirth;

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomerByIdAsync), new { id = customerToUpdate.Id }, null);
        }

        /// <summary>
        /// Delete specific customer by id
        /// </summary>
        /// <param name="id">unique customer id</param>
        /// <returns></returns>
        /// <response code="400">request has invalid parameters</response>
        /// <response code="404">customer not found</response>
        /// <response code="204">customer deleted successfully</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> DeleteCustomerByIdAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            var customerToDelete = await _context.Customers.FindAsync(id);

            if (customerToDelete is null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customerToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
