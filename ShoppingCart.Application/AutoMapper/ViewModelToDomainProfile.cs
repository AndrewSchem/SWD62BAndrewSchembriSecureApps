using AutoMapper;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.AutoMapper
{
    public class ViewModelToDomainProfile:Profile
    {
        public ViewModelToDomainProfile()
        {
            CreateMap<ProductViewModel, Product>().ForMember(x=>x.Category, opt => opt.Ignore());
            CreateMap<CommentViewModel, Comment>().ForMember(x => x.Submission, opt => opt.Ignore());
            CreateMap<TaskViewModel, Task>();
            CreateMap<SubmissionViewModel, Submission>().ForMember(x=>x.Task, opt => opt.Ignore());
            CreateMap<CategoryViewModel, Category>();
        }
    }
}
