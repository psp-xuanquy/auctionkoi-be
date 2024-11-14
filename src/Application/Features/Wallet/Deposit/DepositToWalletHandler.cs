using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Library;
using AutoMapper;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Microsoft.Extensions.Options;
using Net.payOS.Types;

namespace Application.Features.Wallet.Deposit;
public class DepositToWalletHandler : IRequestHandler<DepositToWalletCommand, string>
{
    private readonly PayOSSetting _payOSSetting;
    private readonly PayOSService _payOSService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public DepositToWalletHandler(IOptions<PayOSSetting> payOSSetting, PayOSService payOSService, ICurrentUserService currentUserService, IMapper mapper, IUserRepository userRepository)
    {
        _currentUserService = currentUserService;
        _mapper = mapper;
        _payOSService = payOSService;
        _payOSSetting = payOSSetting.Value;
        _userRepository = userRepository;
    }

    public async Task<string> Handle(DepositToWalletCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        long orderCode = long.Parse(DateTimeOffset.Now.ToString("yyMMddHHmmss"));
        var items = new List<ItemData>
            {
                new ItemData($"NẠP {request.DepositAmount/10} $ VÀO VÍ", 1, request.DepositAmount)
            };
            //https://koiauctionwebapp.azurewebsites.net/
        var payOSModel = new PaymentData(
               orderCode: orderCode,
               amount: request.DepositAmount,
               description: $"{user.FullName} - NapVi",
               items: items,
               returnUrl: $"https://koiauction.site/payment-success?userID={user.Id}&amount={request.DepositAmount/10}",
               cancelUrl: $"https://koiauction.site/payment-cancel?userID={user.Id}&amount={request.DepositAmount/10}",           
               buyerName: user.FullName,
               buyerEmail: user.Email,
               buyerPhone: user.PhoneNumber,
               buyerAddress: user.Address
         );

        try
        {
            var paymentUrl = await _payOSService.CreatePaymentLink(payOSModel);
            if (paymentUrl != null)
            {          
                return paymentUrl.checkoutUrl;
            }
        }
        catch (Exception ex)
        {
            throw new NotFoundException(ex.Message);
        }

        return "An error occurred while creating the payment link.";
    }
}
