﻿using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ShopTestCase.Contracts;
using ShopTestCase.Data.DTO;
using ShopTestCase.Data.Entities;

namespace ShopTestCase.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _repository;
        private readonly ILogger<OrderController> _logger;
        private readonly IMapper _mapper;
        private readonly IValidator<Order> _validator;

        public OrderController(IOrderRepository order, IMapper mapper, ILogger<OrderController> logger, IValidator<Order> validator)
        {
            _repository = order ?? throw new ArgumentNullException(nameof(order));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderForCreationResponse>> CreateOrder([FromBody] OrderForCreationDTO createOrderDTO)
        {
            if (createOrderDTO == null)
            {
                _logger.LogError("CreateOrderRequest object sent from client is null.");
                return BadRequest("CreateOrderRequest object is null");
            }
            var result = await _validator.ValidateAsync(_mapper.Map<Order>(createOrderDTO));
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(errorMessages);
            }
            var orderEntity = _mapper.Map<Order>(createOrderDTO);
            await _repository.CreateOrder(orderEntity);
            var orderToReturn = _mapper.Map<OrderForCreationResponse>(orderEntity);

            return CreatedAtRoute("GetOrderById", new { id = orderToReturn.Id }, orderToReturn);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            try
            {
                var orders = await _repository.GetOrders();
                if (orders == null)
                {
                    return NotFound();
                }

                var orderDTOs = _mapper.Map<List<OrderDTO>>(orders);
                return Ok(orderDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetOrders)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "GetOrderById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OrderDTO>> GetOrderById(int id)
        {
            try
            {
                var order = await _repository.GetOrder(id);
                if (order == null)
                {
                    return NotFound();
                }
                var orderDTOs = _mapper.Map<OrderDTO>(order);
                return Ok(orderDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetOrderById)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
