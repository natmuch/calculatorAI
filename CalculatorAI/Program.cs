// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

Console.WriteLine(SolveEquation("3 + 5 * 2")); // 13

Console.WriteLine(SolveEquation("(3 + 5) * 2")); // 16

Console.WriteLine(SolveEquation("3 * (5 + 2)")); // 21

Console.WriteLine(SolveEquation("(3 + 5) / (2 - 1)")); // 8

Console.WriteLine(SolveEquation("3 + (5 - 2) * 4 / 2")); // 9

Console.WriteLine(SolveEquation("(3 + (5 - 2) * 4) / (2 * (1 + 1))")); // 4.5
Console.ReadKey();
static double SolveEquation(string equation)
{ 
    // Remove all whitespaces from the equation
    equation = equation.Replace(" ", ""); 
    // Check if the equation is empty
    if (equation == "")
    {
        throw new ArgumentException("The equation cannot be empty.");
    }

    // Check if the equation contains any invalid characters
    if (!Regex.IsMatch(equation, @"^[0-9\+\-\*\/\(\)]+$"))
    {
        throw new ArgumentException("The equation contains invalid characters.");
    }



    // Check if the equation has balanced parentheses
    int openCount = 0;
    int closeCount = 0;
    foreach (char c in equation)
    {
        if (c == '(')
        {
            openCount++;
        }
        else if (c == ')')
        {
            closeCount++;
        }
        if (closeCount > openCount)
        {
            throw new ArgumentException("The equation has unbalanced parentheses.");
        }
    }
    if (openCount != closeCount)
    {
        throw new ArgumentException("The equation has unbalanced parentheses.");
    }

    // Solve the equation recursively by finding the innermost parentheses and evaluating them first
    return SolveEquationRec(equation);

}
static double SolveEquationRec(string equation)
{
    // Base case: if the equation does not contain any parentheses, evaluate it directly
    if (!equation.Contains("(") && !equation.Contains(")"))
    {
        return Evaluate(equation);
    }


    // Recursive case: find the innermost parentheses and replace them with their evaluated value

    int start = -1; // The index of the opening parenthesis
    int end = -1; // The index of the closing parenthesis
    for (int i = 0; i < equation.Length; i++)
    {
        char c = equation[i];
        if (c == '(')
        {
            start = i; // Update the start index when encountering an opening parenthesis
        }
        else if (c == ')')
        {
            end = i; // Update the end index when encountering a closing parenthesis
            break; // Break the loop when finding a matching pair of parentheses
        }
    }

    // Extract the substring between the parentheses
    string subEquation = equation.Substring(start + 1, end - start - 1);
    // Evaluate the substring and convert it to a string
    string subValue = SolveEquationRec(subEquation).ToString();
    // Replace the parentheses and the substring with the evaluated value
    string newEquation = equation.Remove(start, end - start + 1).Insert(start, subValue);
    // Solve the new equation recursively
    return SolveEquationRec(newEquation);

}
static double Evaluate(string expression)
{
    // Split the expression by the operators + and - and store them in a list
    List<string> terms = expression.Split(new char[] { '+', '-' }, StringSplitOptions.RemoveEmptyEntries).ToList();
    // Check if the number of terms is equal to the number of operators + and - plus one
    int opCount = expression.Count(c => c == '+' || c == '-');
    if (terms.Count != opCount + 1)
    {
        throw new ArgumentException("The expression is invalid.");
    }

    // Store the operators + and - in a list
    List<char> ops = expression.Where(c => c == '+' || c == '-').ToList();

    // Loop through the terms and evaluate them by the operators * and /

    for (int i = 0; i < terms.Count; i++)
    {
        string term = terms[i];
        // Split the term by the operators * and / and store them in a list
        List<string> factors = term.Split(new char[] { '*', '/' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        // Check if the number of factors is equal to the number of operators * and / plus one
        int opCount2 = term.Count(c => c == '*' || c == '/');
        if (factors.Count != opCount2 + 1)
        {
            throw new ArgumentException("The expression is invalid.");
        }

        // Store the operators * and / in a list
        List<char> ops2 = term.Where(c => c == '*' || c == '/').ToList();
        // Initialize the value of the term as the first factor
        double value = double.Parse(factors[0]);
        // Loop through the remaining factors and evaluate them by the operators * and /
        for (int j = 1; j < factors.Count; j++)
        {
            // Get the next factor and operator
            double factor = double.Parse(factors[j]);
            char op = ops2[j - 1];
            // Perform the operation and update the value
            if (op == '*')
            {
                value *= factor;
            }
            else if (op == '/')
            {
                // Check if the factor is zero
                if (factor == 0)
                {
                    throw new DivideByZeroException("The expression cannot be divided by zero.");
                }
                value /= factor;
            }
        }

        // Replace the term with the evaluated value in the list
        terms[i] = value.ToString();
    }

    // Initialize the result of the expression as the first term
    double result = double.Parse(terms[0]);
    // Loop through the remaining terms and evaluate them by the operators + and -
    for (int i = 1; i < terms.Count; i++)
    {
        // Get the next term and operator
        double term = double.Parse(terms[i]);
        char op = ops[i - 1];
        // Perform the operation and update the result
        if (op == '+')
        {
            result += term;
        }
        else if (op == '-')
        {
            result -= term;
        }

    }



    // Return the result of the expression

    return result;

}