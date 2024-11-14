using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;
using MediatR;

namespace Application.Features.Wallet.Deposit;
public class DepositToWalletCommand : IRequest<string>, IMapFrom<UserEntity>
{
    public int DepositAmount { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<DepositToWalletCommand, UserEntity>();
    }
}
