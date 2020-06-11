using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace Programowanie2
{
    public enum TokenType
    {
        None,
        Number,
        Constant,
        Plus,
        Minus,
        Multiply,
        Divide,
        Exponent,
        UnaryMinus,
        Sin,
        Cos,
        Tan,
        Abs,
        Exp,
        Log,
        Sqrt,
        Cosh,
        Sinh,
        Tanh,
        Acos,
        Asin,
        Atan,
        Pi,
        E,
        LeftParenthesis,
        RightParenthesis,
        Variable
    }

    public class PointType
    {
        public double x;
        public double y;

        public PointType(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public struct ReversePolishNotationToken
    {
        public string TokenValue;
        public TokenType TokenValueType;
    }

    public class ReversePolishNotation
    {
        private Queue output;
        private Stack ops;

        private string sOriginalExpression;
        public string OriginalExpression
        {
            get { return sOriginalExpression; }
        }

        private string sTransitionExpression;
        public string TransitionExpression
        {
            get { return sTransitionExpression; }
        }

        private string sPostfixExpression;
        public string PostfixExpression
        {
            get { return sPostfixExpression; }
        }

        private double sVariableX;
        public double VariableX
        {
            get { return sVariableX; }
            set { sVariableX = value; }
        }

        public ReversePolishNotation()
        {
            sOriginalExpression = string.Empty;
            sTransitionExpression = string.Empty;
            sPostfixExpression = string.Empty;
        }

        public void Parse(string Expression)
        {
            output = new Queue();
            ops = new Stack();

            sOriginalExpression = Expression;

            string sBuffer = Expression.ToLower();

            sBuffer = Regex.Replace(sBuffer, @"(?<number>\d+(\.\d+)?)", " ${number} ");                     //numbers
            sBuffer = Regex.Replace(sBuffer, @"(?<ops>[+\-*/^()])", " ${ops} ");                            //symbols
            sBuffer = Regex.Replace(sBuffer, "(?<alpha>(exp|asin|sinh|acos|cosh|atan|tanh|pi|e|sin|cos|tan|abs|log|sqrt))", " ${alpha} ");       //functions and x
            sBuffer = Regex.Replace(sBuffer, @"\s+", " ").Trim();                                           // trims up consecutive spaces and replace it with one space

            sBuffer = Regex.Replace(sBuffer, "-", "MINUS");
            sBuffer = Regex.Replace(sBuffer, @"(?<number>(pi|e|([)]|\d+(\.\d+)?)))\s+MINUS", "${number} -");
            sBuffer = Regex.Replace(sBuffer, "MINUS", "~");

            sTransitionExpression = sBuffer;

            //TOKEN
            string[] saParsed = sBuffer.Split(" ".ToCharArray());
            int i = 0;
            double tokenvalue;
            ReversePolishNotationToken token, opstoken;
            for (i = 0; i < saParsed.Length; ++i)
            {
                token = new ReversePolishNotationToken();
                token.TokenValue = saParsed[i];
                token.TokenValueType = TokenType.None;

                try
                {
                    tokenvalue = double.Parse(saParsed[i]);
                    token.TokenValueType = TokenType.Number;

                    output.Enqueue(token);      // If the token is a number, then add it to the output queue.
                }
                catch
                {
                    switch (saParsed[i])
                    {
                        case "+":
                            token.TokenValueType = TokenType.Plus;
                            if (ops.Count > 0)
                            {
                                opstoken = (ReversePolishNotationToken)ops.Peek();

                                while (IsOperatorToken(opstoken.TokenValueType))        // while there is an operator, o2, at the top of the stack
                                {

                                    output.Enqueue(ops.Pop());      // pop o2 off the stack, onto the output queue;
                                    if (ops.Count > 0)
                                    {
                                        opstoken = (ReversePolishNotationToken)ops.Peek();
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }

                            ops.Push(token);        // push o1 onto the operator stack
                            break;
                        case "-":
                            token.TokenValueType = TokenType.Minus;
                            if (ops.Count > 0)
                            {
                                opstoken = (ReversePolishNotationToken)ops.Peek();

                                while (IsOperatorToken(opstoken.TokenValueType))        // while there is an operator, o2, at the top of the stack
                                {

                                    output.Enqueue(ops.Pop());      // pop o2 off the stack, onto the output queue;
                                    if (ops.Count > 0)
                                    {
                                        opstoken = (ReversePolishNotationToken)ops.Peek();
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }

                            ops.Push(token);        // push o1 onto the operator stack
                            break;
                        case "*":
                            token.TokenValueType = TokenType.Multiply;
                            if (ops.Count > 0)
                            {
                                opstoken = (ReversePolishNotationToken)ops.Peek();

                                while (IsOperatorToken(opstoken.TokenValueType))        // while there is an operator, o2, at the top of the stack
                                {
                                    if (opstoken.TokenValueType == TokenType.Plus || opstoken.TokenValueType == TokenType.Minus)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        output.Enqueue(ops.Pop()); // pop o2 off the stack, onto the output queue
                                        if (ops.Count > 0)
                                        {
                                            opstoken = (ReversePolishNotationToken)ops.Peek();
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }

                            ops.Push(token); // push o1 onto the operator stack
                            break;
                        case "/":
                            token.TokenValueType = TokenType.Divide;
                            if (ops.Count > 0)
                            {
                                opstoken = (ReversePolishNotationToken)ops.Peek();

                                while (IsOperatorToken(opstoken.TokenValueType)) // while there is an operator, o2, at the top of the stack
                                {
                                    if (opstoken.TokenValueType == TokenType.Plus || opstoken.TokenValueType == TokenType.Minus)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        output.Enqueue(ops.Pop()); // pop o2 off the stack, onto the output queue;
                                        if (ops.Count > 0)
                                        {
                                            opstoken = (ReversePolishNotationToken)ops.Peek();
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }

                            ops.Push(token);    // push o1 onto the operator stack.
                            break;
                        case "^":
                            token.TokenValueType = TokenType.Exponent;

                            ops.Push(token);    // push o1 onto the operator stack.
                            break;
                        case "~":
                            token.TokenValueType = TokenType.UnaryMinus;

                            ops.Push(token); // push o1 onto the operator stack.
                            break;
                        case "(":
                            token.TokenValueType = TokenType.LeftParenthesis;

                            ops.Push(token); // If the token is a left parenthesis, then push it onto the stack.
                            break;
                        case ")":
                            token.TokenValueType = TokenType.RightParenthesis;
                            if (ops.Count > 0)
                            {
                                opstoken = (ReversePolishNotationToken)ops.Peek();

                                while (opstoken.TokenValueType != TokenType.LeftParenthesis) // Until the token at the top of the stack is a left parenthesis
                                {

                                    output.Enqueue(ops.Pop()); // pop operators off the stack onto the output queue
                                    if (ops.Count > 0)
                                    {
                                        opstoken = (ReversePolishNotationToken)ops.Peek();
                                    }
                                    else
                                    {
                                        throw new Exception("Unbalanced parenthesis!"); // If the stack runs out without finding a left parenthesis, then there are mismatched parentheses.
                                    }

                                }

                                ops.Pop(); // Pop the left parenthesis from the stack, but not onto the output queue.
                            }

                            if (ops.Count > 0)
                            {
                                opstoken = (ReversePolishNotationToken)ops.Peek();

                                if (IsFunctionToken(opstoken.TokenValueType)) // If the token at the top of the stack is a function token
                                {

                                    output.Enqueue(ops.Pop()); // pop it and onto the output queue.
                                }
                            }
                            break;
                        case "x":
                            token.TokenValueType = TokenType.Variable;
                            output.Enqueue(token);
                            break;
                        case "pi":
                            token.TokenValueType = TokenType.Constant;
                            output.Enqueue(token);
                            break;
                        case "e":
                            token.TokenValueType = TokenType.Constant;
                            output.Enqueue(token);
                            break;
                        case "sin":
                            token.TokenValueType = TokenType.Sin;
                            ops.Push(token);
                            break;
                        case "cos":
                            token.TokenValueType = TokenType.Cos;
                            ops.Push(token);
                            break;
                        case "tan":
                            token.TokenValueType = TokenType.Tan;
                            ops.Push(token);
                            break;
                        case "abs":
                            token.TokenValueType = TokenType.Abs;
                            ops.Push(token);
                            break;
                        case "exp":
                            token.TokenValueType = TokenType.Exp;
                            ops.Push(token);
                            break;
                        case "log":
                            token.TokenValueType = TokenType.Log;
                            ops.Push(token);
                            break;
                        case "sqrt":
                            token.TokenValueType = TokenType.Sqrt;
                            ops.Push(token);
                            break;
                        case "cosh":
                            token.TokenValueType = TokenType.Cosh;
                            ops.Push(token);
                            break;
                        case "sinh":
                            token.TokenValueType = TokenType.Sinh;
                            ops.Push(token);
                            break;
                        case "tanh":
                            token.TokenValueType = TokenType.Tanh;
                            ops.Push(token);
                            break;
                        case "acos":
                            token.TokenValueType = TokenType.Acos;
                            ops.Push(token);
                            break;
                        case "asin":
                            token.TokenValueType = TokenType.Asin;
                            ops.Push(token);
                            break;
                        case "atan":
                            token.TokenValueType = TokenType.Atan;
                            ops.Push(token);
                            break;
                    }
                }
            }

            while (ops.Count != 0) // While there are still operator tokens in the stack:
            {
                opstoken = (ReversePolishNotationToken)ops.Pop();

                if (opstoken.TokenValueType == TokenType.LeftParenthesis) // If the operator token on the top of the stack is a parenthesis
                {
                    throw new Exception("Unbalanced parenthesis!"); // then there are mismatched parenthesis.
                }
                else
                {
                    output.Enqueue(opstoken);  // Pop the operator onto the output queue.
                }
            }

            sPostfixExpression = string.Empty;
            foreach (object obj in output)
            {
                opstoken = (ReversePolishNotationToken)obj;
                sPostfixExpression += string.Format("{0} ", opstoken.TokenValue);
            }
        }

        public double Evaluate()
        {
            Stack result = new Stack();
            double oper1 = 0.0, oper2 = 0.0;
            ReversePolishNotationToken token = new ReversePolishNotationToken();
            foreach (object obj in output) // While there are input tokens left
            {
                token = (ReversePolishNotationToken)obj; // Read the next token from input.
                switch (token.TokenValueType)
                {
                    case TokenType.Variable:
                        result.Push(sVariableX);
                        break;
                    case TokenType.Number:
                        // If the token is a value
                        // Push it onto the stack.
                        result.Push(double.Parse(token.TokenValue));
                        break;
                    case TokenType.Constant:
                        // If the token is a value
                        // Push it onto the stack.
                        result.Push(EvaluateConstant(token.TokenValue));
                        break;
                    case TokenType.Plus:
                        // n is 2 in this case
                        // If there are fewer than n values on the stack
                        if (result.Count >= 2)
                        {
                            // So, pop the top n values from the stack.
                            oper2 = (double)result.Pop();
                            oper1 = (double)result.Pop();
                            // Evaluate the function, with the values as arguments.
                            // Push the returned results, if any, back onto the stack.
                            result.Push(oper1 + oper2);
                        }
                        else
                        {
                            // (Error) The user has not input sufficient values in the expression.
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.Minus:
                        if (result.Count >= 2)
                        {
                            oper2 = (double)result.Pop();
                            oper1 = (double)result.Pop();

                            result.Push(oper1 - oper2);
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.Multiply:
                        if (result.Count >= 2)
                        {
                            oper2 = (double)result.Pop();
                            oper1 = (double)result.Pop();

                            result.Push(oper1 * oper2);
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.Divide:
                        if (result.Count >= 2)
                        {
                            oper2 = (double)result.Pop();
                            oper1 = (double)result.Pop();

                            result.Push(oper1 / oper2);
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.Exponent:
                        if (result.Count >= 2)
                        {
                            oper2 = (double)result.Pop();
                            oper1 = (double)result.Pop();

                            result.Push(Math.Pow(oper1, oper2));
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.UnaryMinus:
                        if (result.Count >= 1)
                        {
                            oper1 = (double)result.Pop();

                            result.Push(-oper1);
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.Sin:
                        if (result.Count >= 1)
                        {
                            oper1 = (double)result.Pop();

                            result.Push(Math.Sin(oper1));
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.Cos:
                        if (result.Count >= 1)
                        {
                            oper1 = (double)result.Pop();

                            result.Push(Math.Cos(oper1));
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.Tan:
                        if (result.Count >= 1)
                        {
                            oper1 = (double)result.Pop();

                            result.Push(Math.Tan(oper1));
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.Abs:
                        if (result.Count >= 1)
                        {
                            oper1 = (double)result.Pop();

                            result.Push(Math.Abs(oper1));
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.Exp:
                        if (result.Count >= 1)
                        {
                            oper1 = (double)result.Pop();

                            result.Push(Math.Exp(oper1));
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.Log:
                        if (result.Count >= 1)
                        {
                            oper1 = (double)result.Pop();

                            result.Push(Math.Log(oper1));
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.Sqrt:
                        if (result.Count >= 1)
                        {
                            oper1 = (double)result.Pop();

                            result.Push(Math.Sqrt(oper1));
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.Cosh:
                        if (result.Count >= 1)
                        {
                            oper1 = (double)result.Pop();

                            result.Push(Math.Cosh(oper1));
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.Sinh:
                        if (result.Count >= 1)
                        {
                            oper1 = (double)result.Pop();

                            result.Push(Math.Sinh(oper1));
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.Tanh:
                        if (result.Count >= 1)
                        {
                            oper1 = (double)result.Pop();

                            result.Push(Math.Tanh(oper1));
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.Acos:
                        if (result.Count >= 1)
                        {
                            if (VariableX > 1 || VariableX < -1) throw new Exception("Invalid domain error!");
                            oper1 = (double)result.Pop();

                            result.Push(Math.Acos(oper1));
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.Asin:
                        if (result.Count >= 1)
                        {
                            if (VariableX > 1 || VariableX < -1) throw new Exception("Invalid domain error!");
                            oper1 = (double)result.Pop();

                            result.Push(Math.Asin(oper1));
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.Atan:
                        if (result.Count >= 1)
                        {
                            oper1 = (double)result.Pop();

                            result.Push(Math.Atan(oper1));
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                }
            }

            if (result.Count == 1)
            {
                return (double)result.Pop(); // That value is the result of the calculation.
            }
            else
            {
                // If there are more values in the stack
                // (Error) The user input too many values.
                throw new Exception("Evaluation error!");
            }
        }

        private bool IsOperatorToken(TokenType t)
        {
            bool result = false;
            switch (t)
            {
                case TokenType.Plus:
                case TokenType.Minus:
                case TokenType.Multiply:
                case TokenType.Divide:
                case TokenType.Exponent:
                case TokenType.UnaryMinus:
                    result = true;
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }

        private bool IsFunctionToken(TokenType t)
        {
            bool result = false;
            switch (t)
            {
                case TokenType.Sin:
                case TokenType.Cos:
                case TokenType.Tan:
                case TokenType.Exp:
                case TokenType.Abs:
                case TokenType.Log:
                case TokenType.Sqrt:
                case TokenType.Cosh:
                case TokenType.Sinh:
                case TokenType.Tanh:
                case TokenType.Acos:
                case TokenType.Asin:
                case TokenType.Atan:
                    result = true;
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }

        private double EvaluateConstant(string TokenValue)
        {
            double result = 0.0;
            switch (TokenValue)
            {
                case "pi":
                    result = Math.PI;
                    break;
                case "e":
                    result = Math.E;
                    break;
            }
            return result;
        }

        public List<PointType> CalculateRange(double xMin, double xMax, int n) //calculate range between xMin and xMax
        {
            List<PointType> list = new List<PointType>();
            double result = 0;
            double length = (xMax - xMin) / (n - 1);
            sVariableX = xMin;
            for (int i = 0; i < n; i++)
            {
                result = Evaluate();
                list.Add(new PointType(sVariableX, result));
                sVariableX += length;
            }
            return list;
        }
    }
}