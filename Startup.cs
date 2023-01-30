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
        // swagger�ĵ�����
        c.SwaggerDoc(version, new OpenApiInfo
        {
            Version = version,
            Title = title,
            //Description = $"{title} HTTP API " + v,
            //Contact = new OpenApiContact { Name = "Contact", Email = "xx@xxx.xx", Url = new Uri("https://www.cnblogs.com/straycats/") },
            //License = new OpenApiLicense { Name = "License", Url = new Uri("https://www.cnblogs.com/straycats/") }
        });

        // �ӿ�����
        c.OrderActionsBy(o => o.RelativePath);
        // ��ȡxml�ļ���
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        // ����xml�ĵ�
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);

        c.OperationFilter<AddResponseHeadersFilter>();
        c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
        // ��ȫУ��
        c.OperationFilter<SecurityRequirementsOperationFilter>();

        // ����oauth2��ȫ����
        c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) ֱ�����¿�������Bearer {token}��ע������֮����һ���ո�\"",
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
            //�����м����������Swagger��ΪJSON�ս��
            app.UseSwagger();
            app.UseCors();
            //�����м�������swagger-ui��ָ��Swagger JSON�ս��
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", title);//ע�������v1�Ǹ��������version�����
            });
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
