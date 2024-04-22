using MembershipPortal.DTOs;
using MembershipPortal.IRepositories;
using MembershipPortal.IServices;
using MembershipPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static MembershipPortal.DTOs.ProductDTO;
using static MembershipPortal.DTOs.UserDTO;

namespace MembershipPortal.Services
{
    public class ProductService : IProductService
    {

        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<GetProductDTO> CreateProductAsync(CreateProductDTO createProductDTO)
        {
            try

            {
                var product = await _productRepository.CreateAsync(new Product()
                {
                    ProductName = createProductDTO.ProductName,
                    Price = createProductDTO.Price,

                });
                return new GetProductDTO(
                    product.Id,
                    product.ProductName,
                    product.Price
                    );
            }
            catch (Exception)
            {
                //Console.WriteLine($"Error occurred in CreateProductAsync: {ex.Message}");
                throw; ;
            }


        }

        public async Task<bool> DeleteProductAsync(long Id)
        {
            try
            {
                var product = await _productRepository.GetAsyncById(Id);
                if (product != null)
                {

                    await _productRepository.DeleteAsync(product);
                    return true;
                }
            }
            catch (Exception)
            {
                // Console.WriteLine($"Error occurred in DeleteProductAsync: {ex.Message}");
                throw;
            }
            return false;
        }

        public async Task<IEnumerable<GetProductDTO>> GetProductsAsync()
        {
            try
            {
                var products = await _productRepository.GetAsyncAll();

                var productDTO = products.Select(product => new GetProductDTO(

                    product.Id,
                    product.ProductName,
                    product.Price
                    ));
                return productDTO;
            }
            catch (Exception)
            {

                //Console.WriteLine($"Error occurred in GetProductsAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<GetProductDTO> GetProductAsync(long Id)
        {
            try
            {
                var product = await _productRepository.GetAsyncById(Id);
                if (product != null)
                {
                    return new GetProductDTO(

                        product.Id,
                        product.ProductName,
                        product.Price
                        );
                }

            }
            catch (Exception)
            {
                // Console.WriteLine($"Error occurred in GetProductAsync: {ex.Message}");
                throw;

            }
            return null;
        }


        public async Task<GetProductDTO> UpdateProductAsync(long id, UpdateProductDTO updateProductDTO)
        {
            try
            {
                var oldProduct = await _productRepository.GetAsyncById(id);
                if (oldProduct != null)
                {
                    oldProduct.ProductName = updateProductDTO.ProductName;
                    oldProduct.Price = updateProductDTO.Price;

                    var result = await _productRepository.UpdateAsync(oldProduct);
                    if (result != null)
                    {
                        return new GetProductDTO(
                               oldProduct.Id,
                               oldProduct.ProductName,
                               oldProduct.Price
                          );
                    }

                }
                return null;
            }
            catch (Exception)
            {

                //Console.WriteLine($"Error occurred in UpdateProductAsync: {ex.Message}");
                throw;

            }

        }



        public async Task<IEnumerable<GetProductDTO>> GetProductSearchAsync(string find)
        {
            var productList = await _productRepository.GetProductSearchAsync(find);

            var productdto = productList.Select(
                product => new GetProductDTO(
                    product.Id,
                    product.ProductName,
                    product.Price

                ));

            return productdto;
        }

        public async Task<IEnumerable<GetProductDTO>> GetProductAdvanceSearchAsync(GetProductDTO getProductDTO)
        {
            try
            {
                var productList = await _productRepository.GetProductAdvanceSearchAsync(new Product()
                {
                    Id = getProductDTO.Id,
                    ProductName = getProductDTO.ProductName,
                    Price = getProductDTO.Price

                });


                var productdto = productList.Select(
                    product => new GetProductDTO(
                        product.Id,
                        product.ProductName,
                        product.Price

                        ));
                return productdto;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<(IEnumerable<GetProductDTO>, int)> GetAllPaginatedProductAsync(string? sortColumn, string? sortOrder, int page, int pageSize, Product product)
        {
            var productListAndTotalPages = await _productRepository.GetAllPaginatedProductAsync(sortColumn, sortOrder, page, pageSize, product);
            var productDTOList = productListAndTotalPages.Item1.Select(product =>

                    new GetProductDTO(
                            product.Id,
                            product.ProductName,
                            product.Price
                        )
                ).ToList();
            return (productDTOList, productListAndTotalPages.Item2);

        }

        public async Task<IEnumerable<GetProductDTO>> GetAllSortedProducts(string? sortColumn, string? sortOrder)
        {
            try
            {
                var sortedProductsList = await _productRepository.GetAllSortedProducts(sortColumn, sortOrder);
                if (sortedProductsList != null)
                {
                    var sortedProductsDTOList = sortedProductsList
                        .Select(product => new GetProductDTO(product.Id, product.ProductName, product.Price)
                    ).ToList();
                    return sortedProductsDTOList;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
