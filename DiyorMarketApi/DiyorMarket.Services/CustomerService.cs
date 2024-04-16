using AutoMapper;
using DiyorMarket.Domain.DTOs.Customer;
using DiyorMarket.Domain.Entities;
using DiyorMarket.Domain.Interfaces.Services;
using DiyorMarket.Domain.Pagniation;
using DiyorMarket.Domain.ResourceParameters;
using DiyorMarket.Domain.Responses;
using DiyorMarket.Infrastructure.Persistence;

namespace DiyorMarket.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly DiyorMarketDbContext _context;

        public CustomerService(IMapper mapper, DiyorMarketDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public GetBaseResponse<CustomerDto> GetCustomers(CustomerResourceParameters customerResourceParameters)
        {
            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(customerResourceParameters.SearchString))
            {
                query = query.Where(x => x.FullName.Contains(customerResourceParameters.SearchString)
                || x.PhoneNumber.Contains(customerResourceParameters.SearchString));
            }

            if (!string.IsNullOrEmpty(customerResourceParameters.OrderBy))
            {
                query = customerResourceParameters.OrderBy.ToLowerInvariant() switch
                {
                    "firstname" => query.OrderBy(x => x.FullName),
                    "firstnamedesc" => query.OrderByDescending(x => x.FullName),
                    "phonenumber" => query.OrderBy(x => x.PhoneNumber),
                    "phonenumberdesc" => query.OrderByDescending(x => x.PhoneNumber),
                    _ => query.OrderBy(x => x.FullName),
                };
            }

            var customers = query.ToPaginatedList(customerResourceParameters.PageSize, customerResourceParameters.PageNumber);

            var customerDtos = _mapper.Map<List<CustomerDto>>(customers);

            var paginatedResult = new PaginatedList<CustomerDto>(customerDtos, customers.TotalCount, customers.CurrentPage, customers.PageSize);

            return paginatedResult.ToResponse();
        }

        public IEnumerable<CustomerDto> GetCustomers()
        {
            var customers = _context.Customers.ToList();

            return _mapper.Map<IEnumerable<CustomerDto>>(customers) ?? Enumerable.Empty<CustomerDto>();
        }

        public CustomerDto? GetCustomerById(int id)
        {
            var customer = _context.Customers.FirstOrDefault(x => x.Id == id);

            var customerDto = _mapper.Map<CustomerDto>(customer);

            return customerDto;
        }

        public CustomerDto CreateCustomer(CustomerForCreateDto customerToCreate)
        {
            var customerEntity = _mapper.Map<Customer>(customerToCreate);

            _context.Customers.Add(customerEntity);
            _context.SaveChanges();

            var customerDto = _mapper.Map<CustomerDto>(customerEntity);

            return customerDto;
        }

        public CustomerDto UpdateCustomer(CustomerForUpdateDto customerToUpdate)
        {
            var customerEntity = _mapper.Map<Customer>(customerToUpdate);

            _context.Customers.Update(customerEntity);
            _context.SaveChanges();

            var customerDto = _mapper.Map<CustomerDto>(customerEntity);

            return customerDto;
        }

        public void DeleteCustomer(int id)
        {
            var customer = _context.Customers.FirstOrDefault(x => x.Id == id);
            if (customer is not null)
            {
                _context.Customers.Remove(customer);
            }
            _context.SaveChanges();
        }
    }
}
