﻿using Dapper;
using EntityFrameworkVsCoreDapper.Context;
using EntityFrameworkVsCoreDapper.Contracts;
using EntityFrameworkVsCoreDapper.DtoDapper;
using EntityFrameworkVsCoreDapper.Helpers;
using EntityFrameworkVsCoreDapper.Results;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkVsCoreDapper.Tests
{
    public class DapperService : IDapperService
    {
        private readonly DapperContext _dapperContext;
        private readonly ConsoleHelper _consoleHelper;
        private readonly ResultService _resultService;
        public DapperService(DapperContext dapperContext, ConsoleHelper consoleHelper, ResultService resultService)
        {
            _dapperContext = dapperContext;
            _consoleHelper = consoleHelper;
            _resultService = resultService;
        }

        public async Task<TimeSpan> SelectSingleProducts(int take)
        {
            var sql = $@"select top(@take) id, name, description, price, old_price OldPrice,
                        brand, customer_id CustomerId, product_page_id ProductPageId
                        from efdp_product";

            var watch = _consoleHelper.StartChrono();

            var rders = await _dapperContext.OpenedConnection.QueryAsync<Product>(
                sql, new { take });

            var result = _consoleHelper.StopChrono(watch, "Dapper single select");

            await _resultService.SaveSelect(take, result.Tempo, watch.InitMemory, TypeTransaction.Dapper, OperationType.SelectSingle);

            return result.Tempo;
        }
        public async Task<TimeSpan> SelectComplexCustomers(int take)
        {
            var sql = @"select
                        -- Customer
                        cust.id CustomerId, (cust.first_name + cust.last_name) Name, cust.email, cust.birth_date BirthDate,
                        -- Address
                        addr.id AddressId, addr.street, addr.city, addr.number, addr.zip_code ZipCode, addr.country,
                        -- Product
                        prod.id ProductId, prod.name, prod.description, prod.brand, prod.Price,
                        -- Product Page
                        prod_pg.id ProductPageId, prod_pg.title, prod_pg.small_description Description, 
                            prod_pg.image_link ImageLink
                        from (
							select top(@take) id, first_name, last_name, email, birth_date, address_id from efdp_customer
						) cust
                        left join efdp_address addr on addr.id = cust.address_id
                        left join efdp_product prod on prod.customer_id = cust.Id
                        left join efdp_product_page prod_pg on prod_pg.Id = prod.product_page_id";

            var watch = _consoleHelper.StartChrono();

            var customerDictionary = new Dictionary<Guid, CustomerDtoDapper>();
            var productDict = new Dictionary<Guid, ProductDtoDapper>();

            await _dapperContext.OpenedConnection
                .QueryAsync<CustomerDtoDapper, AddressDtoDapper, ProductDtoDapper, ProductPageDtoDapper, CustomerDtoDapper>(sql,
                (cust, address, prod, page) =>
                {
                    var customerIsAreadyAdded = customerDictionary.TryGetValue(cust.CustomerId, out CustomerDtoDapper customer);
                    if (!customerIsAreadyAdded)
                        customerDictionary.Add(cust.CustomerId, customer = cust);

                    if (cust is not null && prod is not null)
                    {
                        if (!productDict.TryGetValue(prod.ProductId, out ProductDtoDapper product))
                        {
                            productDict.Add(prod.ProductId, product = prod);
                            product.ProductPage = page;
                            customer.Products.Add(product);
                        }
                    }

                    if (!customerIsAreadyAdded)
                        customer.Address = address;

                    return customer;
                },
                new { take },
                splitOn: "AddressId, ProductId, ProductPageId");
            var tt = customerDictionary.Values.ToList();

            var tempoResult = _consoleHelper.StopChrono(watch, "Dapper").Tempo;
            await _resultService.SaveSelect(take, tempoResult, watch.InitMemory, TypeTransaction.Dapper, OperationType.SelectComplex);
            var cout = tt.Count();
            return tempoResult;
        }

        public async Task<TimeSpan> InsertSingleProducts(int interactions)
        {
            var watch = _consoleHelper.StartChrono();

            
            await AddProducts(new ListTests().ObtenirListProductsAleatoire(interactions, null));
                
            var tempoResult = _consoleHelper.StopChrono(watch, "Dapper").Tempo;

            await _resultService.SaveSelect(interactions, tempoResult, watch.InitMemory, TypeTransaction.Dapper, OperationType.InsertSingle);
            return tempoResult;
        }
        public async Task<TimeSpan> InsertComplexCustomers(int interactions)
        {
            var watch = _consoleHelper.StartChrono();

            await AddCustomers(new ListTests().ObtenirListCustomersAleatoire(interactions));

            var tempoResult = _consoleHelper.StopChrono(watch, "Dapper").Tempo;
            await _resultService.SaveSelect(interactions, tempoResult, watch.InitMemory, TypeTransaction.Dapper,
                OperationType.InsertComplex);
            return tempoResult;
        }

        public async Task AddCustomers(IEnumerable<Customer> customers)
        {
            var adresses = customers.Select(c => c.Address);
            var sqlAdresses = "Insert into efdp_address (id, number, street, city, country, zip_code, administrative_region) Values" +
               "(@Id, @Number, @Street, @City, @Country, @ZipCode, @AdministrativeRegion)";
            await _dapperContext.OpenedConnection.ExecuteAsync(sqlAdresses, adresses);

            var sql = "Insert into efdp_customer (id, first_name, last_name, email, status, birth_date, address_id) Values" +
                $"(@Id, @FirstName, @LastName, @Email, @Status, @BirthDate, @AddressId)";
            await _dapperContext.OpenedConnection.ExecuteAsync(sql, customers);

            var productsPageSql =
                @"insert into efdp_product_page
                  (id, title, small_description, full_description, image_link)
                  Values (@Id, @Title, @SmallDescription, @FullDescription, @ImageLink)";
            var pages = customers.SelectMany(c => c.Products).Select(_ => _.ProductPage);
            await _dapperContext.OpenedConnection.ExecuteAsync(productsPageSql, pages);

            var products = customers.SelectMany(c => c.Products);
            var sqlProducts = "insert into efdp_product (id, name, description, price, old_price, brand, customer_id, product_page_id) Values" +
              "(@Id, @Name, @Description, @Price, @OldPrice, @Brand, @CustomerId, @ProductPageId);";
            await _dapperContext.OpenedConnection.ExecuteAsync(sqlProducts, products);
        }
        public async Task AddProducts(IEnumerable<Product> products)
        {
            var sql = "insert into efdp_product (id, name, description, price, old_price, brand, customer_id) Values" +
                "(@Id, @Name, @Description, @Price, @OldPrice, @Brand, @CustomerId);";
            await _dapperContext.OpenedConnection.ExecuteAsync(sql, products);
        }
    }
}