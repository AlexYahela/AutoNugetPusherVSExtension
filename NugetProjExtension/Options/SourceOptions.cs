using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NugetProjExtension.Options
{
    public class SourceOptions<T> : BaseOptionModel<T> where T : BaseOptionModel<T>, new()
    {

        [Category("Source")]
        [DisplayName("In use")]
        public bool IsUseSpecificPath { get; set; }

        [Category("Source")]
        [DisplayName("Path")]
        public string SpecificPath { get; set; } = "";

        [Category("Source")]
        [DisplayName("Name")]
        [DefaultValue("otherSource")]
        public string SpecificName { get; set; } = "";

        [Category("Source")]
        [DisplayName("User name")]
        public string UserName { get; set; } = "";

        [Category("Source")]
        [DisplayName("Password/Token")]

        [PasswordPropertyText]
        public string PasswordToken { get; set; } = "";

    }
}
