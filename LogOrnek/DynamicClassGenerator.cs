using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem
{
    public class DynamicClassGenerator
    {
        public static string GenerateClassFile(string className, string directory)
        {
            string classContent =
                $@"
                using System;
                namespace PaymentSystem
                {{
                    public class {className} : IPayment
                    {{
                        public string pay(long amount)
                        {{
                        return $""{{this.GetType().Name}} Class automaticly generated, Payment amount: {{amount}}"";
                        }}
                    }}
                }}";

            string filePath = Path.Combine(directory, $"{className}.cs");
            File.WriteAllText(filePath, classContent);

            return filePath;
        }
    }
}
