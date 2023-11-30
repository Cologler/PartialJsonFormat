using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PartialJsonFormat
{
    public class PartialJsonFormater
    {
        public int Indent { get; set; } = 3;

        public string FormatJson(string json)
        {
            var indent = this.Indent;
            if (indent < 0)
                throw new InvalidOperationException();

            if (string.IsNullOrEmpty(json))
            {
                return string.Empty;
            }

            var containerHasContent = false;
            var formattedJson = new StringBuilder();
            var containerStack = new Stack<char>();

            for (int i = 0; i < json.Length; i++)
            {
                char currentChar = json[i];

                if (GetContainerPrefix() == '"') // inside string
                {
                    formattedJson.Append(currentChar);
                    if (currentChar == '"')
                    {
                        var ch = PopContainerPrefix(); // pop '"'
                        Debug.Assert(ch == '"');
                    }
                }
                else
                {
                    switch (currentChar)
                    {
                        case '{':
                        case '[':
                            formattedJson.Append(currentChar);
                            containerStack.Push(currentChar);
                            containerHasContent = false; // reset
                            break;

                        case '}':
                        case ']':
                            var ch = PopContainerPrefix();
                            if ((currentChar == '}' && ch != '{') || (currentChar == ']' && ch != '['))
                            {
                                // wrong format
                                formattedJson.Append(json[i..]);
                                i = json.Length; // jump to end
                                break;
                            }

                            if (containerHasContent)
                            {
                                WriteIndent();
                            }
                            formattedJson.Append(currentChar);

                            containerHasContent = GetContainerPrefix() != null;
                            break;

                        case ',':
                            formattedJson.Append(currentChar);
                            WriteIndent();
                            break;

                        default:
                            if (GetContainerPrefix() != null) // inside a container
                            {
                                if (currentChar == ' ')
                                {
                                    // ignore whitespace
                                    break;
                                }

                                if (!containerHasContent)
                                { 
                                    WriteIndent();
                                }
                                containerHasContent = true;
                            }

                            formattedJson.Append(currentChar);

                            switch (currentChar)
                            {
                                case '"':
                                    containerStack.Push(currentChar);
                                    break;

                                case ':':
                                    formattedJson.Append(' ');
                                    break;
                            }

                            break;
                    }
                }
            }

            return formattedJson.ToString();

            void WriteIndent()
            {
                formattedJson!.AppendLine();
                formattedJson.Append(' ', containerStack!.Count * indent);
            }

            char? GetContainerPrefix() => containerStack!.Count <= 0 ? null : (char?)containerStack.Peek();

            char? PopContainerPrefix() => containerStack!.Count <= 0 ? null : (char?)containerStack.Pop();
        }
    }
}
