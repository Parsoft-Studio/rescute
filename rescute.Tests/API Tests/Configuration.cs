using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rescute.Tests.APITests
{
    public class Configuration : IConfiguration
    {
        private Dictionary<string, string> values = new Dictionary<string, string>();
        public Configuration()
        {
            values.Add("RelativeContentRootPath", "/content");
            values.Add("RelativeAttachmentsRootPath", "content/attachments");
        }
        public string this[string key] { get => values[key]; set => values[key] = value; }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new System.NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new System.NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            throw new System.NotImplementedException();
        }
    }
}
