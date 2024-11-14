using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Application.Common.Library;
using AutoMapper;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.Features.Wallet.UpdateBalance;
public class UpdateBalanceHandler : IRequestHandler<UpdateBalanceCommand, string>
{
   
    private readonly PayOSSetting _payOSSetting;
    private readonly PayOSService _payOSService;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UpdateBalanceHandler(IOptions<PayOSSetting> payOSSetting, IMapper mapper, PayOSService payOSService, IUserRepository userRepository)
    {
        _payOSSetting = payOSSetting.Value;  
        _payOSService = payOSService;
        _userRepository = userRepository;
        _mapper = mapper;    
    }

    public async Task<string> Handle(UpdateBalanceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.FindAsync(x => x.Id == request.UserID, cancellationToken);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
           

            if (request.Status.ToUpper() == "PAID")
            {
                user.Balance += request.Amount;
                _userRepository.Update(user);
                await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);          
                return "Deposit to wallet successful.";
            }
            else
            {       
                return "Deposit to wallet has been canceled.";
            }


        }
        catch (KeyNotFoundException ex)
        {     
            return $"Error: {ex.Message}";
        }
        catch (ApplicationException ex)
        {         
            return $"Error: {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"{ex.Message}";
        }
    }
}
