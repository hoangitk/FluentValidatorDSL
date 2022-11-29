// See https://aka.ms/new-console-template for more information
using FluentValidation;
using FluentValidatorDSL;
using System.Text.Json;

/*
 Purpose:how to use ANTLR to build a DSL
 Demo: provider away to define validation rules for validating data model base on FluentValidation
 Flow: Rules => transpile to C# => evaludate via CSharpScript
 */

var rules = @"
// this is a comment
RuleFor FirstName
    : NotEmpty() // call a function
End

RuleFor LastName
    : NotEqual(FirstName) 
End

RuleFor Age
    : GreaterThanOrEqualTo(10 /*this is a number*/)
End"; // load some where: text, db
var patientValidator = new PatientValidator(rules);

foreach (var tc in new Patient[]
{
    new() { LastName = "Nguyen" },
    new() { FirstName = "Hoang", LastName = "Hoang" },
    new() { Age = 8 }
})
{
    patientValidator.Validate(tc).Dump(tc.ToString());
}

"Press any key to exit".Dump();
Console.ReadKey();

public class PatientValidator : AbstractValidator<Patient>
{
    public PatientValidator(string rule)
    {
        Scriptor.Instance.RunAsync(this, rule).GetAwaiter();
    }
}

public class Patient
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public override string ToString() => $"FirstName: {FirstName} | Lastname: {LastName} | Age: {Age}";
}

public static class DumEx
{
    public static T Dump<T>(this T obj, string title = "")
    {
        if (!string.IsNullOrEmpty(title))
        {
            Console.WriteLine(title);
            Console.WriteLine("<<<");
        }
        Console.WriteLine(JsonSerializer.Serialize(obj, DumJsonOptions));
        if (!string.IsNullOrEmpty(title))
        {
            Console.WriteLine(">>>");
        }
        return obj;
    }

    static readonly JsonSerializerOptions DumJsonOptions = new JsonSerializerOptions
    {
        WriteIndented = true
    };
}