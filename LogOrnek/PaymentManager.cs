using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem
{
    public class PaymentManager : IPaymentManager
    {
        [Required]
        private Assembly _paymentAssembly;
        private string _paymentName;
        private Dictionary<string, string> _paymentNames;
        private static string paymentClassDirectory;

        public PaymentManager() {
            var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();

            paymentClassDirectory = configuration.GetValue<string>("AppSettings:ClassPath");
        }
        public bool CheckConfigFile()
        {
            
            _paymentNames = getPaymentNamesFromConfig();
            List<string> missingClasses = checkMissingClassFiles(paymentClassDirectory, _paymentNames);
            return CreateMissingLoggers(missingClasses, paymentClassDirectory);

        }

        public Dictionary<string, string> GetPaymentTypes()
        {
            return _paymentNames;
        }

        public bool SetPaymentType(string PaymentType)
        {
            try
            {
                _paymentAssembly = Compiler.CompileClass(Path.Combine(paymentClassDirectory, $"{PaymentType}.cs"));
                _paymentName = PaymentType;
                return true;
                    }

            catch { return false; }
        }

        public string pay(long amount)
        {

            Type type = _paymentAssembly.GetType($"PaymentSystem.{_paymentName}");
            if (type == null)
            {
                throw new Exception($"Class {_paymentName} not found in assembly.");
            }

            object instance = Activator.CreateInstance(type);
            var method = type.GetMethod("pay");

            string result = (string)method.Invoke(instance, new object[] { amount });
            return result;
        }

        private Dictionary<string, string> getPaymentNamesFromConfig()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var logTypes = configuration.GetSection("AppSettings:PaymentTypes").Get<Dictionary<string, string>>();

            return logTypes;
        }



        private List<string> checkMissingClassFiles(string directoryPath, Dictionary<string, string> paymentTypes)
        {
            List<string> missingFiles = new();

            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException($"The directory '{directoryPath}' does not exist.");
            }

            var existingFiles = Directory.GetFiles(directoryPath, "*.cs")
                                         .Select(Path.GetFileNameWithoutExtension)
                                         .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var entry in paymentTypes)
            {
                string expectedClass = entry.Value;
                if (!existingFiles.Contains(expectedClass))
                {
                    missingFiles.Add(expectedClass);
                }
            }

            return missingFiles;
        }


        private bool CreateMissingLoggers(List<string> missingClasses, string directory)
        {
            try
            {
                foreach (var missingClass in missingClasses)
                {
                    DynamicClassGenerator.GenerateClassFile(missingClass, directory);
                }
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
