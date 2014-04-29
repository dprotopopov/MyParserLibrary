using System;
using System.Collections.Generic;
using System.Linq;
using MyLibrary;
using String = MyLibrary.Types.String;

namespace MyParser
{
    [Serializable]
    public class BuilderInfos : Dictionary<string, BuilderInfo>, IValueable
    {
        public Values ToValues()
        {
            return new Values(this);
        }

        public override string ToString()
        {
            return
                String.Parse(new Transformation().ParseTemplate(new Values(Keys, this.Select(item => item.ToString()))));
        }

        public void Add(BuilderInfo builderInfo)
        {
            Add(builderInfo.TableName.ToString(), builderInfo);
        }
    }
}