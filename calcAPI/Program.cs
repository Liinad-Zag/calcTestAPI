using calcAPI;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/{operand1}/{operation}/{operand2}", (string operand1, string operand2, string operation) => calculatorLogic.oneStepExpression(operand1,operand2,operation));
app.MapGet("/{expression}", (string expression) => calculatorLogic.hardExpression(ref expression));
app.Run();
