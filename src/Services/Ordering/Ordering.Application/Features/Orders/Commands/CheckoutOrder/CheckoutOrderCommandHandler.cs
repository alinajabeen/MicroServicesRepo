using System;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Ordering.Application.Contracts.Persistence;
using AutoMapper;
using Ordering.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using Ordering.Application.Models;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IEmailService emailService, ILogger<CheckoutOrderCommandHandler> logger  )
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request);
            var newOrder= await _orderRepository.AddAsync(orderEntity);
            _logger.LogInformation($"Order {newOrder.Id} is Successfully Created.");
            await SendMail(newOrder); 
            return newOrder.Id;
        }
        private async Task SendMail(Order order)
        {
            var email = new Email() { To = "alina@mailinator.com", Body = $"Order was created.", Subject = "Order Testing Email"};
            try
            {
                await _emailService.SendEmail(email);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Order {order.Id} failed due to an error with the mail service : {ex.Message}");
            }
        }
    }
}
