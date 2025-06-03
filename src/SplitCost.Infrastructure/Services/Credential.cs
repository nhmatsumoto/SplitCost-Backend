using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitCost.Infrastructure.Services;

public class Credential
{
    public string type { get; set; } = "password";
    public string value { get; set; } = string.Empty;
    public bool temporary { get; set; } = false;
}
