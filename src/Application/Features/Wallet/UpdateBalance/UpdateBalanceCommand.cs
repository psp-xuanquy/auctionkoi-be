using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiAuction.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Wallet.UpdateBalance;
public class UpdateBalanceCommand : IRequest<string>, ICommand
{
    public string UserID { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; }
    public UpdateBalanceCommand(string userID, decimal amount, string status)
    {
        UserID = userID;
        Amount = amount;
        Status = status;
    }

}
