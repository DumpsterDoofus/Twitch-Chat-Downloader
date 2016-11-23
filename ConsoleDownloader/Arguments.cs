using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace ConsoleDownloader
{
    /// <summary>
    ///     Arguments class. Taken from http://www.codeproject.com/Articles/3111/C-NET-Command-Line-Arguments-Parser
    /// </summary>
    public class Arguments
    {
        // Variables
        private readonly StringDictionary _parameters;

        // Constructor
        public Arguments(string[] args)
        {
            _parameters = new StringDictionary();
            var spliter = new Regex(@"^-{1,2}|^/|=|:",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

            var remover = new Regex(@"^['""]?(.*?)['""]?$",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

            string parameter = null;

            foreach (var txt in args)
            {
                var parts = spliter.Split(txt, 3);

                switch (parts.Length)
                {
                    case 1:
                        if (parameter != null)
                        {
                            if (!_parameters.ContainsKey(parameter))
                            {
                                parts[0] =
                                    remover.Replace(parts[0], "$1");

                                _parameters.Add(parameter, parts[0]);
                            }
                            parameter = null;
                        }
                        break;

                    case 2:
                        if (parameter != null)
                            if (!_parameters.ContainsKey(parameter))
                                _parameters.Add(parameter, "true");
                        parameter = parts[1];
                        break;

                    case 3:
                        if (parameter != null)
                            if (!_parameters.ContainsKey(parameter))
                                _parameters.Add(parameter, "true");

                        parameter = parts[1];

                        if (!_parameters.ContainsKey(parameter))
                        {
                            parts[2] = remover.Replace(parts[2], "$1");
                            _parameters.Add(parameter, parts[2]);
                        }

                        parameter = null;
                        break;
                }
            }
            if (parameter == null) return;
            if (!_parameters.ContainsKey(parameter))
                _parameters.Add(parameter, "true");
        }

        public string this[string param] => _parameters[param];
    }
}
