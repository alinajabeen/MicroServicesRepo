using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //It will check the profile information (Is there any class inherited from Profile and executing these profiles when running the application)
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //It will looking for the object inherited from abstract validator class
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            //mediatr will look for the classes which are inherited from IRequest and IRequestHandler
            services.AddMediatR(Assembly.GetExecutingAssembly());

            //Now we are adding the Pipeline Behaviours
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            return services;
        }
    }
}
