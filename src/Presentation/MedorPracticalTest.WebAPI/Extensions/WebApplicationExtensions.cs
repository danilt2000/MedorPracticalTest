namespace MedorPracticalTest.WebAPI.Extensions
{
        /// <summary>
        /// Extensions for WebApplication
        /// </summary>
        public static class WebApplicationExtensions
        {
                /// <summary>
                /// Register swagger 
                /// </summary>
                /// <param name="app">WebApplication instance</param>
                public static void AddSwagger(this WebApplication app)
                {
                        if (!app.Environment.IsDevelopment()) return;

                        app.UseSwagger();
                        app.UseSwaggerUI(c =>
                        {
                                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MedorPracticalTest API v1");
                                c.SwaggerEndpoint("/swagger/v2/swagger.json", "MedorPracticalTest API v2");
                        });
                }
        }
}
