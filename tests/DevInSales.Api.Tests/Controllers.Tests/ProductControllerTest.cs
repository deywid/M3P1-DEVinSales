using DevInSales.Api.Controllers;
using DevInSales.Api.Dtos;
using DevInSales.Core.Entities;
using DevInSales.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DevInSales.Api.Tests.Controllers.Tests
{
    public class ProductControllerTest
    {
        private Mock<IProductService> _productService;
        private ProductController _productController;
        public ProductControllerTest()
        {
            _productService = new Mock<IProductService>();
            _productController = new ProductController(_productService.Object);
        }

        [Fact]
        public void ObterProdutoPorId_SeIdExiste_DeveriaRetornarOProdutoSolicitado()
        {
            var id = 1;
            var mock = new Product(id, "mock", 1.99m);
            _productService.Setup(s => s.ObterProductPorId(id)).Returns(mock);

            var produto = _productController.ObterProdutoPorId(id);
            var result = produto.Result as OkObjectResult;

            Assert.Same(mock, result.Value);
            _productService.Verify();
        }
        [Fact]
        public void ObterProdutoPorId_SeIdExiste_DeveriaRetornarSucesso()
        {
            var id = 1;
            var mock = new Product(id, "mock", 1.99m);
            _productService.Setup(s => s.ObterProductPorId(id)).Returns(mock);

            var produto = _productController.ObterProdutoPorId(id);

            Assert.IsType<OkObjectResult>(produto.Result);
            _productService.Verify();
        }

        [Fact]
        public void ObterProdutoPorId_SeIdNaoExiste_DeveriaRetornarNaoEncontrado()
        {
            var id = 1;
            var mock = new Product(id, "mock", 1.99m);
            _productService.Setup(s => s.ObterProductPorId(2)).Returns(mock);

            var produto = _productController.ObterProdutoPorId(id);

            Assert.IsType<NotFoundResult>(produto.Result);
            _productService.Verify();
        }

        [Fact]
        public void AtualizarProduto_SeProdutoExisteEAtualizacaoEhValida_DeveriaRetornarSemConteudo()
        {
            var id = 1;
            var atualizarMock = new AddProduct("mock atualizado", 1.99m);
            var mock = new Product(id, "mock", 1.99m);
            _productService.Setup(s => s.ObterProductPorId(id)).Returns(mock);

            var produto = _productController.AtualizarProduto(atualizarMock, id);
            
            Assert.IsType<NoContentResult>(produto);
            _productService.Verify();
        }

        [Fact]
        public void AtualizarProduto_SeAtualizacaoEhInvalida_DeveriaRetornarRequisicaoRuim()
        {
            var id = 1;
            var atualizarMock = new AddProduct("string", 1.99m);
            var mock = new Product(id, "mock", 1.99m);
            _productService.Setup(s => s.ObterProductPorId(id)).Returns(mock);

            var produto = _productController.AtualizarProduto(atualizarMock, id);

            Assert.IsType<BadRequestObjectResult>(produto);
            _productService.Verify();
        }

        [Fact]
        public void AtualizarProduto_SeProdutoNaoExiste_DeveriaRetornarNaoEncontrado()
        {
            var id = 1;
            var atualizarMock = new AddProduct("mock atualizado", 1.99m);
            var mock = new Product(id, "mock", 1.99m);
            _productService.Setup(s => s.ObterProductPorId(2)).Returns(mock);

            var produto = _productController.ObterProdutoPorId(id);

            Assert.IsType<NotFoundResult>(produto.Result);
            _productService.Verify();
        }

        [Fact]
        public void AtualizarProduto_SeNomeProdutoExiste_DeveriaRetornarRequisicaoRuim()
        {
            var id = 1;
            var atualizarMock = new AddProduct("mock", 1.99m);
            var mock = new Product(id, "mock", 1.99m);
            _productService.Setup(s => s.ObterProductPorId(id)).Returns(mock);
            _productService.Setup(s => s.ProdutoExiste(atualizarMock.Name)).Returns(true);

            var produto = _productController.AtualizarProduto(atualizarMock, id);

            Assert.IsType<BadRequestObjectResult>(produto);
            _productService.Verify();
        }

        [Fact]
        public void Delete_SeProdutoExiste_DeveriaRetornarSucesso()
        {
            var id = 1;
            var mock = new Product(id, "mock", 1.99m);
            _productService.Setup(s => s.ObterProductPorId(id)).Returns(mock);
            _productService.Setup(s => s.Delete(id));

            var produto = _productController.Delete(id);

            Assert.IsType<NoContentResult>(produto);
            _productService.Verify();
        }

        [Fact]
        public void PostProduct_SeProdutoNaoExiste_DeveriaRetornarCriado()
        {
            var mock = new AddProduct("mock", 1.99m);
            var produtoMock = new Product(mock.Name, mock.SuggestedPrice);
            _productService.Setup(s => s.ProdutoExiste(mock.Name)).Returns(false);
            _productService.Setup(s => s.CreateNewProduct(produtoMock)).Returns(It.IsAny<int>);

            var produto = _productController.PostProduct(mock);

            Assert.IsType<CreatedAtActionResult>(produto);
            _productService.Verify();
        }
    }
}
