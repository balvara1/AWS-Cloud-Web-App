using _301171014deguzman_301128209alvarado_Lab3.Models;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _301171014deguzman_301128209alvarado_Lab3
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
            services.AddControllersWithViews();
            var builder = new SqlConnectionStringBuilder(Configuration.GetConnectionString("Connection2RDS"));
            builder.UserID = Configuration["DbUser"];
            builder.Password = Configuration["DbPassword"];
            var connection = builder.ConnectionString;
            services.AddDbContext<MOVIEContext>(options => options.UseSqlServer(connection));


            // for RDS
            //services.AddDbContext<MOVIEContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Connection2RDS")));

            // Create AWS credentials for DynamoDB connectivity
            String accessKey = Configuration.GetSection("AWSCredentials").GetSection("AccesskeyID").Value;
            String secretKey = Configuration.GetSection("AWSCredentials").GetSection("Secretaccesskey").Value;

            // create the AWS Credentials
            var awsCredential = new BasicAWSCredentials(accessKey, secretKey);
            var amazonDynamoDBClient = new AmazonDynamoDBClient(awsCredential, RegionEndpoint.USEast1);
            var s3Client = new AmazonS3Client(awsCredential, RegionEndpoint.USEast1);

            services.AddSingleton<IAmazonDynamoDB>(amazonDynamoDBClient);
            services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
            services.AddSingleton<IAmazonS3>(s3Client);

            // #begin - saving session
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = ".TestApp.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(1800);
                options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews();
            services.AddRazorPages();

            // #end - saving session
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // #begin - saving session
            app.UseSession();
            // #end - saving session

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
