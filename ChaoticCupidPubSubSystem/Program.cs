using ChaoticCupidPubSubSystem;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapHub<MessageHub>("/messageHub");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
