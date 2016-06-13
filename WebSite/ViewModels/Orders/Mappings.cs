using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AutoMapper;

using BusinessLogic.Models;

namespace WebSite.ViewModels.Orders
{
    using ViewModels.Mappings;

    public class Mappings : IViewModelMapping
    {
        public void Create(IMapperConfiguration configuration)
        {
            configuration.CreateMap<UserOrder, UserOrderListViewModel>("")
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.User.FirstName))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.User.LastName));

            configuration.CreateMap<UserOrder, CreateEditUserOrderViewModel>("")
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.User.Id))
                .ForMember(d => d.Users, o => o.Ignore());
        }
    }
}