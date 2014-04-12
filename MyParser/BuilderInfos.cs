using System.Collections.Generic;
using System.Linq;
using MyLibrary;
using MyLibrary.Types;

namespace MyParser
{
    public class BuilderInfos : Dictionary<string, BuilderInfo>, IValueable
    {
        public Values ToValues()
        {
            return new Values(this);
        }

        public override string ToString()
        {
            var values = new Values
            {
                Key = Keys.ToList(),
                Value = this.Select(item => item.ToString()).ToList(),
            };
            return String.Parse(new Transformation().ParseTemplate(values));
        }

        public void Add(BuilderInfo builderInfo)
        {
            Add(builderInfo.TableName.ToString(), builderInfo);
        }
    }
}