using Microsoft.AspNetCore.Mvc;
using ShopTestCase.Contracts;
using AutoMapper;
using ShopTestCase.Data.DTO;
using ShopTestCase.Data.Entities;
using FluentValidation;

namespace ShopTestCase.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;
        private readonly IValidator<Product> _validator;

        public ProductController(IProductRepository product, IMapper mapper, ILogger<ProductController> logger, IValidator<Product> validator)
        {
            _repository = product ?? throw new ArgumentNullException(nameof(product));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            try
            {
                var products = await _repository.GetProducts();
                var productsDto = products.Select(product => _mapper.Map<ProductDTO>(product)).ToList();
                return Ok(productsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetProducts)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "GetProductById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            try
            {
                var product = await _repository.GetProduct(id);
                if (product == null)
                {
                    return NotFound();
                }
                var productDto = _mapper.Map<ProductDTO>(product);
                return Ok(productDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetProductById)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{code}/getByCode", Name = "GetProductByCode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductDTO>> GetProductByCode(string code)
        {
            try
            {
                var product = await _repository.GetProductByCode(code);
                if (product == null)
                {
                    return NotFound();
                }
                var productDto = _mapper.Map<ProductDTO>(product);
                return Ok(productDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetProductByCode)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDTO>> CreateProduct([FromBody] ProductForCreateDTO productDTO)
        {
            if (productDTO == null)
            {
                _logger.LogError("CreateProductRequest object sent from client is null.");
                return BadRequest("CreateProductRequest object is null");
            }
            var result = await _validator.ValidateAsync(_mapper.Map<Product>(productDTO));
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(errorMessages);
            }
            var productEntity = _mapper.Map<Product>(productDTO);
            await _repository.CreateProduct(productEntity);
            var productToReturn = _mapper.Map<ProductDTO>(productEntity);

            return CreatedAtRoute("GetProductById", new { id = productToReturn.Id }, productToReturn);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateProduct(int id,[FromBody] ProductForUpdateDTO productDto)
        {
            if (productDto == null)
            {
                _logger.LogError("UpdateProductRequest object sent from client is null.");
                return BadRequest("UpdateProductRequest object is null");
            }
            var product =await _repository.GetProduct(id);
            if (product == null)
            {
                _logger.LogInformation($"Product with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var result = await _validator.ValidateAsync(_mapper.Map(productDto, product));
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(errorMessages);
            }
            await _repository.UpdateProduct(_mapper.Map(productDto, product));
            return NoContent();
        }
    }
}
