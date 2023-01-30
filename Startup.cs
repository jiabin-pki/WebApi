using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Reflection;
using WebApi.IServices;
using WebApi.Services;

namespace WebApi
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Context>(opt => opt.UseInMemoryDatabase("UserList"));
            services.AddControllers();
            services.AddScoped<IUserService, UserService>();
            var title = "myApi";
            var version = "v1";
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.AllowAnyOrigin().AllowAnyHeader()
                                                  .AllowAnyMethod(); ;
                    });
            });
            services.AddSwaggerGen(c =>
    {
        // swagger文档配置
        c.SwaggerDoc(version, new OpenApiInfo
        {
            Version = version,
            Title = title,
            //Description = $"{title} HTTP API " + v,
            //Contact = new OpenApiContact { Name = "Contact", Email = "xx@xxx.xx", Url = new Uri("https://www.cnblogs.com/straycats/") },
            //License = new OpenApiLicense { Name = "License", Url = new Uri("https://www.cnblogs.com/straycats/") }
        });

        // 接口排序
        c.OrderActionsBy(o => o.RelativePath);
        // 获取xml文件名
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        // 配置xml文档
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);

        c.OperationFilter<AddResponseHeadersFilter>();
        c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
        // 安全校验
        c.OperationFilter<SecurityRequirementsOperationFilter>();

        // 开启oauth2安全描述
        c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey
        });
    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            var title = "myApi";
            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            app.UseCors();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", title);//注意这里的v1是根据上面的version来填的
            });
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
